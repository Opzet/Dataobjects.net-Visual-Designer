using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class PersistentTypeSerializer
    {
        private void CustomConstructor()
        {
            this.DefaultConstructor();
        }

        private static void CustomWritePropertiesAsElements(SerializationContext context, PersistentType element, XmlWriter writer)
        {
            if (!context.Result.Failed)
            {
                element.DataContract.SerializeToXml(writer, "dataContract");
            }
        }

        private static void CustomReadPropertiesFromElements(SerializationContext serializationContext, PersistentType element, XmlReader reader)
        {
            while (!serializationContext.Result.Failed && !reader.EOF && reader.NodeType == global::System.Xml.XmlNodeType.Element)
            {
                switch (reader.LocalName)
                {
                    case "dataContract":
                    if (reader.IsEmptyElement)
                    {	// No serialized value, must be default one.
                        SerializationUtilities.Skip(reader);  // Skip this tag.
                    }
                    else
                    {
                        DataContractDescriptor dataContractDescriptor = new DataContractDescriptor();
                        dataContractDescriptor.DeserializeFromXml(reader);
                        element.DataContract = dataContractDescriptor;

                        SerializationUtilities.SkipToNextElement(reader);
                        reader.SkipToNextElementFix();
                    }
                    break;
                    default:
                    return;  // Don't know this element.
                }
            }
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

        private void CustomWritePropertiesAsAttributes(SerializationContext serializationContext, ModelElement element, XmlWriter writer)
        {
            DefaultWritePropertiesAsAttributes(serializationContext, element, writer);
        }

        private void CustomReadPropertiesFromAttributes(SerializationContext serializationContext, ModelElement element, XmlReader reader)
        {
            DefaultReadPropertiesFromAttributes(serializationContext, element, reader);
        }
    }
}