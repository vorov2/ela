namespace Elide.ElaCode.Configuration
{
    partial class EilGeneratorConfigPage
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
            this.optionsLabel = new System.Windows.Forms.Label();
            this.debug = new System.Windows.Forms.CheckBox();
            this.offsets = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // optionsLabel
            // 
            this.optionsLabel.AutoSize = true;
            this.optionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.optionsLabel.Location = new System.Drawing.Point(12, 10);
            this.optionsLabel.Name = "optionsLabel";
            this.optionsLabel.Size = new System.Drawing.Size(51, 13);
            this.optionsLabel.TabIndex = 0;
            this.optionsLabel.Text = "&Options:";
            // 
            // debug
            // 
            this.debug.AutoSize = true;
            this.debug.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.debug.Location = new System.Drawing.Point(15, 47);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(172, 17);
            this.debug.TabIndex = 15;
            this.debug.Text = "Generate EIL in &debug mode";
            this.debug.UseVisualStyleBackColor = true;
            this.debug.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // offsets
            // 
            this.offsets.AutoSize = true;
            this.offsets.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.offsets.Location = new System.Drawing.Point(15, 28);
            this.offsets.Name = "offsets";
            this.offsets.Size = new System.Drawing.Size(169, 17);
            this.offsets.TabIndex = 14;
            this.offsets.Text = "Print EIL instructions &offsets";
            this.offsets.UseVisualStyleBackColor = true;
            this.offsets.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // EilGeneratorConfigPage
            // 
            this.Controls.Add(this.debug);
            this.Controls.Add(this.offsets);
            this.Controls.Add(this.optionsLabel);
            this.Name = "EilGeneratorConfigPage";
            this.Size = new System.Drawing.Size(473, 367);
            this.Load += new System.EventHandler(this.LinkerConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label optionsLabel;
        private System.Windows.Forms.CheckBox debug;
        private System.Windows.Forms.CheckBox offsets;
    }
}
