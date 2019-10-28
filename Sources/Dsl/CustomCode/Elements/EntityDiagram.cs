using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class EntityDiagram
    {
        private CustomDesignSurfaceElementOperations elementOperations;

        public override DesignSurfaceElementOperations ElementOperations
        {
            get
            {
                if (elementOperations == null)
                {
                    elementOperations = new CustomDesignSurfaceElementOperations(Store, this);
                }

                return elementOperations;
            }
        }

        #region Custom Source for 'StructurePropertyTypeOfConnector'

        private NodeShape GetSourceShapeForStructurePropertyTypeOfConnector(StructurePropertyTypeOfConnector connector)
        {
            var shape = connector.ParentShape;
            return (NodeShape) shape;
        }

        private ModelElement GetSourceRolePlayerForLinkMappedByStructurePropertyTypeOfConnector(StructurePropertyTypeOfConnector connector)
        {
            ModelElement element = connector.FromShape.ModelElement;
            return element;
        }

        #endregion Custom Source for 'StructurePropertyTypeOfConnector'

        #region Custom Source and Target for 'ScalarPropertyTypeOfConnector'

        /*private NodeShape GetSourceShapeForScalarPropertyTypeOfConnector(ScalarPropertyTypeOfConnector connector)
        {
            var shape = connector.ParentShape;
            return (NodeShape)shape;
        }

        private NodeShape GetTargetShapeForScalarPropertyTypeOfConnector(ScalarPropertyTypeOfConnector connector)
        {
            var shape = connector.ParentShape;
            return (NodeShape)shape;
        }

        private ModelElement GetSourceRolePlayerForLinkMappedByScalarPropertyTypeOfConnector(ScalarPropertyTypeOfConnector connector)
        {
            ModelElement element = connector.FromShape.ModelElement;
            return element;
        }

        private ModelElement GetTargetRolePlayerForLinkMappedByScalarPropertyTypeOfConnector(ScalarPropertyTypeOfConnector connector)
        {
            ModelElement element = connector.FromShape.ModelElement;
            return element;
        }*/

        #endregion Custom Source and Target for 'ScalarPropertyTypeOfConnector'

    }
}