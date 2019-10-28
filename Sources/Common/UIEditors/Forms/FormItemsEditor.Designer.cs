using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    partial class FormItemsEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemsEditor));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvItems = new LabelEditEnhancedTreeView();
            this.lineControl1 = new LineControl();
            this.panelSortIconsHint = new System.Windows.Forms.Panel();
            this.lbSortIconsHint = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel2.SuspendLayout();
            this.panelSortIconsHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(206, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(127, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList.Images.SetKeyName(0, "bullet.png");
            this.imageList.Images.SetKeyName(1, "ascend.png");
            this.imageList.Images.SetKeyName(2, "descend.png");
            // 
            // btnRemove
            // 
            this.btnRemove.Image = global::TXSoftware.DataObjectsNetEntityModel.Common.Properties.Resources.remove;
            this.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemove.Location = new System.Drawing.Point(2, 7);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(25, 23);
            this.btnRemove.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnRemove, "Remove selected item");
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Image = global::TXSoftware.DataObjectsNetEntityModel.Common.Properties.Resources.delete_all;
            this.btnRemoveAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveAll.Location = new System.Drawing.Point(27, 7);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(25, 23);
            this.btnRemoveAll.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnRemoveAll, "Remove all items");
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnRemoveAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 309);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(284, 33);
            this.panel2.TabIndex = 7;
            // 
            // lbTitle
            // 
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(284, 19);
            this.lbTitle.TabIndex = 5;
            this.lbTitle.Text = "Title";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(0, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 21);
            this.label1.TabIndex = 8;
            this.label1.Text = "● Press F2 to rename item";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvItems
            // 
            this.lvItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvItems.FullRowSelect = true;
            this.lvItems.ImageIndex = 0;
            this.lvItems.ImageList = this.imageList;
            this.lvItems.LabelEdit = true;
            this.lvItems.Location = new System.Drawing.Point(0, 19);
            this.lvItems.Name = "lvItems";
            this.lvItems.SelectedImageIndex = 0;
            this.lvItems.ShowNodeToolTips = true;
            this.lvItems.ShowPlusMinus = false;
            this.lvItems.ShowRootLines = false;
            this.lvItems.Size = new System.Drawing.Size(284, 222);
            this.lvItems.TabIndex = 0;
            this.lvItems.ValidateLabelEdit += new LabelEditEnhancedTreeView.ValidateLabelEditEventHandler(this.lvItems_ValidateLabelEdit);
            this.lvItems.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.lvItems_BeforeLabelEdit);
            this.lvItems.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.lvItems_AfterLabelEdit);
            this.lvItems.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.lvItems_NodeMouseDoubleClick);
            this.lvItems.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvItems_KeyUp);
            // 
            // lineControl1
            // 
            this.lineControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lineControl1.Location = new System.Drawing.Point(0, 304);
            this.lineControl1.Name = "lineControl1";
            this.lineControl1.Size = new System.Drawing.Size(284, 5);
            this.lineControl1.TabIndex = 10;
            this.lineControl1.Text = "lineControl1";
            // 
            // panelSortIconsHint
            // 
            this.panelSortIconsHint.Controls.Add(this.lbSortIconsHint);
            this.panelSortIconsHint.Controls.Add(this.linkLabel2);
            this.panelSortIconsHint.Controls.Add(this.linkLabel1);
            this.panelSortIconsHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSortIconsHint.Location = new System.Drawing.Point(0, 262);
            this.panelSortIconsHint.Name = "panelSortIconsHint";
            this.panelSortIconsHint.Size = new System.Drawing.Size(284, 42);
            this.panelSortIconsHint.TabIndex = 11;
            // 
            // lbSortIconsHint
            // 
            this.lbSortIconsHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbSortIconsHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbSortIconsHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbSortIconsHint.Location = new System.Drawing.Point(0, 0);
            this.lbSortIconsHint.Name = "lbSortIconsHint";
            this.lbSortIconsHint.Size = new System.Drawing.Size(284, 21);
            this.lbSortIconsHint.TabIndex = 10;
            this.lbSortIconsHint.Text = "● Double click on icon to change sort direction";
            this.lbSortIconsHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel2
            // 
            this.linkLabel2.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.linkLabel2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.linkLabel2.Image = global::TXSoftware.DataObjectsNetEntityModel.Common.Properties.Resources.descend;
            this.linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel2.LinkColor = System.Drawing.SystemColors.ControlText;
            this.linkLabel2.Location = new System.Drawing.Point(86, 23);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(81, 13);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Descending";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.linkLabel2.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.SystemColors.ControlText;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.linkLabel1.Image = global::TXSoftware.DataObjectsNetEntityModel.Common.Properties.Resources.ascend;
            this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.SystemColors.ControlText;
            this.linkLabel1.Location = new System.Drawing.Point(5, 23);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(75, 13);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Ascending";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.linkLabel1.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            // 
            // FormItemsEditor
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 342);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelSortIconsHint);
            this.Controls.Add(this.lineControl1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormItemsEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Items Editor";
            this.panel2.ResumeLayout(false);
            this.panelSortIconsHint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private Components.LabelEditEnhancedTreeView lvItems;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label label1;
        private Components.LineControl lineControl1;
        private System.Windows.Forms.Panel panelSortIconsHint;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lbSortIconsHint;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}