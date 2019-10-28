using System.ComponentModel;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    #region class UsedNotUsedValueTypeConverter

    public class UsedNotUsedValueTypeConverter : StandardValuesTypeConverterBase
    {
        public const string VALUE_TYPE_USED = "Used";
        public const string VALUE_TYPE_NOT_USED = "NotUsed";
        internal static readonly string[] standardValues = new[] { VALUE_TYPE_USED, VALUE_TYPE_NOT_USED };

        protected internal override string[] BindStandardValues(ITypeDescriptorContext context)
        {
            return standardValues;
        }
    }

    #endregion class UsedNotUsedValueTypeConverter

    #region class UsedNotUsedMultiTypeConvertor

    public class UsedNotUsedMultiTypeConvertor : DefaultableMultiTypeConvertor
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
            return UsedNotUsedValueTypeConverter.standardValues;
        }
    }

    #endregion class UsedNotUsedMultiTypeConvertor

    #region class UsedNotUsedEditor

    public class UsedNotUsedEditor : StandardValuesEditorBase<UsedNotUsedValueTypeConverter>
    { }

    #endregion class UsedNotUsedEditor
}