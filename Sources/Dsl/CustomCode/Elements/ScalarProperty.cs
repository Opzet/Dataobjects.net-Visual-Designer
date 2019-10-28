using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    internal partial class ScalarProperty : IScalarProperty
    {
        private const string ERROR_MISSING_TYPE = "Type must be specified for scalar property '{0}' of type '{1}'";
        private const string CODE_MISSING_TYPE = "MissingType";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public ScalarProperty(Store store, params PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public ScalarProperty(Partition partition, params PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.KeyAttribute = new OrmKeyAttribute();
            this.Type = (DomainType) this.Store.GetEntityModel().GetDomainType(SystemPrimitiveTypesConverter.TYPE_ID_STRING);
        }

        protected override PropertyKind GetPropertyKindValue()
        {
            return PropertyKind.Scalar;
        }

        protected override void BuildTypeAttributes(List<IOrmAttribute> typeAttributes)
        {
            typeAttributes.Add(this.KeyAttribute);
        }

        protected override void OnCopy(ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);

            ScalarProperty sourceProperty = (ScalarProperty) sourceElement;

            this.KeyAttribute = Common.ExtensionMethods.Clone(sourceProperty.KeyAttribute);
            this.Type = sourceProperty.Type;
        }


        #region Validation

        [ValidationMethod(ValidationCategories.Menu | ValidationCategories.Save)]
        private void ValidateType(ValidationContext context)
        {
            if (this.Type == null)
            {
                context.LogError(string.Format(ERROR_MISSING_TYPE, this.Name, this.PersistentType.Name),
                    CODE_MISSING_TYPE, new ModelElement[] {this});
            }
        }

        #endregion Validation

        #region Implementation of IScalarProperty

        IDomainType IScalarProperty.Type
        {
            get { return this.Type; }
        }

//        public Type ClrType
//        {
//            get
//            {
//                return this.Type.TryGetClrType(typeof(string));
//            }
//        }

        OrmKeyAttribute IScalarProperty.KeyAttribute
        {
            get { return this.KeyAttribute; }
        }

        #endregion
    }
}