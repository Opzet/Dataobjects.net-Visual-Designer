namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    partial class FormProductUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProductUpdate));
            this.htmlNewVersion = new System.Windows.Forms.HtmlPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.htmlCurrentVersion = new System.Windows.Forms.HtmlPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chDoNotShow = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.htmlChanges = new System.Windows.Forms.HtmlPanel();
            this.htmlReleaseDate = new System.Windows.Forms.HtmlPanel();
            this.htmlPanel1 = new System.Windows.Forms.HtmlPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htmlNewVersion
            // 
            this.htmlNewVersion.AutoScroll = true;
            this.htmlNewVersion.AutoScrollExplicit = false;
            this.htmlNewVersion.AutoScrollMinSize = new System.Drawing.Size(112, 13);
            this.htmlNewVersion.BackColor = System.Drawing.Color.White;
            this.htmlNewVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlNewVersion.Location = new System.Drawing.Point(50, 85);
            this.htmlNewVersion.Name = "htmlNewVersion";
            this.htmlNewVersion.Size = new System.Drawing.Size(374, 16);
            this.htmlNewVersion.TabIndex = 9;
            this.htmlNewVersion.Text = "<font style=\"font-family:tahoma;font-size:8pt\">A new version <b>1.1.1.1</b> is av" +
                "ailable on the web.<font>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(45, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 14);
            this.label1.TabIndex = 10;
            this.label1.Text = "DataObjects.Net Entity Model Designer update available";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htmlCurrentVersion
            // 
            this.htmlCurrentVersion.AutoScroll = true;
            this.htmlCurrentVersion.AutoScrollExplicit = false;
            this.htmlCurrentVersion.AutoScrollMinSize = new System.Drawing.Size(112, 13);
            this.htmlCurrentVersion.BackColor = System.Drawing.Color.White;
            this.htmlCurrentVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlCurrentVersion.Location = new System.Drawing.Point(50, 60);
            this.htmlCurrentVersion.Name = "htmlCurrentVersion";
            this.htmlCurrentVersion.Size = new System.Drawing.Size(346, 17);
            this.htmlCurrentVersion.TabIndex = 11;
            this.htmlCurrentVersion.Text = "<font style=\"font-family:tahoma;font-size:8pt\">Current version is <b>0.0.0.0</b>," +
                " upgrade is recommended.<font>";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.chDoNotShow);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 335);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(488, 51);
            this.panel1.TabIndex = 12;
            // 
            // chDoNotShow
            // 
            this.chDoNotShow.AutoSize = true;
            this.chDoNotShow.Location = new System.Drawing.Point(12, 17);
            this.chDoNotShow.Name = "chDoNotShow";
            this.chDoNotShow.Size = new System.Drawing.Size(165, 17);
            this.chDoNotShow.TabIndex = 2;
            this.chDoNotShow.Text = "Don\'t show this update again";
            this.chDoNotShow.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(401, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(224, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(171, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Open site with latest version";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // htmlChanges
            // 
            this.htmlChanges.AutoScroll = true;
            this.htmlChanges.AutoScrollExplicit = true;
            this.htmlChanges.AutoScrollMinSize = new System.Drawing.Size(385, 81);
            this.htmlChanges.BackColor = System.Drawing.Color.White;
            this.htmlChanges.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlChanges.Location = new System.Drawing.Point(50, 132);
            this.htmlChanges.Name = "htmlChanges";
            this.htmlChanges.Size = new System.Drawing.Size(385, 175);
            this.htmlChanges.TabIndex = 13;
            this.htmlChanges.Text = resources.GetString("htmlChanges.Text");
            // 
            // htmlReleaseDate
            // 
            this.htmlReleaseDate.AutoScroll = true;
            this.htmlReleaseDate.AutoScrollExplicit = false;
            this.htmlReleaseDate.AutoScrollMinSize = new System.Drawing.Size(112, 13);
            this.htmlReleaseDate.BackColor = System.Drawing.Color.White;
            this.htmlReleaseDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlReleaseDate.Location = new System.Drawing.Point(50, 110);
            this.htmlReleaseDate.Name = "htmlReleaseDate";
            this.htmlReleaseDate.Size = new System.Drawing.Size(374, 16);
            this.htmlReleaseDate.TabIndex = 14;
            this.htmlReleaseDate.Text = "<font style=\"font-family:tahoma;font-size:8pt\">Released at <b>24.12.2011</b> with" +
                " those changes:<font>";
            // 
            // htmlPanel1
            // 
            this.htmlPanel1.AutoScroll = true;
            this.htmlPanel1.AutoScrollExplicit = false;
            this.htmlPanel1.AutoScrollMinSize = new System.Drawing.Size(112, 13);
            this.htmlPanel1.BackColor = System.Drawing.Color.White;
            this.htmlPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.htmlPanel1.Location = new System.Drawing.Point(360, 315);
            this.htmlPanel1.Name = "htmlPanel1";
            this.htmlPanel1.Size = new System.Drawing.Size(116, 13);
            this.htmlPanel1.TabIndex = 15;
            this.htmlPanel1.Text = "<font style=\"font-family:tahoma;font-size:8pt\"><a href=\"http://doemd.codeplex.com" +
                "\" style=\"text-decoration:none\">doemd.codeplex.com</a><font>";
            // 
            // FormProductUpdate
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(488, 386);
            this.Controls.Add(this.htmlPanel1);
            this.Controls.Add(this.htmlReleaseDate);
            this.Controls.Add(this.htmlNewVersion);
            this.Controls.Add(this.htmlChanges);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.htmlCurrentVersion);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProductUpdate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataObjects.Net Entity Model Designer - Update available";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HtmlPanel htmlNewVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HtmlPanel htmlCurrentVersion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chDoNotShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.HtmlPanel htmlChanges;
        private System.Windows.Forms.HtmlPanel htmlReleaseDate;
        private System.Windows.Forms.HtmlPanel htmlPanel1;
    }
}