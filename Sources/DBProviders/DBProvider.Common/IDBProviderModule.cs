namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public interface IDBProviderModule
    {
        ErrorCollection LastErrors { get; }

        bool Initialize();

        void Deinitialize();

        IDBProvider[] Providers { get; }
    }
}