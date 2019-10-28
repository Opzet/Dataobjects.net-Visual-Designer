using System;
using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    public sealed class PropertyEmailConstraint : PropertyConstraint
    {
        public const string DISPLAY_NAME = "Email";
        public const string DESCRIPTION = "Ensures that email address is in correct format";
        public readonly OrmAttributeGroup ATTRIBUTE_NAME = new OrmAttributeGroup("EmailConstraint", OrmUtils.GetOrmNamespace(OrmNamespace.OrmValidation));

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
            return PropertyConstrainType.Email;
        }

        protected internal override OrmAttributeGroup GetAttributeGroup()
        {
            return ATTRIBUTE_NAME;
        }

//        protected override void FilterAttributeGroups(List<string> attributeGroupsToFilter)
//        {}

        protected override void FillAttributeGroupItems(Dictionary<string, Defaultable> attributeGroupItems)
        {}
    }
}