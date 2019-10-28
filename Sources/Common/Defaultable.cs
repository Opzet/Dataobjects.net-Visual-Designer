using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    #region class Defaultable

    [TypeConverter(typeof(EnumConverter))]
    public enum DefaultableValueType
    {
        Default,
        Custom
    }

    #region class DefaultableBase

    [Serializable]
    public abstract class DefaultableBase: ISerializableObject
    {
        private string defaultValueType;
        public const string DEFAULT_DISPLAY_NAME = "(Default)";

        protected internal bool updatingValueType = false;
        protected internal bool stopPropagateValueChanges = false;

        protected bool ValueTypeIs(string compareTo)
        {
            return ValueType == compareTo;
        }

        [Browsable(true)]
        [DisplayName("Value Type")]
        [RefreshProperties(RefreshProperties.All)]
        [XmlAttribute("valueType")]
        [NotifyParentProperty(true)]
        public virtual string ValueType
        {
            get { return defaultValueType; }
            set
            {
                if (value != defaultValueType)
                {
                    updatingValueType = true;
                    try
                    {
                        defaultValueType = value;
                        InternalSetValueType();
                    }
                    finally
                    {
                        updatingValueType = false;
                    }
                }
            }
        }

        [XmlIgnore]
        [Browsable(false)]
        public string ValueTypeForNull
        {
            get { return GetValueTypeForNull(); }
        }

        protected virtual void InternalSetValueType()
        {}

        protected abstract string GetValueTypeForNull();

        public void DeserializeFromXml(XmlReader reader)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            DeserializeFromXml(xmlRoot);
        }

        public virtual bool EqualsTo(DefaultableBase other)
        {
            if (other != null)
            {
                return Util.StringEqual(this.ValueType, other.ValueType, true);
            }

            return false;
        }

        public virtual void DeserializeFromXml(XmlProxy xmlRoot)
        {
            if (xmlRoot == null)
            {
                return;
            }

            string valueType = xmlRoot.GetAttr("valueType");
            this.ValueType = valueType;
            if (CanDeserializeValue())
            {
                //XmlProxy xmlContent = xmlRoot["value"];
                DeserializeValueFromXml(xmlRoot);
            }
        }

        public void SerializeToXml(XmlWriter writer)
        {
            SerializeToXml(writer, "content");
        }

        public void SerializeToXml(XmlWriter writer, string propertyName)
        {
            XmlProxy xmlRoot = new XmlProxy(propertyName);
            SerializeToXml(xmlRoot);
            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        public virtual void SerializeToXml(XmlProxy xmlRoot)
        {
            xmlRoot.AddAttribute("valueType", ValueType);

            if (CanSerializeValue())
            {
                SerializeValueToXml(xmlRoot);
            }
        }

        protected virtual bool CanSerializeValue()
        {
            return true;
        }

        protected virtual bool CanDeserializeValue()
        {
            return true;
        }

        protected abstract void DeserializeValueFromXml(XmlProxy parent);

        protected abstract void SerializeValueToXml(XmlProxy parent);
    }


    #endregion class DefaultableBase

    [Serializable]
    public abstract class Defaultable : DefaultableBase, ICloneable
    {
        private bool valueBackupInitialized = false;
        private object valueBackup;

        [TypeConverter(typeof(DefaultCustomValueTypeConverter))]
        [XmlAttribute("valueType")]
        [NotifyParentProperty(true)]
        public override string ValueType
        {
            get { return base.ValueType; }
            set { base.ValueType = value; }
        }

        public bool IsDefault()
        {
            return ValueTypeIs(DefaultCustomValueTypeConverter.VALUE_TYPE_DEFAULT);
        }

        public bool IsCustom()
        {
            return !IsDefault();
        }

        public void SetAsDefault()
        {
            if (stopPropagateValueChanges || updatingValueType)
            {
                return;
            }

            this.ValueType = DefaultCustomValueTypeConverter.VALUE_TYPE_DEFAULT;
        }

        public void SetAsCustom()
        {
            if (stopPropagateValueChanges || updatingValueType)
            {
                return;
            }

            this.ValueType = DefaultCustomValueTypeConverter.VALUE_TYPE_CUSTOM;
        }

        protected override void InternalSetValueType()
        {
            if (IsDefault())
            {
                object currentValue = this.InternalGetValue();

                if (currentValue != null)
                {
                    valueBackup = this.InternalGetValue();
                    valueBackupInitialized = true;
                }
                SetValue(null);
            }
            else
            {
                if (valueBackupInitialized)
                {
                    SetValue(valueBackup);
                }
            }
        }

        public static Defaultable<T> MakeGeneric<T>()
        {
            return (Defaultable<T>) MakeGeneric(typeof (T));
        }

        public static Defaultable MakeGeneric(Type valueType)
        {
            Type genericType = typeof (Defaultable<>).MakeGenericType(valueType);
            return (Defaultable) Activator.CreateInstance(genericType);
        }

        public override string ToString()
        {
            object value = InternalGetValue();
            return IsDefault() || value == null ? DEFAULT_DISPLAY_NAME : value.ToString();
            //return ValueType; NOTE: Test001
        }

        protected override string GetValueTypeForNull()
        {
            return DefaultCustomValueTypeConverter.VALUE_TYPE_DEFAULT;
        }

        protected internal void SetValue(object value)
        {
            InternalSetValue(value);
        }

        public object GetValue()
        {
            return InternalGetValue();
        }

        protected internal abstract object InternalGetValue();
        protected internal abstract void InternalSetValue(object value);
        protected internal abstract Type GetTypeOfValue();

        protected override void DeserializeValueFromXml(XmlProxy parent)
        {
            bool currspvc = stopPropagateValueChanges;
            stopPropagateValueChanges = true;

            try
            {
                string elementValue;
                Type typeOfValue = GetTypeOfValue();

                if (typeOfValue.IsPrimitive)
                {
                    elementValue = parent.GetAttr("value");
                }
                else
                {
                    XmlProxy xmlContent = parent["value"];
                    elementValue = xmlContent.ElementValue;
                }

                object val = elementValue == null ? elementValue: InternalValueFromString(typeOfValue, elementValue);
                SetValue(val);
            }
            finally
            {
                stopPropagateValueChanges = currspvc;
            }
        }

        private object InternalValueFromString(Type typeOfValue, string value)
        {
            object result = value;

            if (typeOfValue == typeof(byte[]))
            {
                result = Util.HexStringToBytes(value);
            }
            else if (typeOfValue == typeof(DateTime))
            {
                result = XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind);
            }
            else if (typeOfValue == typeof(DateTimeOffset))
            {
                string[] parts = value.Split('@');
                long ticks = long.Parse(parts[0]);
                long offsetTicks = long.Parse(parts[1]);

                result = new DateTimeOffset(ticks, TimeSpan.FromTicks(offsetTicks));
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(typeOfValue);
                try
                {
                    result = converter.ConvertFromString(value);
                }
                catch
                {
                    result = value;
                }
            }

            return result;
        }

        private string InternalValueToString(object value)
        {
            Type valueType = value.GetType();
            if (valueType == typeof(byte[]))
            {
                return Util.BytesToHexString((byte[]) value);
            }
            if (valueType == typeof(DateTime))
            {
                return XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind);
            }
            if (valueType == typeof(DateTimeOffset))
            {
                DateTimeOffset dateTimeOffset = ((DateTimeOffset) value);
                return string.Format("{0}@{1}", dateTimeOffset.Ticks, dateTimeOffset.Offset.Ticks);
            }

            string result = value.ToString();

            try
            {
                var typeConverter = TypeDescriptor.GetConverter(valueType);
                result = (string)typeConverter.ConvertTo(value, typeof (string));
            }
            catch
            {
                result = value.ToString();
            }

            return result;
        }

        protected override bool CanSerializeValue()
        {
            return !IsDefault();
        }

        protected override bool CanDeserializeValue()
        {
            return !IsDefault();
        }

        protected override void SerializeValueToXml(XmlProxy parent)
        {
            object value = InternalGetValue();
            string valueString = value == null ? string.Empty : InternalValueToString(value);
            //string valueString = value == null ? string.Empty : value.ToString();

            Type typeOfValue = GetTypeOfValue();
            if (typeOfValue.IsPrimitive)
            {
                parent.AddAttribute("value", valueString);
            }
            else
            {
//                if (typeOfValue == typeof(byte[]))
//                {
//                    valueString = Util.BytesToHexString((byte[]) value);
//                }

                XmlProxy xmlContent = parent.AddChild("value");
                xmlContent.ElementValue = valueString;
            }
        }

        public abstract object Clone();

        public Defaultable Merge(Defaultable other, MergeConflictAction mergeConflictAction)
        {
            if (this.GetTypeOfValue() != other.GetTypeOfValue())
            {
                throw new ArgumentException("Type of 'other' is not equal as current type!");
            }

            Defaultable mergedResult = (Defaultable) this.Clone();
            object mergedValue;
            string mergedValueType;
            if ((this.IsCustom() && other.IsCustom()) || (this.IsDefault() && other.IsDefault()))
            {
                if (mergeConflictAction == MergeConflictAction.TakeOther)
                {
                    mergedValue = other.GetValue();
                    mergedValueType = other.ValueType;
                }
                else
                {
                    mergedValue = this.GetValue();
                    mergedValueType = this.ValueType;
                }
            }
            else
            {
                if (this.IsCustom())
                {
                    mergedValue = this.GetValue();
                    mergedValueType = this.ValueType;
                }
                else // other.IsCustom()
                {
                    mergedValue = other.GetValue();
                    mergedValueType = other.ValueType;
                }
            }

            mergedResult.SetValue(mergedValue);
            mergedResult.ValueType = mergedValueType;
            return mergedResult;
        }

        public object CloneFromPropertyValues(IDictionary propertyValues)
        {
            Defaultable cloned = (Defaultable) this.Clone();
            cloned.AssignFromPropertyValues(propertyValues);
            return cloned;
        }

        private void AssignFromPropertyValues(IDictionary propertyValues)
        {
            this.updatingValueType = true;
            try
            {
                this.ValueType = (string)propertyValues["ValueType"];
                this.updatingValueType = false;

                if (propertyValues.Contains("Value"))
                {
                    object propertyValue = propertyValues["Value"];
                    if (propertyValue != null && propertyValue is Defaultable)
                    {
                        propertyValue = (propertyValue as Defaultable).Clone();
                    }
                    this.stopPropagateValueChanges = true;
                    this.SetValue(propertyValue);
                }
            }
            finally
            {
                this.updatingValueType = false;
                this.stopPropagateValueChanges = false;
            }
        }
    }

    #endregion class Defaultable
    
    #region class Defaultable<TValueType>

    [TypeConverter(typeof(DefaultableTypeConvertor))]
    [Editor(typeof(DefaultableTypeEditor), typeof(UITypeEditor))]
    [Serializable]
    public class Defaultable<TValueType> : Defaultable
    {
        private readonly Type realValueType;
        private TValueType value;

        [Browsable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [XmlElement]
        public TValueType Value
        {
            get { return value; }
            [RefreshProperties(RefreshProperties.All)]
            set
            {
                SetValue(value);
            }
        }

        public Defaultable(Defaultable<TValueType> other)
        {
            this.ValueType = other.ValueType;
            this.value = other.value;
            this.realValueType = other.realValueType;
        }

        public Defaultable(string valueType, TValueType value)
        {
            this.ValueType = valueType;
            this.value = value;
            this.realValueType = typeof(TValueType);
        }

        public Defaultable()
        {
            SetAsDefault();
            this.realValueType = typeof(TValueType);
        }

        protected internal override object InternalGetValue()
        {
            return Value;
        }

        public void SetAsCustom(TValueType value)
        {
            base.SetAsCustom();
            this.Value = value;
        }

        protected internal override void InternalSetValue(object value)
        {
            if (updatingValueType)
            {
                return;
            }

            if (value != null && value.GetType() != realValueType)
            {
                var typeConverter = TypeDescriptor.GetConverter(realValueType);
                object convValue = typeConverter.ConvertFrom(value);
                this.value = convValue == null ? default(TValueType) : (TValueType)convValue;
            }
            else
            {
                this.value = value == null ? default(TValueType) : (TValueType)value;
            }

            if (stopPropagateValueChanges)
            {
                return;
            }

            bool useDefault = value == null || value == (object)default(TValueType);
            if (useDefault)
            {
                SetAsDefault();
            }
            else
            {
                SetAsCustom();
            }
        }

        protected internal override Type GetTypeOfValue()
        {
            return realValueType;
        }

        public override object Clone()
        {
            Defaultable cloned = (Defaultable)Activator.CreateInstance(this.GetType());
            cloned.ValueType = this.ValueType;
            cloned.stopPropagateValueChanges = true;
            try
            {
                cloned.SetValue(this.value);
            }
            finally
            {
                cloned.stopPropagateValueChanges = false;
            }

            return cloned;
        }

        public TValueType GetValueOrDefault(TValueType defaultValue)
        {
            return IsDefault() ? defaultValue : this.value;
        }

        public Defaultable<TValueType> Merge(Defaultable<TValueType> other, MergeConflictAction mergeConflictAction)
        {
            return (Defaultable<TValueType>) base.Merge(other, mergeConflictAction);
        }

        public override bool EqualsTo(DefaultableBase other)
        {
            bool equals = base.EqualsTo(other);
            if (equals)
            {
                if (other is Defaultable)
                {
                    Defaultable defaultable = other as Defaultable;
                    Type typeOfValue = this.GetTypeOfValue();
                    if (typeOfValue == defaultable.GetTypeOfValue())
                    {
                        if (EqualsToCanCompareValues(defaultable))
                        {
                            if (GetValue() == null && defaultable.GetValue() == null)
                            {
                                return true;
                            }

                            if (typeOfValue == typeof (byte[]))
                            {
                                byte[] val1 = (byte[]) GetValue();
                                byte[] val2 = (byte[]) defaultable.GetValue();
                                return Util.BytesEqual(val1, val2);
                            }

                            return Comparer.Default.Compare(this.Value, defaultable.GetValue()) == 0;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        private bool EqualsToCanCompareValues(Defaultable other)
        {
            return !IsDefault() && !other.IsDefault();
        }
    }

    #endregion class Defaultable<TValueType>
}