using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    [TypeConverter(typeof(PropertyAccessModifiersTypeConverter))]
    public class PropertyAccessModifiers : ISerializableObject
    {
        //private static readonly Type typeAccessModifier = typeof(PropertyAccessModifier);

        [Description("Property getter access modifier.")]
        public PropertyAccessModifier Getter { get; set; }

        [Description("Property setter access modifier.")]
        public PropertyAccessModifier Setter { get; set; }

        public PropertyAccessModifiers()
        {
            this.Getter = PropertyAccessModifier.Public;
            this.Setter = PropertyAccessModifier.Public;
        }

        public override string ToString()
        {
            return string.Format("Getter: {0}, Setter: {1}", this.Getter, this.Setter);
        }

        public void DeserializeFromXml(XmlReader reader)
        {
            DeserializeFromXml(reader, "propertyAccessModifiers");
        }

        public void DeserializeFromXml(XmlReader reader, string propertyName)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            if (xmlRoot.ElementName == propertyName)
            {
                this.Getter = (PropertyAccessModifier)xmlRoot.GetAttr("getter", (int)PropertyAccessModifier.Public);
                this.Setter = (PropertyAccessModifier)xmlRoot.GetAttr("setter", (int)PropertyAccessModifier.Public);
            }
        }

        public void SerializeToXml(XmlWriter writer)
        {
            SerializeToXml(writer, "propertyAccessModifiers");
        }

        public void SerializeToXml(XmlWriter writer, string propertyName)
        {
            XmlProxy xmlRoot = new XmlProxy(propertyName);
            xmlRoot.AddAttribute("getter", this.Getter);
            xmlRoot.AddAttribute("setter", this.Setter);

            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        public PropertyAccessModifiers Clone()
        {
            return new PropertyAccessModifiers
                {
                    Getter = this.Getter,
                    Setter = this.Setter
                };
        }

        public PropertyAccessModifier GetHigherModifier()
        {
            int getterModifier = (int) this.Getter;
            int setterModifier = (int) this.Setter;

            var modifier = getterModifier > setterModifier ? Getter : Setter;
            return modifier;
        }
    }

    public class PropertyAccessModifiersTypeConverter : ExpandableObjectConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            //ModelElement owner = context.Instance as ModelElement;
            PropertyAccessModifiers currentObj = (PropertyAccessModifiers)context.PropertyDescriptor.GetValue(context.Instance);

            PropertyAccessModifier getter = (PropertyAccessModifier)propertyValues["Getter"];
            PropertyAccessModifier setter = (PropertyAccessModifier)propertyValues["Setter"];

            PropertyAccessModifiers result = new PropertyAccessModifiers
            {
                Getter = getter,
                Setter = setter
            };

            return result;
        }
    }
}