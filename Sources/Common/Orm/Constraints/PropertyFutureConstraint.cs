using System;
using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    public sealed class PropertyFutureConstraint : PropertyConstraint
    {
        public const string DISPLAY_NAME = "Future";
        public const string DESCRIPTION = "Ensures that date value is in the future";
        public readonly OrmAttributeGroup ATTRIBUTE_NAME = new OrmAttributeGroup("FutureConstraint", OrmUtils.GetOrmNamespace(OrmNamespace.OrmValidation));

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
            return PropertyConstrainType.Future;
        }

        protected internal override OrmAttributeGroup GetAttributeGroup()
        {
            return ATTRIBUTE_NAME;
        }

//        protected override void FilterAttributeGroups(List<string> attributeGroupsToFilter)
//        {}

        protected override void FillAttributeGroupItems(Dictionary<string, Defaultable> attributeGroupItems)
        {
        }
    }
}