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
    public class OrmKeyAttribute : DefaultableMulti, IOrmAttribute
    {
        public readonly OrmAttributeGroup ATTRIBUTE_GROUP_KEY = new OrmAttributeGroup("Key", OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        public const string ATTRIBUTE_GROUP_ITEM_DIRECTION = "Direction";
        public const string ATTRIBUTE_GROUP_ITEM_POSITION = "Position";

        #region fields

        private readonly string[] propertiesToHide = new[]
                                                     {
                                                         ATTRIBUTE_GROUP_ITEM_DIRECTION, ATTRIBUTE_GROUP_ITEM_POSITION
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

        /// <summary>
        /// Value indicating the sort direction.
        /// </summary>
        [Description("Value indicating the sort direction.")]
        [NotifyParentProperty(true)]
        public Defaultable<KeyDirection> Direction { get; set; }

        /// <summary>
        /// Value indicating the position of persistent property inside primary key.
        /// </summary>
        [Description("Value indicating the position of persistent property inside primary key.")]
        [NotifyParentProperty(true)]
        public Defaultable<int> Position { get; set; }

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

        public OrmKeyAttribute()
        {
            this.Enabled = false;
            this.Direction = new Defaultable<KeyDirection>();
            this.Direction.IsDefault();
            this.Position = new Defaultable<int>();
            this.Position.IsDefault();
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
            if (other is OrmKeyAttribute)
            {
                OrmKeyAttribute attr = (OrmKeyAttribute) other;
                this.Enabled = attr.Enabled;
                this.Position = (Defaultable<int>)attr.Position.Clone();
                this.Direction = (Defaultable<KeyDirection>)attr.Direction.Clone();
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            Defaultable<int> propertyValue = (Defaultable<int>) propertyValues[ATTRIBUTE_GROUP_ITEM_POSITION];
            this.Position = (Defaultable<int>) propertyValue.Clone();

            Defaultable<KeyDirection> keyDirection = (Defaultable<KeyDirection>) propertyValues[ATTRIBUTE_GROUP_ITEM_DIRECTION];
            this.Direction = (Defaultable<KeyDirection>) keyDirection.Clone();
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            OrmKeyAttribute otherAttr = (OrmKeyAttribute) other;
            return this.Position.EqualsTo(otherAttr.Position) &&
                   this.Direction.EqualsTo(otherAttr.Direction);
        }

        public override string ToString()
        {
            return Enabled
                       ? "Is Key"
                       : EnabledDisabledValueTypeConverter.VALUE_TYPE_DISABLED;
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

        public void DeserializeFromXml(XmlReader reader)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            DeserializeFromXml(xmlRoot);
        }

        protected override void DeserializeValueFromXml(XmlProxy parent)
        {
            XmlProxy xmlDirection = parent["direction"];
            this.Direction = new Defaultable<KeyDirection>();
            this.Direction.DeserializeFromXml(xmlDirection);

            XmlProxy xmlPosition = parent["position"];
            this.Position = new Defaultable<int>();
            this.Position.DeserializeFromXml(xmlPosition);
        }

        protected override bool CanSerializeValue()
        {
            return Enabled;
        }

        public void SerializeToXml(XmlWriter writer)
        {
            XmlProxy xmlRoot = new XmlProxy("key");
            SerializeToXml(xmlRoot);
            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        protected override void SerializeValueToXml(XmlProxy parent)
        {
            XmlProxy xmlDirection = parent.AddChild("direction");
            this.Direction.SerializeToXml(xmlDirection);

            XmlProxy xmlPosition = parent.AddChild("position");
            this.Position.SerializeToXml(xmlPosition);
        }

        #endregion methods

        #region Implementation of IOrmAttribute

        public OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            List<OrmAttributeGroup> result = new List<OrmAttributeGroup>();

            if (listMode == AttributeGroupsListMode.All || this.Enabled)
            {
                result.Add(ATTRIBUTE_GROUP_KEY);
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
            get { return OrmAttributeKind.Property; }
        }

        public Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            var result = new Dictionary<string, Defaultable>();

            if (group == ATTRIBUTE_GROUP_KEY)
            {
                result.Add(ATTRIBUTE_GROUP_ITEM_DIRECTION, this.Direction);
                result.Add(ATTRIBUTE_GROUP_ITEM_POSITION, this.Position);
            }

            return result;
        }

        public IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            OrmKeyAttribute other = (OrmKeyAttribute) otherAttribute;
            OrmKeyAttribute mergedResult = new OrmKeyAttribute();

            bool enabled = this.Enabled;
            //if (!other.Enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            if (other.Enabled && !enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            {
                enabled = other.Enabled;
            }

            mergedResult.Enabled = enabled;

            mergedResult.Direction = this.Direction.Merge(other.Direction, mergeConflictAction);
            mergedResult.Position = this.Position.Merge(other.Position, mergeConflictAction);

            return mergedResult;
        }

        public void Validate(ValidationContext context, ModelElement ownerElement)
        {}

        #endregion
    }
}