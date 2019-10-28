using System;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public static partial class AssociationConnectorBuilder
    {
        public static bool CanAcceptSource(ModelElement sourceElement)
        {
            return TestPersistentType(sourceElement,
                                      persistentType =>
                                      persistentType.TypeKind.In(PersistentTypeKind.Interface, PersistentTypeKind.Entity,
                                                                 PersistentTypeKind.TypedEntitySet));
        }

        private static bool TestPersistentType(ModelElement sourceElement, 
                                               Func<PersistentType, bool> validateFunc)
        {
            PersistentType persistentType;
            return TestPersistentType(sourceElement, out persistentType, validateFunc);
        }

        private static bool TestPersistentType(ModelElement sourceElement, out PersistentType persistentType, 
            Func<PersistentType, bool> validateFunc)
        {
            persistentType = sourceElement as PersistentType;
            if (persistentType != null)
            {
                return validateFunc(persistentType);
            }

            return false;
        }

        public static bool CanAcceptSourceAndTarget(ModelElement sourceElement, ModelElement targetElement)
        {
            PersistentType sourcePersistentType;
            PersistentType targetPersistentType;

            bool sourceOk = TestPersistentType(sourceElement, out sourcePersistentType,
                                               persistentType =>
                                               persistentType.TypeKind.In(PersistentTypeKind.Interface,
                                                                          PersistentTypeKind.Entity,
                                                                          PersistentTypeKind.TypedEntitySet));
            bool targetOk = false;

            if (sourceOk)
            {
                targetOk = TestPersistentType(targetElement, out targetPersistentType,
                            delegate(PersistentType persistentType)
                                {
                                    bool isValid = persistentType.TypeKind.In(PersistentTypeKind.Interface,
                                        PersistentTypeKind.Entity, PersistentTypeKind.TypedEntitySet);

                                    if (isValid && persistentType.TypeKind == PersistentTypeKind.TypedEntitySet)
                                    {
                                        // if source is 'TypedEntitySet' and target is 'TypedEntitySet' - disallow it!
                                        isValid = sourcePersistentType.TypeKind != PersistentTypeKind.TypedEntitySet;
                                    }

                                    return isValid;
                                });

                if (targetOk && sourcePersistentType.TypeKind.In(PersistentTypeKind.Interface, PersistentTypeKind.Entity))
                {
                    // if source is 'Interface' or 'Entity' and target is 'TypedEntitySet' - disallow it!
                    targetOk = targetPersistentType.TypeKind != PersistentTypeKind.TypedEntitySet;
                }
            }

            return sourceOk && targetOk;
        }

        public static void Connect(ModelElement sourceElement, ModelElement targetElement)
        {
            PersistentType sourceEntity = (PersistentType) sourceElement;
            PersistentType targetEntity = (PersistentType) targetElement;

            if (sourceEntity is TypedEntitySet && targetEntity is Interface)
            {
                TypedEntitySetHasItemType connection = new TypedEntitySetHasItemType((TypedEntitySet)sourceEntity, (Interface)targetEntity);

                if (DomainClassInfo.HasNameProperty(connection))
                {
                    DomainClassInfo.SetUniqueName(connection);
                }
                
                return;
            }

            IAssociationInfo sourceInfo = new AssociationInfo
                {
                    Multiplicity =MultiplicityKind.One,
                    OnOwnerRemove =  AssociationOnRemoveAction.Default,
                    OnTargetRemove = AssociationOnRemoveAction.Default
                };
            sourceInfo.PairTo.SetAsCustom(BuildNavigationPropertyName(sourceEntity, targetEntity, true));

            IAssociationInfo targetInfo = new AssociationInfo
            {
                Multiplicity = MultiplicityKind.Many,
                OnOwnerRemove = AssociationOnRemoveAction.Default,
                OnTargetRemove = AssociationOnRemoveAction.Default
            };
            targetInfo.PairTo.SetAsCustom(BuildNavigationPropertyName(sourceEntity, targetEntity, false));

            string associationName = BuildAssociationName(sourceEntity, targetEntity);

            CreatePersistentTypesAssociation(sourceElement, targetElement, sourceInfo, targetInfo, associationName,
                true, true);
        }

        public static IPersistentTypeHasAssociations CreatePersistentTypesAssociation(ModelElement sourceElement,
            ModelElement targetElement, IAssociationInfo sourceInfo, IAssociationInfo targetInfo, string associationName,
            bool createPropertyEnd1, bool createPropertyEnd2)
        {
            PersistentType sourceEntity = (PersistentType)sourceElement;
            PersistentType targetEntity = (PersistentType)targetElement;
            PersistentTypeHasAssociations typeAssociations = new PersistentTypeHasAssociations(sourceEntity, targetEntity);

            typeAssociations.End1.Multiplicity = sourceInfo.Multiplicity;
            typeAssociations.End1.OnOwnerRemove = sourceInfo.OnOwnerRemove;
            typeAssociations.End1.OnTargetRemove = sourceInfo.OnTargetRemove;
            typeAssociations.End1.UseAssociationAttribute = sourceInfo.UseAssociationAttribute;

            // association End2 values
            typeAssociations.End2.Multiplicity = targetInfo.Multiplicity;
            typeAssociations.End2.OnOwnerRemove = targetInfo.OnOwnerRemove;
            typeAssociations.End2.OnTargetRemove = targetInfo.OnTargetRemove;
            typeAssociations.End2.UseAssociationAttribute = targetInfo.UseAssociationAttribute;

            typeAssociations.Name = associationName;


            NavigationProperty sourceNavigationProperty = createPropertyEnd1 ? new NavigationProperty(sourceElement.Partition) : null;

            //bool isToSelfLookup = sourceInfo == targetInfo;

            //NavigationProperty targetNavigationProperty = isToSelfLookup ? null : new NavigationProperty(targetElement.Partition);
            NavigationProperty targetNavigationProperty = null;
            //if (!isToSelfLookup || !createPropertyEnd1)
            if (createPropertyEnd2)
            {
                targetNavigationProperty = new NavigationProperty(targetElement.Partition);
            }


            if (sourceNavigationProperty != null)
            {
                sourceNavigationProperty.PersistentTypeOfNavigationProperty = sourceEntity;
                sourceNavigationProperty.Name = sourceInfo.PairTo.Value;
                sourceNavigationProperty.Multiplicity = targetInfo.Multiplicity;
                sourceNavigationProperty.PersistentTypeHasAssociations = typeAssociations;
            }

            if (targetNavigationProperty != null)
            {
                targetNavigationProperty.PersistentTypeOfNavigationProperty = targetEntity;
                targetNavigationProperty.Name = targetInfo.PairTo.Value;
                targetNavigationProperty.Multiplicity = sourceInfo.Multiplicity;
                targetNavigationProperty.PersistentTypeHasAssociations = typeAssociations;
            }

            typeAssociations.End1.PairTo.SetAsCustom(createPropertyEnd1 ? sourceNavigationProperty.Name : targetNavigationProperty.Name);
            typeAssociations.End2.PairTo.SetAsCustom(createPropertyEnd2 ? targetNavigationProperty.Name : sourceNavigationProperty.Name);

            return typeAssociations;
        }

        internal static string BuildAssociationName(PersistentType sourceEntity, PersistentType targetEntity)
        {
            string result = string.Format("{0}{1}", sourceEntity.Name, targetEntity.Name);

            bool nameUsed = true;
            var associationsLink = PersistentTypeHasAssociations.GetLinksToPersistentTypeAssociations(sourceEntity);
            while (nameUsed)
            {
                nameUsed = associationsLink.Any(item => Util.StringEqual(item.Name, result, true));
                if (nameUsed)
                {
                    result += "_1";
                }
            }

            return result;
        }

        private static string BuildNavigationPropertyName(PersistentType sourceEntity, PersistentType targetEntity,
            bool forSource)
        {
            PersistentType entity = forSource ? sourceEntity : targetEntity;
            PersistentType oppositeEntity = forSource ? targetEntity : sourceEntity;

            string navigationPropertyName = oppositeEntity.Name;

            bool nameUsed = true;
            while (nameUsed)
            {
                nameUsed = entity.NavigationProperties.Any(property => Util.StringEqual(property.Name, navigationPropertyName, true));
                if (nameUsed)
                {
                    navigationPropertyName += "_1";
                }
            }

            return navigationPropertyName;
        }
    }
}