namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive.Implementation
{
    public class ConnectionInfoProvider : IConnectionInfoProvider
    {
        public IConnectionInfo CreateInfo(StorageEngine storageEngine, string user, string password, string server, string database)
        {
            return new ConnectionInfo(storageEngine, user, password, server, database);
        }

        public IConnectionInfo Clone(IConnectionInfo source)
        {
            IConnectionInfo result = null;

            if (source is ConnectionInfo)
            {
                result = (source as ConnectionInfo).Clone();
            }

            return result;
        }
    }
}