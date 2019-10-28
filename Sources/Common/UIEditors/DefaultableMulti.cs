using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    [Serializable]
    public abstract class DefaultableMulti : DefaultableBase, ICloneable
    {
        private List<string> propertyNamesToHideOnDefault;

        protected abstract string[] GetPropertyNamesToHideOnDefault();

        protected abstract bool CanHideProperties();

        protected internal bool CanHideProperty(string propertyName)
        {
            return CanHideProperties() && PropertyNamesToHideOnDefault.Contains(propertyName);
        }

        protected List<string> PropertyNamesToHideOnDefault
        {
            get
            {
                if (propertyNamesToHideOnDefault == null)
                {
                    string[] propertyNames = GetPropertyNamesToHideOnDefault();
                    propertyNamesToHideOnDefault = propertyNames == null
                                                       ? new List<string>()
                                                       : new List<string>(propertyNames);
                }
                return propertyNamesToHideOnDefault;
            }
        }

        public object Clone()
        {
            DefaultableMulti cloned = (DefaultableMulti)Activator.CreateInstance(this.GetType());
            cloned.ValueType = this.ValueType;
            cloned.stopPropagateValueChanges = true;
            try
            {
                cloned.AssignFrom(this);
            }
            finally
            {
                cloned.stopPropagateValueChanges = false;
            }
            return cloned;
        }

        protected void AssignFrom(DefaultableMulti other)
        {
            InternalAssignFrom(other);
        }
        protected abstract void InternalAssignFrom(DefaultableMulti other);

        protected void AssignFromPropertyValues(IDictionary propertyValues)
        {
            InternalAssignFromPropertyValues(propertyValues);
        }

        protected abstract void InternalAssignFromPropertyValues(IDictionary propertyValues);

        public object CloneFromPropertyValues(IDictionary propertyValues)
        {
            DefaultableMulti cloned = (DefaultableMulti) Clone();
            cloned.AssignFromPropertyValues(propertyValues);
            return cloned;
        }

        public override bool EqualsTo(DefaultableBase other)
        {
            if (other == null || this.GetType() != other.GetType())
            {
                return false;
            }

            DefaultableMulti otherDefaultableMulti = (DefaultableMulti) other;
            if (base.EqualsTo(other))
            {
                return InternalEqualsTo(otherDefaultableMulti);
            }

            return false;
        }

        protected abstract bool InternalEqualsTo(DefaultableMulti other);
    }

    #region class DefaultableMultiTypeConvertor

    public abstract class DefaultableMultiTypeConvertor : StandardValuesTypeConverterBase
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            object result = InternalCreateInstance(context, propertyValues);
            if (result == null)
            {
                result = base.CreateInstance(context, propertyValues);
            }

            return result;
        }

        protected virtual object InternalCreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            object propertyValue = context.PropertyDescriptor.GetValue(context.Instance);
            DefaultableMulti defaultableMulti = propertyValue as DefaultableMulti;
            if (defaultableMulti != null)
            {
                return defaultableMulti.CloneFromPropertyValues(propertyValues);
            }

            return null;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            DefaultableMulti defaultableMulti = (DefaultableMulti)value;

            var descriptor = TypeDescriptor.GetProperties(defaultableMulti.GetType(), attributes);

            PropertyDescriptor[] propertyDescriptors = descriptor.OfType<PropertyDescriptor>().ToArray();
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(propertyDescriptors, false);

            foreach (PropertyDescriptor prop in descriptor)
            {
                if (defaultableMulti.CanHideProperty(prop.Name))
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
            return true;
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
                DefaultableMulti defaultableMulti = propertyValue as DefaultableMulti;

                if (value is string && defaultableMulti != null)
                {
                    defaultableMulti.ValueType = (string)value;
                    return defaultableMulti.Clone();
                }
            }
            else
            {
                return null;
            }

            object result = base.ConvertFrom(context, culture, value);
            return result;
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
            return true;
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

            if (value is DefaultableMulti)
            {
                if (destinationType == typeof(string))
                {
                    DefaultableMulti defaultableMulti = (DefaultableMulti) value;
                    return defaultableMulti.ValueType;
                }
            }

            if (destinationType == typeof(DefaultableMulti))
            {
                bool e = true;
            }

            result = base.ConvertTo(context, culture, value, destinationType);
            return result;
        }
    }

    #endregion class DefaultableMultiTypeConvertor
}