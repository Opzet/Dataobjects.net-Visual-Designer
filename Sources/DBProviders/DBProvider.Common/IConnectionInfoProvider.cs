using System;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public interface IConnectionInfoProvider
    {
        IConnectionInfo CreateInfo(StorageEngine storageEngine, string user, string password, string server, string database);

        IConnectionInfo Clone(IConnectionInfo source);

        //        IConnectionInfo BuildConnectionInfo(string connectionString);
        //
        //        string BuildConnectionString(IConnectionInfo connectionInfo);
    }

    public interface IConnectionInfo
    {
        string ServerName { get; set; }

        string DatabaseName { get; set; }

        AuthenticationType AuthenticationType { get; set; }

        string UserID { get; set; }

        string Password { get; set; }

        IConnectionInfo Clone();

        bool EqualsTo(IConnectionInfo other);
    }

    [Serializable]
    public enum AuthenticationType
    {
        Windows = 0,
        SqlServer
    }
}