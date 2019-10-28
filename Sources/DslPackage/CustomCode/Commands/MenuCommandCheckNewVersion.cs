using System;
using System.ComponentModel.Design;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandCheckNewVersion: MenuCommandDesignerBase
    {
        public const int commandId = 0x920;

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
            VersionUpgradeManager.CheckForUpdate(true, false);
        }
    }
}