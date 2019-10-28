using System;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class DBProviderRegisterAttribute : Attribute
    {
        public Type Type { get; set; }

        public DBProviderRegisterAttribute(Type type)
        {
            this.Type = type;
        }
    }
}