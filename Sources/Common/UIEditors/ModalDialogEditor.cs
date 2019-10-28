using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    public class ModalDialogEditor<TForm> : UITypeEditor where TForm : Form, IModalDialogForm, new()
    {
        private IWindowsFormsEditorService _service = null;
        private TForm form = null;

        [SecurityPermission(SecurityAction.LinkDemand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (null != context && null != context.Instance && null != provider)
            {
                this._service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (null != this._service)
                {
                    //if (this.form == null)
                    //{
                    form = new TForm();
                    //}
                    
                    PropertyDescriptor propertyDescriptor = context.PropertyDescriptor;
                    object[] attrArguments =
                        propertyDescriptor.Attributes.OfType<ModalDialogEditorArgumentAttribute>().Select(
                            attr => attr.Argumments).SingleOrDefault();

                    form.BindData(context, value, attrArguments);

                    switch (this._service.ShowDialog(form))
                    {
                        case DialogResult.OK:
                        {
                            value = form.SaveData();

                            if (!propertyDescriptor.PropertyType.In(typeof(object), typeof(ObjectValue)))
                            {
                                value = propertyDescriptor.Converter.ConvertTo(value, propertyDescriptor.PropertyType);
                            }

                            propertyDescriptor.SetValue(context.Instance, value);

                            break;
                        }
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            return value;
        }
    }

    #region class ModalDialogEditorArgumentAttribute

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ModalDialogEditorArgumentAttribute : Attribute
    {
        public object[] Argumments { get; set; }

        public ModalDialogEditorArgumentAttribute(params object[] argumments)
        {
            this.Argumments = argumments ?? new object[0];
        }
    }

    #endregion class ModalDialogEditorArgumentAttribute
}