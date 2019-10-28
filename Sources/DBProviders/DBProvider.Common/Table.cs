using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    #region class Table

    [Serializable]
    public class Table : TableBase
    {
        #region fields 

        public static readonly IdentityProjectionEqualityComparer<Table, string> TableEqualityComparer = 
            new IdentityProjectionEqualityComparer<Table, string>(table => table.FormatName());

        #endregion fields

        #region properties

        [XmlArrayItem("Index")]
        [Browsable(false)]
        public IndexCollection Indexes { get; set; }

        [XmlArrayItem("Key")]
        [Browsable(false)]
        public ForeignKeyCollection ForeignKeys { get; set; }

        [XmlIgnore]
        public override DbItemType ItemType
        {
            get { return DbItemType.Table; }
        }

        #endregion

        #region constructors

        public Table(string name, Schema owner) : base(name, owner)
        {
            this.Indexes = new IndexCollection();
            this.ForeignKeys = new ForeignKeyCollection();
        }

        public Table(): this(null, null)
        {
        }

        #endregion

        #region methods

        public string FormatName()
        {
            //return string.IsNullOrEmpty(this.Schema) ? this.Name : string.Format("{0} ({1})", this.Name, this.Schema);
            return string.Format("{0} ({1})", this.Name, this.Owner.Name);
        }

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);

            sb.AppendFormat(", Indicies: {0}, ForeignKeys: {1}",
                            this.Indexes != null ? this.Indexes.Count.ToString() : "-",
                            ForeignKeys != null ? ForeignKeys.Count.ToString() : "-");
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = base.InternalResolveChildObject(childObjectUrn);
            if (result == null && Util.StringEqual(childObjectUrn[ItemType], this.Name, true))
            {
                result = this;

                string indexName = childObjectUrn[DbItemType.Index];
                if (!string.IsNullOrEmpty(indexName))
                {
                    Index indexObj = this.Indexes.SingleOrDefault(item => Util.StringEqual(item.Name, indexName, true));
                    if (indexObj != null)
                    {
                        result = indexObj.InternalResolveChildObject(childObjectUrn);
                    }
                }

                string foreignKeyName = childObjectUrn[DbItemType.ForeignKey];
                if (!string.IsNullOrEmpty(foreignKeyName))
                {
                    ForeignKey foreignKeyObj = this.ForeignKeys.SingleOrDefault(item => Util.StringEqual(item.Name, foreignKeyName, true));
                    if (foreignKeyObj != null)
                    {
                        result = foreignKeyObj.InternalResolveChildObject(childObjectUrn);
                    }
                }


                return result;
            }

            return result;
        }

        public bool DependsOn(Table table)
        {
            return this.ForeignKeys.Any(foreignKey => table.EqualsByUrn(foreignKey.ForeignTable));
        }

        public IEnumerable<Table> GetDependencyTables()
        {
            return this.ForeignKeys.Select(foreignKey => foreignKey.ForeignTable).Distinct(TableEqualityComparer);
        }

        #endregion
    }

    #endregion class Table

    #region class TableCollection

    public sealed class TableCollection : ObjectCollection<Table>
    {
        #region constructors

        public TableCollection(IList<Table> list) : base(list) { }

        public TableCollection()
        {
        }

        #endregion
    }

    #endregion
}