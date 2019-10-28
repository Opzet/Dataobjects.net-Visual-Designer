using System;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class InterfaceInheritInterfaces
    {
        private string GetBaseTypeValue()
        {
            return this.TargetInheritByInterface == null ? string.Empty : this.TargetInheritByInterface.Name;
        }

        private string GetDerivedTypeValue()
        {
            return this.SourceInheritInterface == null ? string.Empty : this.SourceInheritInterface.Name;
        }
    }
}