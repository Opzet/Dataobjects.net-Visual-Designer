using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    public sealed class PropertyRangeConstraint : PropertyConstraint
    {
        public const string DISPLAY_NAME = "Range";
        public const string DESCRIPTION = "Ensures field value fits in the specified range";
        public readonly OrmAttributeGroup ATTRIBUTE_NAME = new OrmAttributeGroup("RangeConstraint", OrmUtils.GetOrmNamespace(OrmNamespace.OrmValidation));

        #region properties

        /// <summary>
        /// Gets or sets the minimal allowed value. <c>null</c> means "ignore this boundary". Default value is <c>null</c>.
        /// </summary>
        [XmlAttribute("min")]
        //TODO: Prefilter available Value Type(s) according to selected property type
        public ObjectValueInfo Min { get; set; }

        /// <summary>
        /// Gets or sets the maximal allowed value. <c>null</c> means "ignore this boundary". Default value is <c>null</c>.
        /// </summary>
        [XmlAttribute("max")]
        public ObjectValueInfo Max { get; set; }

        #endregion properties

        #region constructors 

        public PropertyRangeConstraint()
        {
            this.Min = new ObjectValueInfo();
            this.Max = new ObjectValueInfo();
        }

        #endregion constructors

        #region methods

        protected override void FillPropertyNamesToHideOnDefault(List<string> propertiesToHide)
        {
            propertiesToHide.AddRange(new[]
                                      {
                                          "Min", "Max"
                                      });
        }

        protected override void InternalAssignFrom(DefaultableMulti other)
        {
            base.InternalAssignFrom(other);
            PropertyRangeConstraint otherConstraint = other as PropertyRangeConstraint;
            if (otherConstraint != null)
            {
                this.Min = (ObjectValueInfo) otherConstraint.Min.Clone();
                this.Max = (ObjectValueInfo)otherConstraint.Max.Clone();
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            base.InternalAssignFromPropertyValues(propertyValues);
            this.Min = (ObjectValueInfo)(propertyValues["Min"] as ObjectValueInfo).Clone();
            this.Max = (ObjectValueInfo)(propertyValues["Max"] as ObjectValueInfo).Clone();
        }

        protected override string InternalToString()
        {
            StringBuilder sb = new StringBuilder(base.InternalToString());
            if (this.Min.Enabled || this.Max.Enabled)
            {
                sb.Append(", ");
                bool isMin = this.Min.Enabled;
                if (isMin)
                {
                    sb.AppendFormat("Min: {0}", this.Min.Value);
                }

                if (this.Max.Enabled)
                {
                    if (isMin)
                    {
                        sb.Append(", ");
                    }

                    sb.AppendFormat("Max: {0}", this.Max.Value);
                }
            }

            return sb.ToString();

        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            bool equals = base.InternalEqualsTo(other);

            PropertyRangeConstraint otherConstraint = other as PropertyRangeConstraint;

            if (equals && otherConstraint != null)
            {
                equals = this.Min.EqualsTo(otherConstraint.Min) &&
                         this.Max.EqualsTo(otherConstraint.Max);
            }

            return equals;
        }

        protected override void InternalDeserializeFromXml(XmlProxy content)
        {
            XmlProxy minXml = content.Childs["min"];
            XmlProxy maxXml = content.Childs["max"];

            this.Min = new ObjectValueInfo();
            this.Min.DeserializeFromXml(minXml);

            this.Max = new ObjectValueInfo();
            this.Max.DeserializeFromXml(maxXml);
        }

        protected override void InternalSerializeToXml(XmlProxy content)
        {
            XmlProxy minXml = content.AddChild("min");
            this.Min.SerializeToXml(minXml);

            XmlProxy maxXml = content.AddChild("max");
            this.Max.SerializeToXml(maxXml);
        }

        internal Defaultable<ObjectValue> GetMinAsDefaultable()
        {
            Defaultable<ObjectValue> defValue = new Defaultable<ObjectValue>();
            if (this.Min.Enabled)
            {
                defValue.SetAsCustom(this.Min.Value);
            }
            return defValue;
        }

        internal Defaultable<ObjectValue> GetMaxAsDefaultable()
        {
            Defaultable<ObjectValue> defValue = new Defaultable<ObjectValue>();
            if (this.Max.Enabled)
            {
                defValue.SetAsCustom(this.Max.Value);
            }
            return defValue;
        }

        protected override string GetDisplayName()
        {
            return DISPLAY_NAME;
        }

        protected override string GetDescription()
        {
            return DESCRIPTION;
        }

        protected override PropertyConstrainType GetConstrainType()
        {
            return PropertyConstrainType.Range;
        }

        protected internal override OrmAttributeGroup GetAttributeGroup()
        {
            return ATTRIBUTE_NAME;
        }

        protected override void FillAttributeGroupItems(Dictionary<string, Defaultable> attributeGroupItems)
        {
            attributeGroupItems.Add("Min", GetMinAsDefaultable());
            attributeGroupItems.Add("Max", GetMaxAsDefaultable());
        }

        protected override void InternalMergeChanges(ref PropertyConstraint mergedConstraint, PropertyConstraint otherConstraint,
            MergeConflictAction mergeConflictAction)
        {
            PropertyRangeConstraint other = (PropertyRangeConstraint)otherConstraint;
            PropertyRangeConstraint merged = (PropertyRangeConstraint) mergedConstraint;

            Defaultable<ObjectValue> mergedMin = GetMinAsDefaultable().Merge(other.GetMinAsDefaultable(),
               mergeConflictAction);
            merged.Min = new ObjectValueInfo();
            merged.Min.Enabled = mergedMin.IsCustom();
            merged.Min.Value = mergedMin.Value;

            Defaultable<ObjectValue> mergedMax = GetMaxAsDefaultable().Merge(other.GetMaxAsDefaultable(),
                mergeConflictAction);
            merged.Max = new ObjectValueInfo();
            merged.Max.Enabled = mergedMax.IsCustom();
            merged.Max.Value = mergedMax.Value;
        }

        #endregion methods
    }
}