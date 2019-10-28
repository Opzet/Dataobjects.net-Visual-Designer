namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public interface INamedObject
    {
        string Name { get; }
        string Description { get; }
    }
}