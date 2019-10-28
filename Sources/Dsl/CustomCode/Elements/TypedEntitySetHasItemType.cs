using System;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class TypedEntitySetHasItemType
    {
        private string GetResultTypeNameValue()
        {
            Interface itemType = this.TypedEntitySet.ItemType;
            return itemType == null ? string.Empty : string.Format("EntitySet<{0}>", itemType.Name);
        }
    }
}