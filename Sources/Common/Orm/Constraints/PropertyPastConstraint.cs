using System;
using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    public sealed class PropertyPastConstraint : PropertyConstraint
    {
        public const string DISPLAY_NAME = "Past";
        public const string DESCRIPTION = "Ensures that date value is in the past";
        public readonly OrmAttributeGroup ATTRIBUTE_NAME = new OrmAttributeGroup("PastConstraint", OrmUtils.GetOrmNamespace(OrmNamespace.OrmValidation));

        protected override void FillPropertyNamesToHideOnDefault(List<string> propertiesToHide)
        {}

        protected override void InternalDeserializeFromXml(XmlProxy content)
        {}

        protected override void InternalSerializeToXml(XmlProxy content)
        {}

        protected override string GetDisplayName()
        {
            return DISPLAY_NAME;
        }

        protected override string GetDescription()
        {
            return DESCRIPTION;
        }

        protected override PropertyConstrainType GetConstrainType()
        {
            return PropertyConstrainType.Past;
        }

        protected internal override OrmAttributeGroup GetAttributeGroup()
        {
            return ATTRIBUTE_NAME;
        }

        protected override void FillAttributeGroupItems(Dictionary<string, Defaultable> attributeGroupItems)
        {
        }
    }
}