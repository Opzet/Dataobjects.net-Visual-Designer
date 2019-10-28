using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    [TypeConverter(typeof(EnabledDisabledMultiTypeConvertor))]
    [Editor(typeof(EnabledDisabledEditor), typeof(UITypeEditor))]
    public class OrmKeyGeneratorAttribute : DefaultableMulti, IOrmAttribute
    {
        #region fields 

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_KEY_GENERATOR = 
            new OrmAttributeGroup("KeyGenerator", OrmUtils.GetOrmNamespace(OrmNamespace.Orm));
        public const string ATTRIBUTE_GROUP_ITEM_KIND = "Kind";
        public const string ATTRIBUTE_GROUP_ITEM_NAME = "Name";
        public const string ATTRIBUTE_GROUP_ITEM_TYPE = "Type";

        private readonly string[] propertiesToHide = new[]
                                                     {
                                                         ATTRIBUTE_GROUP_ITEM_KIND, ATTRIBUTE_GROUP_ITEM_NAME, ATTRIBUTE_GROUP_ITEM_TYPE
                                                     };

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

        [Description("Specifies key generator type to use for a particular hierarchy.")]
        [DisplayName(ATTRIBUTE_GROUP_ITEM_KIND)]
        [NotifyParentProperty(true)]
        public KeyGeneratorKind Kind { get; set; }

        [Description("Name of key generator")]
        public string Name { get; set; }

        [Description("CLR Full type name of key generator.")]
        public string Type { get; set; }

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

        public OrmKeyGeneratorAttribute()
        {
            this.Enabled = true;

            this.Kind = KeyGeneratorKind.Default;

            this.Name = string.Empty;

            this.Type = string.Empty;
        }

        #endregion constructors

        #region methods 

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
            if (other is OrmKeyGeneratorAttribute)
            {
                OrmKeyGeneratorAttribute attr = (OrmKeyGeneratorAttribute) other;
                this.Enabled = attr.Enabled;
                this.Kind = attr.Kind;
                this.Name = attr.Name;
                this.Type = attr.Type;
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            this.Kind = (KeyGeneratorKind) propertyValues[ATTRIBUTE_GROUP_ITEM_KIND];
            this.Name = (string) propertyValues[ATTRIBUTE_GROUP_ITEM_NAME];
            this.Type = (string) propertyValues[ATTRIBUTE_GROUP_ITEM_TYPE];
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            OrmKeyGeneratorAttribute otherAttr = (OrmKeyGeneratorAttribute) other;
            return this.Kind == otherAttr.Kind &&
                   Util.StringEqual(this.Name, otherAttr.Name, false) &&
                   Util.StringEqual(this.Type, otherAttr.Type, false);
        }

        public override string ToString()
        {
            string str = this.Enabled
                             ? EnabledDisabledValueTypeConverter.VALUE_TYPE_ENABLED
                             : EnabledDisabledValueTypeConverter.VALUE_TYPE_DISABLED;

            if (this.Enabled && this.Kind == KeyGeneratorKind.Custom && !string.IsNullOrEmpty(this.Type))
            {
                str = string.Format("Custom: {0}", this.Type);
            }

            return str;
        }

        protected override void InternalSetValueType()
        {
        }

        protected override string GetValueTypeForNull()
        {
            return EnabledDisabledValueTypeConverter.VALUE_TYPE_ENABLED;
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
            XmlProxy elementValue = parent["value"];

            string kind = elementValue.GetAttr("kind");
            this.Kind = (KeyGeneratorKind) Enum.Parse(typeof (KeyGeneratorKind), kind, true);

            this.Name = elementValue.GetAttr("name");
            this.Type = elementValue.GetAttr("type");
        }

        protected override bool CanSerializeValue()
        {
            return Enabled;
        }

        public void SerializeToXml(XmlWriter writer, string propertyName)
        {
            XmlProxy xmlRoot = new XmlProxy(propertyName);
            SerializeToXml(xmlRoot);
            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        protected override void SerializeValueToXml(XmlProxy parent)
        {
            XmlProxy elementValue = parent.AddChild("value");
            elementValue.SetAttr("kind", this.Kind.ToString());
            elementValue.SetAttr("name", this.Name);
            elementValue.SetAttr("type", this.Type);
        }

        #endregion methods

        #region Implementation of IOrmAttribute


        public OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            List<OrmAttributeGroup> result = new List<OrmAttributeGroup>();

            if (listMode == AttributeGroupsListMode.All || this.Enabled)
            {
                result.Add(ATTRIBUTE_GROUP_KEY_GENERATOR);
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

        [Browsable(false)]
        OrmAttributeKind IOrmAttribute.AttributeKind
        {
            get { return OrmAttributeKind.Type; }
        }

        public Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            Dictionary<string, Defaultable> result = new Dictionary<string, Defaultable>();

            if (group == ATTRIBUTE_GROUP_KEY_GENERATOR)
            {
                var defaultableItems = GetDefaultableItems();

                result.Add(ATTRIBUTE_GROUP_ITEM_KIND, defaultableItems.Item1);
                result.Add(ATTRIBUTE_GROUP_ITEM_NAME, defaultableItems.Item2);
                result.Add(ATTRIBUTE_GROUP_ITEM_TYPE, defaultableItems.Item3);
            }

            return result;
        }

        private Tuple<Defaultable<KeyGeneratorKind>, Defaultable<string>, Defaultable<string>> GetDefaultableItems()
        {
            Defaultable<KeyGeneratorKind> defKind = new Defaultable<KeyGeneratorKind>();
            if (this.Kind != KeyGeneratorKind.Default)
            {
                defKind.SetAsCustom(this.Kind);
            }

            Defaultable<string> defName = new Defaultable<string>();
            if (!string.IsNullOrEmpty(this.Name))
            {
                defName.SetAsCustom(this.Name);
            }

            Defaultable<string> defType = new Defaultable<string>();
            if (!string.IsNullOrEmpty(this.Type))
            {
                defType.SetAsCustom(this.Type);
            }

            return new Tuple<Defaultable<KeyGeneratorKind>, Defaultable<string>, Defaultable<string>>(defKind, defName,
                defType);
        }

        public IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            OrmKeyGeneratorAttribute other = (OrmKeyGeneratorAttribute) otherAttribute;
            OrmKeyGeneratorAttribute mergedResult = new OrmKeyGeneratorAttribute();

            bool enabled = this.Enabled;
            //if (!other.Enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            if (other.Enabled && !enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            {
                enabled = other.Enabled;
            }

            mergedResult.Enabled = enabled;

            var defaultableItems = GetDefaultableItems();
            var otherDefaultableItems = other.GetDefaultableItems();

            var mergedKind = defaultableItems.Item1.Merge(otherDefaultableItems.Item1, mergeConflictAction);
            mergedResult.Kind = mergedKind.IsDefault() ? KeyGeneratorKind.Default : mergedKind.Value;

            var mergedName = defaultableItems.Item2.Merge(otherDefaultableItems.Item2, mergeConflictAction);
            mergedResult.Name = mergedName.Value;

            var mergedType = defaultableItems.Item3.Merge(otherDefaultableItems.Item3, mergeConflictAction);
            mergedResult.Type = mergedType.Value;

            return mergedResult;
        }

        public void Validate(ValidationContext context, ModelElement ownerElement)
        {
            if (!this.Enabled)
            {
                return;
            }
        }

        #endregion
    }

    #region enum KeyGeneratorKind

    [Serializable]
    public enum KeyGeneratorKind
    {
        [XmlEnum("None")]
        None = 0,

        [XmlEnum("Default")]
        Default = 1,

        [XmlEnum("Custom")]
        Custom = 2,
    }

    #endregion enum KeyGeneratorKind
}