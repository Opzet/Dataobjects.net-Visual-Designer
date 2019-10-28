using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class EntityBase : IEntityBase
    {
        /// <summary>
        /// Returns the ancestors of this BaseConfigurationType (includes this type)
        /// </summary>
        /// <param name="ancestors">The list of ancestors</param>
        /// <returns><see langref="false"/> if there are no loops in the inheritance graph, else <see langref="true"/></returns>
        protected internal bool GetAncestors(out IEnumerable<EntityBase> ancestors)
        {
            LinkedList<EntityBase> list = new LinkedList<EntityBase>();
            ancestors = list;
            for (EntityBase ancestor = this; ancestor != null; ancestor = ancestor.BaseType)
            {
                if (ancestors.Contains(ancestor))
                {
                    return true;
                }
                list.AddLast(ancestor);
            }
            return false;
        }


        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateInheritance(ValidationContext context)
        {
            if (this.BaseType != null)
            {
                EntityBaseHasBaseType link = EntityBaseHasBaseType.GetLinkToBaseType(this);
                ValidationResult validationResult = EntityBaseHasBaseTypeRules.Validate(link, false);
                
                context.LogErrorIfAny(validationResult);
            }
        }

        #region Implementation of IEntityBase

        InheritanceModifiers IEntityBase.InheritanceModifier
        {
            get
            {
                return this.InheritanceModifier;
            }
        }

        IEntityBase IEntityBase.BaseType
        {
            get { return this.BaseType; }
        }

        ReadOnlyCollection<IEntityBase> IEntityBase.ReferencesAsBaseType
        {
            get
            {
                var items = this.BaseTypeOf.OfType<EntityBase>().ToArray();
                ReadOnlyCollection<IEntityBase> result = new ReadOnlyCollection<IEntityBase>(items);
                return result;
            }
        }

        public ReadOnlyCollection<IEntityBase> GetBaseTypesGraph(InheritanceGraphDirection graphDirection)
        {
            List<IEntityBase> list = new List<IEntityBase>();

            IEntityBase baseEntity = this.BaseType;
            while (baseEntity != null)
            {
                list.Add(baseEntity);
                baseEntity = baseEntity.BaseType;
            }

            if (graphDirection == InheritanceGraphDirection.RootToType)
            {
                list.Reverse();
            }

            return new ReadOnlyCollection<IEntityBase>(list);
        }

        #endregion

        public override IEnumerable<IInterface> GetCurrentLevelInheritedInterfaces()
        {
            List<IInterface> list = new List<IInterface>(base.InheritedInterfaces);
            if (this.BaseType != null)
            {
                list.Add(BaseType);
            }

            return list;
        }
    }
}