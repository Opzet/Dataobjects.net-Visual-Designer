using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IEntityIndex : IElement, IOrmAttribute
    {
        Defaultable<bool> Unique { get; }

        Defaultable<double> FillFactor { get; }

        Defaultable<string> IndexName { get; }

        OrmIndexFields Fields { get; }

        IInterface OwnerInterface { get; }
    }
}