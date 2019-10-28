using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling.Shell;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal static class CommandsUtil
    {
        internal static DynamicStatusMenuCommand CreateCommand<T>(object owner) where T : MenuCommandBase, new()
        {
            T command = new T();
            command.SetOwner(owner);
            DynamicStatusMenuCommand dynamicStatusMenuCommand = CreateDynamicStatusMenuCommand(
                command.QueryStatus, command.ExecCommand,
                new CommandID(new Guid(GetMenuGroupFromOwner(owner)), command.CommandId));

            return dynamicStatusMenuCommand;
        }

        private static string GetMenuGroupFromOwner(object owner)
        {
            if (owner is DONetEntityModelDesignerCommandSet)
            {
                return Constants.DONetEntityModelDesignerCommandSetId;
            }
            else if (owner is DONetEntityModelDesignerExplorer)
            {
                return Constants.DONetEntityModelDesignerExplorerCommandSetId;
            }

            throw new ArgumentOutOfRangeException("owner");
        }

        private static DynamicStatusMenuCommand CreateDynamicStatusMenuCommand(Action<MenuCommand> onStatusAction,
            Action<MenuCommand> onMenuAction, CommandID commandID)
        {
            var menuCommand = new DynamicStatusMenuCommand(
                (sender, eventArgs) => onStatusAction(sender as MenuCommand),
                (sender, eventArgs) => onMenuAction(sender as MenuCommand),
                commandID);

            return menuCommand;
        }
    }
}