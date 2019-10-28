using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    partial class FormImportDBSchema
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportDBSchema));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbPageTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.phPages = new System.Windows.Forms.Panel();
            this.pageDatabase = new DataObjects.EntityGenerator.Controls.ControlDBSchemaPageDB();
            this.pageTables = new TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors.ControlDBSchemaPageTables();
            this.bevelControl2 = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.BevelControl();
            this.lineControl1 = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.LineControl();
            this.mainProgress = new System.Windows.Forms.ProgressBar();
            this.pageAuxiliaryTables = new TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors.ControlDBSchemaAuxiliaryTables();
            this.panel1.SuspendLayout();
            this.phPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.lbPageTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 48);
            this.panel1.TabIndex = 0;
            // 
            // lbPageTitle
            // 
            this.lbPageTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbPageTitle.Location = new System.Drawing.Point(12, 11);
            this.lbPageTitle.Name = "lbPageTitle";
            this.lbPageTitle.Size = new System.Drawing.Size(472, 27);
            this.lbPageTitle.TabIndex = 0;
            this.lbPageTitle.Text = "-";
            this.lbPageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(397, 351);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(316, 351);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 19;
            this.btnNext.Text = "&Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(235, 352);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 20;
            this.btnBack.Text = "&Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // phPages
            // 
            this.phPages.Controls.Add(this.pageAuxiliaryTables);
            this.phPages.Controls.Add(this.pageDatabase);
            this.phPages.Controls.Add(this.pageTables);
            this.phPages.Location = new System.Drawing.Point(18, 56);
            this.phPages.Name = "phPages";
            this.phPages.Size = new System.Drawing.Size(466, 280);
            this.phPages.TabIndex = 21;
            // 
            // pageDatabase
            // 
            this.pageDatabase.Location = new System.Drawing.Point(93, 62);
            this.pageDatabase.Name = "pageDatabase";
            this.pageDatabase.SelectedProviderChangedDisabled = false;
            this.pageDatabase.Size = new System.Drawing.Size(328, 143);
            this.pageDatabase.TabIndex = 2;
            // 
            // pageTables
            // 
            this.pageTables.Location = new System.Drawing.Point(27, 24);
            this.pageTables.Name = "pageTables";
            this.pageTables.Size = new System.Drawing.Size(221, 116);
            this.pageTables.TabIndex = 1;
            // 
            // bevelControl2
            // 
            this.bevelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bevelControl2.Location = new System.Drawing.Point(14, 342);
            this.bevelControl2.Name = "bevelControl2";
            this.bevelControl2.Sides = System.Windows.Forms.Border3DSide.Top;
            this.bevelControl2.Size = new System.Drawing.Size(474, 5);
            this.bevelControl2.Style = System.Windows.Forms.Border3DStyle.Etched;
            this.bevelControl2.TabIndex = 17;
            this.bevelControl2.Text = "bevelControl2";
            // 
            // lineControl1
            // 
            this.lineControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineControl1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lineControl1.Location = new System.Drawing.Point(0, 48);
            this.lineControl1.Name = "lineControl1";
            this.lineControl1.Size = new System.Drawing.Size(503, 2);
            this.lineControl1.TabIndex = 1;
            this.lineControl1.Text = "lineControl1";
            // 
            // mainProgress
            // 
            this.mainProgress.Location = new System.Drawing.Point(15, 355);
            this.mainProgress.Name = "mainProgress";
            this.mainProgress.Size = new System.Drawing.Size(214, 17);
            this.mainProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.mainProgress.TabIndex = 22;
            this.mainProgress.Value = 100;
            this.mainProgress.Visible = false;
            // 
            // pageAuxiliaryTables
            // 
            this.pageAuxiliaryTables.Location = new System.Drawing.Point(168, 190);
            this.pageAuxiliaryTables.Name = "pageAuxiliaryTables";
            this.pageAuxiliaryTables.Size = new System.Drawing.Size(286, 78);
            this.pageAuxiliaryTables.TabIndex = 3;
            // 
            // FormImportDBSchema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 385);
            this.Controls.Add(this.mainProgress);
            this.Controls.Add(this.phPages);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.bevelControl2);
            this.Controls.Add(this.lineControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportDBSchema";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Existing Database Schema Into Designer";
            this.panel1.ResumeLayout(false);
            this.phPages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbPageTitle;
        private LineControl lineControl1;
        private BevelControl bevelControl2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel phPages;
        private ControlDBSchemaPageTables pageTables;
        private DataObjects.EntityGenerator.Controls.ControlDBSchemaPageDB pageDatabase;
        private System.Windows.Forms.ProgressBar mainProgress;
        private ControlDBSchemaAuxiliaryTables pageAuxiliaryTables;
    }
}