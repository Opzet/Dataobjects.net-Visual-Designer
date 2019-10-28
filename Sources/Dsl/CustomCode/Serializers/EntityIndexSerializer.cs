using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class EntityIndexSerializer
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

        private static void CustomWritePropertiesAsElements(SerializationContext context, EntityIndex element, XmlWriter writer)
        {
            // Unique
            if (!context.Result.Failed)
            {
                Defaultable<bool> propValue = element.Unique;
                propValue.SerializeToXml(writer, "unique");
            }

            // FillFactor
            if (!context.Result.Failed)
            {
                Defaultable<double> propValue = element.FillFactor;
                propValue.SerializeToXml(writer, "fillFactor");
            }

            // IndexName
            if (!context.Result.Failed)
            {
                Defaultable<string> indexName = element.IndexName;
                indexName.SerializeToXml(writer, "indexName");
            }

            // Fields
            if (!context.Result.Failed)
            {
                element.Fields.SerializeToXml(writer);
            }
        }

        private static void CustomReadPropertiesFromElements(SerializationContext serializationContext, EntityIndex element, XmlReader reader)
        {
            while (!serializationContext.Result.Failed && !reader.EOF && reader.NodeType == System.Xml.XmlNodeType.Element)
            {
                switch (reader.LocalName)
                {
                    case "unique":
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            Defaultable<bool> unique = new Defaultable<bool>();
                            unique.DeserializeFromXml(reader);
                            element.Unique = unique;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    case "fillFactor":
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            Defaultable<double> fillfactor = new Defaultable<double>();
                            fillfactor.DeserializeFromXml(reader);
                            element.FillFactor = fillfactor;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    case "indexName":
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            Defaultable<string> indexName = new Defaultable<string>();
                            indexName.DeserializeFromXml(reader);
                            element.IndexName = indexName;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    case "fields":
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmIndexFields fields = new OrmIndexFields();
                            fields.DeserializeFromXml(reader);
                            element.Fields = fields;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
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

        private void CustomReadPropertiesFromAttributes(SerializationContext context, ModelElement element, XmlReader reader)
        {
            DefaultReadPropertiesFromAttributes(context, element, reader);
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

        private void CustomWritePropertiesAsAttributes(SerializationContext context, ModelElement element, XmlWriter writer)
        {
            DefaultWritePropertiesAsAttributes(context, element, writer);
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
