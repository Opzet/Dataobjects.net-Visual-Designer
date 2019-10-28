namespace DataObjects.EntityGenerator.Controls
{
    partial class ControlDBSchemaPageDB
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlDBSchemaPageDB));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelProvider = new System.Windows.Forms.Panel();
            this.cmbDBProviders = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelStorageEngineSelector = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.cmbStorageEngine = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbAuthSQL = new System.Windows.Forms.RadioButton();
            this.rbAuthWindows = new System.Windows.Forms.RadioButton();
            this.edAuthPassword = new System.Windows.Forms.TextBox();
            this.edAuthUserID = new System.Windows.Forms.TextBox();
            this.lbAuthPassword = new System.Windows.Forms.Label();
            this.lbAuthUserID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDatabases = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edServerName = new System.Windows.Forms.TextBox();
            this.panelUserPass = new System.Windows.Forms.Panel();
            this.panelDatabase = new System.Windows.Forms.Panel();
            this.panelServerAuth = new System.Windows.Forms.Panel();
            this.panelProvider.SuspendLayout();
            this.panelStorageEngineSelector.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelUserPass.SuspendLayout();
            this.panelDatabase.SuspendLayout();
            this.panelServerAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "database_engine_x16.png");
            this.imageList.Images.SetKeyName(1, "database_server_x16.png");
            this.imageList.Images.SetKeyName(2, "authentication_x16.png");
            this.imageList.Images.SetKeyName(3, "database_x16.png");
            this.imageList.Images.SetKeyName(4, "storage_x16.png");
            // 
            // panelProvider
            // 
            this.panelProvider.Controls.Add(this.cmbDBProviders);
            this.panelProvider.Controls.Add(this.label1);
            this.panelProvider.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProvider.Location = new System.Drawing.Point(0, 0);
            this.panelProvider.Name = "panelProvider";
            this.panelProvider.Size = new System.Drawing.Size(466, 35);
            this.panelProvider.TabIndex = 0;
            // 
            // cmbDBProviders
            // 
            this.cmbDBProviders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDBProviders.DefaultImage = null;
            this.cmbDBProviders.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbDBProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBProviders.FormattingEnabled = true;
            this.cmbDBProviders.Location = new System.Drawing.Point(114, 8);
            this.cmbDBProviders.Name = "cmbDBProviders";
            this.cmbDBProviders.Size = new System.Drawing.Size(301, 21);
            this.cmbDBProviders.TabIndex = 0;
            this.cmbDBProviders.SelectedIndexChanged += new System.EventHandler(this.cmbDBProviders_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(25, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Provider:";
            // 
            // panelStorageEngineSelector
            // 
            this.panelStorageEngineSelector.Controls.Add(this.linkLabel1);
            this.panelStorageEngineSelector.Controls.Add(this.cmbStorageEngine);
            this.panelStorageEngineSelector.Controls.Add(this.label5);
            this.panelStorageEngineSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStorageEngineSelector.Location = new System.Drawing.Point(0, 35);
            this.panelStorageEngineSelector.Name = "panelStorageEngineSelector";
            this.panelStorageEngineSelector.Size = new System.Drawing.Size(466, 37);
            this.panelStorageEngineSelector.TabIndex = 1;
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.Location = new System.Drawing.Point(421, 15);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(34, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Info...";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.linkLabel1.Enter += new System.EventHandler(this.linkLabel1_Enter);
            // 
            // cmbStorageEngine
            // 
            this.cmbStorageEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStorageEngine.DefaultImage = null;
            this.cmbStorageEngine.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbStorageEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorageEngine.FormattingEnabled = true;
            this.cmbStorageEngine.Location = new System.Drawing.Point(114, 7);
            this.cmbStorageEngine.Name = "cmbStorageEngine";
            this.cmbStorageEngine.Size = new System.Drawing.Size(301, 21);
            this.cmbStorageEngine.TabIndex = 0;
            this.cmbStorageEngine.SelectedIndexChanged += new System.EventHandler(this.cmbStorageEngine_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(25, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Storage:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(25, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Authentication:";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.SystemColors.Window;
            this.panel4.Controls.Add(this.rbAuthSQL);
            this.panel4.Controls.Add(this.rbAuthWindows);
            this.panel4.Location = new System.Drawing.Point(114, 32);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(301, 49);
            this.panel4.TabIndex = 1;
            // 
            // rbAuthSQL
            // 
            this.rbAuthSQL.AutoSize = true;
            this.rbAuthSQL.Location = new System.Drawing.Point(10, 26);
            this.rbAuthSQL.Name = "rbAuthSQL";
            this.rbAuthSQL.Size = new System.Drawing.Size(126, 17);
            this.rbAuthSQL.TabIndex = 1;
            this.rbAuthSQL.TabStop = true;
            this.rbAuthSQL.Text = "Server authentication";
            this.rbAuthSQL.UseVisualStyleBackColor = true;
            this.rbAuthSQL.CheckedChanged += new System.EventHandler(this.Authentication_CheckedChanged);
            // 
            // rbAuthWindows
            // 
            this.rbAuthWindows.AutoSize = true;
            this.rbAuthWindows.Location = new System.Drawing.Point(10, 4);
            this.rbAuthWindows.Name = "rbAuthWindows";
            this.rbAuthWindows.Size = new System.Drawing.Size(139, 17);
            this.rbAuthWindows.TabIndex = 0;
            this.rbAuthWindows.TabStop = true;
            this.rbAuthWindows.Text = "Windows authentication";
            this.rbAuthWindows.UseVisualStyleBackColor = true;
            this.rbAuthWindows.CheckedChanged += new System.EventHandler(this.Authentication_CheckedChanged);
            // 
            // edAuthPassword
            // 
            this.edAuthPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edAuthPassword.Location = new System.Drawing.Point(114, 34);
            this.edAuthPassword.Name = "edAuthPassword";
            this.edAuthPassword.Size = new System.Drawing.Size(301, 20);
            this.edAuthPassword.TabIndex = 1;
            // 
            // edAuthUserID
            // 
            this.edAuthUserID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edAuthUserID.Location = new System.Drawing.Point(114, 10);
            this.edAuthUserID.Name = "edAuthUserID";
            this.edAuthUserID.Size = new System.Drawing.Size(301, 20);
            this.edAuthUserID.TabIndex = 0;
            // 
            // lbAuthPassword
            // 
            this.lbAuthPassword.AutoSize = true;
            this.lbAuthPassword.Location = new System.Drawing.Point(25, 36);
            this.lbAuthPassword.Name = "lbAuthPassword";
            this.lbAuthPassword.Size = new System.Drawing.Size(56, 13);
            this.lbAuthPassword.TabIndex = 16;
            this.lbAuthPassword.Text = "Password:";
            // 
            // lbAuthUserID
            // 
            this.lbAuthUserID.AutoSize = true;
            this.lbAuthUserID.Location = new System.Drawing.Point(25, 13);
            this.lbAuthUserID.Name = "lbAuthUserID";
            this.lbAuthUserID.Size = new System.Drawing.Size(61, 13);
            this.lbAuthUserID.TabIndex = 17;
            this.lbAuthUserID.Text = "User name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.ImageKey = "(none)";
            this.label3.Location = new System.Drawing.Point(25, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Database:";
            // 
            // cmbDatabases
            // 
            this.cmbDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDatabases.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDatabases.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDatabases.Enabled = false;
            this.cmbDatabases.FormattingEnabled = true;
            this.cmbDatabases.Location = new System.Drawing.Point(114, 10);
            this.cmbDatabases.Name = "cmbDatabases";
            this.cmbDatabases.Size = new System.Drawing.Size(301, 21);
            this.cmbDatabases.TabIndex = 0;
            this.cmbDatabases.SelectedIndexChanged += new System.EventHandler(this.cmbDatabases_SelectedIndexChanged);
            this.cmbDatabases.TextChanged += new System.EventHandler(this.cmbDatabases_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(25, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Server Name:";
            // 
            // edServerName
            // 
            this.edServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edServerName.Location = new System.Drawing.Point(114, 6);
            this.edServerName.Name = "edServerName";
            this.edServerName.Size = new System.Drawing.Size(301, 20);
            this.edServerName.TabIndex = 0;
            this.edServerName.Text = "localhost";
            this.edServerName.TextChanged += new System.EventHandler(this.edServerName_TextChanged);
            // 
            // panelUserPass
            // 
            this.panelUserPass.Controls.Add(this.edAuthUserID);
            this.panelUserPass.Controls.Add(this.lbAuthPassword);
            this.panelUserPass.Controls.Add(this.edAuthPassword);
            this.panelUserPass.Controls.Add(this.lbAuthUserID);
            this.panelUserPass.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUserPass.Location = new System.Drawing.Point(0, 159);
            this.panelUserPass.Name = "panelUserPass";
            this.panelUserPass.Size = new System.Drawing.Size(466, 63);
            this.panelUserPass.TabIndex = 3;
            // 
            // panelDatabase
            // 
            this.panelDatabase.Controls.Add(this.cmbDatabases);
            this.panelDatabase.Controls.Add(this.label3);
            this.panelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDatabase.Location = new System.Drawing.Point(0, 222);
            this.panelDatabase.Name = "panelDatabase";
            this.panelDatabase.Size = new System.Drawing.Size(466, 58);
            this.panelDatabase.TabIndex = 4;
            // 
            // panelServerAuth
            // 
            this.panelServerAuth.Controls.Add(this.label2);
            this.panelServerAuth.Controls.Add(this.label4);
            this.panelServerAuth.Controls.Add(this.panel4);
            this.panelServerAuth.Controls.Add(this.edServerName);
            this.panelServerAuth.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelServerAuth.Location = new System.Drawing.Point(0, 72);
            this.panelServerAuth.Name = "panelServerAuth";
            this.panelServerAuth.Size = new System.Drawing.Size(466, 87);
            this.panelServerAuth.TabIndex = 2;
            // 
            // ControlDBSchemaPageDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDatabase);
            this.Controls.Add(this.panelUserPass);
            this.Controls.Add(this.panelServerAuth);
            this.Controls.Add(this.panelStorageEngineSelector);
            this.Controls.Add(this.panelProvider);
            this.Name = "ControlDBSchemaPageDB";
            this.panelProvider.ResumeLayout(false);
            this.panelProvider.PerformLayout();
            this.panelStorageEngineSelector.ResumeLayout(false);
            this.panelStorageEngineSelector.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panelUserPass.ResumeLayout(false);
            this.panelUserPass.PerformLayout();
            this.panelDatabase.ResumeLayout(false);
            this.panelDatabase.PerformLayout();
            this.panelServerAuth.ResumeLayout(false);
            this.panelServerAuth.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel panelProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelStorageEngineSelector;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rbAuthSQL;
        private System.Windows.Forms.RadioButton rbAuthWindows;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edAuthPassword;
        private System.Windows.Forms.TextBox edAuthUserID;
        private System.Windows.Forms.Label lbAuthPassword;
        private System.Windows.Forms.Label lbAuthUserID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDatabases;
        private System.Windows.Forms.TextBox edServerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelDatabase;
        private System.Windows.Forms.Panel panelUserPass;
        private System.Windows.Forms.Panel panelServerAuth;
        private TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox cmbDBProviders;
        private TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox cmbStorageEngine;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
