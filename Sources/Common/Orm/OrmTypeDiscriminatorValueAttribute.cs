using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    [TypeConverter(typeof(EnabledDisabledMultiTypeConvertor))]
    [Editor(typeof(EnabledDisabledEditor), typeof(UITypeEditor))]
    public class OrmTypeDiscriminatorValueAttribute : DefaultableMulti, IOrmAttribute
    {
        #region fields 

        private readonly string[] propertiesToHide = new[] { "Value", "Default" };

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR_VALUE =
            new OrmAttributeGroup("TypeDiscriminatorValue", OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        public const string ATTRIBUTE_GROUP_ITEM_DEFAULT = "Default";
        public const string ATTRIBUTE_GROUP_ITEM_VALUE = "Value";

        #endregion fields

        #region properties 

        [Browsable(false)]
        [TypeConverter(typeof(EnabledDisabledValueTypeConverter))]
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

        [NotifyParentProperty(true)]
        public bool Default { get; set; }

        [ModalDialogEditorArgument(typeof(FilterObjectValueEditorTypes))]
        public ObjectValue Value { get; set; }

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

        public OrmTypeDiscriminatorValueAttribute()
        {
            this.Default = false;
            this.Enabled = false;

            this.Value = new ObjectValue(null);
        }

        #endregion constructors

        #region methods 

        public override string ToString()
        {
            return Enabled
                       ? string.Format("Enabled (Value: {0})", this.Value)
                       : base.ToString();
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
            if (other is OrmTypeDiscriminatorValueAttribute)
            {
                OrmTypeDiscriminatorValueAttribute attr = (OrmTypeDiscriminatorValueAttribute) other;
                this.Enabled = attr.Enabled;
                this.Default = attr.Default;
                this.Value = attr.Value.Clone();
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            this.Default = (bool) propertyValues["Default"];
            this.Value = ((ObjectValue)propertyValues["Value"]).Clone();
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            OrmTypeDiscriminatorValueAttribute otherAttr = (OrmTypeDiscriminatorValueAttribute) other;
            return this.Value.EqualsTo(otherAttr.Value) &&
                   this.Default && otherAttr.Default;
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
            this.Default = xmlContent.GetAttr("default", false);

            this.Value = new ObjectValue();
            this.Value.DeserializeFromXml(xmlContent, "value");
        }

        protected override bool CanSerializeValue()
        {
            return Enabled;
        }

//        public void SerializeToXml(XmlWriter writer, string propertyName)
//        {
//            XmlProxy xmlRoot = new XmlProxy(propertyName);
//            SerializeToXml(xmlRoot);
//            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
//        }

        protected override void SerializeValueToXml(XmlProxy parent)
        {
            XmlProxy xmlContent = parent.AddChild("content");
            xmlContent.SetAttr("default", this.Default);

            this.Value.SerializeToXml(xmlContent, "value");
        }

        #endregion methods

        #region Implementation of IOrmAttribute

        public OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            List<OrmAttributeGroup> result = new List<OrmAttributeGroup>();

            if (listMode == AttributeGroupsListMode.All || this.Enabled)
            {
                result.Add(ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR_VALUE);
            }

            return result.ToArray();
        }

        public List<Defaultable> GetAttributeGroupItems(AttributeGroupsListMode listMode)
        {
            var query = from attrGroup in GetAttributeGroups(listMode)
                        select GetAttributeGroupItems(attrGroup).Values;

            List<Defaultable> items = query.SelectMany(list => list).ToList();
            return items;
        }

/*
        [Browsable(false)]
        string[] IOrmAttribute.AttributeGroups
        {
            get
            {
                List<string> groups = new List<string>();

                if (this.Enabled)
                {
                    groups.Add(ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR_VALUE);
                }

                return groups.ToArray();
            }
        }
*/

        [Browsable(false)]
        OrmAttributeKind IOrmAttribute.AttributeKind
        {
            get
            {
                return OrmAttributeKind.Type;
            }
        }

        public Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            Dictionary<string, Defaultable> result = new Dictionary<string, Defaultable>();

            if (group == ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR_VALUE)
            {
                //if (this.Enabled)
                {
                    Defaultable<bool> defaultable = GetDefaultAsDefaultable();
                    result.Add(ATTRIBUTE_GROUP_ITEM_DEFAULT, defaultable);

                    Defaultable<ObjectValue> defValue = new Defaultable<ObjectValue>();
                    defValue.SetAsCustom(this.Value);

                    result.Add(ATTRIBUTE_GROUP_ITEM_VALUE, defValue);
                }
            }

            return result;
        }

        private Defaultable<bool> GetDefaultAsDefaultable()
        {
            Defaultable<bool> defaultable = new Defaultable<bool>();
            if (this.Enabled)
            {
                defaultable.SetAsCustom(this.Default);
            }
            return defaultable;
        }

        public IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            OrmTypeDiscriminatorValueAttribute other = (OrmTypeDiscriminatorValueAttribute) otherAttribute;
            OrmTypeDiscriminatorValueAttribute mergedResult = new OrmTypeDiscriminatorValueAttribute();

            bool enabled = this.Enabled;
            //if (!other.Enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            if (other.Enabled && !enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            {
                enabled = other.Enabled;
            }

            mergedResult.Enabled = enabled;

            Defaultable<bool> mergedDefault = GetDefaultAsDefaultable().Merge(other.GetDefaultAsDefaultable(), mergeConflictAction);
            mergedResult.Default = mergedDefault.Value;

            ObjectValue value = this.Value;
            if (!this.Value.EqualsTo(other.Value) && other.Enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            {
                value = other.Value;
            }

            mergedResult.Value = value;

            return mergedResult;
        }

        public void Validate(ValidationContext context, ModelElement ownerElement)
        {
            
        }

        #endregion
    }

    #region class FilterObjectValueEditorTypes

    public class FilterObjectValueEditorTypes : IFilterObjectValueEditorTypes
    {
        private ITypeDescriptorContext context;

        public void Initialize(ITypeDescriptorContext context)
        {
            this.context = context;
        }

        public IEnumerable<StandartType> FilterStandartTypes(IEnumerable<StandartType> standartTypes)
        {
            return standartTypes.Where(type => !type.In(StandartType.DateTime, StandartType.TimeSpan));
        }
    }

    #endregion class FilterObjectValueEditorTypes
}