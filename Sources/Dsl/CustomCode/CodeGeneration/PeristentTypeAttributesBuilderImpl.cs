using System.Collections.Generic;
using System.Text;
using TXSoftware.DataObjectsNetEntityModel.Common;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class CommonAttributesBuilderImpl : TypeAttributesBuilderBase
    {
        public CommonAttributesBuilderImpl(IEnumerable<IOrmAttribute> attributes) : base(attributes)
        {
            Build();
        }
    }

    internal class PeristentTypeAttributesBuilderImpl: TypeAttributesBuilderBase
    {
        private IPersistentType persistentType;

        internal PeristentTypeAttributesBuilderImpl(IPersistentType persistentType, IEnumerable<IOrmAttribute> attributes) : base(attributes)
        {
            this.persistentType = persistentType;
            Build();
        }

        public override bool BuildCustomFormat(OrmAttributeGroup attributeGroup, string attributeItemName,
            string valueLiteral, object realValue, ref StringBuilder attributeBuilder)
        {
            if (attributeGroup.In(OrmHierarchyRootAttribute.ATTRIBUTE_GROUP_TABLE_MAPPING))
            {
                attributeBuilder.Append(valueLiteral);
                return true;
            }

            if (attributeGroup == OrmTypeDiscriminatorValueAttribute.ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR_VALUE)
            {
                if (attributeItemName == OrmTypeDiscriminatorValueAttribute.ATTRIBUTE_GROUP_ITEM_VALUE)
                {
                    attributeBuilder.Append(valueLiteral + ",");
                    return true;
                }
            }

            if (attributeGroup == OrmKeyGeneratorAttribute.ATTRIBUTE_GROUP_KEY_GENERATOR)
            {
                if (attributeItemName == OrmKeyGeneratorAttribute.ATTRIBUTE_GROUP_ITEM_KIND)
                {
                    attributeBuilder.Append(valueLiteral);
                    return true;
                }
            }

            if (attributeGroup == EntityIndex.ATTRIBUTE_GROUP_INDEX)
            {
                if (attributeItemName == OrmIndexFields.ATTRIBUTE_GROUP_ITEM_KEY_FIELDS)
                {
                    string[] keyFields = (string[]) realValue;
                    if (keyFields != null && keyFields.Length > 0)
                    {
                        //attributeBuilder.Append(string.Format("\"{0}\"", keyFieldItems[0]));
                        string allKeyFields = Util.JoinCollection(keyFields, ",", "\"");
                        attributeBuilder.Append(allKeyFields + ",");
                        return true;
                    }
                }
            }

            return base.BuildCustomFormat(attributeGroup, attributeItemName, valueLiteral, realValue, ref attributeBuilder);
        }

        public override IEnumerable<KeyValuePair<string, Defaultable>> SortAttributeGroupItems(
            OrmAttributeGroup attributeGroup, Dictionary<string, Defaultable> attributeGroupItems)
        {
            IComparer<string> customComparer = null;

            if (attributeGroup == EntityIndex.ATTRIBUTE_GROUP_INDEX)
            {
                customComparer = new IndexKeyFieldsComparer();
            }

            if (attributeGroup == OrmTypeDiscriminatorValueAttribute.ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR_VALUE)
            {
                customComparer = new TypeDiscriminatorValueFieldsComparer();
            }

            if (customComparer != null)
            {
                return attributeGroupItems.ToList().OrderBy(pair => pair.Key, customComparer);
            }

            return base.SortAttributeGroupItems(attributeGroup, attributeGroupItems);
        }

        #region class IndexKeyFieldsComparer

        internal class IndexKeyFieldsComparer : IComparer<string>
        {
            public int Compare(string itemA, string itemB)
            {
                if (itemA == OrmIndexFields.ATTRIBUTE_GROUP_ITEM_KEY_FIELDS)
                {
                    return 1;
                }

                if (itemB == OrmIndexFields.ATTRIBUTE_GROUP_ITEM_KEY_FIELDS)
                {
                    return 1;
                }

                return 0;
            }
        }

        #endregion class IndexKeyFieldsComparer

        #region class TypeDiscriminatorValueFieldsComparer

        internal class TypeDiscriminatorValueFieldsComparer : IComparer<string>
        {
            public int Compare(string itemA, string itemB)
            {
                if (itemA == OrmTypeDiscriminatorValueAttribute.ATTRIBUTE_GROUP_ITEM_VALUE)
                {
                    return 1;
                }

                if (itemB == OrmTypeDiscriminatorValueAttribute.ATTRIBUTE_GROUP_ITEM_VALUE)
                {
                    return 1;
                }

                return 0;
            }
        }

        #endregion class TypeDiscriminatorValueFieldsComparer
    }
}