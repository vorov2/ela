namespace Elide.ElaCode.Configuration
{
    partial class InteractiveConfigPage
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
            this.caretSettingsLabel = new System.Windows.Forms.Label();
            this.caretWidthLabel = new System.Windows.Forms.Label();
            this.caretWidthCombo = new System.Windows.Forms.ComboBox();
            this.caretStyleLabel = new System.Windows.Forms.Label();
            this.caretStyleCombo = new System.Windows.Forms.ComboBox();
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
            // caretSettingsLabel
            // 
            this.caretSettingsLabel.AutoSize = true;
            this.caretSettingsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.caretSettingsLabel.Location = new System.Drawing.Point(12, 10);
            this.caretSettingsLabel.Name = "caretSettingsLabel";
            this.caretSettingsLabel.Size = new System.Drawing.Size(81, 13);
            this.caretSettingsLabel.TabIndex = 9;
            this.caretSettingsLabel.Text = "Caret settings:";
            // 
            // caretWidthLabel
            // 
            this.caretWidthLabel.AutoSize = true;
            this.caretWidthLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretWidthLabel.Location = new System.Drawing.Point(154, 26);
            this.caretWidthLabel.Name = "caretWidthLabel";
            this.caretWidthLabel.Size = new System.Drawing.Size(70, 13);
            this.caretWidthLabel.TabIndex = 34;
            this.caretWidthLabel.Text = "Caret &width:";
            // 
            // caretWidthCombo
            // 
            this.caretWidthCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.caretWidthCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretWidthCombo.FormattingEnabled = true;
            this.caretWidthCombo.Location = new System.Drawing.Point(157, 42);
            this.caretWidthCombo.Name = "caretWidthCombo";
            this.caretWidthCombo.Size = new System.Drawing.Size(129, 21);
            this.caretWidthCombo.TabIndex = 33;
            this.caretWidthCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // caretStyleLabel
            // 
            this.caretStyleLabel.AutoSize = true;
            this.caretStyleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretStyleLabel.Location = new System.Drawing.Point(12, 26);
            this.caretStyleLabel.Name = "caretStyleLabel";
            this.caretStyleLabel.Size = new System.Drawing.Size(63, 13);
            this.caretStyleLabel.TabIndex = 32;
            this.caretStyleLabel.Text = "Caret &style:";
            // 
            // caretStyleCombo
            // 
            this.caretStyleCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.caretStyleCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretStyleCombo.FormattingEnabled = true;
            this.caretStyleCombo.Location = new System.Drawing.Point(15, 42);
            this.caretStyleCombo.Name = "caretStyleCombo";
            this.caretStyleCombo.Size = new System.Drawing.Size(129, 21);
            this.caretStyleCombo.TabIndex = 31;
            this.caretStyleCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // selStylingLabel
            // 
            this.selStylingLabel.AutoSize = true;
            this.selStylingLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.selStylingLabel.Location = new System.Drawing.Point(12, 77);
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
            this.selTransparencyCombo.Location = new System.Drawing.Point(302, 110);
            this.selTransparencyCombo.Name = "selTransparencyCombo";
            this.selTransparencyCombo.Size = new System.Drawing.Size(128, 21);
            this.selTransparencyCombo.TabIndex = 45;
            this.selTransparencyCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // selTransparencyLabel
            // 
            this.selTransparencyLabel.AutoSize = true;
            this.selTransparencyLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selTransparencyLabel.Location = new System.Drawing.Point(299, 94);
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
            this.selColorCombo.Location = new System.Drawing.Point(159, 110);
            this.selColorCombo.MinimumSize = new System.Drawing.Size(10, 0);
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
            this.selColorLabel.Location = new System.Drawing.Point(156, 94);
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
            this.selBackgroundCombo.Location = new System.Drawing.Point(15, 110);
            this.selBackgroundCombo.MinimumSize = new System.Drawing.Size(10, 0);
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
            this.selColor.Location = new System.Drawing.Point(15, 137);
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
            this.selBackgroundLabel.Location = new System.Drawing.Point(12, 94);
            this.selBackgroundLabel.Name = "selBackgroundLabel";
            this.selBackgroundLabel.Size = new System.Drawing.Size(123, 13);
            this.selBackgroundLabel.TabIndex = 39;
            this.selBackgroundLabel.Text = "Sele&ction background:";
            // 
            // InteractiveConfigPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selStylingLabel);
            this.Controls.Add(this.selTransparencyCombo);
            this.Controls.Add(this.selTransparencyLabel);
            this.Controls.Add(this.selColorCombo);
            this.Controls.Add(this.selColorLabel);
            this.Controls.Add(this.selBackgroundCombo);
            this.Controls.Add(this.selColor);
            this.Controls.Add(this.selBackgroundLabel);
            this.Controls.Add(this.caretWidthLabel);
            this.Controls.Add(this.caretWidthCombo);
            this.Controls.Add(this.caretStyleLabel);
            this.Controls.Add(this.caretStyleCombo);
            this.Controls.Add(this.caretSettingsLabel);
            this.Name = "InteractiveConfigPage";
            this.Size = new System.Drawing.Size(492, 334);
            this.Load += new System.EventHandler(this.WorkspaceConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label caretSettingsLabel;
        private System.Windows.Forms.Label caretWidthLabel;
        private System.Windows.Forms.ComboBox caretWidthCombo;
        private System.Windows.Forms.Label caretStyleLabel;
        private System.Windows.Forms.ComboBox caretStyleCombo;
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
