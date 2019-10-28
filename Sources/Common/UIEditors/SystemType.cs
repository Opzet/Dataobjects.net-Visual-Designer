using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    public class SystemPrimitiveTypesConverter : TypeConverter
    {
        public static readonly Type[] Types = new[]
                                {
                                    typeof (string),
                                    typeof (sbyte),
                                    typeof (short),
                                    typeof (int),
                                    typeof (long),
                                    typeof (byte),
                                    typeof (ushort),
                                    typeof (uint),
                                    typeof (ulong),
                                    typeof (DateTime),
                                    typeof (Guid),
                                    typeof (byte[]),
                                    typeof (bool),
                                    typeof (TimeSpan)
                                };

        public static readonly Guid TYPE_ID_STRING = new Guid("00000000-0000-0000-0000-000000000001");
        public static readonly Guid TYPE_ID_SBYTE = new Guid("00000000-0000-0000-0000-000000000002");
        public static readonly Guid TYPE_ID_INT16 = new Guid("00000000-0000-0000-0000-000000000003");
        public static readonly Guid TYPE_ID_INT32 = new Guid("00000000-0000-0000-0000-000000000004");
        public static readonly Guid TYPE_ID_INT64 = new Guid("00000000-0000-0000-0000-000000000005");
        public static readonly Guid TYPE_ID_BYTE = new Guid("00000000-0000-0000-0000-000000000006");
        public static readonly Guid TYPE_ID_UINT16 = new Guid("00000000-0000-0000-0000-000000000007");
        public static readonly Guid TYPE_ID_UINT32 = new Guid("00000000-0000-0000-0000-000000000008");
        public static readonly Guid TYPE_ID_UINT64 = new Guid("00000000-0000-0000-0000-000000000009");
        public static readonly Guid TYPE_ID_DATETIME = new Guid("00000000-0000-0000-0000-000000000010");
        public static readonly Guid TYPE_ID_GUID = new Guid("00000000-0000-0000-0000-000000000011");
        public static readonly Guid TYPE_ID_BYTE_ARRAY = new Guid("00000000-0000-0000-0000-000000000012");
        public static readonly Guid TYPE_ID_BOOLEAN = new Guid("00000000-0000-0000-0000-000000000013");
        public static readonly Guid TYPE_ID_TIMESPAN = new Guid("00000000-0000-0000-0000-000000000014");

        public static readonly Dictionary<Guid, Type> TypeIds =
            new Dictionary<Guid, Type>
            {
                {TYPE_ID_STRING, typeof (string)},
                {TYPE_ID_SBYTE, typeof (sbyte)},
                {TYPE_ID_INT16, typeof (short)},
                {TYPE_ID_INT32, typeof (int)},
                {TYPE_ID_INT64, typeof (long)},
                {TYPE_ID_BYTE, typeof (byte)},
                {TYPE_ID_UINT16, typeof (ushort)},
                {TYPE_ID_UINT32, typeof (uint)},
                {TYPE_ID_UINT64, typeof (ulong)},
                {TYPE_ID_DATETIME, typeof (DateTime)},
                {TYPE_ID_GUID, typeof (Guid)},
                {TYPE_ID_BYTE_ARRAY, typeof (byte[])},
                {TYPE_ID_BOOLEAN, typeof (bool)},
                {TYPE_ID_TIMESPAN, typeof (TimeSpan)}
            };

        public static Guid GetTypeId(Type type)
        {
            return TypeIds.Where(item => item.Value == type).Select(item => item.Key).SingleOrDefault();
        }

        public static string GetDisplayName(Type type)
        {
            return GetDisplayName(type.FullName);
        }

        public static string GetDisplayName(string clrName)
        {
            return DisplayNames.ContainsKey(clrName) ? DisplayNames[clrName] : null;
        }

        public static string GetClrName(string displayName)
        {
            string clrName = DisplayNames.Where(pair => pair.Value == displayName).Select(pair => pair.Key).SingleOrDefault();
            return string.IsNullOrEmpty(clrName) ? displayName : clrName;
        }

        public static Type GetClrType(string displayName)
        {
            return Type.GetType(GetClrName(displayName), true);
        }

        private static Dictionary<string, string> displayNames;

        internal static Dictionary<string, string> DisplayNames
        {
            get
            {
                if (displayNames == null)
                {
                    displayNames = new Dictionary<string, string>();
                    foreach (Type type in Types)
                    {
                        displayNames.Add(type.FullName, type.Name);
                    }
                }
                return displayNames;
            }
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(DisplayNames.Values);
        }
    }
}