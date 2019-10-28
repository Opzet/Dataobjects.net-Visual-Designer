using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class TypedEntitySetShape
    {

        public override void EnsureCompartments()
        {
            base.EnsureCompartments();
            foreach (Compartment comp in this.NestedChildShapes)
            {
                comp.TitleVisibility = false;
                comp.SetShowHideState(false);
            }
        }


        public override SizeD MaximumSize
        {
            get { return this.DefaultSize; }
        }

        public override ResizeDirection AllowsChildrenToShrinkParent
        {
            get
            {
                return ResizeDirection.None;

            }
        }

        protected override void CustomInitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
        {
            base.CustomInitializeDecorators(shapeFields, decorators);
            HideExpandCollapseDecorators(decorators);
        }

        private void HideExpandCollapseDecorators(IList<Decorator> decorators)
        {
            foreach (var decorator in decorators)
            {
                if (decorator is ExpandCollapseDecorator)
                {
                    decorator.Field.DefaultVisibility = false;
                }
            }
        }
    }
}