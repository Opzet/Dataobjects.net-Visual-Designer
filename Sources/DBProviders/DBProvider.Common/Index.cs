using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public class Index: Key<TableBase>
    {
        #region properties

        [XmlAttribute("fillFactor")]
        public double FillFactor { get; set; }

        [XmlAttribute("unique")]
        public bool Unique { get; set; }

        [XmlAttribute("primary")]
        public bool Primary { get; set; }

        [XmlAttribute("clustered")]
        public bool Clustered { get; set; }

        [XmlArrayItem("Column")]
        [Browsable(false)]
        public IndexedColumnCollection Columns { get; set; }

        [XmlIgnore]
        public IndexedColumn[] IndexedColumns
        {
            get { return this.Columns.ToArray(); }
        }

        [XmlIgnore]
        public override DbItemType ItemType
        {
            get { return DbItemType.Index; }
        }

        #endregion

        #region constructors

        public Index(string name, TableBase owner): base(name, owner)
        {
            this.Columns = new IndexedColumnCollection();
        }

        public Index(): this(null, null)
        {
        }

        #endregion

        #region methods

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);
            sb.AppendFormat(", Table: '{0}', FillFactor: '{1}'", this.Owner.Name, FillFactor);

            if (Primary)
            {
                sb.Append(", Primary");
            }

            if (Unique)
            {
                sb.Append(", Unique");
            }

            if (Clustered)
            {
                sb.Append(", Clustered");
            }

            if (this.Columns != null && this.Columns.Count > 0)
            {
                var keys = this.Columns.Select(item => string.Format("{0}:{1}{2}", item.Name, item.Order == IndexOrder.Ascending ? "ASC": "DESC",
                                                                            item.IncludeOnly ? "/INC" : string.Empty));

                sb.AppendFormat(", Keys: '{0}'", Util.JoinCollection(keys.ToList(), ","));
            }
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = base.InternalResolveChildObject(childObjectUrn);
            if (result != null)
            {
                result = this;

                string indexedColumnName = childObjectUrn[DbItemType.IndexedColumn];
                if (!string.IsNullOrEmpty(indexedColumnName))
                {
                    IndexedColumn indexedColumnObj = this.Columns.SingleOrDefault(column => Util.StringEqual(column.Name, indexedColumnName, true));
                    if (indexedColumnObj != null)
                    {
                        result = indexedColumnObj.InternalResolveChildObject(childObjectUrn);
                    }
                }

                return result;
            }

            return result;
        }

        #endregion
    }

    #region enum IndexOrder

    [Serializable]
    public enum IndexOrder
    {
        [XmlEnum("Ascending")]
        Ascending,

        [XmlEnum("Descending")]
        Descending
    }

    #endregion

    #region class IndexCollection

    public sealed class IndexCollection : ObjectCollection<Index>
    {
        public IndexCollection(IList<Index> list) : base(list) { }

        public IndexCollection()
        {
        }
    }

    #endregion
}