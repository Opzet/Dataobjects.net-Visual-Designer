using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;
using TXSoftware.DataObjectsNetEntityModel.Dsl.Properties;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class PersistentShape
    {
        public static readonly Dictionary<PersistentTypeKind, Color> persistentTypesColors
            = new Dictionary<PersistentTypeKind, Color>
              {
                  {PersistentTypeKind.Entity, Color.LightSteelBlue},
                  {PersistentTypeKind.Interface, Color.LightSteelBlue},
                  {PersistentTypeKind.Structure, Color.LightSteelBlue},
                  {PersistentTypeKind.TypedEntitySet, Color.Tan},
                  {PersistentTypeKind.ExternalType, Color.PaleTurquoise}
              };

        /// <summary>
        /// Model element that is being dragged.
        /// </summary>
        protected static ModelElement dragStartElement = null;
        /// <summary>
        /// Absolute bounds of the compartment, used to set the cursor.
        /// </summary>
        protected static RectangleD compartmentBounds;

        /// <summary>
        /// Gets the child compartment shape and verifies whether it can resize its parent compartment shape.
        /// </summary>
        /// <value></value>
        /// <returns>true if a child compartment shape can resize its parent compartment shape; otherwise, false.
        /// </returns>
        public override bool AllowsChildrenToResizeParent
        {
            get
            {
                // Overridden to return false so that the visual layout issue is solved, where a shape with a connector that "has custom source"
                // resizes to make the source shape wider until it reaches the edge of the referenced shape.
                // See http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/4cc74f9e-1949-43ba-8407-934f6664167d.
                // It is applied to the base class here so that all inheriting compartment shapes automatically have this fix applied.
                return false;
            }
        }


        public override void EnsureCompartments()
        {
            base.EnsureCompartments();

            foreach (Compartment compartment in this.NestedChildShapes.OfType<Compartment>())
            {
//                if (compartment.Name == "Indexes")
//                {
//                    // for Indexes compartment it will be done in "InterfaceShape" class
//                    continue;
//                }

                compartment.MouseDown += new DiagramMouseEventHandler(Compartment_MouseDown);
                compartment.MouseUp += new DiagramMouseEventHandler(Compartment_MouseUp);
                compartment.MouseMove += new DiagramMouseEventHandler(Compartment_MouseMove);
            }

            UpdateFillColorAccordingToKind();
        }

        /// <summary>
        /// Remember which item the mouse was dragged from.
        /// We don't create an Action immediately, as this would inhibit the
        /// inline text editing feature. Instead, we just remember the details
        /// and will create an Action when/if the mouse moves off this list item.
        /// </summary>
        private void Compartment_MouseDown(object sender, DiagramMouseEventArgs e)
        {
            Compartment compartment = (Compartment) sender;
            var compartmentElementToDrag = GetCompartmentElementToDrag(compartment, e.HitDiagramItem.RepresentedElements);
            dragStartElement = compartmentElementToDrag;
            compartmentBounds = e.HitDiagramItem.Shape.AbsoluteBoundingBox;
        }

        /// <summary>
        /// When the mouse moves away from the initial list item, but still inside the compartment,
        /// create an Action to supervise the cursor and handle subsequent mouse events.
        /// Transfer the details of the initial mouse position to the Action.
        /// </summary>
        private void Compartment_MouseMove(object sender, DiagramMouseEventArgs e)
        {
            if (dragStartElement != null)
            {
                Compartment compartment = (Compartment)sender;
                var compartmentElementToDrag = GetCompartmentElementToDrag(compartment, e.HitDiagramItem.RepresentedElements);

                if (dragStartElement != compartmentElementToDrag)
                {
                    e.DiagramClientView.ActiveMouseAction = CreateCompartmentDragMouseAction(compartment, dragStartElement);
                    dragStartElement = null;
                }
            }
        }

        /// <summary>
        /// User has released the mouse button. 
        /// </summary>
        private void Compartment_MouseUp(object sender, DiagramMouseEventArgs e)
        {
            dragStartElement = null;
        }

        protected virtual ModelElement GetCompartmentElementToDrag(Compartment compartment, ICollection representedElements)
        {
            return representedElements.OfType<PropertyBase>().FirstOrDefault();
        }

        protected virtual MouseAction CreateCompartmentDragMouseAction(Compartment compartment, ModelElement draggingElement)
        {
            return new CompartmentDragMouseAction<PersistentShape, PropertyBase>(
                (PropertyBase)draggingElement, this, compartmentBounds);
        }

        public override void OnMouseUp(DiagramMouseEventArgs e)
        {
            base.OnMouseUp(e);
            dragStartElement = null;
        }



        protected override void InitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
        {
            base.InitializeDecorators(shapeFields, decorators);

            CustomInitializeDecorators(shapeFields, decorators);
        }

        private void UpdateFillColorAccordingToKind()
        {
            PersistentType persistentType = (PersistentType) this.ModelElement;
            if (persistentType != null)
            {
                this.FillColor = persistentTypesColors[persistentType.TypeKind];
            }
        }

        protected virtual void CustomInitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
        {
            
        }

        #region Provide Icons For Compartment Entries

        protected override CompartmentMapping[] GetCompartmentMappings(Type melType)
        {
            CompartmentMapping[] mappings = base.GetCompartmentMappings(melType);

            foreach (ElementListCompartmentMapping mapping in mappings)
            {
                mapping.ImageGetter = CompartmentImageProvider;
            }
            return mappings;
        }

        /// <summary>
        /// Determines the icon to show in a compartment entry, based on its properties.
        /// </summary>
        /// <param name="element">The configuration property being shown in the compartment.</param>
        /// <returns>The icon to use to represent the configuration property.</returns>
        protected virtual Image CompartmentImageProvider(ModelElement element)
        {
            Image propertyImage = Resources.Property;
            
            PropertyBase prop = (PropertyBase)element;
            if (prop is ScalarProperty)
            {
                ScalarProperty scalarProperty = (ScalarProperty) prop;
                var nullable = scalarProperty.FieldAttribute.Nullable;

                if (scalarProperty.KeyAttribute.Enabled)
                {
                    propertyImage = Resources.PropertyKey;
                }
                else if (!nullable.IsDefault() && nullable.Value)
                {
                    propertyImage = Resources.PropertyNullable;
                }
            }
            else if (prop is StructureProperty)
            {
                propertyImage = Resources.StructureProperty;
            }
            else if (prop is NavigationProperty)
            {
                var navigationProperty = prop as NavigationProperty;
                propertyImage = navigationProperty.KeyAttribute.Enabled
                    ? Resources.NavigationPropertyKey
                    : Resources.NavigationProperty;
            }

            return propertyImage;
        }

        #endregion
    }
}