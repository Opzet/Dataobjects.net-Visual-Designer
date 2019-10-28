using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    partial class FormObjectValueEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.valueUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.valueDate = new System.Windows.Forms.DateTimePicker();
            this.panelDateTime = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lbDate = new System.Windows.Forms.Label();
            this.valueTime = new System.Windows.Forms.DateTimePicker();
            this.btnAction2 = new System.Windows.Forms.Button();
            this.btnAction1 = new System.Windows.Forms.Button();
            this.chUseNullValue = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lbValueHint = new System.Windows.Forms.Label();
            this.valueComboBox = new System.Windows.Forms.ComboBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.cmbValueTypes = new IconComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.valueUpDown)).BeginInit();
            this.panelDateTime.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Value Type:";
            // 
            // valueUpDown
            // 
            this.valueUpDown.Location = new System.Drawing.Point(12, 89);
            this.valueUpDown.Name = "valueUpDown";
            this.valueUpDown.Size = new System.Drawing.Size(406, 20);
            this.valueUpDown.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Value:";
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(12, 115);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(406, 20);
            this.valueTextBox.TabIndex = 6;
            // 
            // valueDate
            // 
            this.valueDate.Location = new System.Drawing.Point(39, 0);
            this.valueDate.Name = "valueDate";
            this.valueDate.ShowCheckBox = true;
            this.valueDate.Size = new System.Drawing.Size(210, 20);
            this.valueDate.TabIndex = 1;
            // 
            // panelDateTime
            // 
            this.panelDateTime.Controls.Add(this.label4);
            this.panelDateTime.Controls.Add(this.lbDate);
            this.panelDateTime.Controls.Add(this.valueTime);
            this.panelDateTime.Controls.Add(this.valueDate);
            this.panelDateTime.Location = new System.Drawing.Point(12, 141);
            this.panelDateTime.Name = "panelDateTime";
            this.panelDateTime.Size = new System.Drawing.Size(406, 23);
            this.panelDateTime.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Time:";
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Location = new System.Drawing.Point(0, 3);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(33, 13);
            this.lbDate.TabIndex = 0;
            this.lbDate.Text = "Date:";
            // 
            // valueTime
            // 
            this.valueTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.valueTime.Location = new System.Drawing.Point(299, 0);
            this.valueTime.Name = "valueTime";
            this.valueTime.ShowCheckBox = true;
            this.valueTime.ShowUpDown = true;
            this.valueTime.Size = new System.Drawing.Size(104, 20);
            this.valueTime.TabIndex = 3;
            // 
            // btnAction2
            // 
            this.btnAction2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAction2.Location = new System.Drawing.Point(343, 170);
            this.btnAction2.Name = "btnAction2";
            this.btnAction2.Size = new System.Drawing.Size(75, 23);
            this.btnAction2.TabIndex = 5;
            this.btnAction2.Text = "Load file...";
            this.btnAction2.UseVisualStyleBackColor = true;
            this.btnAction2.Click += new System.EventHandler(this.btnAction2_Click);
            // 
            // btnAction1
            // 
            this.btnAction1.Location = new System.Drawing.Point(285, 170);
            this.btnAction1.Name = "btnAction1";
            this.btnAction1.Size = new System.Drawing.Size(52, 23);
            this.btnAction1.TabIndex = 4;
            this.btnAction1.Text = "Clear";
            this.btnAction1.UseVisualStyleBackColor = true;
            this.btnAction1.Click += new System.EventHandler(this.btnAction1_Click);
            // 
            // chUseNullValue
            // 
            this.chUseNullValue.AutoSize = true;
            this.chUseNullValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chUseNullValue.Location = new System.Drawing.Point(325, 66);
            this.chUseNullValue.Name = "chUseNullValue";
            this.chUseNullValue.Size = new System.Drawing.Size(93, 17);
            this.chUseNullValue.TabIndex = 2;
            this.chUseNullValue.Text = "Use null value";
            this.chUseNullValue.UseVisualStyleBackColor = true;
            this.chUseNullValue.CheckedChanged += new System.EventHandler(this.chUseNullValue_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 204);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 27);
            this.panel1.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(343, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(262, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lbValueHint
            // 
            this.lbValueHint.AutoSize = true;
            this.lbValueHint.Location = new System.Drawing.Point(12, 118);
            this.lbValueHint.Name = "lbValueHint";
            this.lbValueHint.Size = new System.Drawing.Size(10, 13);
            this.lbValueHint.TabIndex = 12;
            this.lbValueHint.Text = "-";
            // 
            // valueComboBox
            // 
            this.valueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueComboBox.FormattingEnabled = true;
            this.valueComboBox.Location = new System.Drawing.Point(12, 167);
            this.valueComboBox.Name = "valueComboBox";
            this.valueComboBox.Size = new System.Drawing.Size(406, 21);
            this.valueComboBox.TabIndex = 8;
            // 
            // ofd
            // 
            this.ofd.DefaultExt = "*";
            this.ofd.Filter = "All files (*.*)|*.*";
            this.ofd.SupportMultiDottedExtensions = true;
            this.ofd.Title = "Import byte array from file...";
            // 
            // cmbValueTypes
            // 
            this.cmbValueTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbValueTypes.DefaultImage = null;
            this.cmbValueTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbValueTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValueTypes.FormattingEnabled = true;
            this.cmbValueTypes.Location = new System.Drawing.Point(12, 39);
            this.cmbValueTypes.Name = "cmbValueTypes";
            this.cmbValueTypes.Size = new System.Drawing.Size(406, 21);
            this.cmbValueTypes.TabIndex = 13;
            this.cmbValueTypes.SelectedIndexChanged += new System.EventHandler(this.cmbValueTypes_SelectedIndexChanged);
            // 
            // FormObjectValueEditor
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(430, 231);
            this.Controls.Add(this.cmbValueTypes);
            this.Controls.Add(this.valueComboBox);
            this.Controls.Add(this.lbValueHint);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chUseNullValue);
            this.Controls.Add(this.btnAction1);
            this.Controls.Add(this.btnAction2);
            this.Controls.Add(this.panelDateTime);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.valueUpDown);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormObjectValueEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Value Editor";
            ((System.ComponentModel.ISupportInitialize)(this.valueUpDown)).EndInit();
            this.panelDateTime.ResumeLayout(false);
            this.panelDateTime.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown valueUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.DateTimePicker valueDate;
        private System.Windows.Forms.Panel panelDateTime;
        private System.Windows.Forms.DateTimePicker valueTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.Button btnAction2;
        private System.Windows.Forms.Button btnAction1;
        private System.Windows.Forms.CheckBox chUseNullValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lbValueHint;
        private System.Windows.Forms.ComboBox valueComboBox;
        private System.Windows.Forms.OpenFileDialog ofd;
        private Components.IconComboBox cmbValueTypes;
    }
}