using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public sealed class ForeignKeyColumn: DBObject<ForeignKey>
    {
        #region properties

        [XmlAttribute("referencedColumn")]
        public string ReferencedColumn { get; set; }

        [XmlIgnore]
        public override DbItemType ItemType
        {
            get { return DbItemType.ForeignKeyColumn; }
        }

        #endregion

        #region constructors

        public ForeignKeyColumn(string name, string referencedColumn, ForeignKey owner) : base(name, owner)
        {
            this.ReferencedColumn = referencedColumn;
        }

        public ForeignKeyColumn()
        {
        }

        #endregion

        #region methods

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);
            sb.AppendFormat(", ReferencedColumn: '{0}'", ReferencedColumn);
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = null;

            if (Util.StringEqual(childObjectUrn[ItemType], this.Name, true))
            {
                result = this;
            }

            return result;
        }

        #endregion
    }

    #region class ForeignKeyColumnCollection

    public sealed class ForeignKeyColumnCollection: ObjectCollection<ForeignKeyColumn>
    {
        #region properties

        [XmlIgnore]
        public List<string> SourceColumns
        {
            get { return this.Select(column => column.Name).ToList(); }
        }

        [XmlIgnore]
        public List<string> ReferencedColumns
        {
            get { return this.Select(column => column.ReferencedColumn).ToList(); }
        }

        #endregion

        #region constructors

        public ForeignKeyColumnCollection(IList<ForeignKeyColumn> list): base(list) {}

        public ForeignKeyColumnCollection()
        {
        }

        #endregion
    }

    #endregion
}