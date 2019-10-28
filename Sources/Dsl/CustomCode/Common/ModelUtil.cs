using System;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public static class ModelUtil
    {
        public static PersistentTypeKind ConvertTo(EntityKind entityKind)
        {
            switch (entityKind)
            {
                case EntityKind.Entity:
                    return PersistentTypeKind.Entity;
                case EntityKind.Structure:
                    return PersistentTypeKind.Structure;
                default:
                    return PersistentTypeKind.Interface;
            }
        }

        public static EntityKind ConvertTo(PersistentTypeKind typeKind)
        {
            switch (typeKind)
            {
                case PersistentTypeKind.Interface:
                {
                    return EntityKind.Interface;
                }
                case PersistentTypeKind.Entity:
                {
                    return EntityKind.Entity;
                }
                default:
                {
                    return EntityKind.Structure;
                }
            }
        }
    }
}