using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface INavigationProperty: IPropertyBase
    {
        MultiplicityKind Multiplicity { get; }

        string PairFrom { get; }

        string PairTo { get; }

        IPersistentType OwnerPersistentType { get; }

        IPersistentTypeHasAssociations PersistentTypeHasAssociations { get; }

        ITypedEntitySet TypedEntitySet { get; }
        OrmKeyAttribute KeyAttribute { get; }
    }
}