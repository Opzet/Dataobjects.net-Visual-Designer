using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ObjectCollection<TDBObject>: List<TDBObject> where TDBObject: DBObject
    {
        protected ObjectCollection(IEnumerable<TDBObject> list) : base(list) { }

        protected ObjectCollection()
        {
        }

        public TDBObject this[string name]
        {
            get { return this.SingleOrDefault(item => item.Name == name); }
        }

        public string SerializeToXml()
        {
            return Util.SerializeObjectToXml(this.GetType(), new Type[0], this);
        }
    }
}