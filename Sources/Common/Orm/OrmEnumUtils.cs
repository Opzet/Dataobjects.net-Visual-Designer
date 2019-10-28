using System;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public static class OrmEnumUtils
    {
        public const string ENUM_TYPE_INHERITANCE_SCHEMA = "InheritanceSchema";
        public const string ENUM_TYPE_ON_REMOVE_ACTION = "OnRemoveAction";
        public const string ENUM_TYPE_DIRECTION = "Direction";
        public const string ENUM_TYPE_KEY_GENERATOR_KIND = "KeyGeneratorKind";
        public const string ENUM_TYPE_VERSION_MODE = "VersionMode";
        public const string ENUM_TYPE_CONSTRAIN_MODE = "ConstrainMode";

        public static string BuildXtensiveType(object enumValue)
        {
            if (enumValue is HierarchyRootInheritanceSchema)
            {
                return BuildXtensiveType((HierarchyRootInheritanceSchema)enumValue);
            }
            
            if (enumValue is AssociationOnRemoveAction)
            {
                return BuildXtensiveType((AssociationOnRemoveAction)enumValue);
            }

            if (enumValue is KeyDirection)
            {
                return BuildXtensiveType((KeyDirection)enumValue);
            }
            
            if (enumValue is KeyGeneratorKind)
            {
                return BuildXtensiveType((KeyGeneratorKind)enumValue);
            }

            if (enumValue is VersionMode)
            {
                return BuildXtensiveType((VersionMode)enumValue);
            }

            if (enumValue is PropertyConstrainMode)
            {
                return BuildXtensiveType((PropertyConstrainMode)enumValue);
            }

            return enumValue.GetType().FullName + "." + enumValue.ToString();
        }

        public static string BuildXtensiveType(HierarchyRootInheritanceSchema enumValue)
        {
            return BuildXtensiveType(OrmNamespace.OrmModel, ENUM_TYPE_INHERITANCE_SCHEMA, enumValue);
        }

        public static string BuildXtensiveType(AssociationOnRemoveAction enumValue)
        {
            return BuildXtensiveType(OrmNamespace.Orm, ENUM_TYPE_ON_REMOVE_ACTION, enumValue);
        }

        public static string BuildXtensiveType(KeyDirection enumValue)
        {
            return BuildXtensiveType(OrmNamespace.Core, ENUM_TYPE_DIRECTION, enumValue);
        }

        public static string BuildXtensiveType(VersionMode enumValue)
        {
            return BuildXtensiveType(OrmNamespace.Orm, ENUM_TYPE_VERSION_MODE, enumValue);
        }

        public static string BuildXtensiveType(PropertyConstrainMode enumValue)
        {
            return BuildXtensiveType(OrmNamespace.OrmValidation, ENUM_TYPE_CONSTRAIN_MODE, enumValue);
        }

        public static string BuildXtensiveType(KeyGeneratorKind enumValue)
        {
            return BuildXtensiveType(OrmNamespace.Orm, ENUM_TYPE_KEY_GENERATOR_KIND, enumValue);
        }

        private static string BuildXtensiveType(OrmNamespace ormNamespace, string type, Enum enumValue)
        {
            string firstPart = OrmUtils.BuildXtensiveType(ormNamespace, type);
            return string.Format("{0}.{1}", firstPart, enumValue);
        }
    }
}