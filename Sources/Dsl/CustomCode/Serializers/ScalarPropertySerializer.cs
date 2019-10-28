using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class ScalarPropertySerializer
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

        private static void CustomWritePropertiesAsElements(SerializationContext context, ScalarProperty element, XmlWriter writer)
        {
            if (!context.Result.Failed)
            {
                element.KeyAttribute.SerializeToXml(writer);
            }
        }

        private static void CustomReadPropertiesFromElements(SerializationContext serializationContext, ScalarProperty element, XmlReader reader)
        {
            while (!serializationContext.Result.Failed && !reader.EOF && reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.LocalName)
                {
                    case "key":	// key
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmKeyAttribute keyAttribute = new OrmKeyAttribute();
                            keyAttribute.DeserializeFromXml(reader);
                            element.KeyAttribute = keyAttribute;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    default:
                        return;  // Don't know this element.
                }
            }
        }

        private void CustomWritePropertiesAsAttributes(SerializationContext serializationContext, ModelElement element, XmlWriter writer)
        {
            // Always call the base class so any extensions are serialized
            base.WritePropertiesAsAttributes(serializationContext, element, writer);

            ScalarProperty instanceOfScalarProperty = element as ScalarProperty;
            global::System.Diagnostics.Debug.Assert(instanceOfScalarProperty != null, "Expecting an instance of ScalarProperty");

            // Type
            if (!serializationContext.Result.Failed)
            {
//                instanceOfScalarProperty.type
//                string clrName = SystemPrimitiveTypesConverter.GetClrName(propValue);

                IDomainType domainType = instanceOfScalarProperty.Type;
                Guid typeId = domainType.GetTypeId();

                DONetEntityModelDesignerSerializationHelper.Instance.WriteAttributeString(serializationContext, element,
                    writer, "typeId", typeId.ToString());
            }
        }

        private void CustomReadPropertiesFromAttributes(SerializationContext serializationContext, ModelElement element, XmlReader reader)
        {
            // Always call the base class so any extensions are deserialized
            base.ReadPropertiesFromAttributes(serializationContext, element, reader);

            ScalarProperty instanceOfScalarProperty = element as ScalarProperty;
            global::System.Diagnostics.Debug.Assert(instanceOfScalarProperty != null, "Expecting an instance of ScalarProperty");

            ModelUpgrader.Instance.RequestSerializationUpgrade(
                upgrader => upgrader.ReadPropertiesFromAttributes(serializationContext, instanceOfScalarProperty, reader));

            // Type
/*
            if (!serializationContext.Result.Failed)
            {
                string attribType = DONetEntityModelDesignerSerializationHelper.Instance.ReadAttribute(serializationContext, element, reader, "type");
                if (!string.IsNullOrEmpty(attribType))
                {
                    string typeDisplayName = SystemPrimitiveTypesConverter.GetDisplayName(attribType);
                    if (!string.IsNullOrEmpty(typeDisplayName))
                    {
                        Type clrType = SystemPrimitiveTypesConverter.GetClrType(typeDisplayName);
                        Guid typeId = SystemPrimitiveTypesConverter.GetTypeId(clrType);
                        IModelRoot entityModel = element.Store.GetEntityModel();
                        IDomainType domainType = entityModel.GetDomainType(typeId);
                        if (domainType == null)
                        {
                            domainType =
                                entityModel.BuildInDomainTypes.SingleOrDefault(type => type.FullName == "System.String");
                        }

                        instanceOfScalarProperty.Type = (DomainType) domainType;

                        element.Store.PropertyBag["@@OldModelConversions"] = true;
                    }
                    else
                    {	// Invalid property value, ignored.
                        EntityModelDesignerSerializationBehaviorSerializationMessages.IgnoredPropertyValue(serializationContext, reader, "type", typeof(global::System.String), attribType);
                    }
                }
            }
*/

            // typeId
            if (!serializationContext.Result.Failed)
            {
                string attribType = DONetEntityModelDesignerSerializationHelper.Instance.ReadAttribute(serializationContext, element, reader, "typeId");
                if (!string.IsNullOrEmpty(attribType))
                {
                    Guid typeId = new Guid(attribType);
                    IModelRoot entityModel = element.Store.GetEntityModel();
                    IDomainType domainType = entityModel.GetDomainType(typeId);
                    if (domainType == null)
                    {
                        domainType =
                            entityModel.BuildInDomainTypes.SingleOrDefault(type => type.FullName == "System.String");
                    }

                    instanceOfScalarProperty.Type = (DomainType)domainType;
                }
            }
        }

        private void CustomRead(SerializationContext context, ModelElement element, XmlReader reader)
        {
            DefaultRead(context, element, reader);
        }

        private void CustomReadElements(SerializationContext context, ModelElement element, XmlReader reader)
        {
            DefaultReadElements(context, element, reader);
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

        private void CustomWriteElements(SerializationContext context, ModelElement element, XmlWriter writer)
        {
            DefaultWriteElements(context, element, writer);
        }

        private string CustomCalculateQualifiedName(DomainXmlSerializerDirectory directory, ModelElement element)
        {
            return DefaultCalculateQualifiedName(directory, element);
        }

        private string CustomGetMonikerQualifier(DomainXmlSerializerDirectory directory, ModelElement element)
        {
            return DefaultGetMonikerQualifier(directory, element);
        }

    }
}