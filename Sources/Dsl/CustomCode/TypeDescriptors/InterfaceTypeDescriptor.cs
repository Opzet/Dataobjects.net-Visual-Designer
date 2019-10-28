using System;
using System.ComponentModel;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class InterfaceTypeDescriptor
    {
        private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
        {
            var attributeBuilder = this.CreateBuilder(attributes, base.GetProperties(attributes),
                modelElement =>
                {
                    Interface @interface = modelElement as Interface;
                    //return @interface == null ? false : @interface.InheritedInterfaces.Count == 0;
                    return @interface != null;
                });

            bool noInheritance = attributeBuilder.TestElement(element => (element as IInterface).InheritedInterfaces.Count == 0, true);

            if (noInheritance)
            {
                attributeBuilder.ReplaceElementPropertyAtribute(Interface.InheritInterfacesDomainPropertyId,
                                                                descriptor => new BrowsableAttribute(false));
            }

            bool isNotInterface = attributeBuilder.TestElement(element => (element as IInterface).TypeKind != PersistentTypeKind.Interface, true);

            if (isNotInterface)
            {
                attributeBuilder.ReplaceElementPropertyAtribute(Interface.InheritsIEntityDomainPropertyId,
                                                                descriptor => new BrowsableAttribute(false));
            }


            return attributeBuilder.Build();
        }

    }
}