using System.ComponentModel;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    public interface IModalDialogForm
    {
        void BindData(ITypeDescriptorContext context, object value, object[] attributeArguments);
        object SaveData();
    }
}