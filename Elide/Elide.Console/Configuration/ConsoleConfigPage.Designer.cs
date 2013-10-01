namespace Elide.Console.Configuration
{
    partial class ConsoleConfigPage
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
            this.historyLabel = new System.Windows.Forms.Label();
            this.historyCombo = new System.Windows.Forms.ComboBox();
            this.historyDescLabel = new System.Windows.Forms.Label();
            this.stylingLabel = new System.Windows.Forms.Label();
            this.styling = new System.Windows.Forms.CheckBox();
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
            // historyLabel
            // 
            this.historyLabel.AutoSize = true;
            this.historyLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.historyLabel.Location = new System.Drawing.Point(12, 10);
            this.historyLabel.Name = "historyLabel";
            this.historyLabel.Size = new System.Drawing.Size(47, 13);
            this.historyLabel.TabIndex = 0;
            this.historyLabel.Text = "&History:";
            // 
            // historyCombo
            // 
            this.historyCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.historyCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.historyCombo.FormattingEnabled = true;
            this.historyCombo.Location = new System.Drawing.Point(15, 26);
            this.historyCombo.Name = "historyCombo";
            this.historyCombo.Size = new System.Drawing.Size(80, 21);
            this.historyCombo.TabIndex = 1;
            this.historyCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // historyDescLabel
            // 
            this.historyDescLabel.AutoSize = true;
            this.historyDescLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.historyDescLabel.Location = new System.Drawing.Point(101, 30);
            this.historyDescLabel.Name = "historyDescLabel";
            this.historyDescLabel.Size = new System.Drawing.Size(174, 13);
            this.historyDescLabel.TabIndex = 2;
            this.historyDescLabel.Text = "items are kept in a history buffer";
            // 
            // stylingLabel
            // 
            this.stylingLabel.AutoSize = true;
            this.stylingLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.stylingLabel.Location = new System.Drawing.Point(12, 60);
            this.stylingLabel.Name = "stylingLabel";
            this.stylingLabel.Size = new System.Drawing.Size(90, 13);
            this.stylingLabel.TabIndex = 3;
            this.stylingLabel.Text = "Styling settings:";
            // 
            // styling
            // 
            this.styling.AutoSize = true;
            this.styling.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.styling.Location = new System.Drawing.Point(15, 78);
            this.styling.Name = "styling";
            this.styling.Size = new System.Drawing.Size(185, 17);
            this.styling.TabIndex = 4;
            this.styling.Text = "Enable &highlighting in console";
            this.styling.UseVisualStyleBackColor = true;
            this.styling.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // caretSettingsLabel
            // 
            this.caretSettingsLabel.AutoSize = true;
            this.caretSettingsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.caretSettingsLabel.Location = new System.Drawing.Point(12, 108);
            this.caretSettingsLabel.Name = "caretSettingsLabel";
            this.caretSettingsLabel.Size = new System.Drawing.Size(81, 13);
            this.caretSettingsLabel.TabIndex = 9;
            this.caretSettingsLabel.Text = "Caret settings:";
            // 
            // caretWidthLabel
            // 
            this.caretWidthLabel.AutoSize = true;
            this.caretWidthLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretWidthLabel.Location = new System.Drawing.Point(154, 124);
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
            this.caretWidthCombo.Location = new System.Drawing.Point(157, 140);
            this.caretWidthCombo.Name = "caretWidthCombo";
            this.caretWidthCombo.Size = new System.Drawing.Size(129, 21);
            this.caretWidthCombo.TabIndex = 33;
            this.caretWidthCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // caretStyleLabel
            // 
            this.caretStyleLabel.AutoSize = true;
            this.caretStyleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretStyleLabel.Location = new System.Drawing.Point(12, 124);
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
            this.caretStyleCombo.Location = new System.Drawing.Point(15, 140);
            this.caretStyleCombo.Name = "caretStyleCombo";
            this.caretStyleCombo.Size = new System.Drawing.Size(129, 21);
            this.caretStyleCombo.TabIndex = 31;
            this.caretStyleCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // selStylingLabel
            // 
            this.selStylingLabel.AutoSize = true;
            this.selStylingLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.selStylingLabel.Location = new System.Drawing.Point(12, 174);
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
            this.selTransparencyCombo.Location = new System.Drawing.Point(302, 207);
            this.selTransparencyCombo.Name = "selTransparencyCombo";
            this.selTransparencyCombo.Size = new System.Drawing.Size(128, 21);
            this.selTransparencyCombo.TabIndex = 45;
            this.selTransparencyCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // selTransparencyLabel
            // 
            this.selTransparencyLabel.AutoSize = true;
            this.selTransparencyLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selTransparencyLabel.Location = new System.Drawing.Point(299, 191);
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
            this.selColorCombo.Location = new System.Drawing.Point(159, 207);
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
            this.selColorLabel.Location = new System.Drawing.Point(156, 191);
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
            this.selBackgroundCombo.Location = new System.Drawing.Point(15, 207);
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
            this.selColor.Location = new System.Drawing.Point(15, 234);
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
            this.selBackgroundLabel.Location = new System.Drawing.Point(12, 191);
            this.selBackgroundLabel.Name = "selBackgroundLabel";
            this.selBackgroundLabel.Size = new System.Drawing.Size(123, 13);
            this.selBackgroundLabel.TabIndex = 39;
            this.selBackgroundLabel.Text = "Sele&ction background:";
            // 
            // ConsoleConfigPage
            // 
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
            this.Controls.Add(this.styling);
            this.Controls.Add(this.stylingLabel);
            this.Controls.Add(this.historyDescLabel);
            this.Controls.Add(this.historyCombo);
            this.Controls.Add(this.historyLabel);
            this.Name = "ConsoleConfigPage";
            this.Size = new System.Drawing.Size(492, 334);
            this.Load += new System.EventHandler(this.WorkspaceConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label historyLabel;
        private System.Windows.Forms.ComboBox historyCombo;
        private System.Windows.Forms.Label historyDescLabel;
        private System.Windows.Forms.Label stylingLabel;
        private System.Windows.Forms.CheckBox styling;
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
