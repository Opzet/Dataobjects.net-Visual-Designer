using System;
using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public static class StandardValues
    {
        public const string TYPE_NAME_STRING = "String";

        public static readonly List<string> TypeStandardValues = new List<string>
                                                                     {
                                                                         "Boolean",
                                                                         "Byte",
                                                                         "Byte[]",
                                                                         "DateTime",
                                                                         "Double",
                                                                         "Guid",
                                                                         "Char",
                                                                         "Int16",
                                                                         "Int32",
                                                                         "Int64",
                                                                         "SByte",
                                                                         "Single",
                                                                         "String",
                                                                         "TimeSpan",
                                                                         "UInt16",
                                                                         "UInt32",
                                                                         "UInt64",
                                                                         "Decimal"
                                                                     };

        public static readonly Type TYPE_BOOLEAN = typeof(Boolean);
        public static readonly Type TYPE_BYTE = typeof(Byte);
        public static readonly Type TYPE_BYTE_ARRAY = typeof(byte[]);
        public static readonly Type TYPE_DATETIME = typeof(DateTime);
        public static readonly Type TYPE_DOUBLE = typeof(double);
        public static readonly Type TYPE_GUID = typeof(Guid);
        public static readonly Type TYPE_CHAR = typeof(Char);
        public static readonly Type TYPE_INT16 = typeof(Int16);
        public static readonly Type TYPE_INT32 = typeof(Int32);
        public static readonly Type TYPE_INT64 = typeof(Int64);
        public static readonly Type TYPE_SBYTE = typeof(SByte);
        public static readonly Type TYPE_SINGLE = typeof(Single);
        public static readonly Type TYPE_TIMESPAN = typeof(TimeSpan);
        public static readonly Type TYPE_UINT16 = typeof(UInt16);
        public static readonly Type TYPE_UINT32 = typeof(UInt32);
        public static readonly Type TYPE_UINT64 = typeof(UInt64);
        public static readonly Type TYPE_STRING = typeof(string);
        public static readonly Type TYPE_DECIMAL = typeof(Decimal);

        public static StandartType ResolveStandardType(Type type)
        {
            if (type == TYPE_BOOLEAN)
            {
                return StandartType.Boolean;
            }

            if (type == TYPE_BYTE)
            {
                return StandartType.Byte;
            }

            if (type == TYPE_BYTE_ARRAY)
            {
                return StandartType.ByteArray;
            }

            if (type == TYPE_DATETIME)
            {
                return StandartType.DateTime;
            }

            if (type == TYPE_DOUBLE)
            {
                return StandartType.Double;
            }

            if (type == TYPE_GUID)
            {
                return StandartType.Guid;
            }

            if (type == TYPE_CHAR)
            {
                return StandartType.Char;
            }

            if (type == TYPE_INT16)
            {
                return StandartType.Int16;
            }

            if (type == TYPE_INT32)
            {
                return StandartType.Int32;
            }

            if (type == TYPE_INT64)
            {
                return StandartType.Int64;
            }

            if (type == TYPE_SBYTE)
            {
                return StandartType.SByte;
            }

            if (type == TYPE_SINGLE)
            {
                return StandartType.Single;
            }

            if (type == TYPE_TIMESPAN)
            {
                return StandartType.TimeSpan;
            }

            if (type == TYPE_UINT16)
            {
                return StandartType.UInt16;
            }

            if (type == TYPE_UINT32)
            {
                return StandartType.UInt32;
            }

            if (type == TYPE_UINT64)
            {
                return StandartType.UInt64;
            }

            if (type == TYPE_DECIMAL)
            {
                return StandartType.Decimal;
            }

            return StandartType.String;
        }
    }
}