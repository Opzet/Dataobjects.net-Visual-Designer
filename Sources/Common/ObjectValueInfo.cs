using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    [TypeConverter(typeof (EnabledDisabledMultiTypeConvertor))]
    [Editor(typeof (EnabledDisabledEditor), typeof (UITypeEditor))]
    public class ObjectValueInfo : DefaultableMulti
    {
        #region fields 

        private readonly string[] propertiesToHide = new[]
            {
                "Value"
            };

        private ObjectValue value;

        #endregion fields

        #region properties

        [Browsable(false)]
        [TypeConverter(typeof (EnabledDisabledValueTypeConverter))]
        [NotifyParentProperty(true)]
        public override string ValueType
        {
            get { return base.ValueType; }
            set
            {
                if (value != ValueType)
                {
                    base.ValueType = value;
                    InternalSetValueType();
                }
            }
        }


        [Description("Gets or sets the default value for this property. Empty (null) indicates default value is provided automatically.")]
        [DisplayName("Default value")]
        [ModalDialogEditorArgument(typeof(FilterObjectValueEditorTypes), typeof(FilterObjectTypesByOwner))]
        public ObjectValue Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (this.value != null)
                {
                    this.value.Tag = this;
                }
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public bool Enabled
        {
            get { return ValueTypeIs(EnabledDisabledValueTypeConverter.VALUE_TYPE_ENABLED); }
            set
            {
                ValueType = value
                                ? EnabledDisabledValueTypeConverter.VALUE_TYPE_ENABLED
                                : EnabledDisabledValueTypeConverter.VALUE_TYPE_DISABLED;
            }
        }

        #endregion properties

        #region constructors 

        public ObjectValueInfo()
        {
            this.Value = new ObjectValue(null);
            this.Enabled = false;
        }

        #endregion constructors

        #region methods

        public override string ToString()
        {
            if (Enabled)
            {
                return Value.Value == null ? "<NULL>" : Value.ToString();
            }

            return "N/A";
        }

        protected override string[] GetPropertyNamesToHideOnDefault()
        {
            return propertiesToHide;
        }

        protected override bool CanHideProperties()
        {
            return !Enabled;
        }

        protected override void InternalAssignFrom(DefaultableMulti other)
        {
            if (other is ObjectValueInfo)
            {
                ObjectValueInfo info = (ObjectValueInfo) other;
                this.Enabled = info.Enabled;
                this.Value = info.Value.Clone();
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            this.Value = ((ObjectValue) propertyValues["Value"]).Clone();
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            ObjectValueInfo otherInfo = (ObjectValueInfo) other;
            return this.Value.EqualsTo(otherInfo.Value);
        }

        protected override void InternalSetValueType()
        {
        }

        protected override string GetValueTypeForNull()
        {
            return EnabledDisabledValueTypeConverter.VALUE_TYPE_DISABLED;
        }

        protected override bool CanDeserializeValue()
        {
            return Enabled;
        }

        protected override bool CanSerializeValue()
        {
            return Enabled;
        }

        public void DeserializeFromXml(XmlReader reader, string propertyName)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            if (xmlRoot.ElementName == propertyName)
            {
                DeserializeFromXml(xmlRoot);
            }
        }

        protected override void DeserializeValueFromXml(XmlProxy parent)
        {
            XmlProxy xmlContent = parent["content"];
            this.Value = new ObjectValue();
            this.Value.DeserializeFromXml(xmlContent, "value");
        }

        protected override void SerializeValueToXml(XmlProxy parent)
        {
            XmlProxy xmlContent = parent.AddChild("content");
            this.Value.SerializeToXml(xmlContent, "value");
        }

        #endregion methods
    }

    public class FilterObjectTypesByOwner : IFilterObjectValueEditorTypes
    {
        private ITypeDescriptorContext context;

        public void Initialize(ITypeDescriptorContext context)
        {
            this.context = context;
        }

        public IEnumerable<StandartType> FilterStandartTypes(IEnumerable<StandartType> standartTypes)
        {
            return standartTypes;
        }
    }
}