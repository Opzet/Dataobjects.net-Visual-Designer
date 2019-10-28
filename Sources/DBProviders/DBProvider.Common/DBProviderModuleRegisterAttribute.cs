using System;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class DBProviderModuleRegisterAttribute: Attribute
    {
        public Type Type { get; set; }

        public DBProviderModuleRegisterAttribute(Type type)
        {
            this.Type = type;
        }
    }
}