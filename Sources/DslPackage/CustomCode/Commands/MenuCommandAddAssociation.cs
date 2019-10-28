using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandAddAssociation : MenuCommandDesignerBase
    {
        private const int commandId = 0x862;

        public override int CommandId
        {
            get { return commandId; }
        }

        public override void QueryStatus(MenuCommand menuCommand)
        {
            CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
            bool selectedEntityDiagram = modelSelection.GetFromSelection<EntityDiagram>(false).Any();

            menuCommand.Visible = selectedEntityDiagram ||
                                  (modelSelection.IsPersistentTypeSelected &&
                                   modelSelection.CurrentPersistentType.TypeKind.In(PersistentTypeKind.Entity,
                                       PersistentTypeKind.Interface, PersistentTypeKind.Structure));
        }

        public override void ExecCommand(MenuCommand menuCommand)
        {
            CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
            EntityDiagram entityDiagram = modelSelection.GetFromSelection<EntityDiagram>(false).SingleOrDefault();
            PersistentType persistentType = modelSelection.IsPersistentTypeSelected
                                                ? modelSelection.CurrentPersistentType
                                                : null;
            

            Store store = persistentType != null ? persistentType.Store : entityDiagram.Store;

            IEnumerable<PersistentTypeItem> existingTypeNames =
                store.ElementDirectory.FindElements<PersistentType>()
                .Where(item => item.TypeKind.In(PersistentTypeKind.Entity, PersistentTypeKind.Interface, PersistentTypeKind.Structure))
                .Select(item => new PersistentTypeItem(item.Name, ModelUtil.ConvertTo(item.TypeKind), 
                    item.AllProperties.Select(prop => prop.Name).ToArray()));

            PersistentTypeItem persistentTypeEnd1 = persistentType == null
                                                        ? null
                                                        : new PersistentTypeItem(
                                                              persistentType.Name,
                                                              ModelUtil.ConvertTo(persistentType.TypeKind),
                                                              persistentType.AllProperties.Select(prop => prop.Name).
                                                              ToArray());

            Func<PersistentTypeItem, PersistentType> findTypeFunc =
                (typeItemd) => store.ElementDirectory
                    .FindElements<PersistentType>()
                    .Where(item => item.Name == typeItemd.Name && ModelUtil.ConvertTo(item.TypeKind) == typeItemd.Kind)
                    .SingleOrDefault();

            IEnumerable<string> existingAssociations =
                store.ElementDirectory.FindElements<PersistentTypeHasAssociations>().Select(assoc => assoc.Name).ToArray();

            if (existingTypeNames.Count() < 2)
            {
                MessageBox.Show("There must be at least 2 persistent types to make association.", "Add Association...",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //FormAddAssociation.ResultData resultData;
            //if (FormAddAssociation.DialogShow(existingTypeNames, existingAssociations, persistentTypeEnd1, out resultData))

            FormAddAssociation.ResultData resultData;
            if (FormAddAssociation.DialogShow(existingTypeNames, existingAssociations, persistentTypeEnd1, out resultData))
            {
                modelSelection.MakeActionWithinTransaction(string.Format("Add new association '{0}'", resultData.AssociationName),
                    delegate
                    {
                        PersistentType sourcePersistentType = findTypeFunc(resultData.PersistentItem1.TypeItem);
                        PersistentType targetPersistentType = findTypeFunc(resultData.PersistentItem2.TypeItem);

                        IAssociationInfo sourceInfo = new AssociationInfo
                            {
                                Multiplicity = resultData.PersistentItem2.Multiplicity,
                                OnOwnerRemove = AssociationOnRemoveAction.Default,
                                OnTargetRemove = AssociationOnRemoveAction.Default,
                                UseAssociationAttribute = resultData.PersistentItem2.UseAssociationAttribute
                            };
                        sourceInfo.PairTo.SetAsCustom(resultData.PersistentItem1.PropertyName);

                        IAssociationInfo targetInfo = new AssociationInfo
                            {
                                Multiplicity = resultData.PersistentItem1.Multiplicity,
                                OnOwnerRemove = AssociationOnRemoveAction.Default,
                                OnTargetRemove = AssociationOnRemoveAction.Default,
                                UseAssociationAttribute = resultData.PersistentItem1.UseAssociationAttribute
                            };
                        targetInfo.PairTo.SetAsCustom(resultData.PersistentItem2.PropertyName);
                        
                        if (resultData.SimpleMode && sourcePersistentType == targetPersistentType)
                        {
                            targetInfo = sourceInfo;
                            targetPersistentType = sourcePersistentType;
                        }

                        var persistentTypesAssociation = AssociationConnectorBuilder.CreatePersistentTypesAssociation(sourcePersistentType,
                            targetPersistentType, sourceInfo, targetInfo, resultData.AssociationName,
                            true, !resultData.SimpleMode);

                        Owner.SelectModelElement((ModelElement)persistentTypesAssociation);
                    });
            }
        }
    }
}