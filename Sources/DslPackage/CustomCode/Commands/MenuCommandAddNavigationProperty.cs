using System.ComponentModel.Design;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandAddNavigationProperty : MenuCommandAddPropertyBase
    {
        public const int commandId = 0x803;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override void InternalQueryStatus(MenuCommand menuCommand, CurrentModelSelection currentSelection,
                                                    bool compartmentNavigationProperties, bool compartmentIndexes)
        {
            var currentPersistentType = currentSelection.CurrentPersistentType;

            menuCommand.Visible = (currentSelection.IsCompartmentSelected && compartmentNavigationProperties && !compartmentIndexes)
                                    ||
                                  (currentSelection.IsPersistentTypeSelected && currentPersistentType.TypeKind != PersistentTypeKind.Structure);
        }

        protected override PropertyKind GetPropertyKind()
        {
            return PropertyKind.Navigation;
        }
    }
}