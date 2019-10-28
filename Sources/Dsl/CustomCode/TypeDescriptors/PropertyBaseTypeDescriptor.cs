using System;
using System.ComponentModel;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class PropertyBaseTypeDescriptor
    {
        private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
        {
            var attributeBuilder = this.CreateBuilder(attributes, base.GetProperties(attributes),
                        modelElement =>
                        {
                            PropertyBase navigationProperty = modelElement as PropertyBase;
                            return navigationProperty == null ? false : true; //navigationProperty.IsInherited;
                        });

//            PropertyKind? propertyKind = attributeBuilder.TestElement(element => (element is PropertyBase)
//                ? (element as PropertyBase).PropertyKind
//                : (PropertyKind?) null, null);

            PropertyBase property = attributeBuilder.TestElement(element => (element is PropertyBase)
                ? (element as PropertyBase)
                : null, null);

            PersistentTypeKind? ownerTypeKind = attributeBuilder.TestElement(element => (element is PropertyBase)
                ? (element as IPropertyBase).Owner.TypeKind
                : (PersistentTypeKind?)null, null);

            // ownerTypeKind.HasValue && ownerTypeKind.Value == PersistentTypeKind.Interface
            bool ownerIsInterface = ownerTypeKind.HasValue && ownerTypeKind.Value == PersistentTypeKind.Interface;

            if (property != null)
            {
                if (ownerIsInterface)
                {
                    //TODO: Uncomment in final!
                    attributeBuilder.ReplaceElementPropertyAtribute(PropertyBase.PropertyAccessDomainPropertyId,
                        //descriptor => new ReadOnlyAttribute(true));
                        descriptor => new BrowsableAttribute(false));
                }

                // set readonly(true) for scalar properties
                if (property.PropertyKind == PropertyKind.Scalar)
                {
                    if (property.IsInherited)
                    {
                        //TODO: Uncomment in final!
//                        attributeBuilder.ReplaceRolePlayerPropertyAtribute(ScalarPropertyHasType.DomainTypeDomainRoleId,
//                            descriptor => new BrowsableAttribute(false));
                        attributeBuilder.ReplaceElementPropertyAtribute(ScalarProperty.TypeDomainPropertyId,
                            descriptor => new BrowsableAttribute(false));
                    }
                }

                // set readonly(true) for structure properties
                if (property.PropertyKind == PropertyKind.Structure)
                {
                    if (property.IsInherited)
                    {
                        attributeBuilder.ReplaceRolePlayerPropertyAtribute(
                            StructurePropertyHasType.StructureDomainRoleId,
                            descriptor => new BrowsableAttribute(false));
                    }
                }

                // set readonly(true) for navigation properties
                if (property.PropertyKind == PropertyKind.Navigation)
                {
                    if (property.IsInherited)
                    {
                        attributeBuilder.ReplaceRolePlayerPropertyAtribute(
                            NavigationPropertyHasAssociation.PersistentTypeHasAssociationsDomainRoleId,
                            descriptor => new BrowsableAttribute(false));
                    }
                }
            }
            return attributeBuilder.Build();
        }
    }
}