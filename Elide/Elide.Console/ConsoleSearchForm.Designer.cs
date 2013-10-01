namespace Elide.Console
{
    partial class ConsoleSearchForm
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
            this.textLabel = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.caseSensitive = new System.Windows.Forms.CheckBox();
            this.search = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textLabel
            // 
            this.textLabel.AutoSize = true;
            this.textLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.textLabel.Location = new System.Drawing.Point(12, 9);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(68, 13);
            this.textLabel.TabIndex = 0;
            this.textLabel.Text = "&Text to find:";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(15, 25);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(276, 20);
            this.textBox.TabIndex = 1;
            // 
            // caseSensitive
            // 
            this.caseSensitive.AutoSize = true;
            this.caseSensitive.Location = new System.Drawing.Point(15, 51);
            this.caseSensitive.Name = "caseSensitive";
            this.caseSensitive.Size = new System.Drawing.Size(96, 17);
            this.caseSensitive.TabIndex = 2;
            this.caseSensitive.Text = "&Case Sensitive";
            this.caseSensitive.UseVisualStyleBackColor = true;
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(125, 76);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(80, 23);
            this.search.TabIndex = 3;
            this.search.Text = "&Search Next";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // close
            // 
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(211, 76);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(80, 23);
            this.close.TabIndex = 4;
            this.close.Text = "&Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // ConsoleSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(306, 111);
            this.Controls.Add(this.close);
            this.Controls.Add(this.search);
            this.Controls.Add(this.caseSensitive);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.textLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsoleSearchForm";
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label textLabel;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.CheckBox caseSensitive;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.Button close;
    }
}