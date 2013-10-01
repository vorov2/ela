namespace Elide.Environment
{
    partial class OpenWindowsDialog
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
            this.windowsList = new Elide.Forms.FlexListBox();
            this.windowsListPanel = new Elide.Forms.FlatPanel();
            this.activate = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.accept = new System.Windows.Forms.Button();
            this.windowsListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // windowsList
            // 
            this.windowsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.windowsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.windowsList.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.windowsList.FormattingEnabled = true;
            this.windowsList.ItemHeight = 16;
            this.windowsList.Location = new System.Drawing.Point(2, 2);
            this.windowsList.Name = "windowsList";
            this.windowsList.Size = new System.Drawing.Size(289, 264);
            this.windowsList.TabIndex = 0;
            this.windowsList.SelectedIndexChanged += new System.EventHandler(this.windowsList_SelectedIndexChanged);
            // 
            // windowsListPanel
            // 
            this.windowsListPanel.Controls.Add(this.windowsList);
            this.windowsListPanel.Location = new System.Drawing.Point(12, 12);
            this.windowsListPanel.Name = "windowsListPanel";
            this.windowsListPanel.Padding = new System.Windows.Forms.Padding(2);
            this.windowsListPanel.Size = new System.Drawing.Size(293, 268);
            this.windowsListPanel.TabIndex = 1;
            this.windowsListPanel.WideRendering = false;
            // 
            // activate
            // 
            this.activate.Enabled = false;
            this.activate.Location = new System.Drawing.Point(311, 12);
            this.activate.Name = "activate";
            this.activate.Size = new System.Drawing.Size(90, 23);
            this.activate.TabIndex = 2;
            this.activate.Text = "&Activate";
            this.activate.UseVisualStyleBackColor = true;
            this.activate.Click += new System.EventHandler(this.activate_Click);
            // 
            // save
            // 
            this.save.Enabled = false;
            this.save.Location = new System.Drawing.Point(311, 41);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(90, 23);
            this.save.TabIndex = 3;
            this.save.Text = "&Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // close
            // 
            this.close.Enabled = false;
            this.close.Location = new System.Drawing.Point(311, 70);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(90, 23);
            this.close.TabIndex = 4;
            this.close.Text = "&Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // accept
            // 
            this.accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.accept.Location = new System.Drawing.Point(311, 257);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(90, 23);
            this.accept.TabIndex = 5;
            this.accept.Text = "OK";
            this.accept.UseVisualStyleBackColor = true;
            // 
            // OpenWindowsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.accept;
            this.ClientSize = new System.Drawing.Size(414, 293);
            this.Controls.Add(this.accept);
            this.Controls.Add(this.close);
            this.Controls.Add(this.save);
            this.Controls.Add(this.activate);
            this.Controls.Add(this.windowsListPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenWindowsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windows";
            this.Load += new System.EventHandler(this.OpenWindowsDialog_Load);
            this.windowsListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Elide.Forms.FlexListBox windowsList;
        private Elide.Forms.FlatPanel windowsListPanel;
        private System.Windows.Forms.Button activate;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button accept;
    }
}