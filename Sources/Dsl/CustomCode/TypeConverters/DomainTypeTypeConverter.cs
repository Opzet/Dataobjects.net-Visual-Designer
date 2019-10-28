using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
//    public class DomainTypeTypeConverter : StandardValuesTypeConverterBase
//    {
//        protected override string[] BindStandardValues(ITypeDescriptorContext context)
//        {
//            ModelElement element = context.Instance as ModelElement;
//            IModelRoot entityModel = element.Store.GetEntityModel();
//            return entityModel.DomainTypes.Select(item => item.FullName).ToArray();
//        }
//    }

    public class DomainTypeTypeConverter : TypeConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var entityModel = GetEntityModel(context);
            var domainTypes = entityModel.DomainTypes.Cast<IDomainType>().ToArray();

            return new StandardValuesCollection(domainTypes);
        }

        private static IModelRoot GetEntityModel(ITypeDescriptorContext context)
        {
            ModelElement element = context.Instance as ModelElement;
            IModelRoot entityModel = element.Store.GetEntityModel();
            return entityModel;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || destinationType == typeof(DomainType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string type = (string) value;
            var entityModel = GetEntityModel(context);
            var result = entityModel.DomainTypes.SingleOrDefault(item => item.FullName == type);
            return result;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var result = base.ConvertTo(context, culture, value, destinationType);
            return result;
        }
    }


//    public class DomainTypeEditor : StandardValuesEditorBase<DomainTypeTypeConverter>
//    {
        //readonly DomainTypeTypeConverter converterProxy = new DomainTypeTypeConverter();
//
//        protected override StandardValuesTypeConverterBase ResolveStandardValuesTypeConverter(object value)
//        {
//            var result = base.ResolveStandardValuesTypeConverter(value);
//            if (result == null)
//            {
//                result = new DomainTypeTypeConverter();
//            }
//
//            return result;
//        }
//    }
}