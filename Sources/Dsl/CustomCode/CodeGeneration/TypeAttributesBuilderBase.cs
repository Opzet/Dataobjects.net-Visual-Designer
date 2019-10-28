using System;
using System.Collections.Generic;
using System.Text;
using TXSoftware.DataObjectsNetEntityModel.Common;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal abstract class TypeAttributesBuilderBase : ITypeAttributesBuilder
    {
        #region class AttributeGroupItemCollection

        internal class AttributeGroupItemCollection : Dictionary<OrmAttributeGroup, Dictionary<string, Defaultable>>
        {
            internal bool ContainsGroup(OrmAttributeGroup @group)
            {
                return this.Keys.Any(item => item.EqualsTo(@group));
            }
        }

        #endregion class AttributeGroupItemCollection

        #region class AttrData

        internal class AttrData
        {
            internal OrmAttributeGroup[] AttributeGroups { get; set; }

            // main key - groupname
            internal AttributeGroupItemCollection AttributeGroupItems { get; private set; }

            public AttrData(OrmAttributeGroup[] attributeGroups)
            {
                AttributeGroups = attributeGroups;
                this.AttributeGroupItems = new AttributeGroupItemCollection();
                foreach (var attributeGroup in attributeGroups)
                {
                    this.AttributeGroupItems.Add(attributeGroup, new Dictionary<string, Defaultable>());
                }
            }
        }

        #endregion class AttrData

        private readonly Dictionary<IOrmAttribute, AttrData> attributeItems = new Dictionary<IOrmAttribute, AttrData>();

        internal TypeAttributesBuilderBase(IEnumerable<IOrmAttribute> attributes)
        {
            foreach (IOrmAttribute attribute in attributes)
            {
                attributeItems.Add(attribute,
                    new AttrData(attribute.GetAttributeGroups(AttributeGroupsListMode.Filtered)));
            }
        }

        public OrmAttributeGroup[] GetAttributeGroups(IOrmAttribute attribute)
        {
            if (attributeItems.ContainsKey(attribute))
            {
                return attributeItems[attribute].AttributeGroups;
            }

            return new OrmAttributeGroup[0];
        }

        public Dictionary<string, Defaultable> GetAttributeGroupItems(IOrmAttribute attribute,
            OrmAttributeGroup attributeGroup)
        {
            if (attributeItems.ContainsKey(attribute))
            {
                var attributeGroupItems = attributeItems[attribute].AttributeGroupItems;
                if (attributeGroupItems.ContainsGroup(attributeGroup))
                {
                    return attributeGroupItems[attributeGroup];
                }
            }

            return new Dictionary<string, Defaultable>();
        }

        public virtual bool BuildCustomFormat(OrmAttributeGroup attributeGroup, string attributeItemName,
            string valueLiteral, object realValue, ref StringBuilder attributeBuilder)
        {
            return false;
        }

        public virtual bool BuildCustomFormatComplete(OrmAttributeGroup attributeGroup, Dictionary<string, Defaultable> attributeGroupItems,
            ref StringBuilder attributeBuilder)
        {
            return false;
        }

        public virtual IEnumerable<KeyValuePair<string, Defaultable>> SortAttributeGroupItems(
            OrmAttributeGroup attributeGroup, Dictionary<string, Defaultable> attributeGroupItems)
        {
            return attributeGroupItems.ToList();
        }

        protected void Build()
        {
            foreach (var pair in attributeItems)
            {
                IOrmAttribute attribute = pair.Key;
                AttrData attributeData = pair.Value;

                foreach (var attributeGroup in attributeData.AttributeGroups)
                {
                    Dictionary<string, Defaultable> attributeGroupItems =
                        attribute.GetAttributeGroupItems(attributeGroup);

                    Prefilter(attribute, attributeGroupItems);

                    attributeData.AttributeGroupItems[attributeGroup] = attributeGroupItems;
                }
            }
        }

        protected virtual void Prefilter(IOrmAttribute attribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
        }
    }
}