using System.ComponentModel.Design;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandAddScalarProperty : MenuCommandAddPropertyBase
    {
        public const int commandId = 0x801;

        public override int CommandId
        {
            get { return commandId; }
        }

        protected override void InternalQueryStatus(MenuCommand menuCommand, CurrentModelSelection currentSelection, 
            bool compartmentNavigationProperties, bool compartmentIndexes)
        {
//            menuCommand.Visible = !compartmentNavigationProperties && !compartmentIndexes;
//            if (menuCommand.Visible && currentSelection.IsPersistentTypeSelected)
//            {
//                menuCommand.Visible =
//                    !currentSelection.CurrentPersistentType.TypeKind.In(PersistentTypeKind.ExternalType,
//                    PersistentTypeKind.TypedEntitySet);
//            }

            var currentPersistentType = currentSelection.CurrentPersistentType;

            menuCommand.Visible = (currentSelection.IsCompartmentSelected && !compartmentNavigationProperties &&
                                   !compartmentIndexes)
                                  ||
                                  (currentSelection.IsPersistentTypeSelected &&
                                   currentPersistentType.TypeKind.In(PersistentTypeKind.ExternalType,
                                       PersistentTypeKind.TypedEntitySet));
        }

        protected override PropertyKind GetPropertyKind()
        {
            return PropertyKind.Scalar;
        }
    }
}