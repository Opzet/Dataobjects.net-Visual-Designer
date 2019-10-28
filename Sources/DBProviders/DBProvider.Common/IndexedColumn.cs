using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public sealed class IndexedColumn: DBObject<Index>
    {
        #region properties

        [XmlIgnore]
        public override DbItemType ItemType
        {
            get { return DbItemType.IndexedColumn; }
        }

        [XmlAttribute("order")]
        public IndexOrder Order { get; set; }

        [XmlAttribute("includeOnly")]
        public bool IncludeOnly { get; set; }

        #endregion

        #region constructors

        public IndexedColumn()
        {
        }

        public IndexedColumn(string name, IndexOrder order, Index owner): this(name, order, false, owner) {}

        public IndexedColumn(string name, IndexOrder order, bool includeOnly, Index owner): base(name, owner)
        {
            this.Order = order;
            this.IncludeOnly = includeOnly;
        }

        #endregion

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = null;

            if (Util.StringEqual(childObjectUrn[ItemType], this.Name, true))
            {
                result = this;
            }

            return result;
        }
    }

    #region class IndexedColumnCollection

    public sealed class IndexedColumnCollection : ObjectCollection<IndexedColumn>
    {
        public IndexedColumnCollection(IList<IndexedColumn> list) : base(list) { }

        public IndexedColumnCollection()
        {
        }
    }

    #endregion
}