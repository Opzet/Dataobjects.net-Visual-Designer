using System;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade
{
    public sealed class ModelVersionUpgrader_1_0_5_0 : IModelVersionUpgrader
    {
        public string TargetVersion
        {
            get { return "1.0.5.0"; }
        }

        public bool ReadPropertiesFromAttributes<TModelElement>(SerializationContext serializationContext, TModelElement element,
            XmlReader reader) where TModelElement : ModelElement
        {
            bool changesWasMade = false;

            if (element is PropertyBase)
            {
                changesWasMade |= PropertyBase_ReadPropertiesFromAttributes(serializationContext, element as PropertyBase, reader);
            }

            if (element is ScalarProperty)
            {
                changesWasMade |= ScalarProperty_ReadPropertiesFromAttributes(serializationContext, element as ScalarProperty, reader);
            }

            return changesWasMade;
        }

        private bool PropertyBase_ReadPropertiesFromAttributes(SerializationContext serializationContext,
            PropertyBase propertyBase, XmlReader reader)
        {
            // Access
            if (!serializationContext.Result.Failed)
            {
                string attribAccess = DONetEntityModelDesignerSerializationHelper.Instance.ReadAttribute(serializationContext, propertyBase, reader, "access");
                if (attribAccess != null)
                {
                    AccessModifier valueOfAccess;
                    if (SerializationUtilities.TryGetValue<AccessModifier>(serializationContext, attribAccess, out valueOfAccess))
                    {
                        propertyBase.PropertyAccess = new PropertyAccessModifiers();
                        PropertyAccessModifier modifier = valueOfAccess == AccessModifier.Public
                                                              ? PropertyAccessModifier.Public
                                                              : PropertyAccessModifier.Internal;
                        propertyBase.PropertyAccess.Getter = modifier;
                        propertyBase.PropertyAccess.Setter = modifier;

                        return true;
                    }
                    else
                    {	// Invalid property value, ignored.
                        EntityModelDesignerSerializationBehaviorSerializationMessages.IgnoredPropertyValue(serializationContext, reader, "access", typeof(AccessModifier), attribAccess);
                    }
                }
                else
                {
                    propertyBase.PropertyAccess = new PropertyAccessModifiers();
                    propertyBase.PropertyAccess.Getter = PropertyAccessModifier.Public;
                    propertyBase.PropertyAccess.Setter = PropertyAccessModifier.Public;

                    return false;
                }
            }

            return false;
        }

        private bool ScalarProperty_ReadPropertiesFromAttributes(SerializationContext serializationContext, 
            ScalarProperty scalarProperty, XmlReader reader)
        {
            // Type
            if (!serializationContext.Result.Failed)
            {
                string attribType = DONetEntityModelDesignerSerializationHelper.Instance.ReadAttribute(serializationContext, scalarProperty, reader, "type");
                if (!string.IsNullOrEmpty(attribType))
                {
                    string typeDisplayName = SystemPrimitiveTypesConverter.GetDisplayName(attribType);
                    if (!string.IsNullOrEmpty(typeDisplayName))
                    {
                        Type clrType = SystemPrimitiveTypesConverter.GetClrType(typeDisplayName);
                        Guid typeId = SystemPrimitiveTypesConverter.GetTypeId(clrType);
                        IModelRoot entityModel = scalarProperty.Store.GetEntityModel();
                        IDomainType domainType = entityModel.GetDomainType(typeId);
                        if (domainType == null)
                        {
                            domainType =
                                entityModel.BuildInDomainTypes.SingleOrDefault(type => type.FullName == "System.String");
                        }

                        scalarProperty.Type = (DomainType)domainType;

                        return true;
                    }
                    else
                    {	// Invalid property value, ignored.
                        EntityModelDesignerSerializationBehaviorSerializationMessages.IgnoredPropertyValue(serializationContext, reader, "type", typeof(global::System.String), attribType);
                    }
                }
            }

            return false;
        }
    }
}