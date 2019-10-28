namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    partial class FormAddAssociation
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
            this.panelTypes = new System.Windows.Forms.Panel();
            this.htmlPanel2 = new System.Windows.Forms.HtmlPanel();
            this.edAssociationName = new System.Windows.Forms.TextBox();
            this.htmlPanel1 = new System.Windows.Forms.HtmlPanel();
            this.htmlPanel4 = new System.Windows.Forms.HtmlPanel();
            this.rbAdvanced = new System.Windows.Forms.RadioButton();
            this.rbSimple = new System.Windows.Forms.RadioButton();
            this.panelContent = new System.Windows.Forms.Panel();
            this.controlAdvanced = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms.ControlAddAssociationAdv();
            this.controlSimple = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms.ControlAddAssociationSimple();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panelTypes.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTypes
            // 
            this.panelTypes.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelTypes.Controls.Add(this.htmlPanel2);
            this.panelTypes.Controls.Add(this.edAssociationName);
            this.panelTypes.Controls.Add(this.htmlPanel1);
            this.panelTypes.Controls.Add(this.htmlPanel4);
            this.panelTypes.Controls.Add(this.rbAdvanced);
            this.panelTypes.Controls.Add(this.rbSimple);
            this.panelTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTypes.Location = new System.Drawing.Point(0, 0);
            this.panelTypes.Name = "panelTypes";
            this.panelTypes.Size = new System.Drawing.Size(468, 136);
            this.panelTypes.TabIndex = 0;
            // 
            // htmlPanel2
            // 
            this.htmlPanel2.AutoScroll = true;
            this.htmlPanel2.AutoScrollMinSize = new System.Drawing.Size(112, 13);
            this.htmlPanel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.htmlPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel2.Location = new System.Drawing.Point(14, 80);
            this.htmlPanel2.Name = "htmlPanel2";
            this.htmlPanel2.Size = new System.Drawing.Size(173, 16);
            this.htmlPanel2.TabIndex = 17;
            this.htmlPanel2.Text = "<font style=\"font-family:tahoma;font-size:8pt\">Association link <b>name</b></font" +
                ">";
            // 
            // edAssociationName
            // 
            this.edAssociationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edAssociationName.Location = new System.Drawing.Point(14, 99);
            this.edAssociationName.Name = "edAssociationName";
            this.edAssociationName.Size = new System.Drawing.Size(439, 20);
            this.edAssociationName.TabIndex = 15;
            this.edAssociationName.TextChanged += new System.EventHandler(this.edAssociationName_TextChanged);
            // 
            // htmlPanel1
            // 
            this.htmlPanel1.AutoScroll = true;
            this.htmlPanel1.AutoScrollMinSize = new System.Drawing.Size(205, 26);
            this.htmlPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.htmlPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel1.Location = new System.Drawing.Point(239, 37);
            this.htmlPanel1.Name = "htmlPanel1";
            this.htmlPanel1.Size = new System.Drawing.Size(205, 27);
            this.htmlPanel1.TabIndex = 14;
            this.htmlPanel1.Text = "<font style=\"font-family:tahoma; font-size: 8pt\">Creates association with <b>two<" +
                "/b> navigation properties with more settings.</font>";
            // 
            // htmlPanel4
            // 
            this.htmlPanel4.AutoScroll = true;
            this.htmlPanel4.AutoScrollMinSize = new System.Drawing.Size(166, 26);
            this.htmlPanel4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.htmlPanel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel4.Location = new System.Drawing.Point(42, 37);
            this.htmlPanel4.Name = "htmlPanel4";
            this.htmlPanel4.Size = new System.Drawing.Size(166, 27);
            this.htmlPanel4.TabIndex = 13;
            this.htmlPanel4.Text = "<font style=\"font-family:tahoma; font-size: 8pt\">Creates association with only <b" +
                ">one</b> navigation property.</font>";
            // 
            // rbAdvanced
            // 
            this.rbAdvanced.AutoSize = true;
            this.rbAdvanced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rbAdvanced.Location = new System.Drawing.Point(239, 14);
            this.rbAdvanced.Name = "rbAdvanced";
            this.rbAdvanced.Size = new System.Drawing.Size(151, 17);
            this.rbAdvanced.TabIndex = 1;
            this.rbAdvanced.TabStop = true;
            this.rbAdvanced.Tag = "1";
            this.rbAdvanced.Text = "Advanced Association";
            this.rbAdvanced.UseVisualStyleBackColor = true;
            this.rbAdvanced.CheckedChanged += new System.EventHandler(this.OnTypeChanged);
            // 
            // rbSimple
            // 
            this.rbSimple.AutoSize = true;
            this.rbSimple.Checked = true;
            this.rbSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rbSimple.Location = new System.Drawing.Point(23, 14);
            this.rbSimple.Name = "rbSimple";
            this.rbSimple.Size = new System.Drawing.Size(131, 17);
            this.rbSimple.TabIndex = 0;
            this.rbSimple.TabStop = true;
            this.rbSimple.Tag = "0";
            this.rbSimple.Text = "Simple Association";
            this.rbSimple.UseVisualStyleBackColor = true;
            this.rbSimple.CheckedChanged += new System.EventHandler(this.OnTypeChanged);
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.controlAdvanced);
            this.panelContent.Controls.Add(this.controlSimple);
            this.panelContent.Controls.Add(this.panel1);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 136);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(468, 522);
            this.panelContent.TabIndex = 1;
            // 
            // controlAdvanced
            // 
            this.controlAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlAdvanced.Location = new System.Drawing.Point(0, 0);
            this.controlAdvanced.Name = "controlAdvanced";
            this.controlAdvanced.Size = new System.Drawing.Size(468, 480);
            this.controlAdvanced.TabIndex = 3;
            this.controlAdvanced.Visible = false;
            // 
            // controlSimple
            // 
            this.controlSimple.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSimple.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.controlSimple.Location = new System.Drawing.Point(0, 0);
            this.controlSimple.Name = "controlSimple";
            this.controlSimple.Size = new System.Drawing.Size(468, 480);
            this.controlSimple.TabIndex = 2;
            this.controlSimple.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 480);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 42);
            this.panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(369, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(288, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormAddAssociation
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(468, 658);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTypes);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddAssociation";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Association";
            this.panelTypes.ResumeLayout(false);
            this.panelTypes.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTypes;
        private System.Windows.Forms.RadioButton rbAdvanced;
        private System.Windows.Forms.RadioButton rbSimple;
        private System.Windows.Forms.HtmlPanel htmlPanel1;
        private System.Windows.Forms.HtmlPanel htmlPanel4;
        private System.Windows.Forms.Panel panelContent;
        private ControlAddAssociationSimple controlSimple;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private ControlAddAssociationAdv controlAdvanced;
        public System.Windows.Forms.TextBox edAssociationName;
        private System.Windows.Forms.HtmlPanel htmlPanel2;
    }
}