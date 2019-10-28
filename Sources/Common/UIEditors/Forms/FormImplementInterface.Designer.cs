using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    partial class FormImplementInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImplementInterface));
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTypes = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.panelIncludeInherited = new System.Windows.Forms.Panel();
            this.tvTree = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.TreeViewEx();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRootInterface = new TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components.IconComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panelIncludeInherited.SuspendLayout();
            this.SuspendLayout();
            // 
            // lineControl1
            // 
            this.lineControl1.Location = new System.Drawing.Point(0, 289);
            this.lineControl1.Size = new System.Drawing.Size(435, 43);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(268, 300);
            this.btnOk.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(349, 300);
            this.btnCancel.TabIndex = 5;
            // 
            // lbMessage
            // 
            this.lbMessage.Location = new System.Drawing.Point(16, 242);
            // 
            // edValue
            // 
            this.edValue.Location = new System.Drawing.Point(19, 258);
            this.edValue.Size = new System.Drawing.Size(391, 20);
            this.edValue.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Implement as Type:";
            // 
            // cmbTypes
            // 
            this.cmbTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTypes.DefaultImage = null;
            this.cmbTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypes.FormattingEnabled = true;
            this.cmbTypes.Location = new System.Drawing.Point(123, 209);
            this.cmbTypes.Name = "cmbTypes";
            this.cmbTypes.Size = new System.Drawing.Size(287, 21);
            this.cmbTypes.TabIndex = 2;
            // 
            // panelIncludeInherited
            // 
            this.panelIncludeInherited.Controls.Add(this.tvTree);
            this.panelIncludeInherited.Controls.Add(this.label4);
            this.panelIncludeInherited.Controls.Add(this.label3);
            this.panelIncludeInherited.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelIncludeInherited.Location = new System.Drawing.Point(0, 0);
            this.panelIncludeInherited.Name = "panelIncludeInherited";
            this.panelIncludeInherited.Size = new System.Drawing.Size(435, 152);
            this.panelIncludeInherited.TabIndex = 0;
            // 
            // tvTree
            // 
            this.tvTree.AutoChangeToIndeterminateState = false;
            this.tvTree.AutoCheckChilds = false;
            this.tvTree.AutoCheckParents = false;
            this.tvTree.HideSelection = false;
            this.tvTree.IndeterminateToChecked = true;
            this.tvTree.Location = new System.Drawing.Point(19, 28);
            this.tvTree.Name = "tvTree";
            this.tvTree.Size = new System.Drawing.Size(391, 100);
            this.tvTree.TabIndex = 0;
            this.tvTree.TriStateCheckBoxesEnabled = true;
            this.tvTree.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTree_BeforeCheck);
            this.tvTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTree_AfterCheck);
            this.tvTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTree_BeforeCollapse);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label4.Location = new System.Drawing.Point(17, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(228, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Select interfaces you want to implement in a new type.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Inheritance Tree:";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "Interface.bmp");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Root Interface:";
            // 
            // cmbRootInterface
            // 
            this.cmbRootInterface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRootInterface.DefaultImage = null;
            this.cmbRootInterface.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbRootInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRootInterface.FormattingEnabled = true;
            this.cmbRootInterface.Location = new System.Drawing.Point(123, 159);
            this.cmbRootInterface.Name = "cmbRootInterface";
            this.cmbRootInterface.Size = new System.Drawing.Size(287, 21);
            this.cmbRootInterface.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label5.Location = new System.Drawing.Point(121, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(262, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "Defines interface from which will be copied common metadata.";
            // 
            // FormImplementInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(435, 332);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbRootInterface);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelIncludeInherited);
            this.Controls.Add(this.cmbTypes);
            this.Controls.Add(this.label2);
            this.Name = "FormImplementInterface";
            this.Controls.SetChildIndex(this.lineControl1, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lbMessage, 0);
            this.Controls.SetChildIndex(this.edValue, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbTypes, 0);
            this.Controls.SetChildIndex(this.panelIncludeInherited, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbRootInterface, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.panelIncludeInherited.ResumeLayout(false);
            this.panelIncludeInherited.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private Components.IconComboBox cmbTypes;
        private System.Windows.Forms.Panel panelIncludeInherited;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private Components.IconComboBox cmbRootInterface;
        private System.Windows.Forms.Label label5;
        private Components.TreeViewEx tvTree;
    }
}
