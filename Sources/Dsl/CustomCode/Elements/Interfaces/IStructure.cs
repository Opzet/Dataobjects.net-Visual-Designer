using System.Collections.ObjectModel;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IStructure: IEntityBase
    {
        ReadOnlyCollection<IStructureProperty> ReferencedInProperties { get; }
    }
}