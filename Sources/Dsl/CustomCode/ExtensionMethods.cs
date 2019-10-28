using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public static class ExtensionMethods
    {
        public static bool CurrentTransactionIsSerializing(this ModelElement modelElement)
        {
            if (modelElement != null)
            {
                var transaction = modelElement.Store.TransactionManager.CurrentTransaction;
                return transaction == null ? false : transaction.IsSerializing;
            }

            return false;
        }

        public static void RollbackCurrentTransaction(this ModelElement modelElement, string errorMessage)
        {
            if (modelElement != null && modelElement.Store.TransactionManager.CurrentTransaction != null)
            {
                modelElement.Store.TransactionManager.CurrentTransaction.Rollback();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void LogErrorIfAny(this ValidationContext context, ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                context.LogError(validationResult.ErrorMessage, validationResult.ErrorCode,
                                 validationResult.ValidationElements);
            }
        }

        public static string GetPropertyType(this INavigationProperty @this, CodeDomProvider code,
            Func<IPersistentType, IPersistentType, string> escapeNameWithNamespaceFunc,
            Func<OrmType, string, string> buildXtensiveTypeFunc)
        {
            string result = string.Empty;

            IPersistentType ownerPersistentType = @this.OwnerPersistentType;
            bool isSource = @this.PersistentTypeHasAssociations.SourcePersistentType == ownerPersistentType;

            IPersistentType targetPersistentType = isSource
                                                      ? @this.PersistentTypeHasAssociations.TargetPersistentType
                                                      : @this.PersistentTypeHasAssociations.SourcePersistentType;

            IPersistentType oppositePersistentType = isSource
                                                      ? @this.PersistentTypeHasAssociations.SourcePersistentType
                                                      : @this.PersistentTypeHasAssociations.TargetPersistentType;

            string targetTypeName = escapeNameWithNamespaceFunc(targetPersistentType, oppositePersistentType);

            switch (@this.Multiplicity)
            {
                case MultiplicityKind.Many:
                    {
                        result = @this.TypedEntitySet == null
                                     ? buildXtensiveTypeFunc(OrmType.EntitySet, targetTypeName) //OrmUtils.BuildXtensiveType(OrmType.EntitySet, targetTypeName)
                                     : escapeNameWithNamespaceFunc(@this.TypedEntitySet, @this.Owner);
                        break;
                    }
                case MultiplicityKind.ZeroOrOne:
                case MultiplicityKind.One:
                    {
                        result = targetTypeName;
                        break;
                    }
            }

            return result;
        }

        public static T GetSerializer<T>(this ModelElement modelElement) where T : DomainClassXmlSerializer
        {
            return (T) GetSerializer(modelElement);
        }

        public static DomainClassXmlSerializer GetSerializer(Guid domainClassId)
        {
            var serializer = EntityModelDesignerSerializationBehavior.Instance.AllSerializers.SingleOrDefault(
                entry => entry.DomainClassId == domainClassId);

            return serializer == null ? null : Activator.CreateInstance(serializer.SerializerType) as DomainClassXmlSerializer;
        }
        
        public static DomainClassXmlSerializer GetSerializer(this ModelElement modelElement)
        {
            Guid domainClassId = modelElement.GetDomainClass().Id;
            return GetSerializer(domainClassId);
        }

        public static T DeserializeFromString<T>(Store store, Guid domainClassId, string xml) where T : ModelElement
        {
            T result = default(T);
            var serializer = GetSerializer(domainClassId);

            DomainXmlSerializerDirectory directory = store.SerializerDirectory;
            SerializationContext serializationContext = new SerializationContext(directory);

            XmlReaderSettings settings = DONetEntityModelDesignerSerializationHelper.Instance.CreateXmlReaderSettings(serializationContext, false);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    // Attempt to read the encoding.
                    reader.Read(); // Move to the first node - will be the XmlDeclaration if there is one.
                    reader.SkipToNextElementFix();

                    store.MakeActionWithinTransaction("Deserializing type",
                        delegate
                        {
                            ModelElement element = serializer.TryCreateInstance(serializationContext, reader,
                                store.DefaultPartition);

                            serializer.Read(serializationContext, element, reader);

                            result = (T) element;
                        });
                }
            }

            return result;
        }

        public static string SerializeToString(this ModelElement modelElement)
        {
            string serializedString = null;
            var serializer = modelElement.GetSerializer();

            if (serializer != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    DomainXmlSerializerDirectory directory = modelElement.Store.SerializerDirectory;
                    SerializationContext serializationContext = new SerializationContext(directory);

                    // MonikerResolver shouldn't be required in Save operation, so not calling SetupMonikerResolver() here.
                    serializationContext.WriteOptionalPropertiesWithDefaultValue = false;
                    var settings =
                        DONetEntityModelDesignerSerializationHelper.Instance.CreateXmlWriterSettings(serializationContext,
                                                                                                     false,
                                                                                                     Encoding.UTF8);

                    using (XmlWriter writer = XmlWriter.Create(stream, settings))
                    {
                        serializer.Write(serializationContext, modelElement, writer);

                        writer.Flush();

                        stream.Position = 0;
                        serializedString = new StreamReader(stream).ReadToEnd();
                        writer.Close();
                        stream.Close();
                    }
                }
                
            }
            return serializedString;
        }

        public static bool HasViolations(this ValidationContext context)
        {
            return context.CurrentViolations.Count > 0;
        }

        public static void BeginValidationGlobalStage(this ValidationContext context, ValidationGlobalStage globalStage)
        {
            ValidationGlobalStage currentStage = GetValidationGlobalStage(context);

            ValidationGlobalStage newStage = (ValidationGlobalStage)Util.SetFlag(globalStage, currentStage);

            context.SetCacheValue("__ValidationGlobalStage", new ValidationGlobalStageImpl(newStage));
        }

        public static void EndValidationGlobalStage(this ValidationContext context, ValidationGlobalStage globalStage)
        {
            ValidationGlobalStage validationGlobalStage = GetValidationGlobalStage(context, "__ValidationGlobalStage");
            ValidationGlobalStage newStage = (ValidationGlobalStage)Util.ClearFlag(globalStage, validationGlobalStage);

            context.SetCacheValue("__ValidationGlobalStage", new ValidationGlobalStageImpl(newStage));
        }

        public static bool IsInGlobalStage(this ValidationContext context, ValidationGlobalStage globalStage)
        {
            ValidationGlobalStage validationGlobalStage = GetValidationGlobalStage(context);
            return Util.IsFlagSet(globalStage, validationGlobalStage);
        }

        public static ValidationGlobalStage GetValidationGlobalStage(this ValidationContext context)
        {
            return GetValidationGlobalStage(context, "__ValidationGlobalStage");
        }

        private static ValidationGlobalStage GetValidationGlobalStage(this ValidationContext context, string key)
        {
            ValidationGlobalStageImpl v;
            if (context.TryGetCacheValue(key, out v))
            {
                return v.Current;
            }

            return ValidationGlobalStage.Unknown;
        }

        public static int CountViolations(this ValidationContext context, params ViolationType[] violationTypes)
        {
            return context.CurrentViolations.Count(message => message.Type.In(violationTypes));
        }
    }
}