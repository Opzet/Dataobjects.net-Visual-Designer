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
    public sealed class ForeignKey : Key<Table>
    {
        #region fields

        private Table foreignTable;

        #endregion

        #region properties

        [XmlIgnore]
        [Browsable(false)]
        public Table ForeignTable
        {
            get { return foreignTable; }
            set
            {
                foreignTable = value;
                ForeignTableUrn = value.Urn;
            }
        }

        [XmlAttribute("foreignTable")]
        public string ForeignTableUrn { get; set; }

        [XmlArrayItem("Column")]
        [Browsable(false)]
        public ForeignKeyColumnCollection Columns { get; set; }

        [XmlIgnore]
        public ForeignKeyColumn[] ForeignKeyColumns
        {
            get { return this.Columns.ToArray(); }
        }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute("notForReplication")]
        public bool NotForReplication { get; set; }

        [XmlElement("OnUpdateAction")]
        public ReferentialAction? OnUpdateAction { get; set; }

        [XmlElement("OnDeleteAction")]
        public ReferentialAction? OnDeleteAction { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public override DbItemType ItemType
        {
            get { return DbItemType.ForeignKey; }
        }

        #endregion

        #region constructors

        public ForeignKey(string name, TableBase owner): base(name, owner)
        {
            this.Columns = new ForeignKeyColumnCollection();
        }

        public ForeignKey(): this(null, null)
        {
        }

        #endregion

        #region methods

        protected override void BuildToString(StringBuilder sb)
        {
            string sourceKeys = this.Columns == null ? string.Empty : Util.JoinCollection(this.Columns.SourceColumns, ",");
            string foreignKeys = this.Columns == null ? string.Empty : Util.JoinCollection(this.Columns.ReferencedColumns, ",");
            sb.AppendFormat("[{7}] Name: '{0}', Source(Table: '{1}', Keys: '{2}'), Foreign(Table: '{3}', Keys: '{4}'), Actions(OnUpdate: '{5}', OnDelete: '{6}')",
                            this.Name, this.Owner.Name, sourceKeys, ForeignTable.Name, foreignKeys, 
                            OnUpdateAction.HasValue ? OnUpdateAction.Value.ToString() : "null", 
                            OnDeleteAction.HasValue ? OnDeleteAction.Value.ToString() : "null",
                            this.ItemType);

            if (!Enabled)
            {
                sb.Append(", Disabled");
            }

            if (NotForReplication)
            {
                sb.Append(", NotForReplication");
            }
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = base.InternalResolveChildObject(childObjectUrn);
            if (result != null)
            {
                result = this;

                string foreignKeyColumnName = childObjectUrn[DbItemType.ForeignKeyColumn];
                if (!string.IsNullOrEmpty(foreignKeyColumnName))
                {
                    ForeignKeyColumn foreignKeyColumnObj = this.ForeignKeyColumns.SingleOrDefault(item => Util.StringEqual(item.Name, foreignKeyColumnName, true));
                    if (foreignKeyColumnObj != null)
                    {
                        result = foreignKeyColumnObj.InternalResolveChildObject(childObjectUrn);
                    }
                }

                return result;
            }

            return result;
        }

        #endregion
    }

    #region enum ForeignUpdateStandardAction

    [Serializable]
    public enum ReferentialAction
    {
        [XmlEnum("NoAction")]
        NoAction,

        [XmlEnum("Cascade")]
        Cascade,

        [XmlEnum("SetNull")]
        SetNull,

        [XmlEnum("SetDefault")]
        SetDefault
    }

    #endregion

    #region class ForeignUpdateMode

/*
    [Serializable]
    public class ForeignUpdateMode
    {
        #region fields

        private ForeignUpdateStandardAction? standardAction;

        #endregion

        #region properties

        [XmlIgnore]
        public ForeignUpdateStandardAction? StandardAction
        {
            get
            {
                if (!standardAction.HasValue && !string.IsNullOrEmpty(StandardActionName))
                {
                    standardAction =
                        (ForeignUpdateStandardAction?)
                        Enum.Parse(typeof (ForeignUpdateStandardAction), StandardActionName, true);
                }

                return standardAction;
            }
            set
            {
                standardAction = value;
                StandardActionName = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }

        [XmlAttribute("standardActionName")]
        public string StandardActionName { get; set; }

        #endregion

        #region constructors

        public ForeignUpdateMode()
        {
            this.StandardAction = null;
        }

        public ForeignUpdateMode(ForeignUpdateStandardAction standardAction)
        {
            this.StandardAction = standardAction;
        }

        #endregion

        #region methods

        public virtual string GetMode()
        {
            return this.StandardAction.HasValue ? this.StandardAction.Value.ToString() : string.Empty;
        }

        public override string ToString()
        {
            return GetMode();
        }

        #endregion
    }
*/

    #endregion

    #region class ForeignKeyCollection

    public sealed class ForeignKeyCollection : ObjectCollection<ForeignKey>
    {
        public ForeignKeyCollection(IList<ForeignKey> list) : base(list) { }

        public ForeignKeyCollection()
        {
        }
    }

    #endregion
}