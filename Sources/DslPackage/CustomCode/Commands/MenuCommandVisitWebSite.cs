using System;
using System.ComponentModel.Design;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandVisitWebSite: MenuCommandDesignerBase
    {
        private const int commandId = 0x921;

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
            VersionUpgradeManager.OpenProductWebSite();
        }
    }
}