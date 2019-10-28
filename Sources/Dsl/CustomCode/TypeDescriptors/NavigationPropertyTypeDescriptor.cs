using System;
using System.ComponentModel;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class NavigationPropertyTypeDescriptor
    {
        private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
        {
            var attributeBuilder = this.CreateBuilder(attributes, base.GetProperties(attributes),
                        modelElement =>
                        {
                            NavigationProperty navigationProperty = modelElement as NavigationProperty;
                            return navigationProperty == null
                                        ? false
                                        : navigationProperty.Multiplicity != MultiplicityKind.Many;
                        });

            Guid typedEntitySetPropertyId = NavigationPropertyHasTypedEntitySet.TypedEntitySetDomainRoleId;

            attributeBuilder.ReplaceRolePlayerPropertyAtribute(typedEntitySetPropertyId,
                                             descriptor => new BrowsableAttribute(false));

            return attributeBuilder.Build();
        }
    }
}