namespace Elide.Configuration
{
    partial class ConfigDialog
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
            this.accept = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.configListPanel = new Elide.Forms.FlatPanel();
            this.configList = new Elide.Forms.GroupListBox();
            this.panel = new Elide.Forms.FlatPanel();
            this.configListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // accept
            // 
            this.accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.accept.Location = new System.Drawing.Point(521, 388);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 2;
            this.accept.Text = "OK";
            this.accept.UseVisualStyleBackColor = true;
            this.accept.Click += new System.EventHandler(this.accept_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(602, 388);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // configListPanel
            // 
            this.configListPanel.Controls.Add(this.configList);
            this.configListPanel.Location = new System.Drawing.Point(12, 10);
            this.configListPanel.Name = "configListPanel";
            this.configListPanel.Padding = new System.Windows.Forms.Padding(2);
            this.configListPanel.Size = new System.Drawing.Size(168, 362);
            this.configListPanel.TabIndex = 4;
            this.configListPanel.WideRendering = false;
            // 
            // configList
            // 
            this.configList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.configList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.configList.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.configList.FormattingEnabled = true;
            this.configList.IntegralHeight = false;
            this.configList.ItemHeight = 18;
            this.configList.Location = new System.Drawing.Point(2, 2);
            this.configList.Name = "configList";
            this.configList.Size = new System.Drawing.Size(164, 358);
            this.configList.TabIndex = 0;
            this.configList.SelectedIndexChanged += new System.EventHandler(this.configList_SelectedIndexChanged);
            // 
            // panel
            // 
            this.panel.Location = new System.Drawing.Point(199, 10);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(2);
            this.panel.Size = new System.Drawing.Size(478, 362);
            this.panel.TabIndex = 1;
            this.panel.WideRendering = false;
            // 
            // ConfigDialog
            // 
            this.AcceptButton = this.accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(691, 422);
            this.Controls.Add(this.configListPanel);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.accept);
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.ConfigDialog_Load);
            this.configListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elide.Forms.GroupListBox configList;
        private Elide.Forms.FlatPanel panel;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
        private Elide.Forms.FlatPanel configListPanel;
    }
}