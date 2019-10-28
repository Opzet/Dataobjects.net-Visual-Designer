using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public abstract class Key<T>: DBObject<TableBase> where T: TableBase
    {
        #region properties

        public abstract override DbItemType ItemType { get; }

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

        #region constructors

        protected Key(string name, TableBase owner): base(name, owner) {}

        protected Key()
        {
        }

        #endregion
    }
}