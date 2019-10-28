using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class Entity : IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public Entity(Store store, params PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public Entity(Partition partition, params PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            Initialize();
        }

        protected override PersistentTypeKind GetTypeKindValue()
        {
            return PersistentTypeKind.Entity;
        }

        private void Initialize()
        {
            this.HierarchyRootAttribute = new OrmHierarchyRootAttribute();
            this.KeyGenerator = new OrmKeyGeneratorAttribute();
            this.KeyGenerator.Enabled = false;

            this.TypeDiscriminatorValue = new OrmTypeDiscriminatorValueAttribute();
            this.TypeDiscriminatorValue.Enabled = false;
        }

        private bool GetIsHierarchyRootValue()
        {
            return HierarchyRootAttribute.Enabled;
        }

        protected override void BuildTypeAttributes(List<IOrmAttribute> typeAttributes)
        {
            base.BuildTypeAttributes(typeAttributes);
            typeAttributes.AddRange(new IOrmAttribute[]
                                    {
                                        this.HierarchyRootAttribute, 
                                        this.KeyGenerator,
                                        this.TypeDiscriminatorValue
                                    });
        }

        #region Implementation of IEntity

        OrmHierarchyRootAttribute IEntity.HierarchyRoot
        {
            get { return this.HierarchyRootAttribute; }
        }

        OrmKeyGeneratorAttribute IEntity.KeyGenerator
        {
            get { return this.KeyGenerator; }
        }

        OrmTypeDiscriminatorValueAttribute IEntity.TypeDiscriminatorValue
        {
            get { return this.TypeDiscriminatorValue; }
        }

        #endregion

        #region validation

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateName(ValidationContext context)
        {
            PersistentTypeValidation.ValidateName(this, context);
        }

        #endregion validation
    }
}