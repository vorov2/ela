namespace Elide.ElaCode.Configuration
{
    partial class CompilerConfigPage
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
            this.libLabel = new System.Windows.Forms.Label();
            this.libBox = new System.Windows.Forms.TextBox();
            this.optimize = new System.Windows.Forms.CheckBox();
            this.hints = new System.Windows.Forms.CheckBox();
            this.warnAsError = new System.Windows.Forms.CheckBox();
            this.noWarn = new System.Windows.Forms.CheckBox();
            this.debug = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
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
            // libLabel
            // 
            this.libLabel.AutoSize = true;
            this.libLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.libLabel.Location = new System.Drawing.Point(12, 130);
            this.libLabel.Name = "libLabel";
            this.libLabel.Size = new System.Drawing.Size(94, 13);
            this.libLabel.TabIndex = 2;
            this.libLabel.Text = "&Prelude Module:";
            // 
            // libBox
            // 
            this.libBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.libBox.Location = new System.Drawing.Point(15, 146);
            this.libBox.Name = "libBox";
            this.libBox.Size = new System.Drawing.Size(446, 22);
            this.libBox.TabIndex = 3;
            this.libBox.TextChanged += new System.EventHandler(this.libBox_TextChanged);
            // 
            // optimize
            // 
            this.optimize.AutoSize = true;
            this.optimize.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.optimize.Location = new System.Drawing.Point(15, 104);
            this.optimize.Name = "optimize";
            this.optimize.Size = new System.Drawing.Size(167, 17);
            this.optimize.TabIndex = 13;
            this.optimize.Text = "Perform code &optimizations";
            this.optimize.UseVisualStyleBackColor = true;
            this.optimize.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // hints
            // 
            this.hints.AutoSize = true;
            this.hints.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.hints.Location = new System.Drawing.Point(15, 85);
            this.hints.Name = "hints";
            this.hints.Size = new System.Drawing.Size(240, 17);
            this.hints.TabIndex = 12;
            this.hints.Text = "Show &hints on fixing warnings and errors";
            this.hints.UseVisualStyleBackColor = true;
            this.hints.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // warnAsError
            // 
            this.warnAsError.AutoSize = true;
            this.warnAsError.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.warnAsError.Location = new System.Drawing.Point(15, 66);
            this.warnAsError.Name = "warnAsError";
            this.warnAsError.Size = new System.Drawing.Size(171, 17);
            this.warnAsError.TabIndex = 11;
            this.warnAsError.Text = "Generate warnings as &errors";
            this.warnAsError.UseVisualStyleBackColor = true;
            this.warnAsError.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // noWarn
            // 
            this.noWarn.AutoSize = true;
            this.noWarn.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.noWarn.Location = new System.Drawing.Point(15, 47);
            this.noWarn.Name = "noWarn";
            this.noWarn.Size = new System.Drawing.Size(155, 17);
            this.noWarn.TabIndex = 10;
            this.noWarn.Text = "Don\'t generate &warnings";
            this.noWarn.UseVisualStyleBackColor = true;
            this.noWarn.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // debug
            // 
            this.debug.AutoSize = true;
            this.debug.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.debug.Location = new System.Drawing.Point(15, 28);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(225, 17);
            this.debug.TabIndex = 9;
            this.debug.Text = "Generate extended &debug information";
            this.debug.UseVisualStyleBackColor = true;
            this.debug.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label1.Location = new System.Drawing.Point(12, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(449, 32);
            this.label1.TabIndex = 14;
            this.label1.Text = "You can specify an alternate implementation of prelude module. Only the module na" +
                "me is needed. Module will be linked using standard linker settings.";
            // 
            // CompilerConfigPage
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.optimize);
            this.Controls.Add(this.hints);
            this.Controls.Add(this.warnAsError);
            this.Controls.Add(this.noWarn);
            this.Controls.Add(this.debug);
            this.Controls.Add(this.libBox);
            this.Controls.Add(this.libLabel);
            this.Controls.Add(this.optionsLabel);
            this.Name = "CompilerConfigPage";
            this.Size = new System.Drawing.Size(475, 321);
            this.Load += new System.EventHandler(this.CompilerConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label optionsLabel;
        private System.Windows.Forms.Label libLabel;
        private System.Windows.Forms.TextBox libBox;
        private System.Windows.Forms.CheckBox optimize;
        private System.Windows.Forms.CheckBox hints;
        private System.Windows.Forms.CheckBox warnAsError;
        private System.Windows.Forms.CheckBox noWarn;
        private System.Windows.Forms.CheckBox debug;
        private System.Windows.Forms.Label label1;
    }
}
