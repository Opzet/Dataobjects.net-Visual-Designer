using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Editor(typeof(ModalDialogEditor<FormObjectValueEditor>), typeof(UITypeEditor))]
    [Serializable]
    public sealed class ObjectValue: IComparable, ICloneable
    {
        [Browsable(false)]
        public bool UseCustomExpression { get; set; }

        [Browsable(false)]
        public string CustomExpression { get; set; }

        public object Value { get; set; }

        [Browsable(false)]
        public object Tag { get; set; }

        public ObjectValue(bool useCustomExpression, string customExpression, object value)
        {
            UseCustomExpression = useCustomExpression;
            CustomExpression = customExpression;
            this.Value = value;
        }

        public ObjectValue(object value): this(false, null, value)
        {
        }

        public ObjectValue() {}

        public bool EqualsTo(ObjectValue other)
        {
//            return (this.UseCustomExpression && other.UseCustomExpression &&
//                    Util.StringEqual(this.CustomExpression, other.CustomExpression, true)) ||
//                   (!this.UseCustomExpression && !other.UseCustomExpression && Object.Equals(this.Value, other.Value));
            return this.UseCustomExpression == other.UseCustomExpression &&
                   Util.StringEqual(this.CustomExpression, other.CustomExpression, true) &&
                   Comparer.Default.Compare(this.Value, other.Value) == 0;
        }

        public override string ToString()
        {
            if (UseCustomExpression)
            {
                return CustomExpression;
            }

            return Value == null ? string.Empty : Value.ToString();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is ObjectValue))
            {
                return -1;
            }

            return EqualsTo((ObjectValue) obj) ? 0 : -1;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ObjectValue Clone()
        {
            object value = this.Value;

            if (this.Value != null && this.Value is ICloneable)
            {
                value = (this.Value as ICloneable).Clone();
            }

            ObjectValue cloned = new ObjectValue(this.UseCustomExpression, this.CustomExpression, value);
            return cloned;
        }

        public void DeserializeFromXml(XmlProxy parent, string propertyName)
        {
            if (parent == null)
            {
                return;
            }

            XmlProxy xmlRoot = parent.Childs[propertyName];
            this.UseCustomExpression = xmlRoot.GetAttr("useCustomExpression", false);
            XmlProxy xmlValue = xmlRoot.Childs["Value"];
            if (this.UseCustomExpression)
            {
                this.CustomExpression = xmlValue.ElementValue;
            }
            else
            {
                string clrTypeName = xmlValue.GetAttr("type");
                string serializedValue = xmlValue.ElementValue;

                if (string.IsNullOrEmpty(clrTypeName) && string.IsNullOrEmpty(serializedValue))
                {
                    this.Value = null;
                }
                else
                {
                    Type type = Type.GetType(clrTypeName);
                    if (type == typeof(byte[]))
                    {
                        this.Value = Util.HexStringToBytes(serializedValue);
                    }
                    else
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(type);
                        this.Value = converter.ConvertFrom(serializedValue);
                    }
                }
            }
        }

        public void SerializeToXml(XmlProxy parent, string propertyName)
        {
            XmlProxy xmlRoot = parent.AddChild(propertyName);
            xmlRoot.AddAttribute("useCustomExpression", UseCustomExpression);
            XmlProxy xmlValue = xmlRoot.AddChild("Value");

            if (UseCustomExpression)
            {
                xmlValue.ElementValue = this.CustomExpression;
            }
            else if (Value != null)
            {
                xmlValue.AddAttribute("type", Value.GetType().AssemblyQualifiedName);
                xmlValue.ElementValue = SerializeValueToXml(Value);
            }
        }

        private string SerializeValueToXml(object value)
        {
            string result = string.Empty;

            if (value != null)
            {
                //StandartType standardType = StandardValues.ResolveStandardType(value.GetType());

                Type valueType = value.GetType();
                if (valueType == typeof(byte[]))
                {
                    result = Util.BytesToHexString((byte[]) value);
                }
                else
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(valueType);
                    result = typeConverter.ConvertTo(value, typeof (string)) as string;
                }
            }

            return result;
        }

/*        public void DeserializeFromXml(XmlReader reader, string propertyName)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            if (xmlRoot.ElementName == propertyName)
            {
                string useCustomExpression = xmlRoot.GetAttr("useCustomExpression");
                this.UseCustomExpression = bool.Parse(useCustomExpression);

                XmlProxy xmlValue = xmlRoot.Childs["Value"];
                if (UseCustomExpression)
                {
                    this.CustomExpression = xmlValue.ElementValue;
                }
                else
                {
                    string serializedValue = xmlValue.ToString();
                    if (!string.IsNullOrEmpty(serializedValue))
                    {
                        this.Value = Util.DeserializeObjectFromXml<object>(serializedValue);
                    }
                }
            }
        }

        public void SerializeToXml(XmlWriter writer, string propertyName)
        {
            writer.WriteStartElement(propertyName);
            writer.WriteAttributeString("useCustomExpression", UseCustomExpression.ToString());
            
            writer.WriteStartElement("Value");
            if (UseCustomExpression)
            {
                if (!string.IsNullOrEmpty(CustomExpression))
                {
                    writer.WriteValue(CustomExpression);
                }
            }
            else
            {
                string serializedValue = string.Empty;
                if (!Object.Equals(this.Value, null))
                {
                    serializedValue = Util.SerializeObjectToXml(this.Value);
                }
                if (!string.IsNullOrEmpty(serializedValue))
                {
                    writer.WriteRaw(serializedValue);
                }
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }*/
    }
}