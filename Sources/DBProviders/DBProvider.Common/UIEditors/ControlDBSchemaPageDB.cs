using System;
using System.Linq;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;
using TXSoftware.DataObjectsNetEntityModel.DBProvider;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.Properties;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors;

namespace DataObjects.EntityGenerator.Controls
{
    public partial class ControlDBSchemaPageDB : ControlDBSchemaPage
    {
        private const string DEFAULT_PAGE_TITLE = "Specify connection settings to the target database.";
        private const string ITEM_PLEASE_SELECT = "(Please Select)";

        private bool selectedProviderChangedDisabled = false;
        private bool lastGlobalUpdateControlsEnabled = false;
        private bool UpdateAuthControlsDisabled;
        private StorageEngine lastSelectedStorageEngine;

        public bool SelectedProviderChangedDisabled
        {
            get
            {
                return this.selectedProviderChangedDisabled;
            }
            set
            {
                this.selectedProviderChangedDisabled = value;
            }
        }


        public ControlDBSchemaPageDB()
        {
            InitializeComponent();

            cmbDBProviders.Select();
        }

        public override string DefaultPageTitle
        {
            get { return DEFAULT_PAGE_TITLE; }
        }

        protected override void InternalInitialize()
        {
            PopulateProviders();
            CheckNextButton();
        }

        public override bool LeavePage(PageDirection leaveDirection)
        {
            bool validated = true;
            if (leaveDirection == PageDirection.Next)
            {
                validated = InternalReconnectServer(true);
                if (validated)
                {
                    SaveSelectedData();
                }
            }

            return validated;
        }

        protected internal override void CheckNextButton()
        {
            bool selectedServerValid = !string.IsNullOrEmpty(SelectedServerName);
            bool selectedDatabaseValid = !string.IsNullOrEmpty(cmbDatabases.Text);

            base.wizardOwner.ButtonNext.Enabled = SelectedProvider != null && selectedServerValid && selectedDatabaseValid;
        }

        internal void SaveSelectedData()
        {
            if (SelectedProvider == null)
            {
                return;
            }

            IConnectionInfo connectionInfo = CreateConnectionInfo(SelectedProvider);
            connectionInfo.AuthenticationType = SelectedAuthenticationType;
            wizardOwner.SelectedDB = new Tuple<IDBProvider, IConnectionInfo>(SelectedProvider, connectionInfo);
        }

        protected internal override void InternalUpdateControls(bool enabled, bool waitCursor)
        {
            this.lastGlobalUpdateControlsEnabled = enabled;
            cmbDBProviders.Enabled = enabled;
            UpdateDbControls();
        }

        private AuthenticationType SelectedAuthenticationType
        {
            get
            {
                return rbAuthWindows.Checked ? AuthenticationType.Windows : AuthenticationType.SqlServer;
            }
            set
            {
                rbAuthWindows.Checked = value == AuthenticationType.Windows;
                rbAuthSQL.Checked = value == AuthenticationType.SqlServer;
            }
        }

        private IDBProvider SelectedProvider
        {
            get
            {
                if (cmbDBProviders.SelectedIndex > -1)
                {
                    return (IDBProvider)(cmbDBProviders.SelectedItem as IconListEntry).Value;
                }

                return null;
            }
        }

        private StorageEngine SelectedStorageEngine
        {
            get
            {
                var selectedProvider = this.SelectedProvider;
                if (selectedProvider != null && selectedProvider.SupportsMultipleStorageEngines && cmbStorageEngine.SelectedItem != null)
                {
                    return (StorageEngine) (cmbStorageEngine.SelectedItem as IconListEntry).Value;
                }

                return null;
            }
        }

        private Tuple<string, string> SelectAuthentication
        {
            get
            {
                return new Tuple<string, string>(edAuthUserID.Text, edAuthPassword.Text);
            }
            set
            {
                edAuthUserID.Text = value.Item1;
                edAuthPassword.Text = value.Item2;
            }
        }

        private string SelectedServerName
        {
            get { return edServerName.Text; }
            set { edServerName.Text = value; }
        }

        private void PopulateProviders()
        {
            Guid selectedProviderUniqueID = this.SelectedProvider != null ? this.SelectedProvider.UniqueID : Guid.Empty;

            bool wasfirstTimeInitialized = false;

            cmbDBProviders.Items.Clear();
            int selIdx = 0;

            cmbDBProviders.Items.Add(new IconListEntry(ITEM_PLEASE_SELECT, null, null));

            foreach (var providerModule in DBProviderManager.Instance.EnumerateModules())
            {
                foreach (IDBProvider provider in providerModule.Providers)
                {
                    cmbDBProviders.Items.Add(new IconListEntry(provider.Name, provider, provider.Icon));

                    if (provider.UniqueID.Equals(selectedProviderUniqueID))
                    {
                        selIdx = cmbDBProviders.Items.Count - 1;
                    }
                }
            }
            
            SelectedProviderChangedDisabled = !wasfirstTimeInitialized;
            try
            {
                cmbDBProviders.SelectedIndex = selIdx;
            }
            finally
            {
                SelectedProviderChangedDisabled = false;
            }
        }

        private void PopulateStorageEngines()
        {
            StorageEngine selectedStorageEngine = SelectedStorageEngine;
            if (selectedStorageEngine == null)
            {
                selectedStorageEngine = lastSelectedStorageEngine;
            }

            cmbStorageEngine.Items.Clear();
            var selectedProvider = SelectedProvider;

            panelStorageEngineSelector.Visible = selectedProvider != null && selectedProvider.SupportsMultipleStorageEngines;

            if (selectedProvider != null && selectedProvider.SupportsMultipleStorageEngines)
            {
                cmbStorageEngine.Items.Add(new IconListEntry(ITEM_PLEASE_SELECT, null, null));

                foreach (StorageEngine storageEngine in selectedProvider.GetSupportedStorageEngines())
                {
                    var item = new IconListEntry { Text = storageEngine.DisplayName, Value = storageEngine, Icon = Resources.database_engine_x16 };
                    cmbStorageEngine.Items.Add(item);
                }

                if (selectedStorageEngine != null)
                {
                    cmbStorageEngine.SelectedIndex = GetStorageEngineItemIndex(selectedStorageEngine);
                }
                else
                {
                    cmbStorageEngine.SelectedIndex = 0;
                }
            }
        }

        private bool InternalReconnectServer(bool checkLastErrors)
        {
            var selectedProvider = SelectedProvider;
            bool result = false;
            if (selectedProvider != null)
            {
                if (selectedProvider.IsConnected)
                {
                    selectedProvider.Disconnect();
                }

                IConnectionInfo connectionInfo = CreateConnectionInfo(selectedProvider);
                result = connectionInfo != null;
                if (result)
                {
                    selectedProvider.ClearLastErrors();
                    result = selectedProvider.Connect(connectionInfo);
                }

                //lastConnectedOk = result;

                if (checkLastErrors)
                {
                    CheckLastErrors();
                }

                UpdateDbControls();
            }

            return result;
        }

        private void UpdateDbControls()
        {
            var selectedProvider = this.SelectedProvider;
            bool selStorageEngine = selectedProvider != null;
            if (selStorageEngine)
            {
                selStorageEngine = !selectedProvider.SupportsMultipleStorageEngines || SelectedStorageEngine != null;
            }

            edServerName.Enabled = this.lastGlobalUpdateControlsEnabled && SelectedProvider != null && selStorageEngine;
            cmbDatabases.Enabled = this.lastGlobalUpdateControlsEnabled;
        }


        private void CheckLastErrors()
        {
            if (SelectedProvider != null && SelectedProvider.LastErrors.Count > 0)
            {
                ShowError("Connect to server", SelectedProvider.LastErrors);
            }
        }

        private void ShowError(string message, ErrorCollection errors)
        {
            string errs = string.Join(Environment.NewLine, errors.Select(item => item.Message));
            MessageBox.Show(string.Format("{0}\n\rErrors:{1}\n\r", message, errs),
                "Errror", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private IConnectionInfo CreateConnectionInfo(IDBProvider provider)
        {
            if (provider.SupportsMultipleStorageEngines && SelectedStorageEngine == null)
            {
                return null;
            }

            var authentication = this.SelectAuthentication;

            string user = SelectedAuthenticationType == AuthenticationType.SqlServer ? authentication.Item1 : null;
            string password = SelectedAuthenticationType == AuthenticationType.SqlServer ? authentication.Item2 : null;
            string server = SelectedServerName;
            string database = cmbDatabases.Text;
            IConnectionInfo connectionInfo = provider.ConnectionInfoProvider.CreateInfo(SelectedStorageEngine, user, password, server, database);

            return connectionInfo;
        }

        private void UpdateAuthControls()
        {
            if (UpdateAuthControlsDisabled)
            {
                return;
            }

            var isSql = SelectedAuthenticationType == AuthenticationType.SqlServer;
            panelUserPass.Visible = isSql;
        }

        private int GetStorageEngineItemIndex(StorageEngine storageEngineKey)
        {
            int idx = -1;
            for (int i = 0; i < this.cmbStorageEngine.Items.Count; i++)
            {
                var listItem = (IconListEntry)this.cmbStorageEngine.Items[i];

                if ((listItem.Value as StorageEngine) == storageEngineKey)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        private void SelectedProviderChanged()
        {
            if (SelectedProviderChangedDisabled)
            {
                return;
            }
            //UpdateControls(false, true, true);
            //UIContext.Context.BeginLoadingAction(true);
            try
            {
                var selectedProvider = SelectedProvider;
                if (selectedProvider != null)
                {
                    PopulateStorageEngines();

                    var connectionInfo = CreateConnectionInfo(selectedProvider);
                    if (connectionInfo != null)
                    {
                        if (selectedProvider.IsConnected)
                        {
                            selectedProvider.Disconnect();
                        }

                        bool connected = selectedProvider.Connect(connectionInfo);
                    }
                }
            }
            finally
            {
                CheckLastErrors();
                CheckNextButton();
                //UIContext.Context.EndLoadingAction();
            }
        }

        private void Authentication_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthControls();
        }

        private void cmbDBProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedProviderChanged();
        }

        private void cmbStorageEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDbControls();
            lastSelectedStorageEngine = SelectedStorageEngine;
        }

        private void edServerName_TextChanged(object sender, EventArgs e)
        {
            CheckNextButton();
        }

        private void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cmbDatabases_TextChanged(object sender, EventArgs e)
        {
            CheckNextButton();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StorageEngine selectedStorageEngine = SelectedStorageEngine;
            if (selectedStorageEngine != null)
            {
                MessageBox.Show(selectedStorageEngine.Description, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void linkLabel1_Enter(object sender, EventArgs e)
        {
//            Control control = (sender as Control);
//            control.SelectNextControl(control, true, false, false, true);
        }
    }
}
