namespace DataObjects.EntityGenerator.Controls
{
    partial class PageConnectionSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageConnectionSettings));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbDBProviders = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelStorageEngineSelector = new System.Windows.Forms.Panel();
            this.cmbStorageEngine = new System.Windows.Forms.ComboBox();
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
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelStorageEngineSelector.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelUserPass.SuspendLayout();
            this.panelDatabase.SuspendLayout();
            this.panelServerAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panelDatabase);
            this.panel2.Controls.Add(this.panelUserPass);
            this.panel2.Controls.Add(this.panelServerAuth);
            this.panel2.Controls.Add(this.panelStorageEngineSelector);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Size = new System.Drawing.Size(607, 338);
            // 
            // lbStepInfo
            // 
            this.lbStepInfo.Location = new System.Drawing.Point(508, 0);
            // 
            // lbTitle
            // 
            this.lbTitle.Size = new System.Drawing.Size(508, 27);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbDBProviders);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(607, 62);
            this.panel3.TabIndex = 0;
            // 
            // cmbDBProviders
            // 
            this.cmbDBProviders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDBProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBProviders.FormattingEnabled = true;
            this.cmbDBProviders.Location = new System.Drawing.Point(153, 28);
            this.cmbDBProviders.Name = "cmbDBProviders";
            this.cmbDBProviders.Size = new System.Drawing.Size(414, 23);
            this.cmbDBProviders.TabIndex = 0;
            this.cmbDBProviders.SelectedIndexChanged += new System.EventHandler(this.cmbDBProviders_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.ImageIndex = 0;
            this.label1.ImageList = this.imageList;
            this.label1.Location = new System.Drawing.Point(29, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Provider:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelStorageEngineSelector
            // 
            this.panelStorageEngineSelector.Controls.Add(this.cmbStorageEngine);
            this.panelStorageEngineSelector.Controls.Add(this.label5);
            this.panelStorageEngineSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStorageEngineSelector.Location = new System.Drawing.Point(0, 62);
            this.panelStorageEngineSelector.Name = "panelStorageEngineSelector";
            this.panelStorageEngineSelector.Size = new System.Drawing.Size(607, 50);
            this.panelStorageEngineSelector.TabIndex = 1;
            // 
            // cmbStorageEngine
            // 
            this.cmbStorageEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStorageEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorageEngine.FormattingEnabled = true;
            this.cmbStorageEngine.Location = new System.Drawing.Point(153, 14);
            this.cmbStorageEngine.Name = "cmbStorageEngine";
            this.cmbStorageEngine.Size = new System.Drawing.Size(414, 23);
            this.cmbStorageEngine.TabIndex = 0;
            this.cmbStorageEngine.SelectedIndexChanged += new System.EventHandler(this.cmbStorageEngine_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.ImageIndex = 4;
            this.label5.ImageList = this.imageList;
            this.label5.Location = new System.Drawing.Point(29, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Storage:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.ImageIndex = 2;
            this.label4.ImageList = this.imageList;
            this.label4.Location = new System.Drawing.Point(29, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Authentication:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.SystemColors.Window;
            this.panel4.Controls.Add(this.rbAuthSQL);
            this.panel4.Controls.Add(this.rbAuthWindows);
            this.panel4.Location = new System.Drawing.Point(153, 40);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(414, 57);
            this.panel4.TabIndex = 1;
            // 
            // rbAuthSQL
            // 
            this.rbAuthSQL.AutoSize = true;
            this.rbAuthSQL.Location = new System.Drawing.Point(12, 30);
            this.rbAuthSQL.Name = "rbAuthSQL";
            this.rbAuthSQL.Size = new System.Drawing.Size(140, 19);
            this.rbAuthSQL.TabIndex = 1;
            this.rbAuthSQL.TabStop = true;
            this.rbAuthSQL.Text = "Server authentication";
            this.rbAuthSQL.UseVisualStyleBackColor = true;
            this.rbAuthSQL.CheckedChanged += new System.EventHandler(this.Authentication_CheckedChanged);
            // 
            // rbAuthWindows
            // 
            this.rbAuthWindows.AutoSize = true;
            this.rbAuthWindows.Location = new System.Drawing.Point(12, 5);
            this.rbAuthWindows.Name = "rbAuthWindows";
            this.rbAuthWindows.Size = new System.Drawing.Size(155, 19);
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
            this.edAuthPassword.Location = new System.Drawing.Point(153, 39);
            this.edAuthPassword.Name = "edAuthPassword";
            this.edAuthPassword.Size = new System.Drawing.Size(414, 21);
            this.edAuthPassword.TabIndex = 1;
            // 
            // edAuthUserID
            // 
            this.edAuthUserID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edAuthUserID.Location = new System.Drawing.Point(153, 12);
            this.edAuthUserID.Name = "edAuthUserID";
            this.edAuthUserID.Size = new System.Drawing.Size(414, 21);
            this.edAuthUserID.TabIndex = 0;
            // 
            // lbAuthPassword
            // 
            this.lbAuthPassword.AutoSize = true;
            this.lbAuthPassword.Location = new System.Drawing.Point(44, 42);
            this.lbAuthPassword.Name = "lbAuthPassword";
            this.lbAuthPassword.Size = new System.Drawing.Size(64, 15);
            this.lbAuthPassword.TabIndex = 16;
            this.lbAuthPassword.Text = "Password:";
            // 
            // lbAuthUserID
            // 
            this.lbAuthUserID.AutoSize = true;
            this.lbAuthUserID.Location = new System.Drawing.Point(44, 15);
            this.lbAuthUserID.Name = "lbAuthUserID";
            this.lbAuthUserID.Size = new System.Drawing.Size(71, 15);
            this.lbAuthUserID.TabIndex = 17;
            this.lbAuthUserID.Text = "User name:";
            // 
            // label3
            // 
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.ImageKey = "database_x16.png";
            this.label3.ImageList = this.imageList;
            this.label3.Location = new System.Drawing.Point(29, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "Database:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDatabases
            // 
            this.cmbDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDatabases.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDatabases.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDatabases.Enabled = false;
            this.cmbDatabases.FormattingEnabled = true;
            this.cmbDatabases.Location = new System.Drawing.Point(153, 12);
            this.cmbDatabases.Name = "cmbDatabases";
            this.cmbDatabases.Size = new System.Drawing.Size(414, 23);
            this.cmbDatabases.TabIndex = 4;
            this.cmbDatabases.SelectedIndexChanged += new System.EventHandler(this.cmbDatabases_SelectedIndexChanged);
            this.cmbDatabases.TextChanged += new System.EventHandler(this.cmbDatabases_TextChanged);
            // 
            // label2
            // 
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.ImageIndex = 1;
            this.label2.ImageList = this.imageList;
            this.label2.Location = new System.Drawing.Point(29, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "Server Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // edServerName
            // 
            this.edServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edServerName.Location = new System.Drawing.Point(153, 11);
            this.edServerName.Name = "edServerName";
            this.edServerName.Size = new System.Drawing.Size(414, 21);
            this.edServerName.TabIndex = 0;
            this.edServerName.TextChanged += new System.EventHandler(this.edServerName_TextChanged);
            // 
            // panelUserPass
            // 
            this.panelUserPass.Controls.Add(this.edAuthUserID);
            this.panelUserPass.Controls.Add(this.lbAuthPassword);
            this.panelUserPass.Controls.Add(this.edAuthPassword);
            this.panelUserPass.Controls.Add(this.lbAuthUserID);
            this.panelUserPass.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUserPass.Location = new System.Drawing.Point(0, 219);
            this.panelUserPass.Name = "panelUserPass";
            this.panelUserPass.Size = new System.Drawing.Size(607, 73);
            this.panelUserPass.TabIndex = 3;
            // 
            // panelDatabase
            // 
            this.panelDatabase.Controls.Add(this.cmbDatabases);
            this.panelDatabase.Controls.Add(this.label3);
            this.panelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDatabase.Location = new System.Drawing.Point(0, 292);
            this.panelDatabase.Name = "panelDatabase";
            this.panelDatabase.Size = new System.Drawing.Size(607, 46);
            this.panelDatabase.TabIndex = 4;
            // 
            // panelServerAuth
            // 
            this.panelServerAuth.Controls.Add(this.label2);
            this.panelServerAuth.Controls.Add(this.label4);
            this.panelServerAuth.Controls.Add(this.panel4);
            this.panelServerAuth.Controls.Add(this.edServerName);
            this.panelServerAuth.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelServerAuth.Location = new System.Drawing.Point(0, 112);
            this.panelServerAuth.Name = "panelServerAuth";
            this.panelServerAuth.Size = new System.Drawing.Size(607, 107);
            this.panelServerAuth.TabIndex = 2;
            // 
            // PageConnectionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PageConnectionSettings";
            this.Size = new System.Drawing.Size(607, 365);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panelStorageEngineSelector.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panelUserPass.ResumeLayout(false);
            this.panelUserPass.PerformLayout();
            this.panelDatabase.ResumeLayout(false);
            this.panelServerAuth.ResumeLayout(false);
            this.panelServerAuth.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cmbDBProviders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelStorageEngineSelector;
        private System.Windows.Forms.ComboBox cmbStorageEngine;
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
    }
}
