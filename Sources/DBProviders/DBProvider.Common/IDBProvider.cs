using System;
using System.Collections.Generic;
using System.Drawing;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public interface IDBProvider : INamedObject
    {
        Guid UniqueID { get; }

        ErrorCollection LastErrors { get; }

        bool SupportsMultipleStorageEngines { get; }

        IConnectionInfoProvider ConnectionInfoProvider { get; }

        bool IsConnected { get; }
        
        Image Icon { get; }

        void ClearLastErrors();

        bool Connect(IConnectionInfo connectionInfo);

        void Disconnect();

        ServerCollection GetAllServers(LoadingMode loadingMode);

        bool Refresh(Server server, LoadingMode loadMode);

        bool Refresh(Database database, LoadingMode loadMode);

        bool Refresh(Schema schema, LoadingMode loadMode);

        bool Refresh(Table table);

        IEnumerable<StorageEngine> GetSupportedStorageEngines();
    }

    #region class StorageEngine

    public class StorageEngine
    {
        public string DisplayName { get; private set; }
        public string Key { get; private set; }
        public string Description { get; private set; }

        public StorageEngine(string key)
        {
            Key = key;
        }

        public StorageEngine(string displayName, string key, string description)
        {
            DisplayName = displayName;
            Key = key;
            Description = description;
        }

        public virtual bool EqualsTo(StorageEngine other)
        {
            return Util.StringEqual(this.Key, other.Key, true);
        }
    }

    #endregion class StorageEngine

    #region enum LoadingMode

    public enum LoadingMode
    {
        /// <summary>
        /// 
        /// </summary>
        TopLevel,
        
        /// <summary>
        ///
        /// </summary>
        RecursiveAllLevels
    }

    #endregion enum LoadingMode
}