using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class EntitySerializer
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

        private static void CustomWritePropertiesAsElements(SerializationContext context, Entity element, XmlWriter writer)
        {
            // HierarchyRootAttribute
            if (!context.Result.Failed)
            {
                element.HierarchyRootAttribute.SerializeToXml(writer, "hierarchyRoot");
            }

            // KeyGenerator
            if (!context.Result.Failed)
            {
                element.KeyGenerator.SerializeToXml(writer, "keyGenerator");
            }

            // TypeDiscriminatorValue
            if (!context.Result.Failed)
            {
                element.TypeDiscriminatorValue.SerializeToXml(writer, "typeDiscriminatorValue");
            }
        }

        private static void CustomReadPropertiesFromElements(SerializationContext serializationContext, Entity element, XmlReader reader)
        {
            while (!serializationContext.Result.Failed && !reader.EOF && reader.NodeType == global::System.Xml.XmlNodeType.Element)
            {
                switch (reader.LocalName)
                {
                    case "hierarchyRoot":
                    {
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmHierarchyRootAttribute hierarchyRootAttribute = new OrmHierarchyRootAttribute();
                            hierarchyRootAttribute.DeserializeFromXml(reader, "hierarchyRoot");
                            element.HierarchyRootAttribute = hierarchyRootAttribute;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    }
                    case "keyGenerator":
                    {
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmKeyGeneratorAttribute keyGeneratorAttr = new OrmKeyGeneratorAttribute();
                            keyGeneratorAttr.DeserializeFromXml(reader, "keyGenerator");
                            element.KeyGenerator = keyGeneratorAttr;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    }
                    case "typeDiscriminatorValue":
                    {
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmTypeDiscriminatorValueAttribute typeDiscriminatorValue = new OrmTypeDiscriminatorValueAttribute();
                            typeDiscriminatorValue.DeserializeFromXml(reader);
                            element.TypeDiscriminatorValue = typeDiscriminatorValue;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    }

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