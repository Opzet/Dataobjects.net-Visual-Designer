using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [TypeConverter(typeof(OrmFieldAttributeTypeConverter))]
    [Serializable]
    public class OrmFieldAttribute : OrmAttributeBase
    {
        #region fields 

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_FIELD = new OrmAttributeGroup("Field",
            OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_FIELD_MAPPING = new OrmAttributeGroup("FieldMapping",
            OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_VERSION = new OrmAttributeGroup("Version",
            OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR = new OrmAttributeGroup(
            "TypeDiscriminator", OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        //public const string ATTRIBUTE_GROUP_FIELD = "Field";
        //public const string ATTRIBUTE_GROUP_FIELD_MAPPING = "FieldMapping";
        //public const string ATTRIBUTE_GROUP_VERSION = "Version";
        //public const string ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR = "TypeDiscriminator";


        private const string ERROR_EMPTY_NAME_IN_MAPPING_NAME = "Mapping name could not be empty when is set as 'Custom'.";
        private const string CODE_EMPTY_NAME_IN_MAPPING_NAME = "EmptyNameInMappingName";

        public const string ATTRIBUTE_GROUP_ITEM_DEFAULT_VALUE = "DefaultValue";
        public const string ATTRIBUTE_GROUP_ITEM_INDEXED = "Indexed";
        public const string ATTRIBUTE_GROUP_ITEM_NULLABLE = "Nullable";
        public const string ATTRIBUTE_GROUP_ITEM_PRECISION = "Precision";
        public const string ATTRIBUTE_GROUP_ITEM_SCALE = "Scale";
        public const string ATTRIBUTE_GROUP_ITEM_LAZY_LOAD = "LazyLoad";
        public const string ATTRIBUTE_GROUP_ITEM_LENGTH = "Length";
        public const string ATTRIBUTE_GROUP_ITEM_NULLABLE_ON_UPGRADE = "NullableOnUpgrade";

        public const string ATTRIBUTE_GROUP_ITEM_NAME = "Name";
        public const string ATTRIBUTE_GROUP_ITEM_MODE = "Mode";

        #endregion fields

        [Description(
            "Value specifies the name of mapped column, select (default) to let DataObjects.Net engine to generate name of database column."
            )]
        [DisplayName("Field mapping name")]
        [XmlElement]
        public Defaultable<string> MappingName { get; set; }

        [Description("Default value information.")]
        [DisplayName("Default value")]
        [XmlElement]
        public ObjectValueInfo DefaultValue { get; set; }

        [Description("Value indicating whether the field should be indexed.")]
        [XmlElement]
        [DisplayName("Is Indexed")]
        public Defaultable<bool> Indexed { get; set; }

        [Description("Value indicating whether value of this field should be loaded on demand.")]
        [XmlElement]
        [DisplayName("Lazy Loading")]
        public Defaultable<bool> LazyLoad { get; set; }

        [Description("Value indicating the length of the field.")]
        [XmlElement]
        public Defaultable<int> Length { get; set; }

        [Description("Value indicating whether this field is nullable.")]
        [XmlElement]
        [DisplayName("Is Nullable")]
        public Defaultable<bool> Nullable { get; set; }

        [Description("Gets or sets a value indicating whether this field must be Nullable during upgrade.")]
        [XmlAttribute("nullableOnUpgrade")]
        [DisplayName("Nullable On Upgrade")]
        public Defaultable<bool> NullableOnUpgrade { get; set; }

        [Description("Value indicating the precision of the field.")]
        [XmlElement]
        public Defaultable<int> Precision { get; set; }

        [Description("Value indicating the scale of the field.")]
        [XmlElement]
        public Defaultable<int> Scale { get; set; }

        [Description("Value indicates whether this field is used as type discriminator.")]
        [DefaultValue(false)]
        [XmlElement]
        [DisplayName("Type Discriminator")]
        public Defaultable<bool> TypeDiscriminator { get; set; }

        [Description("Marks property as a part of Entity version.")]
        [DisplayName("Is Version Field")]
        public Defaultable<VersionMode> Version { get; set; }

        //public PropertyConstraints Constraints { get; set; }

        public OrmFieldAttribute()
        {
            this.MappingName = new Defaultable<string>();
            this.Indexed = new Defaultable<bool>();
            this.LazyLoad = new Defaultable<bool>();
            this.Length = new Defaultable<int>();
            this.Nullable = new Defaultable<bool>();
            this.NullableOnUpgrade = new Defaultable<bool>();
            this.Precision = new Defaultable<int>();
            this.Scale = new Defaultable<int>();
            this.TypeDiscriminator = new Defaultable<bool>();
            this.Version = new Defaultable<VersionMode>();

            this.DefaultValue = new ObjectValueInfo();
            //this.Constraints = new PropertyConstraints();
        }

        public override string ToString()
        {
            bool isField = this.MappingName.IsDefault() || string.IsNullOrEmpty(this.MappingName.Value);

            return isField ? ATTRIBUTE_GROUP_FIELD.Name : string.Format("Field mapping: {0}", this.MappingName);
        }

        public bool EqualsTo(OrmFieldAttribute other)
        {
            List<Defaultable> currentDefaultableItems = GetAttributeGroupItems(AttributeGroupsListMode.All);
            List<Defaultable> otherDefaultableItems = other.GetAttributeGroupItems(AttributeGroupsListMode.All);
            for (int i = 0; i < currentDefaultableItems.Count; i++)
            {
                Defaultable currentItem = currentDefaultableItems[i];
                Defaultable otherItem = otherDefaultableItems[i];
                if (!currentItem.EqualsTo(otherItem))
                {
                    return false;
                }
            }

            return true;
        }

        public override List<Defaultable> GetAttributeGroupItems(AttributeGroupsListMode listMode)
        {
            List<Defaultable> items = base.GetAttributeGroupItems(listMode);
            items.Add(this.TypeDiscriminator);
            return items;
        }

//        private List<Defaultable> GetAllDefaultableItems()
//        {
//            var query = from attrGroup in GetAttributeGroups(AttributeGroupsListMode.All)
//                        select GetAttributeGroupItems(attrGroup).Values;
//
//            List<Defaultable> items = query.SelectMany(list => list).ToList();
//            items.Add(this.TypeDiscriminator);
//            return items;
//        }

        public override void DeserializeFromXml(XmlReader reader)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            if (xmlRoot.ElementName == "field")
            {
                XmlProxy xmlMappingName = xmlRoot["mappingName"];
                this.MappingName.DeserializeFromXml(xmlMappingName);

                XmlProxy xmlIndexed = xmlRoot["indexed"];
                this.Indexed.DeserializeFromXml(xmlIndexed);

                XmlProxy xmlLazyLoad = xmlRoot["lazyLoad"];
                this.LazyLoad.DeserializeFromXml(xmlLazyLoad);

                XmlProxy xmlLength = xmlRoot["length"];
                this.Length.DeserializeFromXml(xmlLength);

                XmlProxy xmlNullable = xmlRoot["nullable"];
                this.Nullable.DeserializeFromXml(xmlNullable);

                XmlProxy xmlNullableOnUpgrade = xmlRoot["nullableOnUpgrade"];
                this.NullableOnUpgrade.DeserializeFromXml(xmlNullableOnUpgrade);

                XmlProxy xmlPrecision = xmlRoot["precision"];
                this.Precision.DeserializeFromXml(xmlPrecision);

                XmlProxy xmlScale = xmlRoot["scale"];
                this.Scale.DeserializeFromXml(xmlScale);

                XmlProxy xmlTypeDiscriminator = xmlRoot["typeDiscriminator"];
                this.TypeDiscriminator.DeserializeFromXml(xmlTypeDiscriminator);

                XmlProxy xmlVersion = xmlRoot["version"];
                this.Version.DeserializeFromXml(xmlVersion);

                XmlProxy xmlDefaultValue = xmlRoot["defaultValue"];
                this.DefaultValue.DeserializeFromXml(xmlDefaultValue);

//                XmlProxy xmlConstraints = xmlRoot["constraints"];
//                this.Constraints.DeserializeFromXml(xmlConstraints);
            }
        }

        public override void SerializeToXml(XmlWriter writer)
        {
            XmlProxy xmlRoot = new XmlProxy("field");

            XmlProxy xmlMappingName = xmlRoot.AddChild("mappingName");
            this.MappingName.SerializeToXml(xmlMappingName);

            XmlProxy xmlIndexed = xmlRoot.AddChild("indexed");
            this.Indexed.SerializeToXml(xmlIndexed);

            XmlProxy xmlLazyLoad = xmlRoot.AddChild("lazyLoad");
            this.LazyLoad.SerializeToXml(xmlLazyLoad);

            XmlProxy xmlLength = xmlRoot.AddChild("length");
            this.Length.SerializeToXml(xmlLength);

            XmlProxy xmlNullable = xmlRoot.AddChild("nullable");
            this.Nullable.SerializeToXml(xmlNullable);

            XmlProxy xmlNullableOnUpgrade = xmlRoot.AddChild("nullableOnUpgrade");
            this.NullableOnUpgrade.SerializeToXml(xmlNullableOnUpgrade);

            XmlProxy xmlPrecision = xmlRoot.AddChild("precision");
            this.Precision.SerializeToXml(xmlPrecision);

            XmlProxy xmlScale = xmlRoot.AddChild("scale");
            this.Scale.SerializeToXml(xmlScale);

            XmlProxy xmlTypeDiscriminator = xmlRoot.AddChild("typeDiscriminator");
            this.TypeDiscriminator.SerializeToXml(xmlTypeDiscriminator);

            XmlProxy xmlVersion = xmlRoot.AddChild("version");
            this.Version.SerializeToXml(xmlVersion);

            XmlProxy xmlDefaultValue = xmlRoot.AddChild("defaultValue");
            this.DefaultValue.SerializeToXml(xmlDefaultValue);

            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        #region Implementation of IOrmAttribute

        public override void Validate(ValidationContext context, ModelElement ownerElement)
        {
            if (this.MappingName.IsCustom() && string.IsNullOrEmpty(this.MappingName.Value))
            {
                context.LogError(ERROR_EMPTY_NAME_IN_MAPPING_NAME, CODE_EMPTY_NAME_IN_MAPPING_NAME,
                    new[]
                    {
                        ownerElement
                    });
            }

        }

        protected override OrmAttributeGroup[] GetAllAttributeGroups()
        {
            List<OrmAttributeGroup> result = new List<OrmAttributeGroup>
                                             {
                                                 ATTRIBUTE_GROUP_FIELD,
                                                 ATTRIBUTE_GROUP_FIELD_MAPPING,
                                                 ATTRIBUTE_GROUP_VERSION,
                                                 ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR
                                             };

            return result.ToArray();
        }

        protected override void FilterAttributeGroups(List<OrmAttributeGroup> attributeGroupsToFilter)
        {
            if (this.MappingName.IsDefault())
            {
                attributeGroupsToFilter.RemoveAll(item => item.EqualsTo(ATTRIBUTE_GROUP_FIELD_MAPPING));
            }

            if (this.Version.IsDefault())
            {
                attributeGroupsToFilter.RemoveAll(item => item.EqualsTo(ATTRIBUTE_GROUP_VERSION));
            }

            if (this.TypeDiscriminator.IsDefault() || !this.TypeDiscriminator.Value)
            {
                attributeGroupsToFilter.RemoveAll(item => item.EqualsTo(ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR));
            }
        }

        protected override OrmAttributeKind GetAttributeKind()
        {
            return OrmAttributeKind.Property;
        }

        public override Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            var result = new Dictionary<string, Defaultable>();

            if (group == ATTRIBUTE_GROUP_FIELD)
            {
                #region DefaultValue

                Defaultable<ObjectValue> defValue = GetDefaultValueAsDefaultable();
                result.Add(ATTRIBUTE_GROUP_ITEM_DEFAULT_VALUE, defValue);

                #endregion DefaultValue

                result.Add(ATTRIBUTE_GROUP_ITEM_INDEXED, this.Indexed);
                result.Add(ATTRIBUTE_GROUP_ITEM_NULLABLE, this.Nullable);
                result.Add(ATTRIBUTE_GROUP_ITEM_PRECISION, this.Precision);
                result.Add(ATTRIBUTE_GROUP_ITEM_SCALE, this.Scale);
                result.Add(ATTRIBUTE_GROUP_ITEM_LAZY_LOAD, this.LazyLoad);
                result.Add(ATTRIBUTE_GROUP_ITEM_LENGTH, this.Length);
                result.Add(ATTRIBUTE_GROUP_ITEM_NULLABLE_ON_UPGRADE, this.NullableOnUpgrade);
            }
            else if (group == ATTRIBUTE_GROUP_FIELD_MAPPING)
            {
                result.Add(ATTRIBUTE_GROUP_ITEM_NAME, this.MappingName);
            }
            else if (group == ATTRIBUTE_GROUP_VERSION)
            {
                result.Add(ATTRIBUTE_GROUP_ITEM_MODE, this.Version);
            }
            else if (group == ATTRIBUTE_GROUP_TYPE_DISCRIMINATOR)
            {
                // nothing to add, attribute without parameters
            }

            //this.Constraints.FillAttributeGroupItems(result, group);

            return result;
        }

        private Defaultable<ObjectValue> GetDefaultValueAsDefaultable()
        {
            Defaultable<ObjectValue> defValue = new Defaultable<ObjectValue>();
            if (this.DefaultValue.Enabled)
            {
                defValue.SetAsCustom(this.DefaultValue.Value);
            }
            return defValue;
        }

        public override IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            OrmFieldAttribute other = (OrmFieldAttribute) otherAttribute;
            OrmFieldAttribute mergedResult = new OrmFieldAttribute();


            Defaultable<ObjectValue> defaultValue =
                this.GetDefaultValueAsDefaultable().Merge(other.GetDefaultValueAsDefaultable(),
                    mergeConflictAction);
            mergedResult.DefaultValue = new ObjectValueInfo();
            mergedResult.DefaultValue.Enabled = defaultValue.IsCustom();
            mergedResult.DefaultValue.Value = defaultValue.Value;

            mergedResult.Indexed = this.Indexed.Merge(other.Indexed, mergeConflictAction);
            mergedResult.Nullable = this.Nullable.Merge(other.Nullable, mergeConflictAction);
            mergedResult.Precision = this.Precision.Merge(other.Precision, mergeConflictAction);
            mergedResult.Scale = this.Scale.Merge(other.Scale, mergeConflictAction);
            mergedResult.LazyLoad = this.LazyLoad.Merge(other.LazyLoad, mergeConflictAction);
            mergedResult.Length = this.Length.Merge(other.Length, mergeConflictAction);
            mergedResult.NullableOnUpgrade = this.NullableOnUpgrade.Merge(other.NullableOnUpgrade, mergeConflictAction);
            mergedResult.MappingName = this.MappingName.Merge(other.MappingName, mergeConflictAction);
            mergedResult.Version = this.Version.Merge(other.Version, mergeConflictAction);
            mergedResult.TypeDiscriminator = this.TypeDiscriminator.Merge(other.TypeDiscriminator, mergeConflictAction);

            //mergedResult.Constraints = this.Constraints.Merge(other.Constraints, mergeConflictAction);

            return mergedResult;
        }

        #endregion
    }

    #region class OrmFieldAttributeTypeConverter

    public class OrmFieldAttributeTypeConverter : ExpandableObjectConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            OrmFieldAttribute result = new OrmFieldAttribute
                                       {
                                           MappingName = (Defaultable<string>) propertyValues["MappingName"],
                                           DefaultValue = (ObjectValueInfo) propertyValues["DefaultValue"],
                                           Indexed = (Defaultable<bool>) propertyValues["Indexed"],
                                           LazyLoad = (Defaultable<bool>) propertyValues["LazyLoad"],
                                           Length = (Defaultable<int>) propertyValues["Length"],
                                           Nullable = (Defaultable<bool>) propertyValues["Nullable"],
                                           NullableOnUpgrade = (Defaultable<bool>) propertyValues["NullableOnUpgrade"],
                                           Precision = (Defaultable<int>) propertyValues["Precision"],
                                           Scale = (Defaultable<int>) propertyValues["Scale"],
                                           TypeDiscriminator = (Defaultable<bool>) propertyValues["TypeDiscriminator"],
                                           Version = (Defaultable<VersionMode>) propertyValues["Version"]
                                           //Constraints = (PropertyConstraints)propertyValues["Constraints"]
                                       };

            return result;
        }
    }

    #endregion class OrmAssociationEndTypeConverter

    #region class DefaultValueInfo

    //[TypeConverter(typeof(ExpandableObjectConverter))]

    #endregion class DefaultValueInfo
}