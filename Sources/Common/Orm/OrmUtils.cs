using System;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    #region class OrmUtils

    public static class OrmUtils
    {
        public const string NAMESPACE_ROOT = "Xtensive";

        public const string NAMESPACE_CORE = NAMESPACE_ROOT + ".Core";

        public const string NAMESPACE_ORM = NAMESPACE_ROOT + ".Orm";
        public const string NAMESPACE_ORM_MODEL = NAMESPACE_ORM + ".Model";

        public const string NAMESPACE_ORM_VALIDATION = NAMESPACE_ORM + ".Validation";

        //public const string NAMESPACE_CONSTRAINTS = NAMESPACE_INTEGRITY + ".Aspects.Constraints";

        public const string TYPE_ENTITYSET = "EntitySet<{0}>";

        public static string GetOrmNamespace(OrmNamespace ormNamespace)
        {
            switch (ormNamespace)
            {
                case OrmNamespace.Root:
                    return NAMESPACE_ROOT;
                case OrmNamespace.Core:
                    return NAMESPACE_CORE;
                case OrmNamespace.Orm:
                    return NAMESPACE_ORM;
                case OrmNamespace.OrmModel:
                    return NAMESPACE_ORM_MODEL;
                case OrmNamespace.OrmValidation:
                    return NAMESPACE_ORM_VALIDATION;
                default:
                    throw new ArgumentOutOfRangeException("ormNamespace");
            }
        }

        public static string GetOrmType(OrmType ormType, params string[] args)
        {
            switch (ormType)
            {
                case OrmType.Structure:
                case OrmType.Entity:
                case OrmType.Session:
                case OrmType.IEntity:
                case OrmType.FieldInfo:
                {
                    return ormType.ToString();
                }
                case OrmType.EntitySet:
                {
                    if (args == null || args.Length == 0)
                    {
                        return ormType.ToString();
                    }

                    return string.Format(TYPE_ENTITYSET, args[0]);
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("ormType");
                }
            }
        }

        public static OrmNamespace GetOrmNamespace(OrmType ormType)
        {
            switch (ormType)
            {
                case OrmType.Structure:
                case OrmType.EntitySet:
                case OrmType.Entity:
                case OrmType.Session:
                case OrmType.IEntity:
                {
                    return OrmNamespace.Orm;
                }
                case OrmType.FieldInfo:
                {
                    return OrmNamespace.OrmModel;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("ormType");
                }
            }
        }

        public static string BuildXtensiveType(OrmType type, params string[] args)
        {
            OrmNamespace ormNamespace = GetOrmNamespace(type);
            string ormType = GetOrmType(type, args);
            return BuildXtensiveType(ormNamespace, ormType);
        }

        public static string BuildXtensiveType(OrmNamespace ormNamespace, string type)
        {
            return string.Format("{0}.{1}", GetOrmNamespace(ormNamespace), type);
        }
    }

    #endregion class OrmUtils

    #region enum OrmNamespace

    public enum OrmNamespace
    {
        Root = 0,
        Core,
        Orm,
        OrmModel,
        OrmValidation
    }

    #endregion enum OrmNamespace

    #region enum OrmType

    public enum OrmType
    {
        Entity = 0,
        EntitySet,
        Structure,
        Session,
        IEntity,
        FieldInfo
    }

    #endregion enum OrmType
}