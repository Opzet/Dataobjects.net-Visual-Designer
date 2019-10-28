using System.Collections.ObjectModel;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IEntityBase: IInterface
    {
        InheritanceModifiers InheritanceModifier { get; }

        IEntityBase BaseType { get; }

        ReadOnlyCollection<IEntityBase> ReferencesAsBaseType { get; }

        ReadOnlyCollection<IEntityBase> GetBaseTypesGraph(InheritanceGraphDirection graphDirection);
    }

    public enum InheritanceGraphDirection
    {
        TypeToRoot,
        RootToType
    }
}