using System;
using System.Collections.Generic;
using System.Text;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class PropertyAttributesBuilderImpl : TypeAttributesBuilderBase
    {
        private readonly IPropertyBase propertyOwner;
        
        internal PropertyAttributesBuilderImpl(IPropertyBase propertyOwner, IEnumerable<IOrmAttribute> attributes):
            base(attributes)
        {
            this.propertyOwner = propertyOwner;
            Build();
        }

        protected override void Prefilter(IOrmAttribute attribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
            switch (propertyOwner.PropertyKind)
            {
                case PropertyKind.Scalar:
                {
                    IScalarProperty scalarProperty = (IScalarProperty)propertyOwner;
                    InternalPrefilter(scalarProperty, attribute, attributeGroupItems);
                    break;
                }
                case PropertyKind.Structure:
                {
                    IStructureProperty structureProperty = (IStructureProperty)propertyOwner;
                    InternalPrefilter(structureProperty, attribute, attributeGroupItems);
                    break;
                }
                case PropertyKind.Navigation:
                {
                    INavigationProperty navigationProperty = (INavigationProperty)propertyOwner;
                    InternalPrefilter(navigationProperty, attribute, attributeGroupItems);
                    break;
                }
            }
        }

        private void InternalPrefilter(IScalarProperty property, IOrmAttribute attribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
            OrmFieldAttribute fieldAttribute = attribute as OrmFieldAttribute;
            if (fieldAttribute != null)
            {
                Prefilter(property, fieldAttribute, attributeGroupItems);
            }

            IAssociationInfo associationInfo = attribute as IAssociationInfo;
            if (associationInfo != null)
            {
                // No prefiltering at now...
            }
        }

        private void InternalPrefilter(IStructureProperty property, IOrmAttribute attribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
            OrmFieldAttribute fieldAttribute = attribute as OrmFieldAttribute;
            if (fieldAttribute != null)
            {
                Prefilter(property, fieldAttribute, attributeGroupItems);
            }
        }

        private void InternalPrefilter(INavigationProperty property, IOrmAttribute attribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
            OrmFieldAttribute fieldAttribute = attribute as OrmFieldAttribute;
            if (fieldAttribute != null)
            {
                Prefilter(property, fieldAttribute, attributeGroupItems);
            }
        }

        #region prefilter 'OrmFieldAttribute'

        private void Prefilter(IScalarProperty scalarProperty, OrmFieldAttribute fieldAttribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
            Type clrType = scalarProperty.Type.TryGetClrType(null);

            var propertyTypeIsNumber = clrType != null && clrType.IsNumber();

            // if type is number and length is set, remove 'Length'
            if (fieldAttribute.Length.IsCustom() && propertyTypeIsNumber)
            {
                attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_LENGTH);
            }

            // is scale is defined but type is not number remove 'Scale'
            if (fieldAttribute.Scale.IsCustom() && !propertyTypeIsNumber)
            {
                attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_SCALE);
            }

            // is Precision is defined but type is not number remove 'Precision'
            if (fieldAttribute.Precision.IsCustom() && !propertyTypeIsNumber)
            {
                attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_PRECISION);
            }

            // test if type is byte[] or string to use lazy load
            if (fieldAttribute.LazyLoad.IsCustom() && clrType != null)
            {
                if (!clrType.In(typeof(string), typeof(byte[])))
                {
                    attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_LAZY_LOAD);
                }
            }

            attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_NULLABLE);
        }

        private void Prefilter(IStructureProperty scalarProperty, OrmFieldAttribute fieldAttribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
            attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_NULLABLE);
        }

        private void Prefilter(INavigationProperty scalarProperty, OrmFieldAttribute fieldAttribute, Dictionary<string, Defaultable> attributeGroupItems)
        {
            attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_NULLABLE);
            attributeGroupItems.Remove(OrmFieldAttribute.ATTRIBUTE_GROUP_ITEM_LAZY_LOAD);
        }

        #endregion prefilter 'OrmFieldAttribute'

        public override bool BuildCustomFormat(OrmAttributeGroup attributeGroup, string attributeItemName,
            string valueLiteral, object realValue, ref StringBuilder attributeBuilder)
        {
            if (attributeGroup.In(OrmFieldAttribute.ATTRIBUTE_GROUP_FIELD_MAPPING, OrmFieldAttribute.ATTRIBUTE_GROUP_VERSION))
            {
                attributeBuilder.Append(valueLiteral);
                return true;
            }

            return base.BuildCustomFormat(attributeGroup, attributeItemName, valueLiteral, realValue, ref attributeBuilder);
        }

    }
}