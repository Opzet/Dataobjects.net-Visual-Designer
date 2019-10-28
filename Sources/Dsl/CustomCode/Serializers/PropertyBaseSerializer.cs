using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class PropertyBaseSerializer
    {
        private void CustomConstructor()
        {
            this.DefaultConstructor();
        }

        private static void CustomWritePropertiesAsElements(SerializationContext context, PropertyBase element, XmlWriter writer)
        {
            if (!context.Result.Failed)
            {
                element.FieldAttribute.SerializeToXml(writer);
            }

            if (!context.Result.Failed)
            {
                element.Constraints.SerializeToXml(writer, "constraints");
            }

            if (!context.Result.Failed)
            {
                element.DataMember.SerializeToXml(writer, "dataMember");
            }

            if (!context.Result.Failed)
            {
                element.PropertyAccess.SerializeToXml(writer, "propertyAccess");
            }
        }

        private static void CustomReadPropertiesFromElements(SerializationContext serializationContext, PropertyBase element, XmlReader reader)
        {
            while (!serializationContext.Result.Failed && !reader.EOF && reader.NodeType == global::System.Xml.XmlNodeType.Element)
            {
                switch (reader.LocalName)
                {
                    case "field":	// field
                    {
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmFieldAttribute fieldAttribute = new OrmFieldAttribute();
                            fieldAttribute.DeserializeFromXml(reader);
                            element.FieldAttribute = fieldAttribute;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    }
                    case "constraints":	// field
                    {
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmPropertyConstraints constraints = new OrmPropertyConstraints();
                            constraints.DeserializeFromXml(reader);
                            element.Constraints = constraints;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    }
                    case "dataMember":	// field
                    {
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            DataMemberDescriptor dataMemberDescriptor = new DataMemberDescriptor();
                            dataMemberDescriptor.DeserializeFromXml(reader);
                            element.DataMember = dataMemberDescriptor;

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    }
                    case "propertyAccess":	// field
                    {
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            PropertyAccessModifiers modifiers = new PropertyAccessModifiers();
                            modifiers.DeserializeFromXml(reader, "propertyAccess");
                            element.PropertyAccess = modifiers;

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

        private void CustomWritePropertiesAsAttributes(SerializationContext serializationContext, ModelElement element, global::System.Xml.XmlWriter writer)
        {
            DefaultWritePropertiesAsAttributes(serializationContext, element, writer);
        }

        private void CustomReadPropertiesFromAttributes(SerializationContext serializationContext, ModelElement element, global::System.Xml.XmlReader reader)
        {
            DefaultReadPropertiesFromAttributes(serializationContext, element, reader);
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
    }
}