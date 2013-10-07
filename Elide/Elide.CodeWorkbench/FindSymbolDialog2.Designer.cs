namespace Elide.CodeWorkbench
{
    partial class FindSymbolDialog
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
            this.allFiles = new System.Windows.Forms.CheckBox();
            this.cancel = new System.Windows.Forms.Button();
            this.accept = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // allFiles
            // 
            this.allFiles.AutoSize = true;
            this.allFiles.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.allFiles.Location = new System.Drawing.Point(12, 57);
            this.allFiles.Name = "allFiles";
            this.allFiles.Size = new System.Drawing.Size(129, 17);
            this.allFiles.TabIndex = 8;
            this.allFiles.Text = "Search all &open files";
            this.allFiles.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cancel.Location = new System.Drawing.Point(272, 85);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // accept
            // 
            this.accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.accept.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.accept.Location = new System.Drawing.Point(191, 85);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 9;
            this.accept.Text = "OK";
            this.accept.UseVisualStyleBackColor = true;
            // 
            // textBox
            // 
            this.textBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.textBox.Location = new System.Drawing.Point(12, 29);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(335, 22);
            this.textBox.TabIndex = 7;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label.Location = new System.Drawing.Point(9, 13);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(47, 13);
            this.label.TabIndex = 6;
            this.label.Text = "&Symbol:";
            // 
            // FindSymbolDialog2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 122);
            this.Controls.Add(this.allFiles);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.accept);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindSymbolDialog2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Symbol";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox allFiles;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label;
    }
}