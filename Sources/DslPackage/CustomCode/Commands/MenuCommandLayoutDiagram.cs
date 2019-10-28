using System.ComponentModel.Design;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandLayoutDiagram: MenuCommandDesignerBase
    {
        private const int commandId = 0x841;

        public override int CommandId
        {
            get { return commandId; }
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
            bool enabled = modelSelection.GetFromSelection<EntityDiagram>(false).Any();
            menuCommand.Enabled = enabled;
            menuCommand.Visible = enabled;
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            var modelSelection = GetCurrentSelectedPersistentType();
            var diagramDocView = modelSelection.DiagramDocView;
            DiagramUtil.AutoLayout(diagramDocView.CurrentDiagram.NestedChildShapes, diagramDocView.CurrentDiagram);
            new DiagramUtil().GetDiagramClientView(diagramDocView.CurrentDiagram).ZoomToFit();
        }
    }
}