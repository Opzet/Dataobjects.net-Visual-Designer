using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    public partial class ControlDBSchemaPage : UserControl, IDBSchemaPageControl
    {
        protected IDBSchemaWizardControl wizardOwner;

        public ControlDBSchemaPage()
        {
            InitializeComponent();
        }

        public virtual string DefaultPageTitle
        {
            get { return string.Empty; }
        }

        public void Initialize(IDBSchemaWizardControl wizardOwner)
        {
            this.wizardOwner = wizardOwner;
            InternalInitialize();
        }

        public virtual bool LeavePage(PageDirection leaveDirection)
        {
            return true;
        }
        
        public virtual void EnterPage(PageDirection enterDirection)
        {}

        protected virtual void InternalInitialize()
        {}

        protected internal virtual void CheckNextButton()
        {}

        public void UpdateControls(bool enabled, bool waitCursor)
        {
            InternalUpdateControls(enabled, waitCursor);
        }

        protected internal virtual void InternalUpdateControls(bool enabled, bool waitCursor)
        {
            if (waitCursor)
            {
                this.Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            }
        }
    }
}
