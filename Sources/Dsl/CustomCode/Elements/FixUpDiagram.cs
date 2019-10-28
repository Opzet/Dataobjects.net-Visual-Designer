using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal sealed partial class FixUpDiagram
    {
        #region Custom Source for 'StructurePropertyTypeOfConnector'

        private ModelElement GetParentForStructurePropertyHasType(StructurePropertyHasType childElement)
        {
            ModelElement element = childElement.StructureProperty.PersistentType;

            return element;
        }

        #endregion Custom Source for 'StructurePropertyTypeOfConnector'

        #region Custom Source and Target for 'ScalarPropertyTypeOfConnector'

/*
        private ModelElement GetParentForScalarPropertyHasType(ScalarPropertyHasType childElement)
        {
            return childElement.ScalarProperty.PersistentType;
        }
*/

        #endregion Custom Source and Target for 'ScalarPropertyTypeOfConnector'


        #region Custom Source & Target for connector mapping 'AssociationConnector'

        private ModelElement GetParentForPersistentTypeHasAssociations(PersistentTypeHasAssociations childElement)
        {
            ModelElement element = childElement.SourcePersistentType;

            return element;
        }


        #endregion Custom Source & Target for connector mapping 'AssociationConnector'
    }
}