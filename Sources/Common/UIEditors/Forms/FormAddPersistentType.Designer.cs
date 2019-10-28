using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    partial class FormAddPersistentType
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddPersistentType));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbBaseTypes = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPersistentTypes = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.lbBaseTypes = new System.Windows.Forms.Label();
            this.edEntityName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.keyPropertyGroupBox = new System.Windows.Forms.GroupBox();
            this.cmbPropertyTypes = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.edPropertyName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chCreateKey = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            this.keyPropertyGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBaseTypes);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbPersistentTypes);
            this.groupBox1.Controls.Add(this.lbBaseTypes);
            this.groupBox1.Controls.Add(this.edEntityName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // cmbBaseTypes
            // 
            this.cmbBaseTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBaseTypes.DefaultImage = null;
            this.cmbBaseTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbBaseTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaseTypes.FormattingEnabled = true;
            this.cmbBaseTypes.Location = new System.Drawing.Point(9, 138);
            this.cmbBaseTypes.Name = "cmbBaseTypes";
            this.cmbBaseTypes.Size = new System.Drawing.Size(352, 21);
            this.cmbBaseTypes.TabIndex = 2;
            this.cmbBaseTypes.SelectedIndexChanged += new System.EventHandler(this.cmbBaseTypes_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Persistent Type:";
            // 
            // cmbPersistentTypes
            // 
            this.cmbPersistentTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPersistentTypes.DefaultImage = null;
            this.cmbPersistentTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPersistentTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPersistentTypes.FormattingEnabled = true;
            this.cmbPersistentTypes.Location = new System.Drawing.Point(9, 40);
            this.cmbPersistentTypes.Name = "cmbPersistentTypes";
            this.cmbPersistentTypes.Size = new System.Drawing.Size(352, 21);
            this.cmbPersistentTypes.TabIndex = 0;
            this.cmbPersistentTypes.SelectedIndexChanged += new System.EventHandler(this.cmbPersistentTypes_SelectedIndexChanged);
            // 
            // lbBaseTypes
            // 
            this.lbBaseTypes.AutoSize = true;
            this.lbBaseTypes.Location = new System.Drawing.Point(6, 120);
            this.lbBaseTypes.Name = "lbBaseTypes";
            this.lbBaseTypes.Size = new System.Drawing.Size(61, 13);
            this.lbBaseTypes.TabIndex = 2;
            this.lbBaseTypes.Text = "Base Type:";
            // 
            // edEntityName
            // 
            this.edEntityName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edEntityName.Location = new System.Drawing.Point(9, 89);
            this.edEntityName.Name = "edEntityName";
            this.edEntityName.Size = new System.Drawing.Size(352, 20);
            this.edEntityName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type Name:";
            // 
            // keyPropertyGroupBox
            // 
            this.keyPropertyGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.keyPropertyGroupBox.Controls.Add(this.cmbPropertyTypes);
            this.keyPropertyGroupBox.Controls.Add(this.edPropertyName);
            this.keyPropertyGroupBox.Controls.Add(this.label5);
            this.keyPropertyGroupBox.Controls.Add(this.chCreateKey);
            this.keyPropertyGroupBox.Controls.Add(this.label4);
            this.keyPropertyGroupBox.Location = new System.Drawing.Point(12, 189);
            this.keyPropertyGroupBox.Name = "keyPropertyGroupBox";
            this.keyPropertyGroupBox.Size = new System.Drawing.Size(366, 143);
            this.keyPropertyGroupBox.TabIndex = 1;
            this.keyPropertyGroupBox.TabStop = false;
            this.keyPropertyGroupBox.Text = "Key Field Property";
            // 
            // cmbPropertyTypes
            // 
            this.cmbPropertyTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPropertyTypes.DefaultImage = null;
            this.cmbPropertyTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPropertyTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPropertyTypes.FormattingEnabled = true;
            this.cmbPropertyTypes.Location = new System.Drawing.Point(9, 115);
            this.cmbPropertyTypes.Name = "cmbPropertyTypes";
            this.cmbPropertyTypes.Size = new System.Drawing.Size(352, 21);
            this.cmbPropertyTypes.TabIndex = 2;
            // 
            // edPropertyName
            // 
            this.edPropertyName.Location = new System.Drawing.Point(9, 66);
            this.edPropertyName.Name = "edPropertyName";
            this.edPropertyName.Size = new System.Drawing.Size(352, 20);
            this.edPropertyName.TabIndex = 1;
            this.edPropertyName.Text = "Id";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Property Type:";
            // 
            // chCreateKey
            // 
            this.chCreateKey.AutoSize = true;
            this.chCreateKey.Checked = true;
            this.chCreateKey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCreateKey.Location = new System.Drawing.Point(9, 20);
            this.chCreateKey.Name = "chCreateKey";
            this.chCreateKey.Size = new System.Drawing.Size(140, 17);
            this.chCreateKey.TabIndex = 0;
            this.chCreateKey.Text = "Create key field property";
            this.chCreateKey.UseVisualStyleBackColor = true;
            this.chCreateKey.CheckedChanged += new System.EventHandler(this.chCreateKey_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Property Name:";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(304, 341);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(223, 341);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "cancel_16.png");
            // 
            // FormAddPersistentType
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(392, 373);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.keyPropertyGroupBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddPersistentType";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Persistent Type";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddEntityForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.keyPropertyGroupBox.ResumeLayout(false);
            this.keyPropertyGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbBaseTypes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox keyPropertyGroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox edPropertyName;
        private System.Windows.Forms.CheckBox chCreateKey;
        public System.Windows.Forms.TextBox edEntityName;
        private System.Windows.Forms.Label label3;
        private Components.IconComboBox cmbPersistentTypes;
        private Components.IconComboBox cmbBaseTypes;
        private Components.IconComboBox cmbPropertyTypes;
        private System.Windows.Forms.ImageList imageList1;
    }
}