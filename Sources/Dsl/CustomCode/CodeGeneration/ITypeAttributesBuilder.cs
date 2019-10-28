using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface ITypeAttributesBuilder 
    {
        OrmAttributeGroup[] GetAttributeGroups(IOrmAttribute attribute);

        Dictionary<string, Defaultable> GetAttributeGroupItems(IOrmAttribute attribute, OrmAttributeGroup attributeGroup);

        bool BuildCustomFormat(OrmAttributeGroup attributeGroup, string attributeItemName, string valueLiteral,
            object realValue, ref StringBuilder attributeBuilder);

        bool BuildCustomFormatComplete(OrmAttributeGroup attributeGroup,
            Dictionary<string, Defaultable> attributeGroupItems,
            ref StringBuilder attributeBuilder);

        IEnumerable<KeyValuePair<string, Defaultable>> SortAttributeGroupItems(OrmAttributeGroup attributeGroup,
            Dictionary<string, Defaultable> attributeGroupItems);
    }

    public static class TypeAttributesBuilder
    {
        public static ITypeAttributesBuilder Create(IPropertyBase propertyOwner, IEnumerable<IOrmAttribute> attributes)
        {
            return new PropertyAttributesBuilderImpl(propertyOwner, attributes);
        }

        public static ITypeAttributesBuilder Create(IPersistentType persistentType, IEnumerable<IOrmAttribute> attributes)
        {
            return new PeristentTypeAttributesBuilderImpl(persistentType, attributes);
        }

        public static ITypeAttributesBuilder CreateCommon(IEnumerable<IOrmAttribute> attributes)
        {
            return new CommonAttributesBuilderImpl(attributes);
        }
    }
}