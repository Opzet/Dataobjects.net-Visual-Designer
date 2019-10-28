using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IEntity: IEntityBase
    {
        OrmHierarchyRootAttribute HierarchyRoot { get; }

        OrmKeyGeneratorAttribute KeyGenerator { get; }

        OrmTypeDiscriminatorValueAttribute TypeDiscriminatorValue { get; }
    }
}