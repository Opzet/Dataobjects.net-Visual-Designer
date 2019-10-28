using System;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class EntityBaseHasBaseType
    {
        private string GetBaseTypeValue()
        {
            return this.TargetEntityBase == null ? string.Empty : TargetEntityBase.Name;
        }

        private string GetDerivedTypeValue()
        {
            return this.SourceEntityBase == null ? string.Empty : SourceEntityBase.Name;
        }
    }
}