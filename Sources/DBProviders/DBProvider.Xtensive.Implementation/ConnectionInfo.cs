using TXSoftware.DataObjectsNetEntityModel.Common;
using Xtensive.Core;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive.Implementation
{
    public class ConnectionInfo : IConnectionInfo
    {
        private global::Xtensive.Core.ConnectionInfo internalInfo;
        private StorageEngine storageEngine;
        private AuthenticationType authenticationType;
        private string serverName;
        private string databaseName;
        private string userID;
        private string password;

        internal global::Xtensive.Core.ConnectionInfo InternalInfo
        {
            get { return this.internalInfo; }
        }

        internal StorageEngine StorageEngine
        {
            get { return this.storageEngine; }
        }

        internal ConnectionInfo(StorageEngine storageEngine, string user, string password, string server, string database)
        {
            this.storageEngine = storageEngine;
            this.serverName = server;
            this.databaseName = database;
            this.userID = user;
            this.password = password;
            BuildInfo(this.UserID, this.Password, this.ServerName, this.DatabaseName);
        }

        public string ServerName
        {
            get { return serverName; }
            set
            {
                if (value != serverName)
                {
                    serverName = value;
                    BuildInfo(this.UserID, this.Password, value, DatabaseName);
                }

            }
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set
            {
                if (value != databaseName)
                {
                    databaseName = value;
                    BuildInfo(this.UserID, this.Password, this.ServerName, value);
                }
            }
        }

        public AuthenticationType AuthenticationType
        {
            get { return authenticationType; }
            set
            {
                if (authenticationType != value)
                {
                    authenticationType = value;
                    BuildInfo(this.UserID, this.Password, this.ServerName, this.DatabaseName);
                }
            }
        }

        public string UserID
        {
            get { return userID; }
            set
            {
                if (value != userID)
                {
                    userID = value;

                    this.authenticationType = string.IsNullOrEmpty(value) ? AuthenticationType.Windows : AuthenticationType.SqlServer;
                    BuildInfo(this.storageEngine, value, this.Password, this.ServerName, this.DatabaseName);
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    BuildInfo(this.UserID, value, this.ServerName, this.DatabaseName);
                }
            }
        }

        private void BuildInfo(string user, string password, string server, string database)
        {
            BuildInfo(this.storageEngine, user, password, server, database);
        }

        private void BuildInfo(StorageEngine storageEngine, string user, string password, string server, string database) 
        {
            this.storageEngine = storageEngine;
            if (!string.IsNullOrEmpty(server) && !string.IsNullOrEmpty(database))
            {
                //string protocol = DBProvider.ProtocolMappings[storageEngine].Item1;
                //string protocol = DBProvider.GetProtocolMapping(storageEngine).Item1;
                string protocol = storageEngine.Key;
                bool userIsSpecified = !string.IsNullOrEmpty(user);
                bool passIsSpecified = !string.IsNullOrEmpty(password);
                authenticationType = userIsSpecified ? AuthenticationType.SqlServer : AuthenticationType.Windows;
                string url = string.Format("{0}://{1}{2}{3}{4}{5}/{6}",
                    protocol,
                    user,
                    passIsSpecified ? ":" : string.Empty,
                    userIsSpecified ? password : string.Empty,
                    userIsSpecified ? "@" : string.Empty,
                    server,
                    database);
                UrlInfo urlInfo = UrlInfo.Parse(url);

                this.internalInfo = new global::Xtensive.Core.ConnectionInfo(urlInfo);
            }
            else
            {
                this.internalInfo = null;
            }
        }

        public IConnectionInfo Clone()
        {
            ConnectionInfo cloned = new ConnectionInfo(this.storageEngine, this.UserID, this.Password, this.ServerName, this.DatabaseName);
            cloned.authenticationType = this.authenticationType;
            cloned.serverName = this.serverName;
            cloned.databaseName = this.databaseName;
            cloned.userID = this.userID;
            cloned.password = this.password;
            cloned.internalInfo = this.internalInfo != null ? new global::Xtensive.Core.ConnectionInfo(this.internalInfo.ConnectionUrl) : null;
            return cloned;
        }

        public bool EqualsTo(IConnectionInfo other)
        {
            bool result = other is ConnectionInfo;
            if (result)
            {
                ConnectionInfo otherCi = (ConnectionInfo) other;
                result = this.storageEngine.EqualsTo(otherCi.storageEngine) &&
                    this.AuthenticationType == otherCi.AuthenticationType &&
                        Util.StringEqual(this.ServerName, otherCi.ServerName, true) &&
                            Util.StringEqual(this.DatabaseName, otherCi.DatabaseName, true) &&
                                Util.StringEqual(this.UserID, otherCi.UserID, true) &&
                                    Util.StringEqual(this.Password, otherCi.Password, true);
            }

            return result;
        }
    }
}