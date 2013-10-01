namespace Elide.Workbench.Configuration
{
    partial class WorkbenchConfigPage
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
            this.recentItemsLabel = new System.Windows.Forms.Label();
            this.recentFilesCombo = new System.Windows.Forms.ComboBox();
            this.recentFilesLabel = new System.Windows.Forms.Label();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.fullPath = new System.Windows.Forms.CheckBox();
            this.blank = new System.Windows.Forms.CheckBox();
            this.restore = new System.Windows.Forms.CheckBox();
            this.windowHeader = new System.Windows.Forms.CheckBox();
            this.rememberTools = new System.Windows.Forms.CheckBox();
            this.autosave = new System.Windows.Forms.CheckBox();
            this.autosaveSettingsLabel = new System.Windows.Forms.Label();
            this.autosavePeriodLabel = new System.Windows.Forms.Label();
            this.autosaveCombo = new System.Windows.Forms.ComboBox();
            this.showWelcome = new System.Windows.Forms.CheckBox();
            this.welcomePageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // recentItemsLabel
            // 
            this.recentItemsLabel.AutoSize = true;
            this.recentItemsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.recentItemsLabel.Location = new System.Drawing.Point(12, 10);
            this.recentItemsLabel.Name = "recentItemsLabel";
            this.recentItemsLabel.Size = new System.Drawing.Size(76, 13);
            this.recentItemsLabel.TabIndex = 0;
            this.recentItemsLabel.Text = "&Recent items:";
            // 
            // recentFilesCombo
            // 
            this.recentFilesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recentFilesCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.recentFilesCombo.FormattingEnabled = true;
            this.recentFilesCombo.Location = new System.Drawing.Point(15, 26);
            this.recentFilesCombo.Name = "recentFilesCombo";
            this.recentFilesCombo.Size = new System.Drawing.Size(39, 21);
            this.recentFilesCombo.TabIndex = 1;
            this.recentFilesCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // recentFilesLabel
            // 
            this.recentFilesLabel.AutoSize = true;
            this.recentFilesLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.recentFilesLabel.Location = new System.Drawing.Point(60, 30);
            this.recentFilesLabel.Name = "recentFilesLabel";
            this.recentFilesLabel.Size = new System.Drawing.Size(181, 13);
            this.recentFilesLabel.TabIndex = 2;
            this.recentFilesLabel.Text = "items shown in Recent Files menu";
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.settingsLabel.Location = new System.Drawing.Point(12, 60);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(121, 13);
            this.settingsLabel.TabIndex = 3;
            this.settingsLabel.Text = "Environment settings:";
            // 
            // fullPath
            // 
            this.fullPath.AutoSize = true;
            this.fullPath.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.fullPath.Location = new System.Drawing.Point(15, 78);
            this.fullPath.Name = "fullPath";
            this.fullPath.Size = new System.Drawing.Size(239, 17);
            this.fullPath.TabIndex = 4;
            this.fullPath.Text = "Show &full file path in documents window";
            this.fullPath.UseVisualStyleBackColor = true;
            this.fullPath.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // blank
            // 
            this.blank.AutoSize = true;
            this.blank.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.blank.Location = new System.Drawing.Point(15, 97);
            this.blank.Name = "blank";
            this.blank.Size = new System.Drawing.Size(234, 17);
            this.blank.TabIndex = 5;
            this.blank.Text = "Always start with a new &blank document";
            this.blank.UseVisualStyleBackColor = true;
            this.blank.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // restore
            // 
            this.restore.AutoSize = true;
            this.restore.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.restore.Location = new System.Drawing.Point(15, 116);
            this.restore.Name = "restore";
            this.restore.Size = new System.Drawing.Size(228, 17);
            this.restore.TabIndex = 6;
            this.restore.Text = "&Remember open files between sessions";
            this.restore.UseVisualStyleBackColor = true;
            this.restore.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // windowHeader
            // 
            this.windowHeader.AutoSize = true;
            this.windowHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.windowHeader.Location = new System.Drawing.Point(15, 135);
            this.windowHeader.Name = "windowHeader";
            this.windowHeader.Size = new System.Drawing.Size(275, 17);
            this.windowHeader.TabIndex = 7;
            this.windowHeader.Text = "Show current document in main &window header";
            this.windowHeader.UseVisualStyleBackColor = true;
            this.windowHeader.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // rememberTools
            // 
            this.rememberTools.AutoSize = true;
            this.rememberTools.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rememberTools.Location = new System.Drawing.Point(15, 154);
            this.rememberTools.Name = "rememberTools";
            this.rememberTools.Size = new System.Drawing.Size(233, 17);
            this.rememberTools.TabIndex = 8;
            this.rememberTools.Text = "Remember open t&ools between sessions";
            this.rememberTools.UseVisualStyleBackColor = true;
            this.rememberTools.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // autosave
            // 
            this.autosave.AutoSize = true;
            this.autosave.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.autosave.Location = new System.Drawing.Point(15, 198);
            this.autosave.Name = "autosave";
            this.autosave.Size = new System.Drawing.Size(227, 17);
            this.autosave.TabIndex = 10;
            this.autosave.Text = "Enable &autosave of opened documents";
            this.autosave.UseVisualStyleBackColor = true;
            this.autosave.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // autosaveSettingsLabel
            // 
            this.autosaveSettingsLabel.AutoSize = true;
            this.autosaveSettingsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.autosaveSettingsLabel.Location = new System.Drawing.Point(12, 180);
            this.autosaveSettingsLabel.Name = "autosaveSettingsLabel";
            this.autosaveSettingsLabel.Size = new System.Drawing.Size(103, 13);
            this.autosaveSettingsLabel.TabIndex = 9;
            this.autosaveSettingsLabel.Text = "Autosave settings:";
            // 
            // autosavePeriodLabel
            // 
            this.autosavePeriodLabel.AutoSize = true;
            this.autosavePeriodLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.autosavePeriodLabel.Location = new System.Drawing.Point(12, 218);
            this.autosavePeriodLabel.Name = "autosavePeriodLabel";
            this.autosavePeriodLabel.Size = new System.Drawing.Size(122, 13);
            this.autosavePeriodLabel.TabIndex = 11;
            this.autosavePeriodLabel.Text = "Save documents &every:";
            // 
            // autosaveCombo
            // 
            this.autosaveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autosaveCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.autosaveCombo.FormattingEnabled = true;
            this.autosaveCombo.Location = new System.Drawing.Point(137, 215);
            this.autosaveCombo.Name = "autosaveCombo";
            this.autosaveCombo.Size = new System.Drawing.Size(92, 21);
            this.autosaveCombo.TabIndex = 12;
            this.autosaveCombo.SelectedIndexChanged += new System.EventHandler(this.ControlChanges);
            // 
            // showWelcome
            // 
            this.showWelcome.AutoSize = true;
            this.showWelcome.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.showWelcome.Location = new System.Drawing.Point(15, 266);
            this.showWelcome.Name = "showWelcome";
            this.showWelcome.Size = new System.Drawing.Size(246, 17);
            this.showWelcome.TabIndex = 14;
            this.showWelcome.Text = "Show &welcome page at application startup";
            this.showWelcome.UseVisualStyleBackColor = true;
            this.showWelcome.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // welcomePageLabel
            // 
            this.welcomePageLabel.AutoSize = true;
            this.welcomePageLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.welcomePageLabel.Location = new System.Drawing.Point(12, 248);
            this.welcomePageLabel.Name = "welcomePageLabel";
            this.welcomePageLabel.Size = new System.Drawing.Size(87, 13);
            this.welcomePageLabel.TabIndex = 13;
            this.welcomePageLabel.Text = "Welcome page:";
            // 
            // WorkbenchConfigPage
            // 
            this.Controls.Add(this.showWelcome);
            this.Controls.Add(this.welcomePageLabel);
            this.Controls.Add(this.autosaveCombo);
            this.Controls.Add(this.autosavePeriodLabel);
            this.Controls.Add(this.autosave);
            this.Controls.Add(this.autosaveSettingsLabel);
            this.Controls.Add(this.rememberTools);
            this.Controls.Add(this.windowHeader);
            this.Controls.Add(this.restore);
            this.Controls.Add(this.blank);
            this.Controls.Add(this.fullPath);
            this.Controls.Add(this.settingsLabel);
            this.Controls.Add(this.recentFilesLabel);
            this.Controls.Add(this.recentFilesCombo);
            this.Controls.Add(this.recentItemsLabel);
            this.Name = "WorkbenchConfigPage";
            this.Size = new System.Drawing.Size(404, 329);
            this.Load += new System.EventHandler(this.WorkspaceConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label recentItemsLabel;
        private System.Windows.Forms.ComboBox recentFilesCombo;
        private System.Windows.Forms.Label recentFilesLabel;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.CheckBox fullPath;
        private System.Windows.Forms.CheckBox blank;
        private System.Windows.Forms.CheckBox restore;
        private System.Windows.Forms.CheckBox windowHeader;
        private System.Windows.Forms.CheckBox rememberTools;
        private System.Windows.Forms.CheckBox autosave;
        private System.Windows.Forms.Label autosaveSettingsLabel;
        private System.Windows.Forms.Label autosavePeriodLabel;
        private System.Windows.Forms.ComboBox autosaveCombo;
        private System.Windows.Forms.CheckBox showWelcome;
        private System.Windows.Forms.Label welcomePageLabel;
    }
}
