using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;
using TXSoftware.DataObjectsNetEntityModel.Dsl.Properties;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class InterfaceShape
    {
        protected override CompartmentMapping[] GetCompartmentMappings(Type melType)
        {
            CompartmentMapping[] mappings = base.GetCompartmentMappings(melType);


            foreach (ElementListCompartmentMapping mapping in mappings)
            {
                mapping.ImageGetter = CompartmentImageProvider;
            }
            return mappings;
        }

        protected override Image CompartmentImageProvider(ModelElement element)
        {
            if (element is PropertyBase)
            {
                return base.CompartmentImageProvider(element);
            }

            if (element is EntityIndex)
            {
                EntityIndex index = (EntityIndex) element;
                return index.Unique.IsCustom() && index.Unique.Value ? Resources.IndexUnique : Resources.Index;
            }

            return Resources.Property;
        }

        protected override ModelElement GetCompartmentElementToDrag(Compartment compartment, ICollection representedElements)
        {
            if (compartment.Name == "Indexes")
            {
                return representedElements.OfType<EntityIndex>().FirstOrDefault();
            }

            return base.GetCompartmentElementToDrag(compartment, representedElements);
        }

        protected override MouseAction CreateCompartmentDragMouseAction(Compartment compartment, ModelElement draggingElement)
        {
            if (compartment.Name == "Indexes")
            {
                return new CompartmentDragMouseAction<InterfaceShape, EntityIndex>(
                    (EntityIndex) draggingElement, this, compartmentBounds);
            }
            
            return base.CreateCompartmentDragMouseAction(compartment, draggingElement);
        }
    }
}