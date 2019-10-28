using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    #region class DataContractDescriptor

    /// <summary>
    /// Descriptor for class <see cref="DataContract"/>.
    /// </summary>
    [Serializable]
    public class DataContractDescriptor : ContractDescriptorBase
    {
        #region fields 

        private readonly string[] localPropertiesToHide = new[] { "IsReference", "Namespace" };
        private readonly OrmAttributeGroup AttributeGroup = new OrmAttributeGroup("DataContractAttribute", "System.Runtime.Serialization");

        #endregion fields

        /// <summary>
        /// Gets or sets a value indicating whether to preserve object reference data.
        /// See <see cref="DataContractAttribute.IsReference"/>.
        /// </summary>
        public Defaultable<bool> IsReference { get; set; }

        /// <summary>
        /// Gets or sets the namespace for the data contract for the type. 
        /// See <see cref="DataContractAttribute.Namespace"/>.
        /// </summary>
        public string Namespace { get; set; }

        public DataContractDescriptor()
        {
            this.IsReference = new Defaultable<bool>();
            this.IsReference.SetAsDefault();
        }

        protected override void InternalAssignFrom(DefaultableMulti other)
        {
            base.InternalAssignFrom(other);
            DataContractDescriptor otherDescriptor = other as DataContractDescriptor;
            if (otherDescriptor != null)
            {
                this.IsReference = otherDescriptor.IsReference;
                this.Namespace = otherDescriptor.Namespace;
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            base.InternalAssignFromPropertyValues(propertyValues);
            this.IsReference = (Defaultable<bool>) propertyValues["IsReference"];
            this.Namespace = (string) propertyValues["Namespace"];
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            bool equalsTo = base.InternalEqualsTo(other);
            DataContractDescriptor otherDescriptor = other as DataContractDescriptor;
            equalsTo &= otherDescriptor != null;
            if (equalsTo)
            {
                return this.IsReference == otherDescriptor.IsReference &&
                       Util.StringEqual(this.Namespace, otherDescriptor.Namespace, true);
            }

            return equalsTo;
        }

        protected override void FillPropertyNamesToHideOnDefault(List<string> propertiesToHide)
        {
            propertiesToHide.AddRange(localPropertiesToHide);
        }

        protected override void InternalDeserializeFromXml(XmlProxy xmlRoot)
        {
            //this.IsReference = xmlRoot.GetAttr("isReference", false);
            var xmlisReference = xmlRoot["isReference"];
            this.IsReference = new Defaultable<bool>();
            this.IsReference.DeserializeFromXml(xmlisReference);

            this.Namespace = xmlRoot.GetAttr("namespace");
        }

        protected override void InternalSerializeToXml(XmlProxy xmlRoot)
        {
            //xmlRoot.AddAttribute("isReference", IsReference);
            var xmlisReference = xmlRoot.AddChild("isReference");
            this.IsReference.SerializeToXml(xmlisReference);

            xmlRoot.AddAttribute("namespace", this.Namespace);
        }

        protected override OrmAttributeKind GetAttributeKind()
        {
            return OrmAttributeKind.Type;
        }

        protected override OrmAttributeGroup GetOrmAttributeGroup()
        {
            return AttributeGroup;
        }

        protected override void InternalGetAttributeGroupItems(OrmAttributeGroup @group, Dictionary<string, Defaultable> groupItems)
        {
            if (group == AttributeGroup)
            {
                groupItems.Add("IsReference", IsReference);

                Defaultable<string> namespaceDefaultable = new Defaultable<string>();
                if (!string.IsNullOrEmpty(this.Namespace))
                {
                    namespaceDefaultable.Value = this.Namespace;
                }
                groupItems.Add("Namespace", namespaceDefaultable);
            }
        }

        protected override ContractDescriptorBase InternalMergeChanges(ContractDescriptorBase contractDescriptorBase,
            MergeConflictAction mergeConflictAction)
        {
            return contractDescriptorBase;
        }
    }

    #endregion class DataContractDescriptor

    #region class DataMemberDescriptor

    /// <summary>
    /// Descriptor for class <see cref="DataMember"/>.
    /// </summary>
    [Serializable]
    public class DataMemberDescriptor : ContractDescriptorBase
    {
        #region fields

        private readonly string[] localPropertiesToHide = new[] { "EmitDefaultValue", "IsRequired", "Order" };
        private readonly OrmAttributeGroup AttributeGroup = new OrmAttributeGroup("DataMemberAttribute", "System.Runtime.Serialization");

        #endregion fields

        /// <summary>
        /// Gets or sets a value that specifies whether to serialize the default value for a field or property being serialized.
        /// See <see cref="DataMember.EmitDefaultValue"/>.
        /// </summary>
        public Defaultable<bool> EmitDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value that instructs the serialization engine that the member must be present when reading or deserializing.
        /// See <see cref="DataMember.IsRequired"/>
        /// </summary>
        public Defaultable<bool> IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the order of serialization and deserialization of a member.
        /// See <see cref="DataMember.Order"/>
        /// </summary>
        public Defaultable<int> Order { get; set; }

        public DataMemberDescriptor()
        {
            this.EmitDefaultValue = new Defaultable<bool>();
            this.EmitDefaultValue.SetAsDefault();
            this.IsRequired = new Defaultable<bool>();
            this.IsRequired.SetAsDefault();
            this.Order = new Defaultable<int>();
            this.Order.SetAsDefault();
        }

        protected override OrmAttributeKind GetAttributeKind()
        {
            return OrmAttributeKind.Property;
        }

        protected override OrmAttributeGroup GetOrmAttributeGroup()
        {
            return AttributeGroup;
        }

        #region Overrides of ContractDescriptorBase

        protected override void InternalGetAttributeGroupItems(OrmAttributeGroup @group, Dictionary<string, Defaultable> groupItems)
        {
            if (group == AttributeGroup)
            {
                groupItems.Add("EmitDefaultValue", EmitDefaultValue);
                groupItems.Add("IsRequired", IsRequired);
                groupItems.Add("Order", Order);
            }
        }

        #endregion

        protected override ContractDescriptorBase InternalMergeChanges(ContractDescriptorBase contractDescriptorBase,
            MergeConflictAction mergeConflictAction)
        {
            return contractDescriptorBase;
        }

        protected override void InternalAssignFrom(DefaultableMulti other)
        {
            base.InternalAssignFrom(other);
            DataMemberDescriptor otherDescriptor = other as DataMemberDescriptor;
            if (otherDescriptor != null)
            {
                this.EmitDefaultValue = otherDescriptor.EmitDefaultValue;
                this.IsRequired = otherDescriptor.IsRequired;
                this.Order = otherDescriptor.Order;
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            base.InternalAssignFromPropertyValues(propertyValues);
            this.EmitDefaultValue = (Defaultable<bool>)propertyValues["EmitDefaultValue"];
            this.IsRequired = (Defaultable<bool>)propertyValues["IsRequired"];
            this.Order = (Defaultable<int>) propertyValues["Order"];
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            bool equalsTo = base.InternalEqualsTo(other);
            DataMemberDescriptor otherDescriptor = other as DataMemberDescriptor;
            equalsTo &= otherDescriptor != null;
            if (equalsTo)
            {
                return this.EmitDefaultValue.EqualsTo(otherDescriptor.EmitDefaultValue) &&
                       this.IsRequired.EqualsTo(otherDescriptor.IsRequired) &&
                       this.Order.EqualsTo(otherDescriptor.Order);
            }

            return equalsTo;
        }

        protected override void FillPropertyNamesToHideOnDefault(List<string> propertiesToHide)
        {
            propertiesToHide.AddRange(localPropertiesToHide);
        }

        protected override void InternalDeserializeFromXml(XmlProxy xmlRoot)
        {
            //this.EmitDefaultValue = xmlRoot.GetAttr("emitDefaultValue", true);
            var xmlEmitDefaultValue = xmlRoot["emitDefaultValue"];
            this.EmitDefaultValue = new Defaultable<bool>();
            this.EmitDefaultValue.DeserializeFromXml(xmlEmitDefaultValue);

            //this.IsRequired = xmlRoot.GetAttr("isRequired", false);
            var xmlIsRequired = xmlRoot["isRequired"];
            this.IsRequired = new Defaultable<bool>();
            this.IsRequired.DeserializeFromXml(xmlIsRequired);

            //this.Order = xmlRoot.GetAttr("order", 0);
            var xmlOrder = xmlRoot["order"];
            this.Order = new Defaultable<int>();
            this.Order.DeserializeFromXml(xmlOrder);
        }

        protected override void InternalSerializeToXml(XmlProxy xmlRoot)
        {
            var xmlEmitDefaultValue = xmlRoot.AddChild("emitDefaultValue");
            EmitDefaultValue.SerializeToXml(xmlEmitDefaultValue);
            //xmlRoot.AddAttribute("emitDefaultValue", EmitDefaultValue);

            var xmlIsRequired = xmlRoot.AddChild("isRequired");
            IsRequired.SerializeToXml(xmlIsRequired);
            //xmlRoot.AddAttribute("isRequired", IsRequired);

            var xmlOrder = xmlRoot.AddChild("order");
            Order.SerializeToXml(xmlOrder);
            //xmlRoot.AddAttribute("order", Order);
        }
    }

    #endregion class DataMemberDescriptor

    #region class ContractDescriptorBase

    [Serializable]
    [TypeConverter(typeof(ContractDescriptorApplyModeTypeConvertor))]
    [Editor(typeof(ContractDescriptorApplyModeEditor), typeof(UITypeEditor))]
    public abstract class ContractDescriptorBase : DefaultableMulti, ISerializableObject, IOrmAttribute
    {
        /// <summary>
        /// Gets or sets the name of the data contract for the type. 
        /// </summary>
        [NotifyParentProperty(true)]
        public string Name { get; set; }

        [Browsable(false)]
        [TypeConverter(typeof(ContractDescriptorApplyModeValueTypeConverter))]
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
        public ContractDescriptorApplyMode Mode
        {
            get { return GetMode(); }
            set
            {
                SetMode(value);
            }
        }

        private ContractDescriptorApplyMode GetMode()
        {
            ContractDescriptorApplyMode mode = ContractDescriptorApplyMode.Default;
            if (ValueTypeIs(ContractDescriptorApplyModeValueTypeConverter.VALUE_TYPE_ENABLED))
            {
                mode = ContractDescriptorApplyMode.Enabled;
            }
            else if (ValueTypeIs(ContractDescriptorApplyModeValueTypeConverter.VALUE_TYPE_DISABLED))
            {
                mode = ContractDescriptorApplyMode.Disabled;
            }

            return mode;
        }

        private void SetMode(ContractDescriptorApplyMode mode)
        {
            switch (mode)
            {
                case ContractDescriptorApplyMode.Default:
                {
                    this.ValueType = ContractDescriptorApplyModeValueTypeConverter.VALUE_TYPE_DEFAULT;
                    break;
                }
                case ContractDescriptorApplyMode.Enabled:
                {
                    this.ValueType = ContractDescriptorApplyModeValueTypeConverter.VALUE_TYPE_ENABLED;
                    break;
                }
                case ContractDescriptorApplyMode.Disabled:
                {
                    this.ValueType = ContractDescriptorApplyModeValueTypeConverter.VALUE_TYPE_DISABLED;
                    break;
                }
            }
        }

        protected ContractDescriptorBase()
        {
            this.Mode = ContractDescriptorApplyMode.Default;
        }

        public override string ToString()
        {
            return this.Mode.ToString();
        }

        protected override bool CanHideProperties()
        {
            return this.Mode == ContractDescriptorApplyMode.Disabled;
        }

        protected override string[] GetPropertyNamesToHideOnDefault()
        {
            List<string> propertiesToHide = new List<string>();
            propertiesToHide.AddRange(new[]
                                      {
                                          "Name"
                                      });
            FillPropertyNamesToHideOnDefault(propertiesToHide);

            return propertiesToHide.ToArray();
        }

        protected abstract void FillPropertyNamesToHideOnDefault(List<string> propertiesToHide);

        protected override void InternalAssignFrom(DefaultableMulti other)
        {
            if (other is ContractDescriptorBase)
            {
                ContractDescriptorBase otherDescriptor = (ContractDescriptorBase)other;
                this.Name = otherDescriptor.Name;
                this.Mode = otherDescriptor.Mode;
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            this.Name = (string)propertyValues["Name"];
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            ContractDescriptorBase otherDescriptor = (ContractDescriptorBase)other;
            return this.Mode == otherDescriptor.Mode &&
                   Util.StringEqual(this.Name, otherDescriptor.Name, false);
        }

        protected override string GetValueTypeForNull()
        {
            return ContractDescriptorApplyModeValueTypeConverter.VALUE_TYPE_DISABLED;
        }

        protected override bool CanDeserializeValue()
        {
            return Mode != ContractDescriptorApplyMode.Disabled;
        }

        protected override bool CanSerializeValue()
        {
            return Mode != ContractDescriptorApplyMode.Disabled;
        }

        protected override void DeserializeValueFromXml(XmlProxy parent)
        {
            XmlProxy xmlContent = parent["content"];
            this.Name = xmlContent.GetAttr("name");
            InternalDeserializeFromXml(xmlContent);
        }

        protected override void SerializeValueToXml(XmlProxy parent)
        {
            XmlProxy xmlContent = parent.AddChild("content");
            xmlContent.AddAttribute("name", Name);
            InternalSerializeToXml(xmlContent);
        }

        protected abstract void InternalDeserializeFromXml(XmlProxy xmlRoot);

        protected abstract void InternalSerializeToXml(XmlProxy xmlRoot);

        #region Implementation of IOrmAttribute

        [Browsable(false)]
        public OrmAttributeKind AttributeKind
        {
            get { return GetAttributeKind(); }
        }

        protected abstract OrmAttributeKind GetAttributeKind();

        public OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            OrmAttributeGroup attributeGroup = GetOrmAttributeGroup();
            return new[] {attributeGroup};
        }

        protected abstract OrmAttributeGroup GetOrmAttributeGroup();

        public List<Defaultable> GetAttributeGroupItems(AttributeGroupsListMode listMode)
        {
            var query = from attrGroup in GetAttributeGroups(listMode)
                        select GetAttributeGroupItems(attrGroup).Values;

            List<Defaultable> items = query.SelectMany(list => list).ToList();
            return items;
        }

        public virtual Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            Dictionary<string, Defaultable> result = new Dictionary<string, Defaultable>();
            Defaultable<string> nameDefaultable = new Defaultable<string>();
            if (!string.IsNullOrEmpty(this.Name))
            {
                nameDefaultable.Value = this.Name;
            }
            result.Add("Name", nameDefaultable);
            InternalGetAttributeGroupItems(group, result);

            return result;
        }

        protected abstract void InternalGetAttributeGroupItems(OrmAttributeGroup @group,
            Dictionary<string, Defaultable> groupItems);

        public IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            return
                (ContractDescriptorBase)
                InternalMergeChanges(otherAttribute as ContractDescriptorBase, mergeConflictAction);
        }

        protected abstract ContractDescriptorBase InternalMergeChanges(ContractDescriptorBase contractDescriptorBase,
            MergeConflictAction mergeConflictAction);

        public void Validate(ValidationContext context, ModelElement ownerElement)
        {
            
        }

        #endregion
    }

    #endregion class ContractDescriptorBase

    #region class ContractDescriptorApplyModeValueTypeConverter

    public class ContractDescriptorApplyModeValueTypeConverter : EnabledDisabledValueTypeConverter
    {
        public const string VALUE_TYPE_DEFAULT = "Default";
        internal static readonly string[] _standardValues = new[] { VALUE_TYPE_DEFAULT};

        internal static string[] GetStandardValues()
        {
            return _standardValues.Concat(standardValues).ToArray();
        }

        protected internal override string[] BindStandardValues(ITypeDescriptorContext context)
        {
            return _standardValues.Concat(standardValues).ToArray();
        }
    }

    #endregion class ContractDescriptorApplyModeValueTypeConverter

    #region class ContractDescriptorApplyModeTypeConvertor

    public class ContractDescriptorApplyModeTypeConvertor : DefaultableMultiTypeConvertor
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        protected internal override bool SelectListItemByValue(ListItem item, object value)
        {
            DefaultableMulti defaultableMulti = value as DefaultableMulti;
            if (defaultableMulti != null)
            {
                return defaultableMulti.ValueType.Equals(item.Value);
            }

            return false;
        }

        protected internal override object GetValueFromSelectedListItem(object lastSelectedValue, ListItem listItem)
        {
            DefaultableMulti lastValue = lastSelectedValue as DefaultableMulti;
            if (lastValue != null)
            {
                DefaultableMulti clone = (DefaultableMulti)lastValue.Clone();
                clone.ValueType = listItem == null ? clone.ValueTypeForNull : (string)listItem.Value;
                return clone;
            }

            return listItem.Value;
        }

        protected internal override string[] BindStandardValues(ITypeDescriptorContext context)
        {
            return ContractDescriptorApplyModeValueTypeConverter.GetStandardValues();
        }
    }

    #endregion class ContractDescriptorApplyModeTypeConvertor
    
    #region class ContractDescriptorApplyModeEditor

    public class ContractDescriptorApplyModeEditor : StandardValuesEditorBase<ContractDescriptorApplyModeValueTypeConverter>
    { }

    #endregion class ContractDescriptorApplyModeEditor

    #region enum ContractDescriptorApplyMode

    [Serializable]
    public enum ContractDescriptorApplyMode
    {
        /// <summary>
        /// [DataContract] attribute will decorate generated classes by rules defined in target T4 template.
        /// </summary>
        Default,

        /// <summary>
        /// [DataContract] attribute will decorate generated classes always, it overrites rules defined in target T4 template.
        /// </summary>
        Enabled,

        /// <summary>
        /// [DataContract] attribute will NOT decorate generated classes always, it overrites rules defined in target T4 template.
        /// </summary>
        Disabled
    }

    #endregion enum ContractDescriptorApplyMode
}