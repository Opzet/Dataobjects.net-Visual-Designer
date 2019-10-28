using System;
using System.Collections.Generic;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class StructureProperty : IStructureProperty
    {
        protected override void BuildTypeAttributes(List<IOrmAttribute> typeAttributes)
        {}

        protected override PropertyKind GetPropertyKindValue()
        {
            return PropertyKind.Structure;
        }

        protected override void OnCopy(Microsoft.VisualStudio.Modeling.ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);

            StructureProperty sourceProperty = (StructureProperty) sourceElement;
            this.Type = sourceProperty.Type;
        }

        #region Implementing interface IStructureProperty

        IStructure IStructureProperty.TypeOf
        {
            get
            {
                return this.Type;
            }
        }

        #endregion Implementing interface IStructureProperty
    }
}