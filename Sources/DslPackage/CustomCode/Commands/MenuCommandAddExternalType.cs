using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandAddExternalType : MenuCommandExplorerBase
    {
        public const int commandId = 0x900;

        public override int CommandId
        {
            get { return commandId; }
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            ExplorerTreeNode node = Owner.GetSelectedNode();
            ModelElement representedElement = node.RepresentedElement;
            DomainRoleInfo domainRoleInfo = node.RepresentedRole;

            bool enabled = (representedElement != null && representedElement is EntityModel) ||
                           (domainRoleInfo != null &&
                            domainRoleInfo.Id == EntityModelHasDomainTypes.DomainTypeDomainRoleId);



            menuCommand.Visible = enabled;
            menuCommand.Enabled = enabled;
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            EntityModel entityModel = Owner.GetEntityModel();
            DomainType externalType = null;
            entityModel.Store.MakeActionWithinTransaction("Add new external type...",
                delegate
                {
                    string[] existingExternalTypeNames = entityModel.DomainTypes.Where(item => item.Namespace == "System").Select(item => item.Name).ToArray();

                    externalType = new DomainType(entityModel.Partition);
                    entityModel.DomainTypes.Add(externalType);
                    externalType.Name = Util.GenerateUniqueName(existingExternalTypeNames, "Name");
                    externalType.Namespace = "System";
                });

            if (externalType != null)
            {
                Owner.SelectTreeNodeByText(externalType.FullName);
            }
        }
    }
}