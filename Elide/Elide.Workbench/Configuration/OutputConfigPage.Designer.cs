namespace Elide.Workbench.Configuration
{
    partial class OutputConfigPage
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
            this.stylingLabel = new System.Windows.Forms.Label();
            this.styling = new System.Windows.Forms.CheckBox();
            this.selStylingLabel = new System.Windows.Forms.Label();
            this.selTransparencyCombo = new System.Windows.Forms.ComboBox();
            this.selTransparencyLabel = new System.Windows.Forms.Label();
            this.selColorCombo = new Elide.Forms.ColorPicker();
            this.selColorLabel = new System.Windows.Forms.Label();
            this.selBackgroundCombo = new Elide.Forms.ColorPicker();
            this.selColor = new System.Windows.Forms.CheckBox();
            this.selBackgroundLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // stylingLabel
            // 
            this.stylingLabel.AutoSize = true;
            this.stylingLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.stylingLabel.Location = new System.Drawing.Point(12, 10);
            this.stylingLabel.Name = "stylingLabel";
            this.stylingLabel.Size = new System.Drawing.Size(90, 13);
            this.stylingLabel.TabIndex = 3;
            this.stylingLabel.Text = "Styling settings:";
            // 
            // styling
            // 
            this.styling.AutoSize = true;
            this.styling.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.styling.Location = new System.Drawing.Point(15, 28);
            this.styling.Name = "styling";
            this.styling.Size = new System.Drawing.Size(181, 17);
            this.styling.TabIndex = 4;
            this.styling.Text = "Enable &highlighting in output";
            this.styling.UseVisualStyleBackColor = true;
            this.styling.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // selStylingLabel
            // 
            this.selStylingLabel.AutoSize = true;
            this.selStylingLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.selStylingLabel.Location = new System.Drawing.Point(12, 56);
            this.selStylingLabel.Name = "selStylingLabel";
            this.selStylingLabel.Size = new System.Drawing.Size(101, 13);
            this.selStylingLabel.TabIndex = 46;
            this.selStylingLabel.Text = "Selection settings:";
            // 
            // selTransparencyCombo
            // 
            this.selTransparencyCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selTransparencyCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selTransparencyCombo.FormattingEnabled = true;
            this.selTransparencyCombo.ItemHeight = 13;
            this.selTransparencyCombo.Location = new System.Drawing.Point(302, 89);
            this.selTransparencyCombo.Name = "selTransparencyCombo";
            this.selTransparencyCombo.Size = new System.Drawing.Size(128, 21);
            this.selTransparencyCombo.TabIndex = 45;
            this.selTransparencyCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // selTransparencyLabel
            // 
            this.selTransparencyLabel.AutoSize = true;
            this.selTransparencyLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selTransparencyLabel.Location = new System.Drawing.Point(299, 73);
            this.selTransparencyLabel.Name = "selTransparencyLabel";
            this.selTransparencyLabel.Size = new System.Drawing.Size(126, 13);
            this.selTransparencyLabel.TabIndex = 44;
            this.selTransparencyLabel.Text = "Selec&tion transparency:";
            // 
            // selColorCombo
            // 
            this.selColorCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.selColorCombo.DropDownHeight = 320;
            this.selColorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selColorCombo.Enabled = false;
            this.selColorCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selColorCombo.FormattingEnabled = true;
            this.selColorCombo.IntegralHeight = false;
            this.selColorCombo.ItemHeight = 15;
            this.selColorCombo.Location = new System.Drawing.Point(159, 89);
            this.selColorCombo.Name = "selColorCombo";
            this.selColorCombo.SelectedColor = null;
            this.selColorCombo.Size = new System.Drawing.Size(129, 21);
            this.selColorCombo.TabIndex = 43;
            this.selColorCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // selColorLabel
            // 
            this.selColorLabel.AutoSize = true;
            this.selColorLabel.Enabled = false;
            this.selColorLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selColorLabel.Location = new System.Drawing.Point(156, 73);
            this.selColorLabel.Name = "selColorLabel";
            this.selColorLabel.Size = new System.Drawing.Size(86, 13);
            this.selColorLabel.TabIndex = 41;
            this.selColorLabel.Text = "Selection c&olor:";
            // 
            // selBackgroundCombo
            // 
            this.selBackgroundCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.selBackgroundCombo.DropDownHeight = 320;
            this.selBackgroundCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selBackgroundCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selBackgroundCombo.FormattingEnabled = true;
            this.selBackgroundCombo.IntegralHeight = false;
            this.selBackgroundCombo.ItemHeight = 15;
            this.selBackgroundCombo.Location = new System.Drawing.Point(15, 89);
            this.selBackgroundCombo.Name = "selBackgroundCombo";
            this.selBackgroundCombo.SelectedColor = null;
            this.selBackgroundCombo.Size = new System.Drawing.Size(129, 21);
            this.selBackgroundCombo.TabIndex = 40;
            this.selBackgroundCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // selColor
            // 
            this.selColor.AutoSize = true;
            this.selColor.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selColor.Location = new System.Drawing.Point(15, 116);
            this.selColor.Name = "selColor";
            this.selColor.Size = new System.Drawing.Size(211, 17);
            this.selColor.TabIndex = 42;
            this.selColor.Text = "&Use different text color in selections";
            this.selColor.UseVisualStyleBackColor = true;
            this.selColor.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // selBackgroundLabel
            // 
            this.selBackgroundLabel.AutoSize = true;
            this.selBackgroundLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selBackgroundLabel.Location = new System.Drawing.Point(12, 73);
            this.selBackgroundLabel.Name = "selBackgroundLabel";
            this.selBackgroundLabel.Size = new System.Drawing.Size(123, 13);
            this.selBackgroundLabel.TabIndex = 39;
            this.selBackgroundLabel.Text = "Sele&ction background:";
            // 
            // OutputConfigPage
            // 
            this.Controls.Add(this.selStylingLabel);
            this.Controls.Add(this.selTransparencyCombo);
            this.Controls.Add(this.selTransparencyLabel);
            this.Controls.Add(this.selColorCombo);
            this.Controls.Add(this.selColorLabel);
            this.Controls.Add(this.selBackgroundCombo);
            this.Controls.Add(this.selColor);
            this.Controls.Add(this.selBackgroundLabel);
            this.Controls.Add(this.styling);
            this.Controls.Add(this.stylingLabel);
            this.Name = "OutputConfigPage";
            this.Size = new System.Drawing.Size(492, 334);
            this.Load += new System.EventHandler(this.WorkspaceConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label stylingLabel;
        private System.Windows.Forms.CheckBox styling;
        private System.Windows.Forms.Label selStylingLabel;
        private System.Windows.Forms.ComboBox selTransparencyCombo;
        private System.Windows.Forms.Label selTransparencyLabel;
        private Forms.ColorPicker selColorCombo;
        private System.Windows.Forms.Label selColorLabel;
        private Forms.ColorPicker selBackgroundCombo;
        private System.Windows.Forms.CheckBox selColor;
        private System.Windows.Forms.Label selBackgroundLabel;
    }
}
