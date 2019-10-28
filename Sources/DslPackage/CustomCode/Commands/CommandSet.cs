using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class DONetEntityModelDesignerCommandSet
    {
        internal new System.Collections.ICollection CurrentSelection
        {
            get { return base.CurrentSelection; }
        }

        internal void SelectModelElement(params ModelElement[] modelElements)
        {
            if (modelElements != null && modelElements.Length > 0)
            {
                CurrentModelingDocView.SelectObjects((uint) modelElements.Length, new[] {modelElements}, 0);
            }
        }

        protected override IList<MenuCommand> GetMenuCommands()
        {
            IList<MenuCommand> commands = base.GetMenuCommands();

            // Entity operations commands
            RegisterCommand<MenuCommandAddScalarProperty>(commands);
            RegisterCommand<MenuCommandAddStructureProperty>(commands);
            RegisterCommand<MenuCommandAddNavigationProperty>(commands);
            RegisterCommand<MenuCommandAddEntityIndex>(commands);

            // Zoom commands
            RegisterCommand<MenuCommandsZoom25>(commands);
            RegisterCommand<MenuCommandsZoom50>(commands);
            RegisterCommand<MenuCommandsZoom100>(commands);
            RegisterCommand<MenuCommandsZoom150>(commands);
            RegisterCommand<MenuCommandsZoom200>(commands);
            RegisterCommand<MenuCommandsZoomToFit>(commands);

            // Export and Layout
            RegisterCommand<MenuCommandExportDiagram>(commands);
            RegisterCommand<MenuCommandLayoutDiagram>(commands);

            // Add operations
            RegisterCommand<MenuCommandAddPersistentType>(commands);
            RegisterCommand<MenuCommandImplementInterface>(commands);
            RegisterCommand<MenuCommandAddAssociation>(commands);
            
            // Import DB Schema
            RegisterCommand<MenuCommandImportDBSchema>(commands);
            RegisterCommand<MenuCommandCheckNewVersion>(commands);
            RegisterCommand<MenuCommandVisitWebSite>(commands);

            // Add external type (from model explorer node)
            //RegisterCommand<MenuCommandAddExternalType>(commands, Constants.DONetEntityModelDesignerExplorerCommandSetId);

            return commands;
        }

        private void RegisterCommand<T>(IList<MenuCommand> globalList) where T : MenuCommandDesignerBase, new()
        {
            var menuCommand = CommandsUtil.CreateCommand<T>(this);
            globalList.Add(menuCommand);
        }

//        private void RegisterCommand<T>(IList<MenuCommand> globalList, string menuGroup = Constants.DONetEntityModelDesignerCommandSetId) 
//            where T : MenuCommandDesignerBase, new()
//        {
//            T command = new T {Owner = this};
//            globalList.Add(CreateDynamicStatusMenuCommand(command.QueryStatus,
//                command.ExecCommand,
//                new CommandID(new Guid(menuGroup),
//                    command.CommandId)));
//        }
//
//        private DynamicStatusMenuCommand CreateDynamicStatusMenuCommand(Action<MenuCommand> onStatusAction, 
//            Action<MenuCommand> onMenuAction, CommandID commandID)
//        {
//            var menuCommand = new DynamicStatusMenuCommand(
//                (sender, eventArgs) => onStatusAction(sender as MenuCommand),
//                (sender, eventArgs) => onMenuAction(sender as MenuCommand), 
//                commandID);
//
//            return menuCommand;
//        }

        internal ModelingDocData GetModelingDocData()
        {
            return this.CurrentDocData;
        }

        internal DiagramDocView GetDiagramDocView()
        {
            return this.CurrentDocView;
        }
    }
}