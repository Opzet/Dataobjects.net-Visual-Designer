using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive.Implementation.Properties;
using Xtensive.Sql;
using Xtensive.Sql.Dml;
using Xtensive.Sql.Model;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive.Implementation
{
    public class DBProvider : IDBProvider
    {
        #region fields

        private readonly List<Exception> lastErrors = new List<Exception>();
        private readonly ConnectionInfoProvider connectionInfoProvider = new ConnectionInfoProvider();
        private static readonly Guid uniqueID = new Guid("{EE2EABB5-7FD1-46E2-9BB1-35C305AD0605}");

//        internal static readonly Dictionary<string, Tuple<string, string>> ProtocolMappings = new Dictionary<string, Tuple<string, string>>
//            {
//                {"Microsoft SQL Server 2005 / 2008", new Tuple<string, string>("sqlserver", "System.Data.SqlClient")},
//                {"Microsoft SQL Server CE 3.5", new Tuple<string, string>("sqlserverce", "System.Data.SqlServerCe.3.5")},
//                {"SQL Azure Database (formerly SQL Data Services, SDS or SSDS)", new Tuple<string, string>("azure", "System.Data.SqlClient")},
//                {"Oracle 9i, 10g and 11g", new Tuple<string, string>("oracle", "System.Data.OracleClient")},
//                {"PostgreSQL 8.2, 8.3, 8.4 (freeware, open source)", new Tuple<string, string>("postgresql", "")},
//            };

        internal static readonly List<StorageEngineAdoNetMapping> ProtocolMappings = new List<StorageEngineAdoNetMapping>
        {
            new StorageEngineAdoNetMapping("System.Data.SqlClient", "Microsoft SQL Server", "sqlserver", "Supports Microsoft SQL Server versions 2005 and 2008"),
            new StorageEngineAdoNetMapping("System.Data.SqlServerCe.3.5", "Microsoft SQL Server CE 3.5", "sqlserverce", "Supports Microsoft SQL Server CE version 3.5"),
            new StorageEngineAdoNetMapping("System.Data.SqlClient", "SQL Azure Database", "azure", "Supports SQL Azure Database (formerly SQL Data Services, SDS or SSDS)"),
            new StorageEngineAdoNetMapping("System.Data.OracleClient", "Oracle", "oracle", "Supports Oracle versions 9i, 10g and 11g"),
            new StorageEngineAdoNetMapping("", "PostgreSQL", "postgresql", "Supports PostgreSQL 8.2, 8.3, 8.4 (freeware, open source)"),
        };

        //private Domain domain;
        //private StorageInfo extractedSchema;
        private ConnectionInfo connectionInfo;
        private SqlDriver sqlDriver;
        private SqlConnection sqlConnection;
        private Catalog extractedCatalog;

        #endregion fields

        internal static StorageEngineAdoNetMapping GetProtocolMapping(StorageEngine engine)
        {
            //return ProtocolMappings.SingleOrDefault(pair => Util.StringEqual(pair.Value.Item1, key, true)).Value;
            return ProtocolMappings.SingleOrDefault(item => Util.StringEqual(item.Key, engine.Key, true));
        }

        public string Name
        {
            get { return "Universal Xtensive Database Engine Provider"; }
        }

        public string Description
        {
            get
            {
                return
                    @"Supports these sql server engines:
- Microsoft SQL Server 2005 / 2008
- Microsoft SQL Server CE 3.5
- Oracle 9i / 10g / 11g
- PostgreSQL 8.2 / 8.3 / 8.4 (freeware, open source)

This provider uses Xtensive framework to read database schema for specified server engine.";
            }
        }

        public Guid UniqueID { get { return uniqueID; } }

        public ErrorCollection LastErrors
        {
            get
            {
                return new ErrorCollection(lastErrors);
            }
        }

        public bool SupportsMultipleStorageEngines
        {
            get { return true; }
        }

        public IConnectionInfoProvider ConnectionInfoProvider
        {
            get { return this.connectionInfoProvider; }
        }

        public bool IsConnected { get; private set; }

        public Image Icon
        {
            get { return Resources.DataObjectsNet16x16.ToBitmap(); }
        }

        private void UpdateLastError(Exception exception, bool append = false)
        {
            if (!append)
            {
                this.lastErrors.Clear();
            }

            this.lastErrors.Add(exception);
        }

        private void ClearLastErrors()
        {
            this.lastErrors.Clear();
        }

        private void EnsureConnected()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Not connected!");
            }
        }

        void IDBProvider.ClearLastErrors()
        {
            ClearLastErrors();
        }

        public bool Connect(IConnectionInfo connectionInfo)
        {
            return InternalReconnect(true, connectionInfo);
        }

        private bool InternalReconnect(bool checkAlreadyConnected, IConnectionInfo connectionInfo)
        {
            if (this.IsConnected)
            {
                if (checkAlreadyConnected)
                {
                    throw new InvalidOperationException("Already connected, call 'Disconnect' before 'Connect'");
                }

                if (this.sqlConnection != null && this.sqlConnection.State == ConnectionState.Open)
                {
                    this.sqlConnection.Close();
                    this.sqlConnection = null;

                    sqlDriver = null;
                }
            }

            this.connectionInfo = (ConnectionInfo)connectionInfo;
            this.IsConnected = false;

            try
            {
                if (!string.IsNullOrEmpty(this.connectionInfo.ServerName) &&
                    !string.IsNullOrEmpty(this.connectionInfo.DatabaseName) &&
                    this.connectionInfo.InternalInfo != null)
                {
                    sqlDriver = SqlDriver.Create(this.connectionInfo.InternalInfo.ConnectionUrl);
                    sqlConnection = this.sqlDriver.CreateConnection();
                    sqlConnection.Open();
                    this.IsConnected = sqlConnection.State == ConnectionState.Open;

                    //                    DomainConfiguration config = new DomainConfiguration(this.connectionInfo.InternalInfo);
                    //                    config.UpgradeMode = DomainUpgradeMode.LegacySkip;
                    //                    this.domain = Domain.Build(config);
                    //                    this.extractedSchema = this.domain.ExtractedSchema;)
                    //                    this.IsConnected = this.domain != null && this.extractedSchema != null;
                }
                else
                {
                    this.IsConnected = true;
                }
            }
            catch (Exception e)
            {
                this.IsConnected = false;
                UpdateLastError(e, true);
            }

            return this.IsConnected;
        }

        private Catalog ExtractCatalog()
        {
            Catalog model = null;

            if (sqlConnection != null)
            {
                try
                {
                    sqlConnection.BeginTransaction();
                    model = sqlDriver.ExtractCatalog(sqlConnection);
                }
                finally
                {
                    sqlConnection.Rollback();
                }
            }

            return model;
        }

        private void EnsureExtractedCatalog()
        {
            bool result = this.extractedCatalog != null;

            if (!result && !string.IsNullOrEmpty(this.connectionInfo.ServerName) &&
                !string.IsNullOrEmpty(this.connectionInfo.DatabaseName) &&
                this.connectionInfo.InternalInfo != null)
            {
                this.extractedCatalog = ExtractCatalog();
                result = this.extractedCatalog != null;
            }

            if (result)
            {
                if (!string.IsNullOrEmpty(this.connectionInfo.DatabaseName) && extractedCatalog != null &&
                    !Util.StringEqual(this.connectionInfo.DatabaseName, extractedCatalog.Name, true))
                {
                    this.extractedCatalog = ExtractCatalog();
                }
            }

            if (!result)
            {
                throw new InvalidOperationException("Server name must be specified!");
            }
        }

        public void Disconnect()
        {
            ClearLastErrors();
            EnsureConnected();


            if (this.sqlConnection != null && this.sqlConnection.State == ConnectionState.Open)
            {
                this.sqlConnection.Close();
                this.sqlConnection = null;

                sqlDriver = null;
            }

            IsConnected = false;
        }

        public ServerCollection GetAllServers(LoadingMode loadingMode)
        {
            ClearLastErrors();
            EnsureConnected();

            ServerCollection servers = null;
            try
            {
                string systemEngineProvider = GetSystemEngineProvider();

                var dataSources = DBEngineUtils.GetDataSources(systemEngineProvider);
                List<Server> items = new List<Server>();
                foreach (var item in dataSources.OrderBy(info => info.Host))
                {
                    items.Add(new Server
                    {
                        Name = item.Host,
                        IsClustered = item.IsClustered,
                        Version = item.Version
                    });
                }

                items.Insert(0, new Server("localhost"));

                servers = new ServerCollection(items);

                if (loadingMode == LoadingMode.RecursiveAllLevels)
                {
                    foreach (Server server in servers)
                    {
                        InternalRefresh(server, loadingMode, false);
                    }
                }
            }
            catch (Exception e)
            {
                UpdateLastError(e);
            }

            return servers;
        }

        private string GetSystemEngineProvider()
        {
            //string xtensiveProvider = this.connectionInfo.InternalInfo.Provider;
            //var mapping = ProtocolMappings[this.connectionInfo.StorageEngineKey];
            StorageEngineAdoNetMapping mapping = GetProtocolMapping(this.connectionInfo.StorageEngine);
            var adoNetInvariantName = mapping.AdoNetKey;

            return DBEngineUtils.GetSystemDBProviders().SingleOrDefault(
                dbEngineName => Util.StringEqual(dbEngineName, adoNetInvariantName, true));
        }

        public bool Refresh(Server server, LoadingMode loadMode)
        {
            return InternalRefresh(server, loadMode, true);
        }

        private bool InternalRefresh(Server server, LoadingMode loadMode, bool clearErrors)
        {
            if (clearErrors)
            {
                ClearLastErrors();
            }

            bool result = Util.StringEqual(this.connectionInfo.ServerName, server.Name, true);
            if (!result)
            {
                if (this.IsConnected)
                {
                    this.Disconnect();
                }
                this.connectionInfo.ServerName = server.Name;

                result = InternalReconnect(true, this.connectionInfo);
            }


            if (result)
            {
                EnsureConnected();

                List<Database> databases = new List<Database>();

                string systemEngineProvider = GetSystemEngineProvider();

                var databaseList = DBEngineUtils.GetDatabases(systemEngineProvider, server.Name, this.connectionInfo.UserID, this.connectionInfo.Password);

                foreach (var database in databaseList.OrderBy(s => s))
                {
                    Database db = new Database(database, server);
                    databases.Add(db);
                }

                server.Databases = new DatabaseCollection(databases);

                if (loadMode == LoadingMode.RecursiveAllLevels)
                {
                    EnsureExtractedCatalog();

                    foreach (Database database in server.Databases)
                    {
                        InternalRefresh(database, loadMode, false);
                    }
                }
            }

            return result;
        }

        public bool Refresh(Database database, LoadingMode loadMode)
        {
            return InternalRefresh(database, loadMode, true);
        }

        public bool Refresh(Schema schema, LoadingMode loadMode)
        {
            return InternalRefresh(schema, loadMode, true);
        }

        public bool InternalRefresh(Database database, LoadingMode loadMode, bool clearErrors)
        {
            if (clearErrors)
            {
                ClearLastErrors();
            }

            EnsureExtractedCatalog();

            bool result = Util.StringEqual(this.connectionInfo.ServerName, database.Owner.Name, true) &&
                Util.StringEqual(this.connectionInfo.DatabaseName, database.Name, true);

            if (!result)
            {
                if (this.IsConnected)
                {
                    this.Disconnect();
                }
                if (this.connectionInfo.ServerName != database.Owner.Name)
                {
                    this.connectionInfo.ServerName = database.Owner.Name;
                }

                if (this.connectionInfo.DatabaseName != database.Name)
                {
                    this.connectionInfo.DatabaseName = database.Name;
                }

                result = InternalReconnect(true, this.connectionInfo);
            }

            if (result)
            {
                EnsureExtractedCatalog();

                List<Schema> schemas = new List<Schema>();
                foreach (var schema in GetFilteredSchemas().Where(item => Util.StringEqual(item.Catalog.Name, database.Name, true)))
                {
                    Schema newSchema = new Schema(schema.Name, database);
                    schemas.Add(newSchema);
                }

                database.Schemas = new SchemaCollection(schemas);

                if (loadMode == LoadingMode.RecursiveAllLevels)
                {
                    foreach (var schema in database.Schemas)
                    {
                        InternalRefresh(schema, loadMode, false);
                    }
                }
            }

            return result;
        }

        private static readonly string[] FILTER_SCHEMAS = new[]
                                          {
                                              "INFORMATION_SCHEMA",
                                              "sys",
                                              "guest"
                                          };

        private IEnumerable<global::Xtensive.Sql.Model.Schema> GetFilteredSchemas()
        {
            return extractedCatalog.Schemas.Where(schema => !schema.Name.In(FILTER_SCHEMAS));
        }

        public bool InternalRefresh(Schema schema, LoadingMode loadMode, bool clearErrors)
        {
            if (clearErrors)
            {
                ClearLastErrors();
            }

            EnsureExtractedCatalog();

            List<Table> tables = new List<Table>();

            global::Xtensive.Sql.Model.Schema ormSchema = extractedCatalog.Schemas[schema.Name];
            foreach (global::Xtensive.Sql.Model.Table table in ormSchema.Tables)
            {
                Table newTable = new Table(table.Name, schema);
                tables.Add(newTable);
            }

            schema.Tables = new TableCollection(tables);

            if (loadMode == LoadingMode.RecursiveAllLevels)
            {
                foreach (var table in schema.Tables)
                {
                    InternalRefresh(table, false);
                }

                // iterate each foreign key if has assigned temporary reference table and if so then try to found real foreign table
                foreach (Table table in schema.Tables)
                {
                    if (table.ForeignKeys != null)
                    {
                        foreach (ForeignKey foreignKey in table.ForeignKeys)
                        {
                            if (foreignKey.ForeignTable is TemporaryReferencedTable)
                            {
                                TemporaryReferencedTable temporaryReferencedTable =
                                    (TemporaryReferencedTable) foreignKey.ForeignTable;

                                var referencedTable = schema.Tables[temporaryReferencedTable.Name];
                                    //, temporaryReferencedTable.Schema];
                                foreignKey.ForeignTable = referencedTable;
                            }
                        }
                    }
                }
            }

            //TODO: When supporting views, add loading of views

            return true;
        }

        public bool Refresh(Table table)
        {
            return InternalRefresh(table, true);
        }

        public bool InternalRefresh(Table table, bool clearErrors)
        {
            if (clearErrors)
            {
                ClearLastErrors();
            }

            EnsureExtractedCatalog();

            global::Xtensive.Sql.Model.Table ormTable = this.extractedCatalog.Schemas[table.Owner.Name].Tables[table.Name];

            if (ormTable != null)
            {
                var tableColumns = new List<Column>();

                // load columns
                foreach (TableColumn column in ormTable.TableColumns)
                {
                    Column newColumn = new Column(column.Name, table);
                    newColumn.DefaultValue = string.Empty;

                    if (column.DefaultValue != null)
                    {
                        SqlLiteral sqlLiteral = column.DefaultValue as SqlLiteral;
                        if (sqlLiteral != null)
                        {
                            object value = sqlLiteral.GetValue();
                            if (value != null)
                            {
                                newColumn.DefaultValue = value.ToString();
                            }
                        }
                    }

                    newColumn.MaxLength = column.DataType.Length.GetValueOrDefault(0);
                    newColumn.IsMaxLength = column.DataType.Type == SqlType.VarCharMax || column.DataType.Type == SqlType.VarBinaryMax;
                    //                        column.DataType.SqlDataType.In(
                    //                        SqlDataType.NVarCharMax, SqlDataType.VarBinaryMax, SqlDataType.VarCharMax);
                    newColumn.Precision = column.DataType.Precision.GetValueOrDefault(0);
                    newColumn.Scale = column.DataType.Scale.GetValueOrDefault(0);
                    newColumn.ID = -1;// column.SequenceDescriptor.Index;
                    newColumn.IdentityIncrement = column.SequenceDescriptor != null ? column.SequenceDescriptor.Increment.GetValueOrDefault(0) : 0;
                    newColumn.IdentitySeed = column.SequenceDescriptor != null ? column.SequenceDescriptor.StartValue.GetValueOrDefault(0) : 0;
                    newColumn.IsIdentity = column.SequenceDescriptor != null;
                    newColumn.IsNullable = column.IsNullable;
                    newColumn.NotForReplication = false;
                    newColumn.RowGuidCol = false;
                    newColumn.SqlDataType = column.DataType.Type.ToString();

                    Type clrType;

                    if (column.DataType.Type == SqlType.Unknown && column.DataType.TypeName.ToLower() == "timestamp")
                    {
                        clrType = typeof(byte[]);
                    }
                    else
                    {
                        clrType = column.DataType.Type.ToClrType();
                    }
                    Type nullableItemType = Nullable.GetUnderlyingType(clrType);

                    newColumn.ClrDataType = nullableItemType ?? clrType;
                    tableColumns.Add(newColumn);
                }

                table.Columns = new ColumnCollection(tableColumns);

                // load foreign keys
                List<ForeignKey> foreignKeys = new List<ForeignKey>();
                foreach (var key in ormTable.TableConstraints.Where(constraint => constraint is global::Xtensive.Sql.Model.ForeignKey).Cast<global::Xtensive.Sql.Model.ForeignKey>())
                {
                    ForeignKey newForeignKey = new ForeignKey(key.Name, table);
                    //TODO: How to get 'IsEnabled' and 'NotForReplication' values ??
                    newForeignKey.Enabled = true;
                    newForeignKey.NotForReplication = true;
                    newForeignKey.OnDeleteAction = ConvertForeignKeyAction(key.OnDelete);
                    newForeignKey.OnUpdateAction = ConvertForeignKeyAction(key.OnUpdate);

                    //newForeignKey.SourceTable = table;
                    List<ForeignKeyColumn> newKeyColumns = new List<ForeignKeyColumn>();

                    for (int i = 0; i < key.ReferencedColumns.Count; i++)
                    {
                        var column = key.Columns[i];
                        var keyColumnRef = key.ReferencedColumns[i];

                        //newKeyColumns.Add(new ForeignKeyColumn(column.Name, column.Value.Name, newForeignKey));
                        newKeyColumns.Add(new ForeignKeyColumn(column.Name, keyColumnRef.Name, newForeignKey));
                    }
                    newForeignKey.Columns = new ForeignKeyColumnCollection(newKeyColumns);

                    string referencedTableName = key.ReferencedTable.Name;
                    string referencedTableSchema = key.ReferencedTable.Schema.Name;

                    var referencedTable = table.Owner.Owner.GetTable(referencedTableSchema, referencedTableName);
                    //var referencedTable = table.Owner.Tables[referencedTableName, referencedTableSchema]; //, key.ReferencedTableSchema];

                    if (referencedTable != null)
                    {
                        newForeignKey.ForeignTable = referencedTable;
                    }
                    else
                    {
                        var temporaryReferencedTable = new TemporaryReferencedTable(referencedTableName, table.Owner);
                        newForeignKey.ForeignTable = temporaryReferencedTable;
                    }

                    foreignKeys.Add(newForeignKey);
                }

                table.ForeignKeys = new ForeignKeyCollection(foreignKeys);

                // load indexes
                List<Index> indices = new List<Index>();

                foreach (global::Xtensive.Sql.Model.Index index in ormTable.Indexes)
                {
                    Index newIndex = new Index(index.Name, table);
                    newIndex.Primary = false; // primaryKeys.Any(item => item.Name == index.Name);
                    newIndex.Clustered = index.IsClustered;
                    newIndex.FillFactor = 0.0;
                    //newIndex.Unique = newIndex.Primary ? false : index.IsUnique;
                    newIndex.Unique = index.IsUnique;

                    List<IndexedColumn> newIndexedColumns = new List<IndexedColumn>();
                    foreach (var indexedColumn in index.Columns)
                    {
                        //ColumnInfo columnInfo = indexedColumn.Value;
                        newIndexedColumns.Add(new IndexedColumn(indexedColumn.Name, ConvertDirection(indexedColumn.Ascending), false, newIndex));
                    }

                    foreach (var includedColumn in index.NonkeyColumns)
                    {
                        newIndexedColumns.Add(new IndexedColumn(includedColumn.Name, IndexOrder.Ascending, true, newIndex));
                    }

                    newIndex.Columns = new IndexedColumnCollection(newIndexedColumns);
                    indices.Add(newIndex);
                }

                var primaryKeys = ormTable.TableConstraints.Where(constraint => constraint is PrimaryKey).Cast<PrimaryKey>();
                foreach (PrimaryKey primaryKey in primaryKeys)
                {
                    Index newIndex = new Index(primaryKey.Name, table);
                    newIndex.Primary = true;
                    newIndex.Clustered = true;
                    newIndex.Unique = true;
                    newIndex.FillFactor = 0.0;
                    List<IndexedColumn> newIndexedColumns = new List<IndexedColumn>();
                    foreach (var indexedColumn in primaryKey.Columns)
                    {
                        newIndexedColumns.Add(new IndexedColumn(indexedColumn.Name, IndexOrder.Ascending, false, newIndex));
                    }

                    newIndex.Columns = new IndexedColumnCollection(newIndexedColumns);
                    indices.Add(newIndex);
                }

                table.Indexes = new IndexCollection(indices);
            }

            return true;
        }

        private IndexOrder ConvertDirection(bool ascending)
        {
            return ascending ? IndexOrder.Ascending : IndexOrder.Descending;
        }

        private ReferentialAction ConvertForeignKeyAction(global::Xtensive.Sql.ReferentialAction value)
        {
            switch (value)
            {
                case global::Xtensive.Sql.ReferentialAction.SetDefault:
                return ReferentialAction.SetDefault;
                case global::Xtensive.Sql.ReferentialAction.Cascade:
                return ReferentialAction.Cascade;
                case global::Xtensive.Sql.ReferentialAction.SetNull:
                return ReferentialAction.SetNull;
                default:
                return ReferentialAction.NoAction;
            }
        }

        public IEnumerable<StorageEngine> GetSupportedStorageEngines()
        {
            return ProtocolMappings;
            /*Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var mapping in ProtocolMappings)
            {
                string value = mapping.Key;
                string key = mapping.Value.Item1;
                result.Add(key, value);
            }

            //return ProtocolMappings.Keys;

            return result;*/
        }
    }

    public class StorageEngineAdoNetMapping: StorageEngine
    {
        internal string AdoNetKey { get; private set; }

        public StorageEngineAdoNetMapping(string adoNetKey, string displayName, string key, string description) : base(displayName, key, description)
        {
            this.AdoNetKey = adoNetKey;
        }

        public override bool EqualsTo(StorageEngine other)
        {
            bool equalsTo = base.EqualsTo(other);
            if (equalsTo)
            {
                StorageEngineAdoNetMapping otherEngine = other as StorageEngineAdoNetMapping;
                equalsTo = otherEngine != null;
                if (equalsTo)
                {
                    equalsTo = Util.StringEqual(this.AdoNetKey, otherEngine.AdoNetKey, true);
                }
            }

            return equalsTo;
        }
    }
}