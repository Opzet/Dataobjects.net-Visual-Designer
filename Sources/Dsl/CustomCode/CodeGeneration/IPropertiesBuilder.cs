using System.Collections.Generic;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IPropertiesBuilder 
    {
        IEnumerable<IOrmAttribute> MergedTypeAttributes { get; }
        IPropertyBase GetProperty(IPropertyBase sourceProperty, InheritanceMember inheritanceMember);
        IEnumerable<IPropertyBase> GetInheritedProperties();
        IOrmAttribute[] GetPropertyTypeAttributes(IPropertyBase sourceProperty);
    }

    public enum InheritanceMember
    {
        Current,
        Inherited
    }
}