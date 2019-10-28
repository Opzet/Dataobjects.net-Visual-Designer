using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    public sealed class PropertyLengthConstraint : PropertyConstraint
    {
        public const string DISPLAY_NAME = "Length";
        public const string DESCRIPTION = "Ensures field length (or item count) fits in specified range";
        public readonly OrmAttributeGroup ATTRIBUTE_NAME = new OrmAttributeGroup("LengthConstraint", OrmUtils.GetOrmNamespace(OrmNamespace.OrmValidation));

        #region properties

        /// <summary>
        /// Gets or sets the minimal allowed length.  Default is <c>0</c>.
        /// </summary>
        [XmlAttribute("min")]
        public Defaultable<long> Min { get; set; }

        /// <summary>
        /// Gets or sets the maximal allowed length. Default is <see cref="long.MaxValue"/>.
        /// </summary>
        [XmlAttribute("max")]
        public Defaultable<long> Max { get; set; }

        #endregion properties

        #region constructors

        public PropertyLengthConstraint()
        {
            this.Min = new Defaultable<long>();
            this.Max = new Defaultable<long>();
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
            PropertyLengthConstraint otherConstraint = other as PropertyLengthConstraint;
            if (otherConstraint != null)
            {
                this.Min = (Defaultable<long>) otherConstraint.Min.Clone();
                this.Max = (Defaultable<long>) otherConstraint.Max.Clone();
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            base.InternalAssignFromPropertyValues(propertyValues);
            this.Min = (Defaultable<long>) (propertyValues["Min"] as Defaultable<long>).Clone();
            this.Max = (Defaultable<long>) (propertyValues["Max"] as Defaultable<long>).Clone();
        }

        protected override string InternalToString()
        {
            StringBuilder sb = new StringBuilder(base.InternalToString());
            if (this.Min.IsCustom() || this.Max.IsCustom())
            {
                sb.Append(", ");
                bool isMin = this.Min.IsCustom();
                if (isMin)
                {
                    sb.AppendFormat("Min: {0}", this.Min.Value);
                }

                if (this.Max.IsCustom())
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

            PropertyLengthConstraint otherConstraint = other as PropertyLengthConstraint;

            if (equals && otherConstraint != null)
            {
                equals = this.Min.EqualsTo(otherConstraint.Min) &&
                         this.Max.EqualsTo(otherConstraint.Max);
            }

            return equals;
        }

        protected override void InternalDeserializeFromXml(XmlProxy content)
        {
            this.Min = new Defaultable<long>();
            this.Min.SetAsDefault();
            var minAttr = content.Attributes["min"];
            if (minAttr != null)
            {
                this.Min.Value = Convert.ToInt64(minAttr.Value);
            }

            this.Max = new Defaultable<long>();
            this.Max.SetAsDefault();
            var maxAttr = content.Attributes["max"];
            if (maxAttr != null)
            {
                this.Max.Value = Convert.ToInt64(maxAttr.Value);
            }
        }

        protected override void InternalSerializeToXml(XmlProxy content)
        {
            if (this.Min.IsCustom())
            {
                content.AddAttribute("min", this.Min.Value);
            }

            if (Max.IsCustom())
            {
                content.AddAttribute("max", this.Max.Value);
            }
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
            return PropertyConstrainType.Length;
        }

        protected internal override OrmAttributeGroup GetAttributeGroup()
        {
            return ATTRIBUTE_NAME;
        }

        protected override void FillAttributeGroupItems(Dictionary<string, Defaultable> attributeGroupItems)
        {
            if (Min.IsCustom())
            {
                attributeGroupItems.Add("Min", Min);
            }

            if (Max.IsCustom())
            {
                attributeGroupItems.Add("Max", Max);
            }
        }

        protected override void InternalMergeChanges(ref PropertyConstraint mergedConstraint, PropertyConstraint otherConstraint,
            MergeConflictAction mergeConflictAction)
        {
            PropertyLengthConstraint merged = (PropertyLengthConstraint) mergedConstraint;
            PropertyLengthConstraint other = (PropertyLengthConstraint) otherConstraint;
            merged.Min = this.Min.Merge(other.Min, mergeConflictAction);
            merged.Max = this.Max.Merge(other.Max, mergeConflictAction);
        }

        #endregion methods
    }
}