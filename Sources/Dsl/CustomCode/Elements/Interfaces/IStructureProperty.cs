namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IStructureProperty: IPropertyBase
    {
        IStructure TypeOf { get; }
    }
}