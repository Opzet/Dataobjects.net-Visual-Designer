using System;
using System.ComponentModel;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    public abstract class ChainedPropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor tail;
        protected PropertyDescriptor Tail { get { return tail; } }

        protected ChainedPropertyDescriptor(PropertyDescriptor tail)
            : base(tail)
        {
            if (tail == null) throw new ArgumentNullException("tail");
            this.tail = tail;
        }
        public override void AddValueChanged(object component, System.EventHandler handler)
        {
            tail.AddValueChanged(component, handler);
        }
        public override AttributeCollection Attributes
        {
            get
            {
                return tail.Attributes;
            }
        }
        public override bool CanResetValue(object component)
        {
            return tail.CanResetValue(component);
        }
        public override string Category
        {
            get
            {
                return tail.Category;
            }
        }
        public override Type ComponentType
        {
            get { return tail.ComponentType; }
        }
        public override TypeConverter Converter
        {
            get
            {
                return tail.Converter;
            }
        }
        public override string Description
        {
            get
            {
                return tail.Description;
            }
        }
        public override bool DesignTimeOnly
        {
            get
            {
                return tail.DesignTimeOnly;
            }
        }
        public override string DisplayName
        {
            get
            {
                return tail.DisplayName;
            }
        }
        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            return tail.GetChildProperties(instance, filter);
        }
        public override object GetEditor(Type editorBaseType)
        {
            return tail.GetEditor(editorBaseType);
        }
        public override object GetValue(object component)
        {
            return tail.GetValue(component);
        }
        public override bool IsBrowsable
        {
            get
            {
                return tail.IsBrowsable;
            }
        }
        public override bool IsLocalizable
        {
            get
            {
                return tail.IsLocalizable;
            }
        }
        public override bool IsReadOnly
        {
            get { return tail.IsReadOnly; }
        }
        public override string Name
        {
            get
            {
                return tail.Name;
            }
        }
        public override Type PropertyType
        {
            get { return tail.PropertyType; }
        }
        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            tail.RemoveValueChanged(component, handler);
        }
        public override void ResetValue(object component)
        {
            tail.ResetValue(component);
        }
        public override void SetValue(object component, object value)
        {
            tail.SetValue(component, value);
        }
        public override bool ShouldSerializeValue(object component)
        {
            return tail.ShouldSerializeValue(component);
        }
        public override bool SupportsChangeEvents
        {
            get
            {
                return tail.SupportsChangeEvents;
            }
        }
    }
}