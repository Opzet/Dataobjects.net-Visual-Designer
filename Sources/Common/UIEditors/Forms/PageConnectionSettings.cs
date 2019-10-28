using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DataObjects.EntityGenerator.Code;
using DataObjects.EntityGenerator.Connector;
using DataObjects.EntityGenerator.DBProvider;
using DataObjects.EntityGenerator.Utils;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace DataObjects.EntityGenerator.Controls
{
    public partial class PageConnectionSettings : Form
    {
        private const string STEP_TITLE = "Database: Select database engine provider, server and its database";
        private bool firstTimeInitialized = false;
        private bool selectedProviderChangedDisabled = false;
        private bool lastConnectedOk;
        private bool lastGlobalUpdateControlsEnabled = false;
        private bool UpdateAuthControlsDisabled;
        private object lastSelectedStorageEngine;

        public PageConnectionSettings()
        {
            InitializeComponent();
            Initialize();
        }

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


        private void Initialize()
        {
            RefreshProviders();
        }

/*
        protected internal override void SaveStateData()
        {
            if (SelectedProvider == null)
            {
                return;
            }

            IConnectionInfo connectionInfo = CreateConnectionInfo(SelectedProvider);
            connectionInfo.AuthenticationType = SelectedAuthenticationType;

            var wizardDataProviderServerDb = WizardContext.Context.DataProviderServerDb;
            wizardDataProviderServerDb.ProviderUniqueID = SelectedProvider.UniqueID;
            wizardDataProviderServerDb.ConnectionInfo = connectionInfo;
            wizardDataProviderServerDb.StorageEngine = SelectedProvider.SupportsMultipleStorageEngines ? (string)SelectedStorageEngine.Value : null;

            ProgramConfiguration programCfg = ProgramConfiguration.Instance;
            programCfg.SelectProviderAndServerData = wizardDataProviderServerDb;
        }
*/

        private void UpdateControls(bool enabled, bool waitCursor)
        {
            this.lastGlobalUpdateControlsEnabled = enabled;
            cmbDBProviders.Enabled = enabled;
            //cmbServers.Enabled = enabled;
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
                return UIUtils.GetSelectedItem<IDBProvider>(cmbDBProviders);
            }
        }

        private ListItem SelectedStorageEngine
        {
            get
            {
                var selectedProvider = this.SelectedProvider;
                if (selectedProvider != null && selectedProvider.SupportsMultipleStorageEngines)
                {
                    return cmbStorageEngine.SelectedItem as ListItem;
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

        private ServersDatabasesCacheProvider CachedProvider
        {
            get { return SelectedProvider == null ? null : ServersDatabasesCache.Current[SelectedProvider.UniqueID]; }
        }

        private string SelectedServerName
        {
            get { return edServerName.Text; }
            set { edServerName.Text = value; }
        }

        private void RefreshProviders()
        {
            Guid selectedProviderUniqueID = this.SelectedProvider != null ? this.SelectedProvider.UniqueID : Guid.Empty;

            bool wasfirstTimeInitialized = false;

            var programCfg = ProgramConfiguration.Instance;
            var programCfgProviderAndServer = programCfg.SelectProviderAndServerData;

            if (!firstTimeInitialized)
            {
                try
                {
                    selectedProviderUniqueID = programCfgProviderAndServer.ProviderUniqueID;
                }
                finally
                {
                    wasfirstTimeInitialized = true;
                    firstTimeInitialized = true;
                }
            }

            cmbDBProviders.Items.Clear();
            int selIdx = -1;
            foreach (var providerModule in ProviderManager.Instance.EnumerateModules())
            {
                foreach (IDBProvider provider in providerModule.Providers)
                {
                    cmbDBProviders.Items.Add(new DataBoundItem<IDBProvider>(provider));

                    if (provider.UniqueID.Equals(selectedProviderUniqueID))
                    {
                        selIdx = cmbDBProviders.Items.Count - 1;
                    }
                }
            }

            if (selIdx > -1)
            {
                SelectedProviderChangedDisabled = !wasfirstTimeInitialized;
                try
                {
                    cmbDBProviders.SelectedIndex = selIdx;
                }
                finally
                {
                    SelectedProviderChangedDisabled = false;
                }

                if (wasfirstTimeInitialized && programCfgProviderAndServer.ConnectInfo != null)
                {
                    SelectValuesByConfig(programCfgProviderAndServer.StorageEngine, programCfgProviderAndServer.ConnectInfo);
                }
            }
        }

        private void PopulateStorageEngines()
        {
            ListItem selectedItem = cmbStorageEngine.SelectedItem as ListItem;
            object selectedStorageEngine = selectedItem != null ? selectedItem.Value : null;
            if (selectedStorageEngine == null)
            {
                selectedStorageEngine = lastSelectedStorageEngine;
            }

            cmbStorageEngine.Items.Clear();
            var selectedProvider = SelectedProvider;

            panelStorageEngineSelector.Visible = selectedProvider != null && selectedProvider.SupportsMultipleStorageEngines;

            if (selectedProvider != null && selectedProvider.SupportsMultipleStorageEngines)
            {
                foreach (var pair in selectedProvider.GetSupportedStorageEngines())
                {
                    string storageEngineKey = pair.Key;
                    string storageEngineDisplayName = pair.Value;
                    ListItem item = new ListItem { Text = storageEngineDisplayName, Value = storageEngineKey };
                    cmbStorageEngine.Items.Add(item);
                }

                if (selectedStorageEngine != null)
                {
                    cmbStorageEngine.SelectedIndex = GetStorageEngineItemIndex((string)selectedStorageEngine); ;
                }
            }
        }

        private bool InternalReconnectServer()
        {
            return InternalReconnectServer(true);
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

                lastConnectedOk = result;

                if (checkLastErrors)
                {
                    CheckLastErrors();
                }

                if (result)
                {
                    var cacheProvider = ServersDatabasesCache.Current.AddProvider(selectedProvider.UniqueID);
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
            cmbDatabases.Enabled = this.lastGlobalUpdateControlsEnabled;// && lastConnectedOk;
            //btnConnect.Enabled = this.lastGlobalUpdateControlsEnabled && SelectedProvider != null;
            //btnRefreshServers.Enabled = cmbServers.Enabled;
            //btnRefreshDatabases.Enabled = this.lastGlobalUpdateControlsEnabled && lastConnectedOk;
        }


        private void CheckLastErrors()
        {
            if (SelectedProvider != null && SelectedProvider.LastErrors.Count > 0)
            {
                UIContext.Context.ShowError("Connect to server", SelectedProvider.LastErrors);
            }
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
            string storageEngineKey = this.SelectedStorageEngine == null ? null : (string)this.SelectedStorageEngine.Value;
            IConnectionInfo connectionInfo = provider.ConnectionInfoProvider.CreateInfo(storageEngineKey, user, password, server, database);

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

        private void SelectValuesByConfig(string storageEngine, TemporaryConnectionInfo connectionInfo)
        {
            if (SelectedProvider != null && SelectedProvider.SupportsMultipleStorageEngines && !string.IsNullOrEmpty(storageEngine))
            {
                int idx = GetStorageEngineItemIndex(storageEngine);
                if (idx > -1)
                {
                    //SelectedStorageEngineChangedDisabled = true;
                    try
                    {
                        cmbStorageEngine.SelectedIndex = idx;
                    }
                    finally
                    {
                        //SelectedStorageEngineChangedDisabled = false;
                    }
                }
            }

            bool reconnectServerResult = false;
            var cachedServer = CachedProvider[connectionInfo.ServerName];
            if (cachedServer != null)
            {
                cachedServer.AuthenticationType = connectionInfo.AuthenticationType;
                cachedServer.UserID = connectionInfo.UserID;
            }
            SelectedAuthenticationType = connectionInfo.AuthenticationType;
            SelectAuthentication = new Tuple<string, string>(connectionInfo.UserID, string.Empty);
            SelectedServerName = connectionInfo.ServerName;
            cmbDatabases.Text = connectionInfo.DatabaseName;

            reconnectServerResult = InternalReconnectServer(!connectionInfo.PasswordIsSet);

            //if (cmbServers.SelectedIndex > -1 && reconnectServerResult)
            if (!string.IsNullOrEmpty(SelectedServerName) && reconnectServerResult)
            {
                UpdateAuthControlsDisabled = true;
                try
                {
                    UpdateAuthControls();
                }
                finally
                {
                    UpdateAuthControlsDisabled = false;
                }
            }
        }

        private int GetStorageEngineItemIndex(string storageEngineKey)
        {
            int idx = -1;
            for (int i = 0; i < this.cmbStorageEngine.Items.Count; i++)
            {
                ListItem listItem = (ListItem)this.cmbStorageEngine.Items[i];

                if ((string)listItem.Value == storageEngineKey)
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

                    /*IConnectionInfo connectionInfo = selectedProvider.ConnectionInfoProvider.CreateInfo(SelectedStorageEngine, __user, __password, __server, __database);
                    connectionInfo.AuthenticationType = SelectAuthenticationType;
                    if (connectionInfo.AuthenticationType != AuthenticationType.Windows)
                    {
                        var selectAuthentication = SelectAuthentication;
                        connectionInfo.UserID = selectAuthentication.Item1;
                        connectionInfo.Password = selectAuthentication.Item2;
                    }*/

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
            lastSelectedStorageEngine = SelectedStorageEngine != null ? SelectedStorageEngine.Value : null;
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
    }
}
