using System;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class DONetEntityModelDesignerDomainModel
    {
        private readonly Type[] modelTypes = new[]
                                                 {
                                                     typeof (TypedEntitySetToNavigationPropertyRules.AddRule),
                                                     typeof (TypedEntitySetToNavigationPropertyRules.ChangeRule),
                                                     typeof (TypedEntitySetToNavigationPropertyRules.RolePlayerChangeRule),

                                                     typeof (InterfaceInheritInterfacesRules.DeleteRule),
                                                     
                                                     typeof (EntityBaseHasBaseTypeRules.AddRule),
                                                     typeof (EntityBaseHasBaseTypeRules.ChangeRule),
                                                     typeof (EntityBaseHasBaseTypeRules.RolePlayerChangeRule),

                                                     typeof (TypedEntitySetHasItemTypeRules.AddRule),
                                                     typeof (TypedEntitySetHasItemTypeRules.ChangeRule),
                                                     typeof (TypedEntitySetHasItemTypeRules.RolePlayerChangeRule)
                                                 };

        protected override Type[] GetCustomDomainModelTypes()
        {
            return modelTypes;
        }
    }
}