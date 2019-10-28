using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IPropertyBase : IElement
    {
        IPersistentType Owner { get; }

        PropertyKind PropertyKind { get; }

        OrmFieldAttribute FieldAttribute { get; }

        OrmPropertyConstraints Constraints { get; }

        IOrmAttribute[] TypeAttributes { get; }

        DataMemberDescriptor DataMember { get; }

        PropertyAccessModifiers PropertyAccess { get; }

        bool IsInherited { get; }

        bool IsImplementedBy(IInterface @interface);

        IPersistentType GetRealOwner();
    }
}