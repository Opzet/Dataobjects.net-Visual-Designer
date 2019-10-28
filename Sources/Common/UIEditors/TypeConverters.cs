using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    #region class StandardValuesBase

    public abstract class StandardValuesTypeConverterBase : TypeConverter
    {
        protected internal abstract string[] BindStandardValues(ITypeDescriptorContext context);

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BindStandardValues(context));
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        protected internal virtual bool SelectListItemByValue(ListItem item, object value)
        {
            return false;
        }
        
        protected internal virtual object GetValueFromSelectedListItem(object lastSelectedValue, ListItem listItem)
        {
            return listItem == null ? null : listItem.Value;
        }
    }

    #endregion class StandardValuesBase

    public abstract class StandardValuesEditorBase<TStandardValues> : UniDropdownEditor where TStandardValues : StandardValuesTypeConverterBase, new()
    {
        readonly TStandardValues proxy = new TStandardValues();
        private ListItemCollection collection;

        #region Overrides of UniDropdownEditor

        public ListItemCollection GetCollection(ITypeDescriptorContext context)
        {
            if (collection == null)
            {
                collection = new ListItemCollection();
                collection.AddRange(proxy.BindStandardValues(context).Select(item => new ListItem(item, item)));
            }
            return collection;
        }

        public override ListItemCollection GetCollection(ITypeDescriptorContext context, bool allowedNull)
        {
            return GetCollection(context);
        }

        public override void ValidateValue(ITypeDescriptorContext context, ref object value)
        {
            if (value is string)
            {
                
            }
        }

        public override bool AllowedNull
        {
            get { return true; }
        }

        #endregion
    }


    #region class DefaultCustomValueTypeConverter

    public class DefaultCustomValueTypeConverter : StandardValuesTypeConverterBase
    {
        public const string VALUE_TYPE_DEFAULT = "Default";
        public const string VALUE_TYPE_CUSTOM = "Custom";
        private readonly string[] standardValues = new[] { VALUE_TYPE_DEFAULT, VALUE_TYPE_CUSTOM };

        protected internal override string[] BindStandardValues(ITypeDescriptorContext context)
        {
            return standardValues;
        }
    }

    #endregion class DefaultCustomValueTypeConverter

    #region class DefaultableTypeEditor

    public class DefaultableTypeEditor : UITypeEditor
    {
        #region Overrides of UITypeEditor

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            UITypeEditorEditStyle style = base.GetEditStyle(context);

            PropertyGrid propertyGrid = context.ResolvePropertyGrid();
            if (propertyGrid != null)
            {
                RunExpandAllOnGrid(propertyGrid, false);
            }

            return style;
        }

        #endregion

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context == null) || (provider == null) || (context.Instance == null))
            {
                return base.EditValue(provider, value);
            }

            object editValue = base.EditValue(provider, value);

            return editValue;
        }

        private Timer timer;

        private void RunExpandAllOnGrid(PropertyGrid currentGrid, bool runOnThread)
        {
//            TimerCallback timerCallback = delegate(object state)
//                                              {
//                                                  PropertyGrid grid = (PropertyGrid)state;
//                                                  grid.Select();
//                                                  GridItem selectedGridItem = grid.SelectedGridItem;
//                                                  if (selectedGridItem != null)
//                                                  {
//                                                      selectedGridItem.Expanded = true;
//                                                  }
//                                              };
//
//            if (runOnThread)
//            {
//                timer = new Timer(timerCallback, currentGrid, 200, Timeout.Infinite);
//            }
//            else
//            {
//                timerCallback(currentGrid);
//            }
        }
    }

    #endregion class DefaultableTypeEditor


    #region class DefaultableTypeConvertor

    public class DefaultableTypeConvertor : TypeConverter
    {
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            object newInstance = null;
            object propertyValue = context.PropertyDescriptor.GetValue(context.Instance);
            Defaultable defaultable = propertyValue as Defaultable;
            if (defaultable != null)
            {
                newInstance = defaultable.CloneFromPropertyValues(propertyValues);
            }

            if (newInstance == null)
            {
                newInstance = base.CreateInstance(context, propertyValues);
            }

            return newInstance;
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            object propertyValue = context.PropertyDescriptor.GetValue(context.Instance);
            bool isExclusive = propertyValue == null;

            if (!isExclusive)
            {
                Defaultable defaultable = propertyValue as Defaultable;
                if (defaultable != null)
                {
                    object exValue = defaultable.InternalGetValue();
                    isExclusive = defaultable.IsDefault() || exValue == null;

                    Type typeOfValue = defaultable.GetTypeOfValue();
                    if (!isExclusive)
                    {
                        isExclusive = (typeOfValue == typeof (bool) || typeOfValue.IsEnum);
                    }

                }
            }

            return isExclusive;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            Defaultable defaultable = (Defaultable)value;

            var descriptor = TypeDescriptor.GetProperties(defaultable.GetType(), attributes);

            List<PropertyDescriptor> propertiesToRemove = new List<PropertyDescriptor>();

            foreach (PropertyDescriptor prop in descriptor)
            {
                if (prop.Name == "Value" && defaultable.IsDefault())
                {
                    propertiesToRemove.Add(prop);
                }
            }


            PropertyDescriptorCollection result = descriptor;

            if (propertiesToRemove.Count > 0)
            {
                PropertyDescriptor[] propertyDescriptors = descriptor.OfType<PropertyDescriptor>().ToArray();
                result = new PropertyDescriptorCollection(propertyDescriptors, false);

                foreach (PropertyDescriptor prop in descriptor)
                {
                    if (prop.Name == "Value" && defaultable.IsDefault())
                    {
                        try
                        {
                            result.Remove(prop);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }

            result.Sort(new[] {"ValueType", "Value"});

            return result;
        }

        /// <summary>
        /// Determines if a value can be converted to a TItemType.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts values to a TItemType.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (context != null)
            {
                object propertyValue = context.PropertyDescriptor.GetValue(context.Instance);
                Defaultable defaultable = propertyValue as Defaultable;

                if (value is string && defaultable != null)
                {
                    defaultable.SetValue(value);
                    return defaultable.Clone();
                }
            }

            return base.ConvertFrom(context, culture, value);
        }


        /// <summary>
        /// Determines if a TItemType can be convert to a value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof(string) || base.CanConvertTo(context, destinationType));
        }

        /// <summary>
        /// Converts a TItemType to a value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override object ConvertTo(ITypeDescriptorContext context,
                                         CultureInfo culture, object value, Type destinationType)
        {
            object result = string.Empty;

            if (value is Defaultable)
            {
                if (destinationType == typeof(string))
                {
                    return value.ToString();
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    #endregion class DefaultableTypeConvertor
}