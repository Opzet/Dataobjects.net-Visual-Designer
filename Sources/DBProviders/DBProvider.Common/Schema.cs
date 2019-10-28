using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public class Schema : DBObject<Database>
    {
        #region properties 

        public override DbItemType ItemType
        {
            get
            {
                return DbItemType.Schema;
            }
        }

        [XmlAttribute("dbOwner")]
        public string DbOwner { get; set; }

        [XmlArrayItem("Table")]
        public TableCollection Tables { get; set; }

        [XmlArrayItem("View")]
        public ViewCollection Views { get; set; }

        #endregion properties

        #region constructors 

        public Schema(string name, Database owner) : base(name, owner)
        {
            this.Tables = new TableCollection();
            this.Views = new ViewCollection();
        }

        public Schema(): this(null, null)
        {}

        #endregion constructors

        #region methods 

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);

            sb.AppendFormat(", Tables: {0}, Views: {1}", Tables.Count, Views.Count);
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = null;

            if (Util.StringEqual(childObjectUrn[DbItemType.Schema], this.Name, true))
            {
                result = this;

                string tableName = childObjectUrn[DbItemType.Table];
                if (!string.IsNullOrEmpty(tableName))
                {
                    Table tableObj = this.Tables.SingleOrDefault(delegate(Table table)
                    {
                        bool tableNameEquals = Util.StringEqual(table.Name, tableName, true);
                        if (!string.IsNullOrEmpty(childObjectUrn.TableSchema))
                        {
                            return tableNameEquals && Util.StringEqual(this.Name, childObjectUrn.TableSchema, true);
                        }

                        return tableNameEquals;
                    });

                    if (tableObj != null)
                    {
                        result = tableObj.InternalResolveChildObject(childObjectUrn);
                    }
                }

                string viewName = childObjectUrn[DbItemType.View];
                if (!string.IsNullOrEmpty(viewName))
                {
                    View viewObj = this.Views.SingleOrDefault(delegate(View view)
                    {
                        bool viewNameEquals = Util.StringEqual(view.Name, viewName, true);
                        if (!string.IsNullOrEmpty(childObjectUrn.TableSchema))
                        {
                            return viewNameEquals && Util.StringEqual(this.Name, childObjectUrn.TableSchema, true);
                        }

                        return viewNameEquals;
                    });

                    if (viewObj != null)
                    {
                        result = viewObj.InternalResolveChildObject(childObjectUrn);
                    }
                }
            }

            return result;
        }

        #endregion methods
    }

    #region class SchemaCollection

    [Serializable]
    public sealed class SchemaCollection : ObjectCollection<Schema>
    {
        public SchemaCollection(IList<Schema> list) : base(list) { }

        public SchemaCollection()
        {
        }
    }

    #endregion
}