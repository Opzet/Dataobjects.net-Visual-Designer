using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
//    [Serializable]
//    public class OrmTableMappingAttribute
//    {}

    [Serializable]
    [TypeConverter(typeof(EnabledDisabledMultiTypeConvertor))]
    [Editor(typeof(EnabledDisabledEditor), typeof(UITypeEditor))]
    public class OrmHierarchyRootAttribute : DefaultableMulti, IOrmAttribute
    {
        #region fields

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_HIERARCHY_ROOT = new OrmAttributeGroup("HierarchyRoot",
            OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_TABLE_MAPPING = new OrmAttributeGroup("TableMapping",
            OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        private readonly string[] propertiesToHide = new[]
                                                     {
                                                         "IncludeTypeId", "InheritanceSchema"
                                                     };

        private const string regexTableMappingNamePattern = "[_A-Za-z0-9-.]";
        private static readonly Regex regexTableMappingName = new Regex(regexTableMappingNamePattern);

        private const string ERROR_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME =
            "Table mapping name '{0}' contains invalid characters, support characters are '{1}'.";

        private const string ERROR_EMPTY_NAME_IN_TABLE_MAPPING_NAME =
            "Table mapping name could not be empty when is set as 'Custom'.";

        private const string CODE_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME = "InvalidCharactersInTableMappingName";
        private const string CODE_EMPTY_NAME_IN_TABLE_MAPPING_NAME = "EmptyNameInTableMappingName";

        #endregion fields

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

        [Description(
            "Value specifies the name of mapped table, select (default) to let DataObjects.Net engine to generate name of table."
            )]
        [DisplayName("Table mapping name")]
        [XmlElement]
        public Defaultable<string> MappingName { get; set; }

        [Description("Indicate whether key should include TypeId field.")]
        [DisplayName("Include TypeId field")]
        [XmlElement]
        [NotifyParentProperty(true)]
        public Defaultable<bool> IncludeTypeId { get; set; }

        [Description("Inheritance schema for this hierarchy. See manual for more information.")]
        [DisplayName("Inheritance Schema")]
        [XmlAttribute("inheritanceSchema")]
        [NotifyParentProperty(true)]
        public HierarchyRootInheritanceSchema InheritanceSchema { get; set; }

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

        #region constructors


        public OrmHierarchyRootAttribute()
        {
            this.Enabled = true;

            this.IncludeTypeId = new Defaultable<bool>();
            this.IncludeTypeId.SetAsDefault();

            this.InheritanceSchema = HierarchyRootInheritanceSchema.Default;

            this.MappingName = new Defaultable<string>();
            this.MappingName.SetAsDefault();
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
            if (other is OrmHierarchyRootAttribute)
            {
                OrmHierarchyRootAttribute attr = (OrmHierarchyRootAttribute) other;
                this.Enabled = attr.Enabled;
                this.IncludeTypeId = (Defaultable<bool>) attr.IncludeTypeId.Clone();
                this.InheritanceSchema = attr.InheritanceSchema;
                this.MappingName = (Defaultable<string>) attr.MappingName.Clone();
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            Defaultable<bool> propertyValue = (Defaultable<bool>) propertyValues["IncludeTypeId"];
            this.IncludeTypeId = (Defaultable<bool>) propertyValue.Clone();

            this.InheritanceSchema = (HierarchyRootInheritanceSchema) propertyValues["InheritanceSchema"];

            Defaultable<string> mappingName = (Defaultable<string>) propertyValues["MappingName"];
            this.MappingName = (Defaultable<string>) mappingName.Clone();
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            OrmHierarchyRootAttribute otherAttr = (OrmHierarchyRootAttribute) other;
            return this.IncludeTypeId.EqualsTo(otherAttr.IncludeTypeId) &&
                   this.InheritanceSchema == otherAttr.InheritanceSchema &&
                   this.MappingName.EqualsTo(otherAttr.MappingName);
        }

        public override string ToString()
        {
            return Enabled
                       ? string.Format("Inheritance Schema: {0}", this.InheritanceSchema)
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

            string schema = elementValue.GetAttr("inheritanceSchema");
            this.InheritanceSchema =
                (HierarchyRootInheritanceSchema) Enum.Parse(typeof (HierarchyRootInheritanceSchema), schema, true);

            XmlProxy elementIncludeTypeId = elementValue["includeTypeId"];
            IncludeTypeId = new Defaultable<bool>();
            IncludeTypeId.DeserializeFromXml(elementIncludeTypeId);

            XmlProxy xmlMappingName = elementValue["mappingName"];
            this.MappingName = new Defaultable<string>();
            this.MappingName.DeserializeFromXml(xmlMappingName);
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

            elementValue.SetAttr("inheritanceSchema", this.InheritanceSchema.ToString());

            XmlProxy elementIncludeTypeId = elementValue.AddChild("includeTypeId");
            IncludeTypeId.SerializeToXml(elementIncludeTypeId);

            XmlProxy xmlMappingName = elementValue.AddChild("mappingName");
            MappingName.SerializeToXml(xmlMappingName);
        }

        #endregion methods

        #region Implementation of IOrmAttribute

        public OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            List<OrmAttributeGroup> result = new List<OrmAttributeGroup>();

            if (listMode == AttributeGroupsListMode.All || this.Enabled)
            {
                result.Add(ATTRIBUTE_GROUP_HIERARCHY_ROOT);
            }

            if (listMode == AttributeGroupsListMode.All || (this.Enabled && this.MappingName.IsCustom()))
            {
                result.Add(ATTRIBUTE_GROUP_TABLE_MAPPING);
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

            if (group == ATTRIBUTE_GROUP_HIERARCHY_ROOT)
            {
                //if (this.Enabled)
                {
                    Defaultable<HierarchyRootInheritanceSchema> defInheritanceSchema = GetDefInheritanceSchema();
                    result.Add("InheritanceSchema", defInheritanceSchema);

                    result.Add("IncludeTypeId", IncludeTypeId);
                }
            }
            else if (group == ATTRIBUTE_GROUP_TABLE_MAPPING)
            {
                result.Add("Name", this.MappingName);
            }

            return result;
        }

        private Defaultable<HierarchyRootInheritanceSchema> GetDefInheritanceSchema()
        {
            var defInheritanceSchema = new Defaultable<HierarchyRootInheritanceSchema>();
            if (this.Enabled && this.InheritanceSchema != HierarchyRootInheritanceSchema.Default)
            {
                defInheritanceSchema.SetAsCustom(this.InheritanceSchema);
            }
            return defInheritanceSchema;
        }

        public IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            OrmHierarchyRootAttribute other = (OrmHierarchyRootAttribute) otherAttribute;
            OrmHierarchyRootAttribute mergedResult = new OrmHierarchyRootAttribute();

            bool enabled = this.Enabled;
            //if (!other.Enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            if (other.Enabled && !enabled && mergeConflictAction == MergeConflictAction.TakeOther)
            {
                enabled = other.Enabled;
            }

            mergedResult.Enabled = enabled;

            var mergedInheritanceSchema = this.GetDefInheritanceSchema().Merge(other.GetDefInheritanceSchema(),
                mergeConflictAction);
            mergedResult.InheritanceSchema = mergedInheritanceSchema.Value;

            mergedResult.IncludeTypeId = this.IncludeTypeId.Merge(other.IncludeTypeId, mergeConflictAction);
            mergedResult.MappingName = this.MappingName.Merge(other.MappingName, mergeConflictAction);

            return mergedResult;
        }

        public void Validate(ValidationContext context, ModelElement ownerElement)
        {
            if (!Enabled)
            {
                return;
            }

            if (MappingName.IsCustom())
            {
                string mappingName = MappingName.Value;

                if (!string.IsNullOrEmpty(mappingName))
                {
                    if (!regexTableMappingName.IsMatch(mappingName))
                    {
                        context.LogError(
                            string.Format(ERROR_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME,
                                mappingName, regexTableMappingNamePattern),
                            CODE_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME,
                            new []
                            {
                                ownerElement
                            });
                    }
                }
                else
                {
                    context.LogError(ERROR_EMPTY_NAME_IN_TABLE_MAPPING_NAME, CODE_EMPTY_NAME_IN_TABLE_MAPPING_NAME,
                        new []
                        {
                            ownerElement
                        });

                }
            }
        }

        #endregion
    }

    #region enum HierarchyRootInheritanceSchema

    [Serializable]
    public enum HierarchyRootInheritanceSchema
    {
        [XmlEnum("Default")]
        Default = -1,

        [XmlEnum("ClassTable")]
        ClassTable = 0,

        [XmlEnum("SingleTable")]
        SingleTable = 1,

        [XmlEnum("ConcreteTable")]
        ConcreteTable = 2
    }

    #endregion enum HierarchyRootInheritanceSchema
}