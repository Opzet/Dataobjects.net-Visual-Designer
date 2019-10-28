namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    partial class ControlAddAssociationSimple
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlAddAssociationSimple));
            this.htmlPanel1 = new System.Windows.Forms.HtmlPanel();
            this.htmlPanel2 = new System.Windows.Forms.HtmlPanel();
            this.edPropertyName = new System.Windows.Forms.TextBox();
            this.htmlPanel3 = new System.Windows.Forms.HtmlPanel();
            this.htmlPanel4 = new System.Windows.Forms.HtmlPanel();
            this.chNavigationToItSelf = new System.Windows.Forms.CheckBox();
            this.htmlNavigationToItSelf = new System.Windows.Forms.HtmlPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.htmlPreview = new System.Windows.Forms.HtmlPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbMultiplicity = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.htmlPanel5 = new System.Windows.Forms.HtmlPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbPersistentTypes = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.cmbPropertyTypes = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // htmlPanel1
            // 
            this.htmlPanel1.AutoScroll = true;
            this.htmlPanel1.AutoScrollMinSize = new System.Drawing.Size(112, 13);
            this.htmlPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.htmlPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel1.Location = new System.Drawing.Point(13, 13);
            this.htmlPanel1.Name = "htmlPanel1";
            this.htmlPanel1.Size = new System.Drawing.Size(112, 16);
            this.htmlPanel1.TabIndex = 8;
            this.htmlPanel1.Text = "<font style=\"font-family:tahoma;font-size:8pt\">For <b>persistent type</b></font>";
            // 
            // htmlPanel2
            // 
            this.htmlPanel2.AutoScroll = true;
            this.htmlPanel2.AutoScrollMinSize = new System.Drawing.Size(233, 13);
            this.htmlPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.htmlPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel2.Location = new System.Drawing.Point(3, 3);
            this.htmlPanel2.Name = "htmlPanel2";
            this.htmlPanel2.Size = new System.Drawing.Size(233, 16);
            this.htmlPanel2.TabIndex = 9;
            this.htmlPanel2.Text = "<font style=\"font-family:tahoma; font-size: 8pt\">Create <b>navigation property</b" +
                "> named</font>";
            // 
            // edPropertyName
            // 
            this.edPropertyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edPropertyName.Location = new System.Drawing.Point(13, 91);
            this.edPropertyName.Name = "edPropertyName";
            this.edPropertyName.Size = new System.Drawing.Size(360, 21);
            this.edPropertyName.TabIndex = 1;
            this.edPropertyName.TextChanged += new System.EventHandler(this.edPropertyName_TextChanged);
            // 
            // htmlPanel3
            // 
            this.htmlPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlPanel3.AutoScroll = true;
            this.htmlPanel3.AutoScrollMinSize = new System.Drawing.Size(91, 13);
            this.htmlPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.htmlPanel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel3.Location = new System.Drawing.Point(266, 3);
            this.htmlPanel3.Name = "htmlPanel3";
            this.htmlPanel3.Size = new System.Drawing.Size(91, 16);
            this.htmlPanel3.TabIndex = 11;
            this.htmlPanel3.Text = "<font style=\"font-family:tahoma; font-size: 8pt\">e.g. <font style=\"font-style:ita" +
                "lic;\">MyProperty</font></font>";
            // 
            // htmlPanel4
            // 
            this.htmlPanel4.AutoScroll = true;
            this.htmlPanel4.AutoScrollMinSize = new System.Drawing.Size(233, 13);
            this.htmlPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.htmlPanel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel4.Location = new System.Drawing.Point(13, 118);
            this.htmlPanel4.Name = "htmlPanel4";
            this.htmlPanel4.Size = new System.Drawing.Size(233, 16);
            this.htmlPanel4.TabIndex = 12;
            this.htmlPanel4.Text = "<font style=\"font-family:tahoma; font-size: 8pt\">Where type points to other <b>pe" +
                "rsistent type</b></font>";
            // 
            // chNavigationToItSelf
            // 
            this.chNavigationToItSelf.AutoSize = true;
            this.chNavigationToItSelf.Location = new System.Drawing.Point(0, 3);
            this.chNavigationToItSelf.Name = "chNavigationToItSelf";
            this.chNavigationToItSelf.Size = new System.Drawing.Size(15, 14);
            this.chNavigationToItSelf.TabIndex = 3;
            this.chNavigationToItSelf.UseVisualStyleBackColor = true;
            this.chNavigationToItSelf.CheckedChanged += new System.EventHandler(this.chNavigationToItSelf_CheckedChanged);
            // 
            // htmlNavigationToItSelf
            // 
            this.htmlNavigationToItSelf.AutoScroll = true;
            this.htmlNavigationToItSelf.AutoScrollMinSize = new System.Drawing.Size(233, 13);
            this.htmlNavigationToItSelf.BackColor = System.Drawing.SystemColors.Control;
            this.htmlNavigationToItSelf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlNavigationToItSelf.Location = new System.Drawing.Point(24, 3);
            this.htmlNavigationToItSelf.Name = "htmlNavigationToItSelf";
            this.htmlNavigationToItSelf.Size = new System.Drawing.Size(233, 16);
            this.htmlNavigationToItSelf.TabIndex = 4;
            this.htmlNavigationToItSelf.Text = "<font style=\"font-family:tahoma; font-size: 8pt\">Navigation property type will po" +
                "int to <b>itself</b></font>";
            this.htmlNavigationToItSelf.Click += new System.EventHandler(this.htmlNavigationToItSelf_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Preview:";
            // 
            // htmlPreview
            // 
            this.htmlPreview.AutoScroll = true;
            this.htmlPreview.AutoScrollMinSize = new System.Drawing.Size(330, 69);
            this.htmlPreview.BackColor = System.Drawing.Color.White;
            this.htmlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPreview.Location = new System.Drawing.Point(0, 16);
            this.htmlPreview.Name = "htmlPreview";
            this.htmlPreview.Size = new System.Drawing.Size(360, 458);
            this.htmlPreview.TabIndex = 5;
            this.htmlPreview.Text = resources.GetString("htmlPreview.Text");
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.htmlPreview);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 256);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 474);
            this.panel1.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(164, -1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "Such class formatting is used only for preview";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.cmbMultiplicity, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.htmlPanel5, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.htmlPanel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbPersistentTypes, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.edPropertyName, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbPropertyTypes, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.htmlPanel4, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 694);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // cmbMultiplicity
            // 
            this.cmbMultiplicity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMultiplicity.DefaultImage = null;
            this.cmbMultiplicity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbMultiplicity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMultiplicity.FormattingEnabled = true;
            this.cmbMultiplicity.Location = new System.Drawing.Point(13, 195);
            this.cmbMultiplicity.Name = "cmbMultiplicity";
            this.cmbMultiplicity.Size = new System.Drawing.Size(360, 22);
            this.cmbMultiplicity.TabIndex = 22;
            this.cmbMultiplicity.SelectedIndexChanged += new System.EventHandler(this.cmbMultiplicity_SelectedIndexChanged);
            // 
            // htmlPanel5
            // 
            this.htmlPanel5.AutoScroll = true;
            this.htmlPanel5.AutoScrollMinSize = new System.Drawing.Size(167, 13);
            this.htmlPanel5.BackColor = System.Drawing.SystemColors.Control;
            this.htmlPanel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel5.Location = new System.Drawing.Point(13, 173);
            this.htmlPanel5.Name = "htmlPanel5";
            this.htmlPanel5.Size = new System.Drawing.Size(167, 16);
            this.htmlPanel5.TabIndex = 21;
            this.htmlPanel5.Text = "<font style=\"font-family:tahoma; font-size: 8pt\">With <b>multiplicity</b></font>";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chNavigationToItSelf);
            this.panel3.Controls.Add(this.htmlNavigationToItSelf);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(13, 223);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(360, 22);
            this.panel3.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.htmlPanel2);
            this.panel2.Controls.Add(this.htmlPanel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(13, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(360, 22);
            this.panel2.TabIndex = 19;
            // 
            // cmbPersistentTypes
            // 
            this.cmbPersistentTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPersistentTypes.DefaultImage = null;
            this.cmbPersistentTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPersistentTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPersistentTypes.FormattingEnabled = true;
            this.cmbPersistentTypes.Location = new System.Drawing.Point(13, 35);
            this.cmbPersistentTypes.Name = "cmbPersistentTypes";
            this.cmbPersistentTypes.Size = new System.Drawing.Size(360, 22);
            this.cmbPersistentTypes.TabIndex = 0;
            this.cmbPersistentTypes.Tag = "0";
            this.cmbPersistentTypes.SelectedIndexChanged += new System.EventHandler(this.PersistentTypeComboSelectionChanged);
            // 
            // cmbPropertyTypes
            // 
            this.cmbPropertyTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPropertyTypes.DefaultImage = null;
            this.cmbPropertyTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPropertyTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPropertyTypes.FormattingEnabled = true;
            this.cmbPropertyTypes.Location = new System.Drawing.Point(13, 140);
            this.cmbPropertyTypes.Name = "cmbPropertyTypes";
            this.cmbPropertyTypes.Size = new System.Drawing.Size(360, 22);
            this.cmbPropertyTypes.TabIndex = 2;
            this.cmbPropertyTypes.Tag = "1";
            this.cmbPropertyTypes.SelectedIndexChanged += new System.EventHandler(this.PersistentTypeComboSelectionChanged);
            // 
            // ControlAddAssociationSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "ControlAddAssociationSimple";
            this.Size = new System.Drawing.Size(386, 694);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Components.IconComboBox cmbPersistentTypes;
        private System.Windows.Forms.HtmlPanel htmlPanel1;
        private System.Windows.Forms.HtmlPanel htmlPanel2;
        private System.Windows.Forms.TextBox edPropertyName;
        private System.Windows.Forms.HtmlPanel htmlPanel3;
        private System.Windows.Forms.HtmlPanel htmlPanel4;
        private Components.IconComboBox cmbPropertyTypes;
        private System.Windows.Forms.CheckBox chNavigationToItSelf;
        private System.Windows.Forms.HtmlPanel htmlNavigationToItSelf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HtmlPanel htmlPreview;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.HtmlPanel htmlPanel5;
        private Components.IconComboBox cmbMultiplicity;
    }
}
