using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class InterfaceSerializer
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

        private static void CustomWritePropertiesAsElements(SerializationContext context, Interface element, XmlWriter writer)
        {
            if (!context.Result.Failed)
            {
                var value = element.InheritsIEntity.ToString();
                DONetEntityModelDesignerSerializationHelper.Instance.WriteElementString(context, element, writer, "inheritsIEntity", value);
            }
        }

        private static void CustomReadPropertiesFromElements(SerializationContext serializationContext, Interface element, XmlReader reader)
        {
            while (!serializationContext.Result.Failed && !reader.EOF && reader.NodeType == global::System.Xml.XmlNodeType.Element)
            {
                switch (reader.LocalName)
                {
                    case "inheritsIEntity":
                    if (reader.IsEmptyElement)
                    {	// No serialized value, must be default one.
                        SerializationUtilities.Skip(reader);  // Skip this tag.
                    }
                    else
                    {
                        string strValue = DONetEntityModelDesignerSerializationHelper.Instance.ReadElementContentAsString(serializationContext, element, reader);
                        InheritsIEntityMode value;
                        if (Enum.TryParse(strValue, true, out value))
                        {
                            element.InheritsIEntity = value;
                        }
                        else
                        {	// Invalid property value, ignored.
                            EntityModelDesignerSerializationBehaviorSerializationMessages.IgnoredPropertyValue(serializationContext, reader, "inheritsIEntity", typeof(InheritsIEntityMode), strValue);
                        }

                        SerializationUtilities.SkipToNextElement(reader);
                    }
                    break;
                    default:
                    return;  // Don't know this element.
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