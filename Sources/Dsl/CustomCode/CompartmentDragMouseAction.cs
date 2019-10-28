using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode
{
    /// <summary>
    /// Manage the mouse while dragging a compartment item.
    /// </summary>
    internal class CompartmentDragMouseAction<TShape, TElement> : MouseAction where TShape : CompartmentShape
        where TElement: ModelElement
    {
        private TElement sourceChild;
        private TShape sourceShape;
        private RectangleD sourceCompartmentBounds;

        public CompartmentDragMouseAction(TElement sourceChildElement, TShape sourceParentShape, RectangleD bounds)
            : base(sourceParentShape.Diagram)
        {
            sourceChild = sourceChildElement;
            sourceShape = sourceParentShape;
            sourceCompartmentBounds = bounds; // For cursor.
        }

        /// <summary>
        /// Call back to the source shape to drop the dragged item.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(DiagramMouseEventArgs e)
        {
            base.OnMouseUp(e);
            //sourceShape.DoMouseUp(sourceChild, e);
            DragDropHelper.DoMouseUpForShape<TElement>(sourceChild, e);
            this.Cancel(e.DiagramClientView);
            e.Handled = true;
        }

        /// <summary>
        /// Ideally, this shouldn't happen. This action should only be active
        /// while the mouse is still pressed. However, it can happen if you
        /// move the mouse rapidly out of the source shape, let go, and then 
        /// click somewhere else in the source shape. Yuk.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(DiagramMouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Cancel(e.DiagramClientView);
            e.Handled = false;
        }

        /// <summary>
        /// Display an appropriate cursor while the drag is in progress:
        /// Up-down arrow if we are inside the original compartment.
        /// No entry if we are elsewhere.
        /// </summary>
        /// <param name="currentCursor"></param>
        /// <param name="diagramClientView"></param>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        public override System.Windows.Forms.Cursor GetCursor(System.Windows.Forms.Cursor currentCursor, DiagramClientView diagramClientView, PointD mousePosition)
        {
            // If the cursor is inside the original compartment, show up-down cursor.
            return sourceCompartmentBounds.Contains(mousePosition)
             ? System.Windows.Forms.Cursors.SizeNS // Up-down arrow.
             : System.Windows.Forms.Cursors.No;
        }
    }
}