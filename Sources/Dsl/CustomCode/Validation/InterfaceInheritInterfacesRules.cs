using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Rules
{
    public sealed class InterfaceInheritInterfacesRules
    {
        [RuleOn(typeof(InterfaceInheritInterfaces), FireTime = TimeToFire.LocalCommit)]
        public sealed class DeleteRule : Microsoft.VisualStudio.Modeling.DeleteRule
        {
            public override void ElementDeleted(ElementDeletedEventArgs e)
            {
                InterfaceInheritInterfaces link = e.ModelElement as InterfaceInheritInterfaces;
                if (link != null)
                {
                    RemoveTypedEntitySetConnectionFromNavigationProperties(link);
                }
            }

            private void RemoveTypedEntitySetConnectionFromNavigationProperties(InterfaceInheritInterfaces link)
            {
                Interface targetInterface = link.TargetInheritByInterface;
                var typedEntitySets = TypedEntitySetHasItemType.GetTypedEntitySets(targetInterface);
                if (typedEntitySets != null)
                {
                    foreach (TypedEntitySet typedEntitySet in typedEntitySets)
                    {
                        var navigationPropertiesLink = NavigationPropertyHasTypedEntitySet.GetTypedEntitySetNavigationProperties(typedEntitySet);
                        if (navigationPropertiesLink != null)
                        {
                            foreach (NavigationProperty navigationProperty in navigationPropertiesLink)
                            {
                                
                            }
                        }
                    }
                }
            }
        }
    }
}