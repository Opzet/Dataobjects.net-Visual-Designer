using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal abstract class MenuCommandAddPropertyBase: MenuCommandDesignerBase
    {
        protected internal const string COMPARTMENT_NAME_NAVIGATION_PROPERTIES = "NavigationProperties";
        protected internal const string COMPARTMENT_NAME_INDEXES = "Indexes";

        protected abstract PropertyKind GetPropertyKind();

        private PropertyBase CreateNewProperty(PersistentType ownerPersistentType, PropertyKind newPropertyKind)
        {
            int propertiesCount = 0;

            PropertyBase newProperty = null;
            switch (newPropertyKind)
            {
                case PropertyKind.Scalar:
                    newProperty = new ScalarProperty(ownerPersistentType.Partition);
                    ownerPersistentType.Properties.Add(newProperty);
                    propertiesCount = ownerPersistentType.Properties.OfType<ScalarProperty>().Count();
                    break;
                case PropertyKind.Structure:
                    newProperty = new StructureProperty(ownerPersistentType.Partition);
                    ownerPersistentType.Properties.Add(newProperty);
                    propertiesCount = ownerPersistentType.Properties.OfType<StructureProperty>().Count();
                    break;
                case PropertyKind.Navigation:
                    newProperty = new NavigationProperty(ownerPersistentType.Partition);
                    ownerPersistentType.NavigationProperties.Add((NavigationProperty)newProperty);
                    propertiesCount = ownerPersistentType.NavigationProperties.OfType<NavigationProperty>().Count();
                    break;
            }

            newProperty.Name = string.Format("{0}Property{1}", newPropertyKind, propertiesCount + 1);

            return newProperty;
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            CurrentModelSelection currentSelection = GetCurrentSelectedPersistentType();
            bool compartmentNavigationProperties = currentSelection.IsCompartmentSelected &&
                                                   Util.StringEqual(currentSelection.CompartmentName,
                                                                    COMPARTMENT_NAME_NAVIGATION_PROPERTIES, true);

            bool compartmentIndexes = currentSelection.IsCompartmentSelected &&
                                      Util.StringEqual(currentSelection.CompartmentName, COMPARTMENT_NAME_INDEXES, true);

            menuCommand.Visible = currentSelection.IsPersistentTypeSelected && 
                !currentSelection.CurrentPersistentType.TypeKind.In(PersistentTypeKind.ExternalType, PersistentTypeKind.TypedEntitySet);

            var selectedShapesCollection = currentSelection.DiagramDocView.CurrentDesigner.Selection;

            if (!menuCommand.Visible)
            {
                InternalQueryStatus(menuCommand, currentSelection, compartmentNavigationProperties, compartmentIndexes);
            }
        }

        protected abstract void InternalQueryStatus(MenuCommand menuCommand, CurrentModelSelection currentSelection,
                                                    bool compartmentNavigationProperties, bool compartmentIndexes);

        public override void ExecCommand(MenuCommand menuCommand)
        {
            var currentSelection = GetCurrentSelectedPersistentType();
            PersistentType currentPersistentType = currentSelection.CurrentPersistentType;

            PropertyBase newProperty = null;
            currentSelection.MakeActionWithinTransaction(string.Format("Add scalar property into '{0}'", currentPersistentType.Name),
                    () =>
                    {
                        newProperty = CreateNewProperty(currentPersistentType, GetPropertyKind());
                    });

            if (newProperty != null)
            {
                Owner.SelectModelElement((ModelElement)currentPersistentType);

                var propertyBase = newProperty as IPropertyBase;
                var persistentType = propertyBase.GetRealOwner();
                string compartmentName = propertyBase.PropertyKind == PropertyKind.Navigation
                                             ? "NavigationProperties"
                                             : "Properties";

                DiagramUtil.SelectCompartmentItem(currentSelection.DiagramDocView, 
                    (ModelElement) persistentType, compartmentName, newProperty);
            }
        }
    }
}