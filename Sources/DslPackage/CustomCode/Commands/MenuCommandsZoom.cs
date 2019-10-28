using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal abstract class MenuCommandsZoom: MenuCommandDesignerBase
    {
        public override void QueryStatus(MenuCommand menuCommand)
        {
            menuCommand.Visible = true;
            menuCommand.Enabled = true;
        }

        protected abstract float ZoomFactor { get; }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            DiagramDocView diagramDocView = GetCurrentSelectedPersistentType().DiagramDocView;
            Diagram currentDiagram = diagramDocView.CurrentDiagram;
            new DiagramUtil().GetDiagramClientView(currentDiagram)
                .SetZoomFactor(ZoomFactor, currentDiagram.Center, true);
        }
    }

    #region class MenuCommandsZoom25

    internal class MenuCommandsZoom25 : MenuCommandsZoom
    {
        internal const int commandId = 0x820;
        internal const float zoomFactor = 0.25f;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override float ZoomFactor
        {
            get { return zoomFactor; }
        }
    }

    #endregion class MenuCommandsZoom25

    #region class MenuCommandsZoom50

    internal class MenuCommandsZoom50 : MenuCommandsZoom
    {
        internal const int commandId = 0x821;
        internal const float zoomFactor = 0.50f;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override float ZoomFactor
        {
            get { return zoomFactor; }
        }
    }

    #endregion class MenuCommandsZoom50

    #region class MenuCommandsZoom100

    internal class MenuCommandsZoom100 : MenuCommandsZoom
    {
        internal const int commandId = 0x822;
        internal const float zoomFactor = 1f;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override float ZoomFactor
        {
            get { return zoomFactor; }
        }
    }

    #endregion class MenuCommandsZoom100

    #region class MenuCommandsZoom150

    internal class MenuCommandsZoom150 : MenuCommandsZoom
    {
        internal const int commandId = 0x823;
        internal const float zoomFactor = 1.5f;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override float ZoomFactor
        {
            get { return zoomFactor; }
        }
    }

    #endregion class MenuCommandsZoom150

    #region class MenuCommandsZoom200

    internal class MenuCommandsZoom200 : MenuCommandsZoom
    {
        internal const int commandId = 0x824;
        internal const float zoomFactor = 2f;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override float ZoomFactor
        {
            get { return zoomFactor; }
        }
    }

    #endregion class MenuCommandsZoom200

    #region class MenuCommandsZoomToFit

    internal class MenuCommandsZoomToFit : MenuCommandsZoom
    {
        internal const int commandId = 0x825;
        internal const float zoomFactor = 1f;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override float ZoomFactor
        {
            get { return zoomFactor; }
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            DiagramDocView diagramDocView = GetCurrentSelectedPersistentType().DiagramDocView;
            new DiagramUtil().GetDiagramClientView(diagramDocView.CurrentDiagram).ZoomToFit();
        }
    }

    #endregion class MenuCommandsZoomToFit
}