namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public static partial class InterfaceInheritInterfacesBuilder
    {
        private static bool CanAcceptInterfaceAsTarget(Interface candidate)
        {
            return candidate.TypeKind == PersistentTypeKind.Interface;
        }

        private static bool CanAcceptInterfaceAndInterfaceAsSourceAndTarget(Interface sourceInterface, Interface targetInterface)
        {
            if (sourceInterface.TypeKind == PersistentTypeKind.Interface && targetInterface.TypeKind != PersistentTypeKind.Interface)
            {
                return false;
            }

            if (targetInterface.InheritedInterfaces.Exists(item => item == sourceInterface))
            {
                return false;
            }

            return true;
        }

        private static bool CanAcceptEntityBaseAsTarget(EntityBase candidate)
        {
            return true;
        }

        private static bool CanAcceptEntityBaseAndEntityBaseAsSourceAndTarget(EntityBase sourceEntityBase, EntityBase targetEntityBase)
        {
            if (sourceEntityBase is Structure && targetEntityBase is Entity)
            {
                return false;
            }

            if (targetEntityBase.BaseType == sourceEntityBase)
            {
                return false;
            }

            return true;
        }
    }
}