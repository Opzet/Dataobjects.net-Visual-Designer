using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public abstract class TableBase : DBObject<Schema>
    {
        #region fields

        //private string schema;

        #endregion fields

        #region properties

        [XmlArrayItem("Column")]
        [Browsable(false)]
        public ColumnCollection Columns { get; set; }

//        [XmlAttribute("schema")]
//        public string Schema
//        {
//            get { return schema; }
//            set
//            {
//                if (schema != value)
//                {
//                    schema = value;
//                    BuildUrn();
//                }
//            }
//        }

        [XmlIgnore]
        public abstract override DbItemType ItemType { get; }

        #endregion

        #region constructors

        protected TableBase(string name, Schema owner) : base(name, owner)
        {
            this.Columns = new ColumnCollection();
        }

        protected TableBase(): this(null, null)
        {
        }

        #endregion

        #region methods

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);

            //sb.AppendFormat(", Schema: '{0}', Columns: {1}", Schema, this.Columns != null ? this.Columns.Count.ToString() : "-");
            sb.AppendFormat(", Columns: {0}", this.Columns != null ? this.Columns.Count.ToString() : "-");
        }

        protected internal override void InternalBuildUrn(StringBuilder sb)
        {
            //string optSchema = string.IsNullOrEmpty(this.Schema) ? string.Empty : string.Format(" and @Schema='{0}'", this.Schema);
            sb.Insert(0, string.Format("/{0}[@Name='{1}' and @Schema='{2}']", this.ItemType, this.Name, this.Owner.Name));

            if (this.Owner != null)
            {
                this.Owner.InternalBuildUrn(sb);
            }
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = null;

            if (Util.StringEqual(childObjectUrn[ItemType], this.Name, true))
            {
                if (!string.IsNullOrEmpty(childObjectUrn.TableSchema))
                {
                    if (!Util.StringEqual(childObjectUrn.TableSchema, this.Owner.Name, true))
                    {
                        return result;
                    }
                }

                result = this;

                string columnName = childObjectUrn[DbItemType.Column];
                if (!string.IsNullOrEmpty(columnName))
                {
                    Column columnObj = this.Columns.SingleOrDefault(column => Util.StringEqual(column.Name, columnName, true));
                    if (columnObj != null)
                    {
                        result = columnObj.InternalResolveChildObject(childObjectUrn);
                    }
                }
            }

            return result;
        }

        #endregion
    }
}