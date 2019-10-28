using System;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class EntityModelSerializer
    {
        private void CustomConstructor()
        {
            this.DefaultConstructor();
        }

        private string CustomXmlTagName
        {
            get { return this.DefaultXmlTagName; }
        }

        private string CustomMonikerTagName
        {
            get { return this.DefaultMonikerTagName; }
        }

        private string CustomMonikerAttributeName
        {
            get { return this.DefaultMonikerAttributeName; }
        }

        private void CustomRead(SerializationContext context, ModelElement element, XmlReader reader)
        {
            //MessageBox.Show("GenerateBuildInDomainTypes from EntityModelSerializer.CustomRead...");
            EntityModel entityModel = (EntityModel)element;
            entityModel.GenerateBuildInDomainTypes();

            if (!context.Result.Failed && !reader.EOF && reader.NodeType == XmlNodeType.Element)
            {
                string modelVersion = reader.GetAttribute("modelVersion");

                ModelUpgrader.Instance.DeserializingModelVersion =
                    string.IsNullOrEmpty(modelVersion)
                        ? ModelUpgrader.Version_1_0_4_0
                        : new VersionNumber(modelVersion);
                reader.SkipToNextElementFix();
            }

            DefaultRead(context, element, reader);
        }

        private void CustomWriteElements(SerializationContext serializationContext, ModelElement element, XmlWriter writer)
        {
            //DefaultWriteElements(context, element, writer);

            if (!serializationContext.Result.Failed)
            {
                writer.WriteAttributeString("modelVersion", ModelApp.ApplicationVersion.ToString());
            }


            // EntityModelHasDomainTypes
            // Non-public getter, use DomainRoleInfo methods.
            System.Collections.ObjectModel.ReadOnlyCollection<EntityModelHasDomainTypes> allEntityModelHasDomainTypesInstances = DomainRoleInfo.GetElementLinks<EntityModelHasDomainTypes>(element, EntityModelHasDomainTypes.EntityModelDomainRoleId);
            if (!serializationContext.Result.Failed && allEntityModelHasDomainTypesInstances.Count > 0)
            {
                writer.WriteStartElement("domainTypes");
                System.Type typeofEntityModelHasDomainTypes = typeof(EntityModelHasDomainTypes);
                foreach (EntityModelHasDomainTypes eachEntityModelHasDomainTypesInstance in allEntityModelHasDomainTypesInstances)
                {
                    if (serializationContext.Result.Failed)
                        break;

                    if (eachEntityModelHasDomainTypesInstance.GetType() != typeofEntityModelHasDomainTypes)
                    {	// Derived relationships will be serialized in full-form.
                        DomainClassXmlSerializer derivedRelSerializer = serializationContext.Directory.GetSerializer(eachEntityModelHasDomainTypesInstance.GetDomainClass().Id);
                        System.Diagnostics.Debug.Assert(derivedRelSerializer != null, "Cannot find serializer for " + eachEntityModelHasDomainTypesInstance.GetDomainClass().Name + "!");
                        derivedRelSerializer.Write(serializationContext, eachEntityModelHasDomainTypesInstance, writer);
                    }
                    else
                    {	// No need to serialize the relationship itself, just serialize the role-player directly.
                        ModelElement targetElement = eachEntityModelHasDomainTypesInstance.DomainType;
                        DomainClassXmlSerializer targetSerializer = serializationContext.Directory.GetSerializer(targetElement.GetDomainClass().Id);
                        System.Diagnostics.Debug.Assert(targetSerializer != null, "Cannot find serializer for " + targetElement.GetDomainClass().Name + "!");
                        targetSerializer.Write(serializationContext, targetElement, writer);
                    }
                }
                writer.WriteEndElement();
            }

            // EntityModelHasPersistentTypes
            // Non-public getter, use DomainRoleInfo methods.
            System.Collections.ObjectModel.ReadOnlyCollection<EntityModelHasPersistentTypes> allEntityModelHasPersistentTypesInstances = DomainRoleInfo.GetElementLinks<EntityModelHasPersistentTypes>(element, EntityModelHasPersistentTypes.EntityModelDomainRoleId);
            if (!serializationContext.Result.Failed && allEntityModelHasPersistentTypesInstances.Count > 0)
            {
                writer.WriteStartElement("persistentTypes");
                Type typeofEntityModelHasPersistentTypes = typeof(EntityModelHasPersistentTypes);
                foreach (EntityModelHasPersistentTypes eachEntityModelHasPersistentTypesInstance in allEntityModelHasPersistentTypesInstances)
                {
                    if (serializationContext.Result.Failed)
                        break;

                    if (eachEntityModelHasPersistentTypesInstance.GetType() != typeofEntityModelHasPersistentTypes)
                    {	// Derived relationships will be serialized in full-form.
                        DomainClassXmlSerializer derivedRelSerializer = serializationContext.Directory.GetSerializer(eachEntityModelHasPersistentTypesInstance.GetDomainClass().Id);
                        System.Diagnostics.Debug.Assert(derivedRelSerializer != null, "Cannot find serializer for " + eachEntityModelHasPersistentTypesInstance.GetDomainClass().Name + "!");
                        derivedRelSerializer.Write(serializationContext, eachEntityModelHasPersistentTypesInstance, writer);
                    }
                    else
                    {	// No need to serialize the relationship itself, just serialize the role-player directly.
                        ModelElement targetElement = eachEntityModelHasPersistentTypesInstance.Element;
                        DomainClassXmlSerializer targetSerializer = serializationContext.Directory.GetSerializer(targetElement.GetDomainClass().Id);
                        System.Diagnostics.Debug.Assert(targetSerializer != null, "Cannot find serializer for " + targetElement.GetDomainClass().Name + "!");
                        targetSerializer.Write(serializationContext, targetElement, writer);
                    }
                }
                writer.WriteEndElement();
            }
        }

        private void CustomReadElements(SerializationContext serializationContext, ModelElement element, XmlReader reader)
        {
            DefaultReadElements(serializationContext, element, reader);
        }

        private ModelElement CustomTryCreateInstance(SerializationContext context, XmlReader reader, Partition partition)
        {
            return DefaultTryCreateInstance(context, reader, partition);
        }

        private ModelElement CustomCreateInstance(SerializationContext context, XmlReader reader, Partition partition)
        {
            return DefaultCreateInstance(context, reader, partition);
        }

        private Moniker CustomTryCreateMonikerInstance(SerializationContext context, XmlReader reader, ModelElement sourceRolePlayer, Guid relDomainClassId, Partition partition)
        {
            return DefaultTryCreateMonikerInstance(context, reader, sourceRolePlayer, relDomainClassId, partition);
        }

        private Moniker CustomCreateMonikerInstance(SerializationContext context, XmlReader reader, ModelElement sourceRolePlayer, Guid relDomainClassId, Partition partition)
        {
            return DefaultCreateMonikerInstance(context, reader, sourceRolePlayer, relDomainClassId, partition);
        }

        private void CustomWriteMoniker(SerializationContext context, ModelElement element, XmlWriter writer, ModelElement sourceRolePlayer, DomainRelationshipXmlSerializer relSerializer)
        {
            DefaultWriteMoniker(context, element, writer, sourceRolePlayer, relSerializer);
        }

        private void CustomWrite(SerializationContext context, ModelElement element, XmlWriter writer, RootElementSettings rootElementSettings)
        {
            DefaultWrite(context, element, writer, rootElementSettings);
        }

        private string CustomCalculateQualifiedName(DomainXmlSerializerDirectory directory, ModelElement element)
        {
            return DefaultCalculateQualifiedName(directory, element);
        }

        private string CustomGetMonikerQualifier(DomainXmlSerializerDirectory directory, ModelElement element)
        {
            return DefaultGetMonikerQualifier(directory, element);
        }

        private void CustomWritePropertiesAsAttributes(SerializationContext serializationContext, ModelElement element, System.Xml.XmlWriter writer)
        {
            DefaultWritePropertiesAsAttributes(serializationContext, element, writer);
        }

        private void CustomReadPropertiesFromAttributes(SerializationContext serializationContext, ModelElement element, System.Xml.XmlReader reader)
        {
            DefaultReadPropertiesFromAttributes(serializationContext, element, reader);
        }
    }
}