namespace Elide.PlainText.Configuration
{
    partial class PlainTextConfigPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hyperlinks = new System.Windows.Forms.CheckBox();
            this.singleClickNavigation = new System.Windows.Forms.CheckBox();
            this.boldItalics = new System.Windows.Forms.CheckBox();
            this.label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hyperlinks
            // 
            this.hyperlinks.AutoSize = true;
            this.hyperlinks.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.hyperlinks.Location = new System.Drawing.Point(15, 27);
            this.hyperlinks.Name = "hyperlinks";
            this.hyperlinks.Size = new System.Drawing.Size(131, 17);
            this.hyperlinks.TabIndex = 0;
            this.hyperlinks.Text = "Highlight &hyperlinks";
            this.hyperlinks.UseVisualStyleBackColor = true;
            this.hyperlinks.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // singleClickNavigation
            // 
            this.singleClickNavigation.AutoSize = true;
            this.singleClickNavigation.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.singleClickNavigation.Location = new System.Drawing.Point(15, 46);
            this.singleClickNavigation.Name = "singleClickNavigation";
            this.singleClickNavigation.Size = new System.Drawing.Size(201, 17);
            this.singleClickNavigation.TabIndex = 1;
            this.singleClickNavigation.Text = "Enable &single click URL navigation";
            this.singleClickNavigation.UseVisualStyleBackColor = true;
            this.singleClickNavigation.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // boldItalics
            // 
            this.boldItalics.AutoSize = true;
            this.boldItalics.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.boldItalics.Location = new System.Drawing.Point(15, 65);
            this.boldItalics.Name = "boldItalics";
            this.boldItalics.Size = new System.Drawing.Size(194, 17);
            this.boldItalics.TabIndex = 2;
            this.boldItalics.Text = "H&ighlight *bold* and _italic_ text";
            this.boldItalics.UseVisualStyleBackColor = true;
            this.boldItalics.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label.Location = new System.Drawing.Point(12, 10);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(83, 13);
            this.label.TabIndex = 3;
            this.label.Text = "Styler settings:";
            // 
            // PlainTextConfigPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label);
            this.Controls.Add(this.boldItalics);
            this.Controls.Add(this.singleClickNavigation);
            this.Controls.Add(this.hyperlinks);
            this.Name = "PlainTextConfigPage";
            this.Size = new System.Drawing.Size(418, 352);
            this.Load += new System.EventHandler(this.PlainTextConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox hyperlinks;
        private System.Windows.Forms.CheckBox singleClickNavigation;
        private System.Windows.Forms.CheckBox boldItalics;
        private System.Windows.Forms.Label label;
    }
}
