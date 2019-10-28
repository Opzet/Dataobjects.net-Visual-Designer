using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    #region enum KeyDirection

    [Serializable]
    public enum KeyDirection
    {
        [XmlEnum("Default")]
        Default = -1,

        [XmlEnum("Negative")]
        Negative,

        [XmlEnum("Positive")]
        Positive
    }

    #endregion enum KeyDirection

    #region enum AssociationOnRemoveAction

    [Serializable]
    public enum AssociationOnRemoveAction
    {
        [XmlEnum("Default")]
        Default = -1,

        [XmlEnum("Deny")]
        Deny = 0,

        [XmlEnum("Cascade")]
        Cascade = 1,

        [XmlEnum("Clear")]
        Clear = 2,

        [XmlEnum("None")]
        None = 3
    }

    #endregion enum AssociationOnRemoveAction

    #region enum VersionMode

    [Serializable]
    public enum VersionMode
    {
        [XmlEnum("Default")]
        Default = -1,

        [XmlEnum("Manual")]
        Manual = 0,

        [XmlEnum("Skip")]
        Skip = 1,

        [XmlEnum("Auto")]
        Auto = 2
    }

    #endregion enum VersionMode

    #region enum StandartType

    public enum StandartType
    {
        Boolean,
        Byte,
        ByteArray,
        DateTime,
        Double,
        Guid,
        Char,
        Int16,
        Int32,
        Int64,
        SByte,
        Single,
        String,
        TimeSpan,
        UInt16,
        UInt32,
        UInt64,
        Decimal
    }

    #endregion enum StandartType

    #region enum MultiplicityKind

    [TypeConverter(typeof(EnumTypeConverter))]
    public enum MultiplicityKind
    {
        [FieldDisplayName("0..1 (Zero or One)")]
        ZeroOrOne = 0,

        [FieldDisplayName("1 (One)")]
        One = 1,

        [FieldDisplayName("* (Many)")]
        Many = 2
    }

    #endregion enum MultiplicityKind

    #region enum TransactionEvent

    [Flags]
    public enum TransactionEvent
    {
        None = 0,
        Beginning = 1,
        Committed = 2,
        RolledBack = 4
    }

    #endregion enum TransactionEvent

    #region enum PropertyConstrainTypes

    [TypeConverter(typeof(EnumTypeConverter))]
    [Flags]
    public enum PropertyConstrainTypes
    {
        [FlagsEnumItem(true, true)]
        None = 0,
        Email = 1,
        Future = 2,
        Length = 4,
        NotEmpty = 8,
        NotNull = 16,
        NotNullOrEmpty = 32,
        Past = 64,
        Range = 128,
        Regex = 256
    }

    #endregion enum PropertyConstrainTypes

    #region enum PropertyConstrainType

    public enum PropertyConstrainType
    {
        Email,
        Future,
        Length,
        NotEmpty,
        NotNull,
        NotNullOrEmpty,
        Past,
        Range,
        Regex
    }

    #endregion enum PropertyConstrainType

    #region enum PropertyConstrainMode

    public enum PropertyConstrainMode
    {
        /// <summary>
        /// Same as OnValidate.
        /// </summary>
        [XmlEnum("Default")]
        Default = -1,

        /// <summary>
        /// Property value will be checked on object validation. 
        /// </summary>
        [XmlEnum("OnValidate")]
        OnValidate = 0,

        /// <summary>
        /// Validation is performed before property value is set. 
        /// </summary>
        [XmlEnum("OnSetValue")]
        OnSetValue = 1,
    }

    #endregion enum PropertyConstrainMode

    #region enum InheritsIEntityMode

    [TypeConverter(typeof(EnumTypeConverter))]
    public enum InheritsIEntityMode
    {
        [FieldDisplayName("Auto")]
        Auto,

        [FieldDisplayName("Always Inherit")]
        AlwaysInherit
    }

    #endregion enum InheritsIEntityMode

    #region class EnumType

    public static class EnumType
    {
        public static ReadOnlyCollection<Enum> GetValues(Type enumType)
        {
            return Enum.GetValues(enumType).OfType<Enum>().ToList().AsReadOnly();
        }
    }

    public static class EnumType<T> where T : struct
    {
        static EnumType()
        {
            Trace.Assert(typeof (T).IsEnum);
        }

        public static ReadOnlyCollection<T> Values = ((T[]) Enum.GetValues(typeof (T))).ToList().AsReadOnly();
    }

    #endregion class EnumType

    #region class KeyPair 

    public class KeyPair
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }

        public KeyPair()
        {}

        public KeyPair(string key1) : this(key1, null)
        {}

        public KeyPair(string key1, string key2)
        {
            Key1 = key1;
            Key2 = key2;
        }
    }

    #endregion class KeyPair

    #region enum PropertyAccessModifier

    [TypeConverter(typeof(EnumTypeConverter))]
    public enum PropertyAccessModifier
    {
        [FieldDisplayName("Private")]
        Private = 0,

        [FieldDisplayName("Protected")]
        Protected = 1,

        [FieldDisplayName("Protected Internal")]
        ProtectedInternal = 2,

        [FieldDisplayName("Internal")]
        Internal = 3,

        [FieldDisplayName("Public")]
        Public = 4
    }

    #endregion enum PropertyAccessModifier
}