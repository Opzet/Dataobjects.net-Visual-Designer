using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    public sealed class PropertyRegexConstraint : PropertyConstraint
    {
        public const string DISPLAY_NAME = "Regex";
        public const string DESCRIPTION = "Ensures property value matches specified regular expression";
        public readonly OrmAttributeGroup ATTRIBUTE_NAME = new OrmAttributeGroup("RegexConstraint", OrmUtils.GetOrmNamespace(OrmNamespace.OrmValidation));
        private const string KEY_PATTERN = "Pattern";
        private const string KEY_OPTIONS = "Options";

        #region properties

        [Description("Regular expression pattern.")]
        public string Pattern { get; set; }

        [Description("Regular expression options, default value is Compiled.")]
        public RegexOptions Options { get; set; }

        #endregion properties

        public PropertyRegexConstraint()
        {
            this.Options = RegexOptions.Compiled;
        }

        #region methods

        protected override void FillPropertyNamesToHideOnDefault(List<string> propertiesToHide)
        {
            propertiesToHide.AddRange(new[] { KEY_PATTERN, KEY_OPTIONS });
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
            return PropertyConstrainType.Regex;
        }

        protected internal override OrmAttributeGroup GetAttributeGroup()
        {
            return ATTRIBUTE_NAME;
        }

        protected override void InternalDeserializeFromXml(XmlProxy content)
        {
            this.Pattern = content.GetAttr("pattern");
            this.Options = (RegexOptions) content.GetAttr("options", (int) RegexOptions.Compiled);
        }

        protected override void InternalSerializeToXml(XmlProxy content)
        {
            content.AddAttribute("pattern", this.Pattern);
            content.AddAttribute("options", this.Options);
        }

        internal Defaultable<string> GetPatternAsDefaultable()
        {
            Defaultable<string> defValue = new Defaultable<string>();
            if (!string.IsNullOrEmpty(Pattern))
            {
                defValue.SetAsCustom(Pattern);
            }
            return defValue;
        }

        internal Defaultable<RegexOptions> GetOptionsAsDefaultable()
        {
            Defaultable<RegexOptions> defValue = new Defaultable<RegexOptions>();
            if (Options != RegexOptions.Compiled)
            {
                defValue.SetAsCustom(Options);
            }
            return defValue;
        }

        protected override string InternalToString()
        {
            StringBuilder sb = new StringBuilder(base.InternalToString());
            sb.AppendFormat(", Pattern: {0}, Options: {1}", this.Pattern, this.Options);
            return sb.ToString();
        }

        protected override void InternalAssignFrom(DefaultableMulti other)
        {
            base.InternalAssignFrom(other);
            PropertyRegexConstraint otherConstraint = other as PropertyRegexConstraint;
            if (otherConstraint != null)
            {
                this.Pattern = otherConstraint.Pattern;
                this.Options = otherConstraint.Options;
            }
        }

        protected override void InternalAssignFromPropertyValues(IDictionary propertyValues)
        {
            base.InternalAssignFromPropertyValues(propertyValues);
            this.Pattern = propertyValues[KEY_PATTERN] as string;
            this.Options = (RegexOptions) propertyValues[KEY_OPTIONS];
        }

        protected override bool InternalEqualsTo(DefaultableMulti other)
        {
            bool equals = base.InternalEqualsTo(other);
            PropertyRegexConstraint otherConstraint = other as PropertyRegexConstraint;
            if (equals && otherConstraint != null)
            {
                equals = Util.StringEqual(this.Pattern, otherConstraint.Pattern, false) &&
                         this.Options == otherConstraint.Options;
            }

            return equals;
        }

        protected override void FillAttributeGroupItems(Dictionary<string, Defaultable> attributeGroupItems)
        {
            attributeGroupItems.Add("Pattern", GetPatternAsDefaultable());
            attributeGroupItems.Add("Options", GetOptionsAsDefaultable());
        }

        protected override void InternalMergeChanges(ref PropertyConstraint mergedConstraint, PropertyConstraint otherConstraint,
            MergeConflictAction mergeConflictAction)
        {
            PropertyRegexConstraint other = (PropertyRegexConstraint)otherConstraint;
            PropertyRegexConstraint merged = (PropertyRegexConstraint) mergedConstraint;

            Defaultable<string> mergedPattern = this.GetPatternAsDefaultable().Merge(other.GetPatternAsDefaultable(),
                mergeConflictAction);
            merged.Pattern = mergedPattern.IsCustom() ? mergedPattern.Value : string.Empty;

            Defaultable<RegexOptions> mergedOptions =
                this.GetOptionsAsDefaultable().Merge(other.GetOptionsAsDefaultable(), mergeConflictAction);
            merged.Options = mergedOptions.IsCustom() ? mergedOptions.Value : RegexOptions.Compiled;
        }

        #endregion methods
    }
}