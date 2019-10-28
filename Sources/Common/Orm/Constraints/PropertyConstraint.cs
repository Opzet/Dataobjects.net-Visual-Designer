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
    [TypeConverter(typeof(UsedNotUsedMultiTypeConvertor))]
    [Editor(typeof(UsedNotUsedEditor), typeof(UITypeEditor))]
    public abstract class PropertyConstraint : DefaultableMulti, IOrmAttribute
    {
        #region properties

        [XmlAttribute("mode")]
        public PropertyConstrainMode Mode { get; set; }

        [Browsable(false)]
        public string DisplayName
        {
            get { return GetDisplayName(); }
        }

        [Browsable(false)]
        public PropertyConstrainType ConstrainType
        {
            get { return GetConstrainType(); }
        }

        [Description("Non-localizable message of exception to show if property value is invalid.")]
        [DisplayName("Error Message")]
        public string ErrorMessage { get; set; }
        //TODO: Add support of 'MessageResourceName' and 'MessageResourceType'


        [Browsable(false)]
        [TypeConverter(typeof(UsedNotUsedValueTypeConverter))]
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

        [Browsable(false)]
        [XmlIgnore]
        public bool Used
        {
            get { return ValueTypeIs(UsedNotUsedValueTypeConverter.VALUE_TYPE_USED); }
            set
            {
                ValueType = value
                                ? UsedNotUsedValueTypeConverter.VALUE_TYPE_USED
                                : UsedNotUsedValueTypeConverter.VALUE_TYPE_NOT_USED;
            }
        }

        #endregion properties

        #region constructors

        protected PropertyConstraint()
        {
            this.Used = false;
            this.Mode = PropertyConstrainMode.Default;
        }

        #endregion constructors

        #region methods

//        public override string ToString()
//        {
//            return this.Used ? InternalToString() : "Not Used";
//        }

        protected virtual string InternalToString()
        {
            return "Used";
        }

//        public virtual void AssignFrom(PropertyConstraint other)
//        {
//            this.Mode = other.Mode;
//        }

        protected override bool CanHideProperties()
        {
            return !Used;
        }

        protected override string[] GetPropertyNamesToHideOnDefault()
        {
            List<string> propertiesToHide = new List<string>();
            propertiesToHide.AddRange(new []
                                      {
                                          "ErrorMessage", "Mode"
                                      });
            FillPropertyNamesToHideOnDefault(propertiesToHide);

            return propertiesToHide.ToArray();
        }

        protected abstract void FillPropertyNamesToHideOnDefault(List<string> propertiesToHide);

        protected override void InternalAssignFrom(DefaultableMulti other)
        {
            if (other is PropertyConstraint)
            {
                PropertyConstraint otherContraint = (PropertyConstraint)other;
                this.Used = otherContraint.Used;
                this.Mode = otherContraint.Mode;
                this.ErrorMessage = otherContraint.ErrorMessage;
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            this.Mode = (PropertyConstrainMode) propertyValues["Mode"];
            this.ErrorMessage = propertyValues["ErrorMessage"] as string;
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            PropertyConstraint otherContraint = (PropertyConstraint)other;
            return this.Mode == otherContraint.Mode &&
                   Util.StringEqual(this.ErrorMessage, otherContraint.ErrorMessage, false);
        }

        protected override string GetValueTypeForNull()
        {
            return UsedNotUsedValueTypeConverter.VALUE_TYPE_NOT_USED;
        }

        protected override bool CanDeserializeValue()
        {
            return Used;
        }

        protected override bool CanSerializeValue()
        {
            return Used;
        }

//        public IOrmAttribute CreateOrmAttribute()
//        {
//            return InternalCreateOrmAttribute();
//        }
//
        //protected internal abstract PropertyConstraintOrmAttribute InternalCreateOrmAttribute();


        internal static PropertyConstraint CreateInstance(PropertyConstrainType constrainType)
        {
            switch (constrainType)
            {
                case PropertyConstrainType.Email:
                    return new PropertyEmailConstraint();
                case PropertyConstrainType.Future:
                    return new PropertyFutureConstraint();
                case PropertyConstrainType.Length:
                    return new PropertyLengthConstraint();
                case PropertyConstrainType.NotEmpty:
                    return new PropertyNotEmptyConstraint();
                case PropertyConstrainType.NotNull:
                    return new PropertyNotNullConstraint();
                case PropertyConstrainType.NotNullOrEmpty:
                    return new PropertyNotNullOrEmptyConstraint();
                case PropertyConstrainType.Past:
                    return new PropertyPastConstraint();
                case PropertyConstrainType.Range:
                    return new PropertyRangeConstraint();
                case PropertyConstrainType.Regex:
                    return new PropertyRegexConstraint();
                default:
                    throw new ApplicationException("unknown constrainType");
            }
        }

        internal static PropertyConstrainType DeserializeType(XmlProxy xml)
        {
            PropertyConstrainType type = (PropertyConstrainType)xml.GetAttr("type", (int)PropertyConstrainType.NotNull);
            return type;
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
//            PropertyConstrainType type = DeserializeType(xmlContent);
//            if (type != this.ConstrainType)
//            {
//                throw new SerializationException(
//                    string.Format("Serialized ConstrainType ({0}) is not equal to current constraint type ({1})", type, this.ConstrainType));
//            }

            this.Mode = (PropertyConstrainMode)xmlContent.GetAttr("mode", (int)PropertyConstrainMode.Default);
            XmlProxy xmlError = xmlContent.Childs["error"];
            this.ErrorMessage = xmlError.GetAttr("message");

            InternalDeserializeFromXml(xmlContent);
        }

        protected override void SerializeValueToXml(XmlProxy parent)
        {
            XmlProxy xmlRoot = new XmlProxy("content");
            if (parent != null)
            {
                parent.AddChild(xmlRoot);
            }
            //xmlRoot.AddAttribute("type", this.ConstrainType);
            xmlRoot.AddAttribute("mode", this.Mode);
            XmlProxy xmlError = xmlRoot.AddChild("error");
            xmlError.AddAttribute("message", this.ErrorMessage);
            InternalSerializeToXml(xmlRoot);
        }

        protected abstract void InternalDeserializeFromXml(XmlProxy content);

        protected abstract void InternalSerializeToXml(XmlProxy content);

        protected abstract string GetDisplayName();

        protected abstract string GetDescription();

        protected abstract PropertyConstrainType GetConstrainType();

        protected internal abstract OrmAttributeGroup GetAttributeGroup();

        #endregion methods

        #region Implementation of IOrmAttribute

        OrmAttributeKind IOrmAttribute.AttributeKind
        {
            get { return OrmAttributeKind.Property; }
        }

        OrmAttributeGroup[] IOrmAttribute.GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            OrmAttributeGroup[] allAttributeGroups =
                listMode == AttributeGroupsListMode.All || this.Used
                ? GetAllAttributeGroups()
                : new OrmAttributeGroup[0];

            return allAttributeGroups;
        }

        List<Defaultable> IOrmAttribute.GetAttributeGroupItems(AttributeGroupsListMode listMode)
        {
            var query = from attrGroup in (this as IOrmAttribute).GetAttributeGroups(listMode)
                        select (this as IOrmAttribute).GetAttributeGroupItems(attrGroup).Values;

            List<Defaultable> items = query.SelectMany(list => list).ToList();
            return items;
        }

        public Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            Dictionary<string, Defaultable> attributeGroupItems = new Dictionary<string, Defaultable>();
            if (GetAttributeGroup().EqualsTo(@group))
            {
                if (this.Mode != PropertyConstrainMode.Default)
                {
                    attributeGroupItems.Add("Mode", GetModeAsDefaultable());
                }

                if (!string.IsNullOrEmpty(this.ErrorMessage))
                {
                    attributeGroupItems.Add("Message", GetErrorMessageAsDefaultable());
                }

                FillAttributeGroupItems(attributeGroupItems);
            }
            return attributeGroupItems;
        }

        private Defaultable<string> GetErrorMessageAsDefaultable()
        {
            Defaultable<string> result = new Defaultable<string>();
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                result.SetAsCustom(this.ErrorMessage);
            }

            return result;
        }

        private Defaultable<PropertyConstrainMode> GetModeAsDefaultable()
        {
            Defaultable<PropertyConstrainMode> result = new Defaultable<PropertyConstrainMode>();
            if (this.Mode != PropertyConstrainMode.Default)
            {
                result.SetAsCustom(this.Mode);
            }

            return result;
        }

        protected OrmAttributeGroup[] GetAllAttributeGroups()
        {
            return new[] { GetAttributeGroup() };
        }

//        protected virtual void FilterAttributeGroups(List<string> attributeGroupsToFilter)
//        {
//            if (!this.Used)
//            {
//                attributeGroupsToFilter.Remove(GetAttributeGroupName());
//            }
//        }

        protected abstract void FillAttributeGroupItems(Dictionary<string, Defaultable> attributeGroupItems);

        public IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            PropertyConstraint mergedConstraint = CreateInstance(this.ConstrainType);
            PropertyConstraint other = (PropertyConstraint) otherAttribute;

            string mergedValueType;
            PropertyConstrainMode mergedMode;
            string mergedErrorMessage;
            if ((this.Used && other.Used) || (!this.Used && !other.Used))
            {
                if (mergeConflictAction == MergeConflictAction.TakeOther)
                {
                    mergedValueType = other.ValueType;
                    mergedMode = other.Mode;
                    mergedErrorMessage = other.ErrorMessage;
                }
                else
                {
                    mergedValueType = this.ValueType;
                    mergedMode = this.Mode;
                    mergedErrorMessage = this.ErrorMessage;
                }
            }
            else
            {
                if (this.Used)
                {
                    mergedValueType = this.ValueType;
                    mergedMode = this.Mode;
                    mergedErrorMessage = this.ErrorMessage;
                }
                else // other.IsCustom()
                {
                    mergedValueType = other.ValueType;
                    mergedMode = other.Mode;
                    mergedErrorMessage = other.ErrorMessage;
                }
            }

            mergedConstraint.Mode = mergedMode;
            mergedConstraint.ErrorMessage = mergedErrorMessage;
            mergedConstraint.ValueType = mergedValueType;
            if (mergedConstraint.Used)
            {
                InternalMergeChanges(ref mergedConstraint, other, mergeConflictAction);
            }

            return mergedConstraint;
        }

        public void Validate(ValidationContext context, ModelElement ownerElement)
        {
            
        }

        protected virtual void InternalMergeChanges(ref PropertyConstraint mergedConstraint, PropertyConstraint otherConstraint,
            MergeConflictAction mergeConflictAction)
        {}

        #endregion
    }
}