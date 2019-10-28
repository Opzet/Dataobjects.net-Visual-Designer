using System;
using System.Drawing;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormInputBox : Form
    {
        public const int DEFAULT_WIDTH = 370;

        public FormInputBox()
        {
            InitializeComponent();
        }

        public static bool DialogShow(string caption, string message, ref string value)
        {
            return DialogShow(caption, message, DEFAULT_WIDTH, ref value);
        }

        public static bool DialogShow(string caption, string message, int width, ref string value)
        {
            return InternalDialogShow<FormInputBox>(width, caption, message, ref value);
        }

        internal static bool InternalDialogShow<T>(int width, string caption, string message, ref string value) where T : FormInputBox, new()
        {
            T form;
            return InternalDialogShow(width, caption, message, null, ref value, out form);
        }

        internal static bool InternalDialogShow<T>(int width, string caption, string message, Action<T> populateForm, 
            ref string value, out T form) where T : FormInputBox, new()
        {
            form = new T();
            form.Size = new Size(width, form.Size.Height);
            form.Text = caption;
            form.lbMessage.Text = message;
            form.edValue.Text = value;

            if (populateForm != null)
            {
                populateForm(form);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                value = form.edValue.Text;
                return true;
            }

            return false;
        }

        protected virtual bool Validate(out string error)
        {
            error = string.Empty;
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string error;
            if (!Validate(out error))
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
