using System;
using System.ComponentModel.Design;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandAddEntityIndex: MenuCommandDesignerBase
    {
        public const int commandId = 0x804;

        public override int CommandId
        {
            get { return commandId; }
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            var currentModelSelection = GetCurrentSelectedPersistentType();
            menuCommand.Visible = currentModelSelection.IsPersistentTypeSelected && currentModelSelection.CurrentPersistentType is Interface;

            if (!menuCommand.Visible && currentModelSelection.IsCompartmentSelected)
            {
                menuCommand.Visible = Util.StringEqual(currentModelSelection.CompartmentName, 
                    MenuCommandAddPropertyBase.COMPARTMENT_NAME_INDEXES, true);
            }
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            var currentSelection = GetCurrentSelectedPersistentType();
            PersistentType currentPersistentType = currentSelection.CurrentPersistentType;

            EntityIndex newIndex = null;
            currentSelection.MakeActionWithinTransaction(
                    string.Format("Add entity index into '{0}'", currentPersistentType.Name),
                    () =>
                    {
                        Interface ownerInterface = (Interface) currentPersistentType;
                        newIndex = new EntityIndex(currentPersistentType.Partition);
                        ownerInterface.Indexes.Add(newIndex);
                        newIndex.Name = string.Format("Index{0}", ownerInterface.Indexes.Count);
                    });

            if (newIndex != null)
            {
                DiagramUtil.SelectCompartmentItem(currentSelection.DiagramDocView,
                    currentPersistentType, "Indexes", newIndex);
                
            }
        }
    }
}