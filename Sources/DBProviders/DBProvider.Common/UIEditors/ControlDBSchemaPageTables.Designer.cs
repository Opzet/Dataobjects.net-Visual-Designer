namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    partial class ControlDBSchemaPageTables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlDBSchemaPageTables));
            this.label1 = new System.Windows.Forms.Label();
            this.treeTables = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.TreeViewEx();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.treeColumns = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.TreeViewEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chSelectDependTables = new System.Windows.Forms.CheckBox();
            this.lbSelectedTables = new System.Windows.Forms.Label();
            this.lbSelectedColumns = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(277, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Schemas / Tables:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeTables
            // 
            this.treeTables.AutoChangeToIndeterminateState = false;
            this.treeTables.AutoCheckChilds = false;
            this.treeTables.AutoCheckParents = false;
            this.treeTables.CheckBoxes = true;
            this.treeTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTables.FullRowSelect = true;
            this.treeTables.HideSelection = false;
            this.treeTables.ImageIndex = 0;
            this.treeTables.ImageList = this.imageList;
            this.treeTables.IndeterminateToChecked = true;
            this.treeTables.Location = new System.Drawing.Point(0, 24);
            this.treeTables.Name = "treeTables";
            this.treeTables.SelectedImageIndex = 0;
            this.treeTables.Size = new System.Drawing.Size(277, 232);
            this.treeTables.TabIndex = 1;
            this.treeTables.TriStateCheckBoxesEnabled = false;
            this.treeTables.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeTables_BeforeCheck);
            this.treeTables.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeTables_AfterCheck);
            this.treeTables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeTables_AfterSelect);
            this.treeTables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeTables_KeyDown);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList.Images.SetKeyName(0, "database_x16.png");
            this.imageList.Images.SetKeyName(1, "database_schema_x16.png");
            this.imageList.Images.SetKeyName(2, "database_table_x16.gif");
            this.imageList.Images.SetKeyName(3, "database_table_column_x16.png");
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Columns:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // treeColumns
            // 
            this.treeColumns.AutoChangeToIndeterminateState = false;
            this.treeColumns.AutoCheckChilds = false;
            this.treeColumns.AutoCheckParents = false;
            this.treeColumns.CheckBoxes = true;
            this.treeColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeColumns.FullRowSelect = true;
            this.treeColumns.HideSelection = false;
            this.treeColumns.ImageIndex = 3;
            this.treeColumns.ImageList = this.imageList;
            this.treeColumns.IndeterminateToChecked = true;
            this.treeColumns.Location = new System.Drawing.Point(0, 24);
            this.treeColumns.Name = "treeColumns";
            this.treeColumns.SelectedImageIndex = 3;
            this.treeColumns.Size = new System.Drawing.Size(185, 232);
            this.treeColumns.TabIndex = 3;
            this.treeColumns.TriStateCheckBoxesEnabled = false;
            this.treeColumns.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeColumns_AfterCheck);
            this.treeColumns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeColumns_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.chSelectDependTables);
            this.splitContainer1.Panel1.Controls.Add(this.treeTables);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.lbSelectedTables);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.treeColumns);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.lbSelectedColumns);
            this.splitContainer1.Size = new System.Drawing.Size(466, 280);
            this.splitContainer1.SplitterDistance = 277;
            this.splitContainer1.TabIndex = 4;
            // 
            // chSelectDependTables
            // 
            this.chSelectDependTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chSelectDependTables.AutoSize = true;
            this.chSelectDependTables.Checked = true;
            this.chSelectDependTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chSelectDependTables.Location = new System.Drawing.Point(105, 260);
            this.chSelectDependTables.Name = "chSelectDependTables";
            this.chSelectDependTables.Size = new System.Drawing.Size(172, 17);
            this.chSelectDependTables.TabIndex = 2;
            this.chSelectDependTables.Text = "Auto select dependency tables";
            this.chSelectDependTables.UseVisualStyleBackColor = true;
            // 
            // lbSelectedTables
            // 
            this.lbSelectedTables.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbSelectedTables.Location = new System.Drawing.Point(0, 256);
            this.lbSelectedTables.Name = "lbSelectedTables";
            this.lbSelectedTables.Size = new System.Drawing.Size(277, 24);
            this.lbSelectedTables.TabIndex = 3;
            this.lbSelectedTables.Text = "Selected Tables: 0";
            this.lbSelectedTables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSelectedColumns
            // 
            this.lbSelectedColumns.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbSelectedColumns.Location = new System.Drawing.Point(0, 256);
            this.lbSelectedColumns.Name = "lbSelectedColumns";
            this.lbSelectedColumns.Size = new System.Drawing.Size(185, 24);
            this.lbSelectedColumns.TabIndex = 4;
            this.lbSelectedColumns.Text = "Selected Columns: 0";
            this.lbSelectedColumns.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label3.Location = new System.Drawing.Point(196, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ctrl+A - Select All";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label4.Location = new System.Drawing.Point(104, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Ctrl+A - Select All";
            // 
            // ControlDBSchemaPageTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ControlDBSchemaPageTables";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Common.UIEditors.Components.TreeViewEx treeTables;
        private System.Windows.Forms.Label label2;
        private Common.UIEditors.Components.TreeViewEx treeColumns;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.CheckBox chSelectDependTables;
        private System.Windows.Forms.Label lbSelectedTables;
        private System.Windows.Forms.Label lbSelectedColumns;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
