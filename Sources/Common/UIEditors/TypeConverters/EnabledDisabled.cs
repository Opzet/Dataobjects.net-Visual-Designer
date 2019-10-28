using System.ComponentModel;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    #region class EnabledDisabledValueTypeConverter

    public class EnabledDisabledValueTypeConverter : StandardValuesTypeConverterBase
    {
        public const string VALUE_TYPE_ENABLED = "Enabled";
        public const string VALUE_TYPE_DISABLED = "Disabled";
        internal static readonly string[] standardValues = new[] { VALUE_TYPE_ENABLED, VALUE_TYPE_DISABLED };

        protected internal override string[] BindStandardValues(ITypeDescriptorContext context)
        {
            return standardValues;
        }
    }

    #endregion class EnabledDisabledValueTypeConverter

    #region class EnabledDisabledMultiTypeConvertor

    public class EnabledDisabledMultiTypeConvertor : DefaultableMultiTypeConvertor
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
                DefaultableMulti clone = (DefaultableMulti) lastValue.Clone();
                clone.ValueType = listItem == null ? clone.ValueTypeForNull : (string) listItem.Value;
                return clone;
            }

            return listItem.Value;
        }

        protected internal override string[] BindStandardValues(ITypeDescriptorContext context)
        {
            return EnabledDisabledValueTypeConverter.standardValues;
        }
    }

    #endregion class EnabledDisabledMultiTypeConvertor

    #region class EnabledDisabledEditor

    public class EnabledDisabledEditor : StandardValuesEditorBase<EnabledDisabledValueTypeConverter>
    {}

    #endregion class EnabledDisabledEditor
}