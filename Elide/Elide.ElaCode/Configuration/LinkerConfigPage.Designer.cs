namespace Elide.ElaCode.Configuration
{
    partial class LinkerConfigPage
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
            this.components = new System.ComponentModel.Container();
            this.optionsLabel = new System.Windows.Forms.Label();
            this.dirLabel = new System.Windows.Forms.Label();
            this.dirList = new System.Windows.Forms.ListBox();
            this.dirInputText = new System.Windows.Forms.TextBox();
            this.add = new System.Windows.Forms.Button();
            this.folder = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.macroLabel = new System.Windows.Forms.Label();
            this.lookup = new System.Windows.Forms.CheckBox();
            this.skipCheck = new System.Windows.Forms.CheckBox();
            this.warnAsError = new System.Windows.Forms.CheckBox();
            this.nowarn = new System.Windows.Forms.CheckBox();
            this.recompile = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            // dirLabel
            // 
            this.dirLabel.AutoSize = true;
            this.dirLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.dirLabel.Location = new System.Drawing.Point(12, 128);
            this.dirLabel.Name = "dirLabel";
            this.dirLabel.Size = new System.Drawing.Size(152, 13);
            this.dirLabel.TabIndex = 4;
            this.dirLabel.Text = "Module &Lookup Directories:";
            // 
            // dirList
            // 
            this.dirList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dirList.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.dirList.FormattingEnabled = true;
            this.dirList.IntegralHeight = false;
            this.dirList.Location = new System.Drawing.Point(15, 144);
            this.dirList.Name = "dirList";
            this.dirList.Size = new System.Drawing.Size(446, 91);
            this.dirList.TabIndex = 5;
            this.dirList.SelectedIndexChanged += new System.EventHandler(this.dirList_SelectedIndexChanged);
            // 
            // dirInputText
            // 
            this.dirInputText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.dirInputText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.dirInputText.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.errorProvider.SetIconPadding(this.dirInputText, -20);
            this.dirInputText.Location = new System.Drawing.Point(144, 243);
            this.dirInputText.Margin = new System.Windows.Forms.Padding(0);
            this.dirInputText.Name = "dirInputText";
            this.dirInputText.Size = new System.Drawing.Size(286, 22);
            this.dirInputText.TabIndex = 6;
            this.dirInputText.WordWrap = false;
            this.dirInputText.TextChanged += new System.EventHandler(this.dirInputText_TextChanged);
            // 
            // add
            // 
            this.add.Enabled = false;
            this.add.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.add.Location = new System.Drawing.Point(78, 242);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(60, 24);
            this.add.TabIndex = 7;
            this.add.Text = "&Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // folder
            // 
            this.folder.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.folder.Location = new System.Drawing.Point(436, 242);
            this.folder.Name = "folder";
            this.folder.Size = new System.Drawing.Size(25, 24);
            this.folder.TabIndex = 8;
            this.folder.Text = "...";
            this.folder.UseVisualStyleBackColor = true;
            this.folder.Click += new System.EventHandler(this.folder_Click);
            // 
            // remove
            // 
            this.remove.Enabled = false;
            this.remove.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.remove.Location = new System.Drawing.Point(15, 242);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(60, 24);
            this.remove.TabIndex = 9;
            this.remove.Text = "&Remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // macroLabel
            // 
            this.macroLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.macroLabel.Location = new System.Drawing.Point(15, 270);
            this.macroLabel.Name = "macroLabel";
            this.macroLabel.Size = new System.Drawing.Size(446, 30);
            this.macroLabel.TabIndex = 10;
            this.macroLabel.Text = "You can use the following macros: %elide% (Elide install directory), %ela% (Ela i" +
                "nstall directory), %root% (Ela Platform install directory).";
            // 
            // lookup
            // 
            this.lookup.AutoSize = true;
            this.lookup.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lookup.Location = new System.Drawing.Point(15, 104);
            this.lookup.Name = "lookup";
            this.lookup.Size = new System.Drawing.Size(271, 17);
            this.lookup.TabIndex = 18;
            this.lookup.Text = "&Lookup referenced modules in startup directory";
            this.lookup.UseVisualStyleBackColor = true;
            this.lookup.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // skipCheck
            // 
            this.skipCheck.AutoSize = true;
            this.skipCheck.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.skipCheck.Location = new System.Drawing.Point(15, 85);
            this.skipCheck.Name = "skipCheck";
            this.skipCheck.Size = new System.Drawing.Size(216, 17);
            this.skipCheck.TabIndex = 17;
            this.skipCheck.Text = "&Skip time stamp check for object files";
            this.skipCheck.UseVisualStyleBackColor = true;
            this.skipCheck.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // warnAsError
            // 
            this.warnAsError.AutoSize = true;
            this.warnAsError.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.warnAsError.Location = new System.Drawing.Point(15, 66);
            this.warnAsError.Name = "warnAsError";
            this.warnAsError.Size = new System.Drawing.Size(171, 17);
            this.warnAsError.TabIndex = 16;
            this.warnAsError.Text = "Generate warnings as &errors";
            this.warnAsError.UseVisualStyleBackColor = true;
            this.warnAsError.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // nowarn
            // 
            this.nowarn.AutoSize = true;
            this.nowarn.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.nowarn.Location = new System.Drawing.Point(15, 47);
            this.nowarn.Name = "nowarn";
            this.nowarn.Size = new System.Drawing.Size(155, 17);
            this.nowarn.TabIndex = 15;
            this.nowarn.Text = "Don\'t generate &warnings";
            this.nowarn.UseVisualStyleBackColor = true;
            this.nowarn.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // recompile
            // 
            this.recompile.AutoSize = true;
            this.recompile.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.recompile.Location = new System.Drawing.Point(15, 28);
            this.recompile.Name = "recompile";
            this.recompile.Size = new System.Drawing.Size(364, 17);
            this.recompile.TabIndex = 14;
            this.recompile.Text = "Always &compile source files, don\'t use object files even if available";
            this.recompile.UseVisualStyleBackColor = true;
            this.recompile.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // LinkerConfigPage
            // 
            this.Controls.Add(this.lookup);
            this.Controls.Add(this.skipCheck);
            this.Controls.Add(this.warnAsError);
            this.Controls.Add(this.nowarn);
            this.Controls.Add(this.recompile);
            this.Controls.Add(this.macroLabel);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.folder);
            this.Controls.Add(this.add);
            this.Controls.Add(this.dirInputText);
            this.Controls.Add(this.dirList);
            this.Controls.Add(this.dirLabel);
            this.Controls.Add(this.optionsLabel);
            this.Name = "LinkerConfigPage";
            this.Size = new System.Drawing.Size(473, 367);
            this.Load += new System.EventHandler(this.LinkerConfigPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label optionsLabel;
        private System.Windows.Forms.Label dirLabel;
        private System.Windows.Forms.ListBox dirList;
        private System.Windows.Forms.TextBox dirInputText;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button folder;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label macroLabel;
        private System.Windows.Forms.CheckBox nowarn;
        private System.Windows.Forms.CheckBox recompile;
        private System.Windows.Forms.CheckBox lookup;
        private System.Windows.Forms.CheckBox skipCheck;
        private System.Windows.Forms.CheckBox warnAsError;
    }
}
