using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class Structure : IStructure
    {

        protected override PersistentTypeKind GetTypeKindValue()
        {
            return PersistentTypeKind.Structure;
        }

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateProperties(ValidationContext context)
        {
            PersistentTypeValidation.ValidateStructureProperties(this, context);
        }

        #region Implementing interface IStructure

        ReadOnlyCollection<IStructureProperty> IStructure.ReferencedInProperties
        {
            get
            {
                var items = this.TypeOf.OfType<StructureProperty>().ToArray();
                ReadOnlyCollection<IStructureProperty> result = new ReadOnlyCollection<IStructureProperty>(items);
                return result;

            }
        }

        #endregion Implementing interface IStructure
    }
}