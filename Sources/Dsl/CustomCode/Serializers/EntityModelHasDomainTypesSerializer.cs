/*using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class EntityModelHasDomainTypesSerializer
    {
        private string CustomXmlTagName
        {
            get { return DefaultXmlTagName; }
        }

        private void CustomWritePropertiesAsAttributes(SerializationContext serializationContext, ModelElement element, System.Xml.XmlWriter writer)
        {
            DefaultWritePropertiesAsAttributes(serializationContext, element, writer);
        }

        private void CustomWriteElements(SerializationContext serializationContext, ModelElement element, System.Xml.XmlWriter writer)
        {
            DefaultWriteElements(serializationContext, element, writer);
        }

        private string CustomCalculateQualifiedName(DomainXmlSerializerDirectory directory, ModelElement element)
        {
            return DefaultCalculateQualifiedName(directory, element);
        }

        private string CustomGetMonikerQualifier(DomainXmlSerializerDirectory directory, ModelElement element)
        {
            return DefaultGetMonikerQualifier(directory, element);
        }

        private void CustomConstructor()
        {
            DefaultConstructor();
        }

        private void CustomRead(SerializationContext serializationContext, ModelElement element, System.Xml.XmlReader reader)
        {
            DefaultRead(serializationContext, element, reader);
        }

        private void CustomReadTargetRolePlayer(SerializationContext serializationContext, ModelElement element, System.Xml.XmlReader reader)
        {
            DefaultReadTargetRolePlayer(serializationContext, element, reader);
        }

        private void CustomReadPropertiesFromAttributes(SerializationContext serializationContext, ModelElement element, System.Xml.XmlReader reader)
        {
            DefaultReadPropertiesFromAttributes(serializationContext, element, reader);
        }

        private void CustomReadElements(SerializationContext serializationContext, ModelElement element, System.Xml.XmlReader reader)
        {
            DefaultReadElements(serializationContext, element, reader);
        }

        private ModelElement CustomTryCreateInstance(SerializationContext serializationContext, System.Xml.XmlReader reader, Partition partition)
        {
            return DefaultTryCreateInstance(serializationContext, reader, partition);
        }

        private ModelElement CustomCreateInstance(SerializationContext serializationContext, System.Xml.XmlReader reader, Partition partition)
        {
            return DefaultTryCreateInstance(serializationContext, reader, partition);
        }

        private Moniker CustomTryCreateMonikerInstance (SerializationContext serializationContext, System.Xml.XmlReader reader, ModelElement sourceRolePlayer, System.Guid relDomainClassId, Partition partition)
        {
            return DefaultTryCreateMonikerInstance(serializationContext, reader, sourceRolePlayer, relDomainClassId,
                partition);
        }

        private Moniker CustomCreateMonikerInstance (SerializationContext serializationContext, System.Xml.XmlReader reader, ModelElement sourceRolePlayer, System.Guid relDomainClassId, Partition partition)
        {
            return DefaultCreateMonikerInstance(serializationContext, reader, sourceRolePlayer, relDomainClassId, partition);
        }

        private void CustomWrite(SerializationContext serializationContext, ModelElement element, System.Xml.XmlWriter writer, RootElementSettings rootElementSettings)
        {
            DefaultWrite(serializationContext, element, writer, rootElementSettings);
        }
    }
}*/