using System.ComponentModel;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    public interface IListItemCollectionProvider
    {
        ListItemCollection GetCollection(ITypeDescriptorContext context, bool allowedNull);
        void ValidateValue(ITypeDescriptorContext context,  ref object value);
    }

    public abstract class ListItemCollectionProviderBase : IListItemCollectionProvider
    {
        protected internal ListItemCollection collection;

        public ListItemCollection GetCollection(ITypeDescriptorContext context, bool allowedNull)
        {
            bool canPopulate = collection == null || !ReuseCollection();

            if (collection == null)
            {
                collection = new ListItemCollection();
            }

            if (canPopulate)
            {
                PopulateCollection(context, allowedNull);
            }

            return collection;
        }

        protected abstract bool ReuseCollection();

        protected abstract bool AllowedNullValue();

        public abstract void ValidateValue(ITypeDescriptorContext context, ref object value);

        protected abstract void PopulateCollection(ITypeDescriptorContext context, bool allowedNull);
    }
}