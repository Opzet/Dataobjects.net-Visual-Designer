using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public class Database: DBObject<Server>
    {
        #region properties

        [XmlIgnore]
        public override DbItemType ItemType
        {
            get { return DbItemType.Database; }
        }

        [XmlArrayItem("Schema")]
        public SchemaCollection Schemas { get; set; }

        #endregion

        #region constructors

        public Database(string name, Server owner) : base(name, owner)
        {
            this.Schemas = new SchemaCollection();
        }

        public Database(): this(null, null)
        {
        }

        #endregion


        #region methods

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);

            sb.AppendFormat(", Schemas: {0}", Schemas.Count);
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = null;

            if (Util.StringEqual(childObjectUrn[DbItemType.Database], this.Name, true))
            {
                result = this;

                string schemaName = childObjectUrn[DbItemType.Schema];
                if (!string.IsNullOrEmpty(schemaName))
                {
                    var schemaObj = this.Schemas.SingleOrDefault(delegate(Schema schema)
                                                                 {
                                                                     bool tableNameEquals = Util.StringEqual(schema.Name, schemaName, true);
                                                                     if (!string.IsNullOrEmpty(childObjectUrn.TableSchema))
                                                                     {
                                                                         return tableNameEquals && Util.StringEqual(schema.Name, childObjectUrn.TableSchema, true);
                                                                     }

                                                                     return tableNameEquals;
                                                                 });

                    if (schemaObj != null)
                    {
                        result = schemaObj.InternalResolveChildObject(childObjectUrn);
                    }
                }
            }

            return result;
        }

        public Table GetTable(string schemaName, string tableName)
        {
            return GetTableOrView<Table>(DbItemType.Table, schemaName, tableName);
        }

        public View GetView(string schemaName, string viewName)
        {
            return GetTableOrView<View>(DbItemType.View, schemaName, viewName);
        }

        private TResult GetTableOrView<TResult>(DbItemType itemType, string schemaName, string objName) where TResult : TableBase
        {
            Schema schema = this.Schemas[schemaName];
            if (schema != null)
            {
                TableBase result = itemType == DbItemType.Table
                                       ? (TableBase) schema.Tables[objName]
                                       : schema.Views[objName];
                
                return (TResult)(result);
            }

            return null;
        }

        /// <summary>
        /// Gets all tables across all schemas in datatabase.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetAllTables()
        {
            return this.Schemas.SelectMany(schema => schema.Tables);
        }

        /// <summary>
        /// Gets all views across all schemas in datatabase.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<View> GetAllViews()
        {
            return this.Schemas.SelectMany(schema => schema.Views);
        }

        #endregion
    }

    #region class DatabaseCollection

    public sealed class DatabaseCollection: ObjectCollection<Database>
    {
        public DatabaseCollection(IList<Database> list): base(list) {}

        public DatabaseCollection()
        {
        }
    }

    #endregion
}