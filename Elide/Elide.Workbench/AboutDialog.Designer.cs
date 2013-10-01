namespace Elide.Workbench
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.close = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.infoBox = new System.Windows.Forms.RichTextBox();
            this.changeLogPage = new System.Windows.Forms.TabPage();
            this.changeLog = new System.Windows.Forms.RichTextBox();
            this.header = new System.Windows.Forms.Label();
            this.progInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.tabControl.SuspendLayout();
            this.generalPage.SuspendLayout();
            this.changeLogPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(256, 256);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // close
            // 
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.close.Location = new System.Drawing.Point(610, 274);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.generalPage);
            this.tabControl.Controls.Add(this.changeLogPage);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.tabControl.Location = new System.Drawing.Point(296, 76);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(389, 188);
            this.tabControl.TabIndex = 2;
            // 
            // generalPage
            // 
            this.generalPage.Controls.Add(this.infoBox);
            this.generalPage.Location = new System.Drawing.Point(4, 22);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalPage.Size = new System.Drawing.Size(381, 162);
            this.generalPage.TabIndex = 0;
            this.generalPage.Text = "General";
            this.generalPage.UseVisualStyleBackColor = true;
            // 
            // infoBox
            // 
            this.infoBox.BackColor = System.Drawing.SystemColors.Window;
            this.infoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.infoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoBox.Location = new System.Drawing.Point(3, 3);
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.infoBox.ShowSelectionMargin = true;
            this.infoBox.Size = new System.Drawing.Size(375, 156);
            this.infoBox.TabIndex = 1;
            this.infoBox.Text = "";
            this.infoBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.infoBox_LinkClicked);
            // 
            // changeLogPage
            // 
            this.changeLogPage.Controls.Add(this.changeLog);
            this.changeLogPage.Location = new System.Drawing.Point(4, 22);
            this.changeLogPage.Name = "changeLogPage";
            this.changeLogPage.Padding = new System.Windows.Forms.Padding(3);
            this.changeLogPage.Size = new System.Drawing.Size(381, 162);
            this.changeLogPage.TabIndex = 1;
            this.changeLogPage.Text = "Change Log";
            this.changeLogPage.UseVisualStyleBackColor = true;
            // 
            // changeLog
            // 
            this.changeLog.BackColor = System.Drawing.SystemColors.Window;
            this.changeLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.changeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.changeLog.Location = new System.Drawing.Point(3, 3);
            this.changeLog.Name = "changeLog";
            this.changeLog.ReadOnly = true;
            this.changeLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.changeLog.ShowSelectionMargin = true;
            this.changeLog.Size = new System.Drawing.Size(375, 156);
            this.changeLog.TabIndex = 1;
            this.changeLog.Text = "";
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.header.Location = new System.Drawing.Point(291, 9);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(42, 30);
            this.header.TabIndex = 3;
            this.header.Text = "{0}";
            // 
            // progInfo
            // 
            this.progInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.progInfo.Location = new System.Drawing.Point(295, 39);
            this.progInfo.Name = "progInfo";
            this.progInfo.Size = new System.Drawing.Size(386, 34);
            this.progInfo.TabIndex = 4;
            this.progInfo.Text = "{0} (pronounced ɪˈlaɪd). Version {1} ({2}). {3}";
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(701, 307);
            this.Controls.Add(this.progInfo);
            this.Controls.Add(this.header);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.close);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Elide...";
            this.Load += new System.EventHandler(this.AboutDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.generalPage.ResumeLayout(false);
            this.changeLogPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage generalPage;
        private System.Windows.Forms.TabPage changeLogPage;
        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Label progInfo;
        private System.Windows.Forms.RichTextBox infoBox;
        private System.Windows.Forms.RichTextBox changeLog;
    }
}