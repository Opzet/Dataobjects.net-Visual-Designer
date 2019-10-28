/*using System;
using System.ComponentModel;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class ExternalPersistentTypeTypeDescriptor
    {
        private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
        {
            var attributeBuilder = this.CreateBuilder(attributes, base.GetProperties(attributes),
                modelElement => modelElement is ExternalPersistentType);
            attributeBuilder.ReplaceElementPropertyAtribute(PersistentType.AccessDomainPropertyId, 
                descriptor => new BrowsableAttribute(false));

            return attributeBuilder.Build();
        }
    }
}*/