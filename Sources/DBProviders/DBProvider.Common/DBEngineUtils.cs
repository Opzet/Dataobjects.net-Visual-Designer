using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public static class DBEngineUtils
    {
        public static List<string> GetSystemDBProviders()
        {
            List<string> result = new List<string>();

            DataTable factoryClasses = DbProviderFactories.GetFactoryClasses();
            foreach (DataRow dataRow in factoryClasses.Rows)
            {
                object o = dataRow["InvariantName"];
                if (o != null)
                {
                    string name = (string)o;
                    if (!string.IsNullOrEmpty(name))
                    {
                        result.Add(name);
                    }
                }
            }

            return result;
        }

        public static List<string> GetDatabases(string providerInvariantName, string dataSource, string user, string password)
        {
            List<string> result = new List<string>();

            try
            {
                DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
                var connectionStringBuilder = CreateConnectionString(providerFactory, dataSource, null, user, password);

                DbConnection connection = providerFactory.CreateConnection();
                connection.ConnectionString = connectionStringBuilder.ConnectionString;
                connection.Open();

                var databases = connection.GetSchema("Databases");
                foreach (DataRow row in databases.Rows)
                {
                    result.Add(row["database_name"] as string);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return result;
        }

        public static List<string> GetTables(string providerInvariantName, string dataSource, string database, string user, string password)
        {
            List<string> result = new List<string>();

            try
            {
                DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
                var connectionStringBuilder = CreateConnectionString(providerFactory, dataSource, database, user, password);

                DbConnection connection = providerFactory.CreateConnection();
                connection.ConnectionString = connectionStringBuilder.ConnectionString;

                connection.Open();

                string[] restrictions = new string[4];

                // Catalog
                restrictions[0] = database;

                // Owner
                restrictions[1] = null; // "dbo";

                // Table - We want all, so null
                restrictions[2] = null;

                // Table Type - Only tables and not views
                restrictions[3] = "BASE TABLE";

                var tables = connection.GetSchema("Tables", restrictions);

                foreach (DataRow row in tables.Rows)
                {
                    result.Add(row["TABLE_NAME"] as string);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message); Console.WriteLine(e);
            }

            return result;
        }

        public static List<string> GetTableSchemes(string providerInvariantName, string dataSource, string database, string user, string password)
        {
            // key = table name
            // value = table schema name
            List<string> result = new List<string>();

            var tables = GetTables(providerInvariantName, dataSource, database, null, user, password);
            foreach (DataRow row in tables.Rows)
            {
                string tableSchema = row["TABLE_SCHEMA"] as string;
                if (!result.Contains(tableSchema))
                {
                    result.Add(tableSchema);
                }
            }

            return result;
        }

        public static DataTable GetTables(string providerInvariantName, string dataSource, string database, string table, string user, string password)
        {
            DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
            var connectionStringBuilder = CreateConnectionString(providerFactory, dataSource, database, user, password);

            DbConnection connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionStringBuilder.ConnectionString;

            connection.Open();

            string[] restrictions = new string[4];

            // Catalog
            restrictions[0] = database;

            // Owner
            restrictions[1] = null; // "dbo";

            // Table - We want all, so null
            restrictions[2] = table;

            // Table Type - Only tables and not views
            restrictions[3] = "BASE TABLE";

            var tables = connection.GetSchema("Tables", restrictions);

            return tables;
        }

        private static DbConnectionStringBuilder CreateConnectionString(DbProviderFactory providerFactory,
            string dataSource, string database, string user, string password)
        {
            DbConnectionStringBuilder connectionStringBuilder = providerFactory.CreateConnectionStringBuilder();
            connectionStringBuilder = providerFactory.CreateConnectionStringBuilder();
            connectionStringBuilder["Data Source"] = dataSource;
            if (!string.IsNullOrEmpty(database))
            {
                connectionStringBuilder["Initial Catalog"] = database;
            }

            if (string.IsNullOrEmpty(user))
            {
                connectionStringBuilder["Integrated Security"] = "SSPI";
            }
            else
            {
                connectionStringBuilder["User ID"] = user;
                connectionStringBuilder["Password"] = password;
            }
            return connectionStringBuilder;
        }

        public static List<DataSourceInfo> GetDataSources(string providerInvariantName)
        {
            List<DataSourceInfo> result = new List<DataSourceInfo>();

            DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
            DbDataSourceEnumerator dsEnum = providerFactory.CreateDataSourceEnumerator();
            if (dsEnum != null)
            {
                DataTable servers = dsEnum.GetDataSources();
                foreach (DataRow row in servers.Rows)
                {
                    var item = new DataSourceInfo()
                    {
                        Host = DBCast<string>(row["ServerName"]),
                        Instance = DBCast<string>(row["InstanceName"]),
                        IsClustered = new[] { "yes", "true" }.Contains((DBCast<string>(row["IsClustered"]) ?? string.Empty).ToLower()),
                        Version = DBCast<string>(row["Version"])
                    };

                    //check if the instance name is empty
                    if (!string.IsNullOrEmpty(item.Instance))
                    {
                        //append the instance name to the server name
                        item.Host += String.Format("\\{0}", item.Instance);
                    }

                    result.Add(item);
                }
            }

            return result;
        }

        private static T DBCast<T>(object value)
        {
            if (value is DBNull)
            {
                return default(T);
            }

            return (T)value;
        }
    }
}