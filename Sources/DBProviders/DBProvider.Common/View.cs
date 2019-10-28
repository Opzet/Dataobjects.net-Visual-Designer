using System;
using System.Collections.Generic;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    /// <summary>
    /// Reserved for future...
    /// </summary>
    public abstract class View : TableBase
    {
        public override DbItemType ItemType
        {
            get
            {
                return DbItemType.View;
            }
        }

        protected View(string name, Schema owner) : base(name, owner) { }
    }

    #region class ViewCollection

    public sealed class ViewCollection : ObjectCollection<View>
    {
        #region constructors

        public ViewCollection(IList<View> list) : base(list) { }

        public ViewCollection()
        {
        }

        #endregion
    }

    #endregion

}