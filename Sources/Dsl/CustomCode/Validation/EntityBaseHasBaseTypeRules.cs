using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules
{
    public sealed class EntityBaseHasBaseTypeRules
    {
        private const string ERROR_BASE_TYPE_CANNOT_BE_STRUCTURE = "Structure types cannot be set as base type for entity or interface. \n\rStructure can be set as base type only for another structure.";
        private const string ERROR_BASE_TYPE_CANNOT_BE_ENTITY_OR_INTERFACE = "Entity or interface types cannot be set as base type for structure.Structure can have only structures for base type.";
        private const string ERROR_CIRCULAR_REFERENCE = "Cannot set entity '{0}' as base type for entity '{1}', because entity '{1}' has already as base type entity '{0}' (Circular reference)";
        private const string ERROR_BASE_TYPE_IS_SEALED = "Cannot set entity '{0}' as base type for entity '{1}', because entity '{0}' is sealed";
        private const string ERROR_BASE_TYPE_CANNOT_BE_ITSELF = "Cannot set base type to itself!";
        private const string ERROR_INTERFACE_CANNOT_HAVE_BASE_TYPE_ENTITY_OR_STRUCTURE = "Interface cannot have base type pointing to entity or structure, only to another interface.";

        private const string CODE_BASE_TYPE = "BaseType";
        private const string CODE_INVALID_INHERITANCE = "InvalidInheritance";
        private const string ERROR_INCOMPATIBLE_ACCESS_MODIFIERS = "A type '{0}' with a public access modifier cannot inherit from an type '{1}' with an internal access modifier";
        private const string ERROR_LOOP_IN_INHERITANCE = "There's a loop in the inheritance of {0}";

        #region Rule classes

        [RuleOn(typeof(EntityBaseHasBaseType), FireTime = TimeToFire.LocalCommit)]
        public sealed class AddRule : Microsoft.VisualStudio.Modeling.AddRule
        {
            public override void ElementAdded(ElementAddedEventArgs e)
            {
                EntityBaseHasBaseType link = e.ModelElement as EntityBaseHasBaseType;
                Validate(link);
            }
        }

        [RuleOn(typeof(EntityBaseHasBaseType), FireTime = TimeToFire.LocalCommit)]
        public sealed class ChangeRule : Microsoft.VisualStudio.Modeling.ChangeRule
        {
            public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
            {
                EntityBaseHasBaseType link = e.ModelElement as EntityBaseHasBaseType;
                Validate(link);
            }
        }

        [RuleOn(typeof(EntityBaseHasBaseType), FireTime = TimeToFire.LocalCommit)]
        public sealed class RolePlayerChangeRule : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
        {
            public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
            {
                EntityBaseHasBaseType link = e.ElementLink as EntityBaseHasBaseType;
                Validate(link);
            }
        }

        #endregion Rule classes

        internal static ValidationResult Validate(EntityBaseHasBaseType link)
        {
            return Validate(link, true);
        }

        internal static ValidationResult Validate(EntityBaseHasBaseType link, bool workWithTransaction)
        {
            ValidationResult result = new ValidationResult();

            if (link != null && (!workWithTransaction || !link.CurrentTransactionIsSerializing()))
            {
                EntityBase sourcePersistentType = link.SourceEntityBase;
                PersistentType targetPersistentType = link.TargetEntityBase;

                result.IsValid = !sourcePersistentType.Id.Equals(targetPersistentType.Id);

                if (result.IsValid)
                {
                    if (targetPersistentType is EntityBase)
                    {
                        EntityBase targetEntityBase = (EntityBase)targetPersistentType;
                        result.IsValid = targetEntityBase.BaseType == null ||
                                  !targetEntityBase.BaseType.Id.Equals(sourcePersistentType.Id);

                        if (!result.IsValid)
                        {
                            result.ErrorMessage = string.Format(ERROR_CIRCULAR_REFERENCE, targetPersistentType.Name, sourcePersistentType.Name);
                            result.ErrorCode = CODE_BASE_TYPE;
                            result.ValidationElements = new[] {targetEntityBase};
                        }
                        else
                        {
                            result.IsValid = targetEntityBase.InheritanceModifier != InheritanceModifiers.Sealed;

                            if (!result.IsValid)
                            {
                                result.ErrorMessage = string.Format(ERROR_BASE_TYPE_IS_SEALED, targetPersistentType.Name,
                                                                    sourcePersistentType.Name);
                                result.ErrorCode = CODE_BASE_TYPE;
                                result.ValidationElements = new[] { targetEntityBase };
                            }
                            else
                            {
                                if (sourcePersistentType.Access == AccessModifier.Public && targetPersistentType.Access == AccessModifier.Internal)
                                {
                                    result.IsValid = false;
                                    result.ErrorCode = CODE_INVALID_INHERITANCE;
                                    result.ValidationElements = new[] { targetPersistentType };
                                    result.ErrorMessage = string.Format(ERROR_INCOMPATIBLE_ACCESS_MODIFIERS, sourcePersistentType.Name, targetPersistentType.Name);
                                }
                                else
                                {
                                    IEnumerable<EntityBase> ancestors;
                                    if (sourcePersistentType.GetAncestors(out ancestors))
                                    {
                                        result.IsValid = false;
                                        result.ErrorCode = CODE_INVALID_INHERITANCE;
                                        result.ValidationElements = ancestors.ToArray();
                                        result.ErrorMessage = string.Format(ERROR_LOOP_IN_INHERITANCE, sourcePersistentType.Name);
                                    }
                                    else
                                    {
                                        if (sourcePersistentType is Entity)
                                        {
                                            result.IsValid = targetPersistentType.TypeKind != PersistentTypeKind.Structure;
                                            if (!result.IsValid)
                                            {
                                                result.ErrorMessage = ERROR_BASE_TYPE_CANNOT_BE_STRUCTURE;
                                                result.ErrorCode = CODE_BASE_TYPE;
                                                result.ValidationElements = new[] {targetEntityBase};
                                            }
                                        }
                                        else // sourcePersistentType is Structure
                                        {
                                            result.IsValid = (targetPersistentType is Structure);
                                            if (!result.IsValid)
                                            {
                                                result.ErrorMessage = ERROR_BASE_TYPE_CANNOT_BE_ENTITY_OR_INTERFACE;
                                                result.ErrorCode = CODE_BASE_TYPE;
                                                result.ValidationElements = new[] {targetEntityBase};
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.ErrorMessage = ERROR_BASE_TYPE_CANNOT_BE_ITSELF;
                    result.ErrorCode = CODE_BASE_TYPE;
                    result.ValidationElements = new[] {sourcePersistentType};
                }

                if (!result.IsValid && workWithTransaction)
                {
                    link.RollbackCurrentTransaction(result.ErrorMessage);
                }
            }

            return result;
        }
    }
}