using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal static class PropertiesValidation
    {
        private const string ERROR_MISSING_NAME = "{0} '{1}' contain {2} property with empty name. Property must have name.";
        private const string ERROR_NO_ASSOCIATION_ASSIGNED = "Navigation property '{0}' of type '{1}' is not assigned to any association.";

        private const string CODE_MISSING_NAME = "MissingName";
        private const string CODE_NO_ASSOCIATION_ASSIGNED = "NoAssociationAssigned";

        internal static void ValidateName<T>(T property, ValidationContext context) where T : PropertyBase
        {
            if (string.IsNullOrEmpty(property.Name))
            {
                IPersistentType owner = (property as IPropertyBase).Owner;
                context.LogError(string.Format(ERROR_MISSING_NAME, owner.TypeKind, owner.Name, property.PropertyKind.ToString().ToLower()), 
                    CODE_MISSING_NAME, new ModelElement[] { property });
            }
        }

        internal static void ValidateNavigationPropertyAssociation(NavigationProperty navigationProperty, ValidationContext context)
        {
            if (navigationProperty.PersistentTypeHasAssociations == null)
            {
                INavigationProperty navProp = navigationProperty;

                context.LogError(
                    string.Format(ERROR_NO_ASSOCIATION_ASSIGNED,
                                  navigationProperty.Name, navProp.Owner.Name),
                    CODE_NO_ASSOCIATION_ASSIGNED, new ModelElement[] { navigationProperty });
            }
        }
    }
}