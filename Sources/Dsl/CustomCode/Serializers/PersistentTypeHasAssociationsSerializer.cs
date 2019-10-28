using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class PersistentTypeHasAssociationsSerializer
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

        private static void CustomWritePropertiesAsElements(SerializationContext context, PersistentTypeHasAssociations element, XmlWriter writer)
        {
            // End1
            if (!context.Result.Failed)
            {
                element.End1.SerializeToXml(writer, "end1");
            }

            // End2
            if (!context.Result.Failed)
            {
                element.End2.SerializeToXml(writer, "end2");
            }
        }

        private static void CustomReadPropertiesFromElements(SerializationContext serializationContext, PersistentTypeHasAssociations element, XmlReader reader)
        {
            Action useAssocAttrUpgradeFunc =
                () =>
                {
                    // this is here when converting from older versions than 1.0.5.0 to set 'UseAssociationAttribute' to true
                    if (ModelUpgrader.Instance.DeserializingModelVersion <
                        ModelUpgrader.Version_1_0_5_0)
                    {
                        ModelUpgrader.Instance.UpdateMakeChangesFlag();
                    }
                };

            while (!serializationContext.Result.Failed && !reader.EOF && reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.LocalName)
                {
                    case "end1":
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmAssociationEnd end1 = new OrmAssociationEnd(element, "End1");
                            end1.DeserializeFromXml(reader, "end1");
                            element.End1 = end1;

                            useAssocAttrUpgradeFunc();

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    case "end2":
                        if (reader.IsEmptyElement)
                        {	// No serialized value, must be default one.
                            SerializationUtilities.Skip(reader);  // Skip this tag.
                        }
                        else
                        {
                            OrmAssociationEnd end2 = new OrmAssociationEnd(element, "End2");
                            end2.DeserializeFromXml(reader, "end2");
                            element.End2 = end2;

                            useAssocAttrUpgradeFunc();

                            SerializationUtilities.SkipToNextElement(reader);
                            reader.SkipToNextElementFix();
                        }
                        break;
                    default:
                        return;  // Don't know this element.
                }
            }
        }

        private void CustomReadPropertiesFromAttributes(SerializationContext serializationContext, ModelElement element, global::System.Xml.XmlReader reader)
        {
            DefaultReadPropertiesFromAttributes(serializationContext, element, reader);
        }

        private void CustomReadTargetRolePlayer(SerializationContext serializationContext, ModelElement element, global::System.Xml.XmlReader reader)
        {
            DefaultReadTargetRolePlayer(serializationContext, element, reader);
        }

        private void CustomWritePropertiesAsAttributes(SerializationContext serializationContext, ModelElement element, global::System.Xml.XmlWriter writer)
        {
            DefaultWritePropertiesAsAttributes(serializationContext, element, writer);
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