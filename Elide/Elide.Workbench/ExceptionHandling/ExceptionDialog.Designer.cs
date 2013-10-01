namespace Elide.Workbench.ExceptionHandling
{
    partial class ExceptionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionDialog));
            this.headerPanel = new System.Windows.Forms.Panel();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.headerLabel = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.messageTextLabel = new System.Windows.Forms.Label();
            this.detailsLabel = new System.Windows.Forms.Label();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.emailCheck = new System.Windows.Forms.CheckBox();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.iconBox);
            this.headerPanel.Controls.Add(this.headerLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(458, 45);
            this.headerPanel.TabIndex = 0;
            // 
            // iconBox
            // 
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            this.iconBox.Location = new System.Drawing.Point(8, 7);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(35, 32);
            this.iconBox.TabIndex = 7;
            this.iconBox.TabStop = false;
            // 
            // headerLabel
            // 
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.headerLabel.Location = new System.Drawing.Point(44, 9);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(402, 32);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Elide has encountered a problem and needs to close. Sorry for the inconvinience.";
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.messageLabel.Location = new System.Drawing.Point(12, 57);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(110, 13);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "Exception Message:";
            // 
            // messageTextLabel
            // 
            this.messageTextLabel.AutoEllipsis = true;
            this.messageTextLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.messageTextLabel.Location = new System.Drawing.Point(12, 75);
            this.messageTextLabel.Name = "messageTextLabel";
            this.messageTextLabel.Size = new System.Drawing.Size(434, 16);
            this.messageTextLabel.TabIndex = 2;
            this.messageTextLabel.Text = "[Text]";
            // 
            // detailsLabel
            // 
            this.detailsLabel.AutoSize = true;
            this.detailsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.detailsLabel.Location = new System.Drawing.Point(12, 105);
            this.detailsLabel.Name = "detailsLabel";
            this.detailsLabel.Size = new System.Drawing.Size(99, 13);
            this.detailsLabel.TabIndex = 3;
            this.detailsLabel.Text = "Exception &Details:";
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.detailsTextBox.Location = new System.Drawing.Point(12, 125);
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detailsTextBox.Size = new System.Drawing.Size(434, 52);
            this.detailsTextBox.TabIndex = 4;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.closeButton.Location = new System.Drawing.Point(371, 196);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // emailCheck
            // 
            this.emailCheck.AutoSize = true;
            this.emailCheck.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.emailCheck.Location = new System.Drawing.Point(12, 200);
            this.emailCheck.Name = "emailCheck";
            this.emailCheck.Size = new System.Drawing.Size(300, 17);
            this.emailCheck.TabIndex = 6;
            this.emailCheck.Text = "Send an email with error dump to program developer";
            this.emailCheck.UseVisualStyleBackColor = true;
            this.emailCheck.Visible = false;
            // 
            // ExceptionDialog
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(458, 228);
            this.Controls.Add(this.emailCheck);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.detailsTextBox);
            this.Controls.Add(this.detailsLabel);
            this.Controls.Add(this.messageTextLabel);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unhandled Exception";
            this.Load += new System.EventHandler(this.ExceptionDialog_Load);
            this.headerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label messageTextLabel;
        private System.Windows.Forms.Label detailsLabel;
        private System.Windows.Forms.TextBox detailsTextBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.CheckBox emailCheck;
        private System.Windows.Forms.PictureBox iconBox;
    }
}