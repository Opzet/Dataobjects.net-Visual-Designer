using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class MenuCommandAddPersistentType: MenuCommandDesignerBase
    {
        private const int commandId = 0x860;

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
            CurrentModelSelection modelSelection = GetCurrentSelectedPersistentType();
            EntityDiagram entityDiagram = modelSelection.GetFromSelection<EntityDiagram>(false).Single();

            EntityModel entityModel = entityDiagram.Store.ElementDirectory.FindElements<EntityModel>().Single();

            IEnumerable<Tuple<string, EntityKind>> existingTypeNames =
                entityDiagram.Store.ElementDirectory.FindElements<PersistentType>()
                .Where(item => item.TypeKind.In(PersistentTypeKind.Entity, PersistentTypeKind.Interface, PersistentTypeKind.Structure))
                .Select(item => new Tuple<string, EntityKind>(item.Name, ConvertTypeKind(item.TypeKind)));

            Func<string, EntityKind, PersistentType> findTypeFunc =
                (typeName, typeKind) => entityDiagram.Store.ElementDirectory
                    .FindElements<PersistentType>()
                    .Where(item => item.Name == typeName && ConvertTypeKind(item.TypeKind) == typeKind)
                    .SingleOrDefault();

            FormAddPersistentType.ResultData resultData;
            if (FormAddPersistentType.DialogShow(existingTypeNames, out resultData))
            {
                modelSelection.MakeActionWithinTransaction(string.Format("Add new {0} with name '{1}'", resultData.TypeKind, resultData.TypeName),
                    delegate
                    {
                        PersistentType newEntity = null;

                        switch (resultData.TypeKind)
                        {
                            case EntityKind.Entity:
                            {
                                newEntity = new Entity(entityModel.Partition);
                                if (resultData.BaseTypeInfo.Item1.HasValue)
                                {
                                    Entity entity = (Entity) newEntity;
                                    entity.BaseType = findTypeFunc(resultData.BaseTypeInfo.Item2, resultData.BaseTypeInfo.Item1.Value) as EntityBase;
                                }
                                break;
                            }
                            case EntityKind.Structure:
                            {
                                newEntity = new Structure(entityModel.Partition);
                                if (resultData.BaseTypeInfo.Item1.HasValue)
                                {
                                    Structure structure = (Structure)newEntity;
                                    structure.BaseType = findTypeFunc(resultData.BaseTypeInfo.Item2, resultData.BaseTypeInfo.Item1.Value) as EntityBase;
                                }
                                break;
                            }
                            case EntityKind.Interface:
                            {
                                newEntity = new Interface(entityModel.Partition);
                                if (resultData.BaseTypeInfo.Item1.HasValue)
                                {
                                    Interface @interface = (Interface)newEntity;
                                    Interface baseInterface = findTypeFunc(resultData.BaseTypeInfo.Item2, resultData.BaseTypeInfo.Item1.Value) as Interface;
                                    @interface.InheritedInterfaces.Add(baseInterface);
                                }
                                break;
                            }
                        }

                        newEntity.Name = resultData.TypeName;
                        entityModel.PersistentTypes.Add(newEntity);

                        // create key property
                        if (resultData.KeyPropertyInfo.Item1)
                        {
                            var newProperty = new ScalarProperty(newEntity.Partition);
                            newProperty.Name = resultData.KeyPropertyInfo.Item2;
                            newProperty.KeyAttribute.Enabled = true;
                            Guid typeId = SystemPrimitiveTypesConverter.GetTypeId(resultData.KeyPropertyInfo.Item3);
                            newProperty.Type = (DomainType) entityModel.GetDomainType(typeId);
                            newEntity.Properties.Add(newProperty);
                        }

                        Owner.SelectModelElement((ModelElement)newEntity);
                    });
            }
        }

        private EntityKind ConvertTypeKind(PersistentTypeKind typeKind)
        {
            switch (typeKind)
            {
                case PersistentTypeKind.Interface:
                {
                    return EntityKind.Interface;
                }
                case PersistentTypeKind.Entity:
                {
                    return EntityKind.Entity;
                }
                default:
                {
                    return EntityKind.Structure;
                }
            }
        }
    }
}