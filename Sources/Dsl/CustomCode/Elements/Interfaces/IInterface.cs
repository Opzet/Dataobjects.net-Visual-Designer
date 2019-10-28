using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IInterface: IPersistentType
    {
        ReadOnlyCollection<IInterface> InheritedInterfaces { get; }

        ReadOnlyCollection<ITypedEntitySet> ReferencedInTypedEntitySets { get; }

        ReadOnlyCollection<IEntityIndex> Indexes { get; }

        ReadOnlyCollection<IInterface> InheritingByInterfaces { get; }

        InheritsIEntityMode InheritsIEntity { get; }

        bool InheritInterface(IInterface @interface);

        IEnumerable<IInterface> GetCurrentLevelInheritedInterfaces();

        InheritanceTree GetInheritanceTree();

        void ImplementToType(IEntityBase targetType, ImplementTypeOptions options);

        IEntityBase ImplementToNewType(PersistentTypeKind newTypeKind, string newTypeName, ImplementTypeOptions options);

        bool ContainsProperty(PropertyKind propertyKind, string propertyName);

        bool ContainsIndex(IEntityIndex other);
    }

    [Flags]
    public enum ImplementTypeOptions
    {
        Default = 0,
        CopyInheritedInterfaces = 1
    }
}