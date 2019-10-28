using System;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IScalarProperty: IPropertyBase
    {
        IDomainType Type { get; }

//        Type ClrType { get; }

        OrmKeyAttribute KeyAttribute { get; }
    }
}