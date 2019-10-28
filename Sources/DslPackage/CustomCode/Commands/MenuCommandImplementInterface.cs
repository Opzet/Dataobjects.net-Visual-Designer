using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandImplementInterface : MenuCommandDesignerBase
    {
        public const int commandId = 0x861;

        public override int CommandId
        {
            get { return commandId; }
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
            bool enabled = modelSelection.GetFromSelection<Interface>().Where(item => item.TypeKind == PersistentTypeKind.Interface).Count() == 1;
            menuCommand.Enabled = enabled;
            menuCommand.Visible = enabled;
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            CurrentModelSelection currentSelection = GetCurrentSelectedPersistentType();
            IInterface sourceInterface = currentSelection.CurrentPersistentType as IInterface;

            FormImplementInterface.ImplementData resultData;

            var helperDic = new Dictionary<IInterface, FormImplementInterface.InterfaceTreeItem>();
            var sourceInterfaceItem = new FormImplementInterface.InterfaceTreeItem(sourceInterface.Name, sourceInterface);
            helperDic.Add(sourceInterface, sourceInterfaceItem);
            var currentInterfaceItem = sourceInterfaceItem;
            int currentLevel = 0;

            var inheritanceTree = InheritanceTreeCache.Get(sourceInterface); //sourceInterface.GetInheritanceTree();
            if (!inheritanceTree.TreeRebuilded)
            {
                inheritanceTree.RebuildTree(false);
            }

            inheritanceTree.IterateTree(false, delegate(InheritanceTreeIterator iterator)
            {
                if (iterator.Level != currentLevel)
                {
                    currentLevel = iterator.Level;
                    currentInterfaceItem = helperDic[iterator.Parent.Interface];
                }

                var childItem = new FormImplementInterface.InterfaceTreeItem(iterator.Current.Interface.Name, iterator.Current.Interface);
                childItem.Parent = currentInterfaceItem;
                currentInterfaceItem.Childs.Add(childItem);

                if (!helperDic.ContainsKey(iterator.Current.Interface))
                {
                    helperDic.Add(iterator.Current.Interface, childItem);
                }

            });


            if (FormImplementInterface.DialogShow("Implement interface into new Entity or Structure", "Type name",
                sourceInterfaceItem, out resultData))
            {
                PersistentTypeKind typeKind = resultData.EntityKind == EntityKind.Entity
                                                  ? PersistentTypeKind.Entity
                                                  : PersistentTypeKind.Structure;

                var rootItem = resultData.Root;
                IInterface rootInterface = (IInterface) rootItem.Data;

                (sourceInterface as PersistentType).Store.MakeActionWithinTransaction(
                    string.Format(
                        "Implementing root interface '{0}' (with in/direct inheritances) to new '{1}' named '{2}'",
                        rootInterface.Name, typeKind, resultData.Name),
                    delegate
                    {
                        IEntityBase newEntity = rootInterface.ImplementToNewType(typeKind, resultData.Name, ImplementTypeOptions.Default);

                        if (newEntity != null)
                        {
                            foreach (var interfaceToImplementItem in resultData.Selection.Where(item => item.Parent == null))
                            {
                                IInterface interfaceToImplement = (IInterface)interfaceToImplementItem.Data;
                                if (interfaceToImplement != rootInterface)
                                {
                                    interfaceToImplement.ImplementToType(newEntity, ImplementTypeOptions.Default);
                                }
                            }

                            Owner.SelectModelElement((ModelElement)newEntity);
                        }
                    });

            }
        }
    }
}