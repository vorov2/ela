namespace Elide.TextWorkbench.Dialogs
{
    partial class SearchDialog
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
            this.findTextLabel = new System.Windows.Forms.Label();
            this.findTextCombo = new System.Windows.Forms.ComboBox();
            this.replaceTextCombo = new System.Windows.Forms.ComboBox();
            this.replaceTextCheckBox = new System.Windows.Forms.CheckBox();
            this.optionsGroup = new System.Windows.Forms.GroupBox();
            this.regex = new System.Windows.Forms.CheckBox();
            this.searchUp = new System.Windows.Forms.CheckBox();
            this.matchWordStart = new System.Windows.Forms.CheckBox();
            this.matchWholeWord = new System.Windows.Forms.CheckBox();
            this.matchCase = new System.Windows.Forms.CheckBox();
            this.lookInGroup = new System.Windows.Forms.GroupBox();
            this.selection = new System.Windows.Forms.RadioButton();
            this.allDoc = new System.Windows.Forms.RadioButton();
            this.currentDoc = new System.Windows.Forms.RadioButton();
            this.findNext = new System.Windows.Forms.Button();
            this.findAll = new System.Windows.Forms.Button();
            this.replaceAll = new System.Windows.Forms.Button();
            this.replace = new System.Windows.Forms.Button();
            this.HiddenCloseButton = new System.Windows.Forms.Button();
            this.optionsGroup.SuspendLayout();
            this.lookInGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // findTextLabel
            // 
            this.findTextLabel.AutoSize = true;
            this.findTextLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.findTextLabel.Location = new System.Drawing.Point(6, 8);
            this.findTextLabel.Name = "findTextLabel";
            this.findTextLabel.Size = new System.Drawing.Size(62, 13);
            this.findTextLabel.TabIndex = 0;
            this.findTextLabel.Text = "Fi&nd what:";
            // 
            // findTextCombo
            // 
            this.findTextCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.findTextCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.findTextCombo.FormattingEnabled = true;
            this.findTextCombo.Location = new System.Drawing.Point(9, 26);
            this.findTextCombo.Name = "findTextCombo";
            this.findTextCombo.Size = new System.Drawing.Size(359, 21);
            this.findTextCombo.TabIndex = 1;
            // 
            // replaceTextCombo
            // 
            this.replaceTextCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceTextCombo.Enabled = false;
            this.replaceTextCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.replaceTextCombo.FormattingEnabled = true;
            this.replaceTextCombo.Location = new System.Drawing.Point(9, 77);
            this.replaceTextCombo.Name = "replaceTextCombo";
            this.replaceTextCombo.Size = new System.Drawing.Size(359, 21);
            this.replaceTextCombo.TabIndex = 3;
            // 
            // replaceTextCheckBox
            // 
            this.replaceTextCheckBox.AutoSize = true;
            this.replaceTextCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.replaceTextCheckBox.Location = new System.Drawing.Point(9, 56);
            this.replaceTextCheckBox.Name = "replaceTextCheckBox";
            this.replaceTextCheckBox.Size = new System.Drawing.Size(95, 17);
            this.replaceTextCheckBox.TabIndex = 2;
            this.replaceTextCheckBox.Text = "Re&place with:";
            this.replaceTextCheckBox.UseVisualStyleBackColor = true;
            this.replaceTextCheckBox.CheckedChanged += new System.EventHandler(this.replaceTextCheckBox_CheckedChanged);
            // 
            // optionsGroup
            // 
            this.optionsGroup.Controls.Add(this.regex);
            this.optionsGroup.Controls.Add(this.searchUp);
            this.optionsGroup.Controls.Add(this.matchWordStart);
            this.optionsGroup.Controls.Add(this.matchWholeWord);
            this.optionsGroup.Controls.Add(this.matchCase);
            this.optionsGroup.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.optionsGroup.Location = new System.Drawing.Point(12, 111);
            this.optionsGroup.Name = "optionsGroup";
            this.optionsGroup.Size = new System.Drawing.Size(170, 135);
            this.optionsGroup.TabIndex = 4;
            this.optionsGroup.TabStop = false;
            this.optionsGroup.Text = "Find options";
            // 
            // regex
            // 
            this.regex.AutoSize = true;
            this.regex.Location = new System.Drawing.Point(8, 111);
            this.regex.Name = "regex";
            this.regex.Size = new System.Drawing.Size(124, 17);
            this.regex.TabIndex = 4;
            this.regex.Text = "Regular &expression";
            this.regex.UseVisualStyleBackColor = true;
            // 
            // searchUp
            // 
            this.searchUp.AutoSize = true;
            this.searchUp.Location = new System.Drawing.Point(8, 88);
            this.searchUp.Name = "searchUp";
            this.searchUp.Size = new System.Drawing.Size(77, 17);
            this.searchUp.TabIndex = 3;
            this.searchUp.Text = "Search &up";
            this.searchUp.UseVisualStyleBackColor = true;
            // 
            // matchWordStart
            // 
            this.matchWordStart.AutoSize = true;
            this.matchWordStart.Location = new System.Drawing.Point(8, 65);
            this.matchWordStart.Name = "matchWordStart";
            this.matchWordStart.Size = new System.Drawing.Size(114, 17);
            this.matchWordStart.TabIndex = 2;
            this.matchWordStart.Text = "Match word &start";
            this.matchWordStart.UseVisualStyleBackColor = true;
            // 
            // matchWholeWord
            // 
            this.matchWholeWord.AutoSize = true;
            this.matchWholeWord.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.matchWholeWord.Location = new System.Drawing.Point(8, 42);
            this.matchWholeWord.Name = "matchWholeWord";
            this.matchWholeWord.Size = new System.Drawing.Size(123, 17);
            this.matchWholeWord.TabIndex = 1;
            this.matchWholeWord.Text = "Match &whole word";
            this.matchWholeWord.UseVisualStyleBackColor = true;
            // 
            // matchCase
            // 
            this.matchCase.AutoSize = true;
            this.matchCase.Location = new System.Drawing.Point(8, 19);
            this.matchCase.Name = "matchCase";
            this.matchCase.Size = new System.Drawing.Size(83, 17);
            this.matchCase.TabIndex = 0;
            this.matchCase.Text = "Match &case";
            this.matchCase.UseVisualStyleBackColor = true;
            // 
            // lookInGroup
            // 
            this.lookInGroup.Controls.Add(this.selection);
            this.lookInGroup.Controls.Add(this.allDoc);
            this.lookInGroup.Controls.Add(this.currentDoc);
            this.lookInGroup.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lookInGroup.Location = new System.Drawing.Point(198, 111);
            this.lookInGroup.Name = "lookInGroup";
            this.lookInGroup.Size = new System.Drawing.Size(170, 135);
            this.lookInGroup.TabIndex = 5;
            this.lookInGroup.TabStop = false;
            this.lookInGroup.Text = "Look in";
            // 
            // selection
            // 
            this.selection.AutoSize = true;
            this.selection.Location = new System.Drawing.Point(7, 65);
            this.selection.Name = "selection";
            this.selection.Size = new System.Drawing.Size(72, 17);
            this.selection.TabIndex = 2;
            this.selection.TabStop = true;
            this.selection.Text = "&Selection";
            this.selection.UseVisualStyleBackColor = true;
            // 
            // allDoc
            // 
            this.allDoc.AutoSize = true;
            this.allDoc.Location = new System.Drawing.Point(7, 42);
            this.allDoc.Name = "allDoc";
            this.allDoc.Size = new System.Drawing.Size(128, 17);
            this.allDoc.TabIndex = 1;
            this.allDoc.TabStop = true;
            this.allDoc.Text = "&All open documents";
            this.allDoc.UseVisualStyleBackColor = true;
            // 
            // currentDoc
            // 
            this.currentDoc.AutoSize = true;
            this.currentDoc.Location = new System.Drawing.Point(7, 19);
            this.currentDoc.Name = "currentDoc";
            this.currentDoc.Size = new System.Drawing.Size(120, 17);
            this.currentDoc.TabIndex = 0;
            this.currentDoc.TabStop = true;
            this.currentDoc.Text = "&Current Document";
            this.currentDoc.UseVisualStyleBackColor = true;
            // 
            // findNext
            // 
            this.findNext.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.findNext.Location = new System.Drawing.Point(12, 262);
            this.findNext.Name = "findNext";
            this.findNext.Size = new System.Drawing.Size(85, 23);
            this.findNext.TabIndex = 6;
            this.findNext.Text = "&Find Next";
            this.findNext.UseVisualStyleBackColor = true;
            this.findNext.Click += new System.EventHandler(this.findNext_Click);
            // 
            // findAll
            // 
            this.findAll.BackColor = System.Drawing.Color.White;
            this.findAll.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.findAll.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.findAll.Location = new System.Drawing.Point(103, 262);
            this.findAll.Name = "findAll";
            this.findAll.Size = new System.Drawing.Size(85, 23);
            this.findAll.TabIndex = 7;
            this.findAll.Text = "Find All";
            this.findAll.UseVisualStyleBackColor = true;
            this.findAll.Click += new System.EventHandler(this.findAll_Click);
            // 
            // replaceAll
            // 
            this.replaceAll.Enabled = false;
            this.replaceAll.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.replaceAll.Location = new System.Drawing.Point(283, 262);
            this.replaceAll.Name = "replaceAll";
            this.replaceAll.Size = new System.Drawing.Size(85, 23);
            this.replaceAll.TabIndex = 9;
            this.replaceAll.Text = "Replace All";
            this.replaceAll.UseVisualStyleBackColor = true;
            this.replaceAll.Click += new System.EventHandler(this.replaceAll_Click);
            // 
            // replace
            // 
            this.replace.Enabled = false;
            this.replace.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.replace.Location = new System.Drawing.Point(194, 262);
            this.replace.Name = "replace";
            this.replace.Size = new System.Drawing.Size(85, 23);
            this.replace.TabIndex = 8;
            this.replace.Text = "&Replace";
            this.replace.UseVisualStyleBackColor = true;
            this.replace.Click += new System.EventHandler(this.replace_Click);
            // 
            // HiddenCloseButton
            // 
            this.HiddenCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.HiddenCloseButton.Location = new System.Drawing.Point(324, 3);
            this.HiddenCloseButton.Name = "HiddenCloseButton";
            this.HiddenCloseButton.Size = new System.Drawing.Size(44, 23);
            this.HiddenCloseButton.TabIndex = 10;
            this.HiddenCloseButton.Text = "HiddenCloseButton";
            this.HiddenCloseButton.UseVisualStyleBackColor = false;
            this.HiddenCloseButton.Visible = false;
            // 
            // SearchDialog
            // 
            this.AcceptButton = this.findNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.HiddenCloseButton;
            this.ClientSize = new System.Drawing.Size(380, 293);
            this.Controls.Add(this.HiddenCloseButton);
            this.Controls.Add(this.replaceAll);
            this.Controls.Add(this.replace);
            this.Controls.Add(this.findAll);
            this.Controls.Add(this.findNext);
            this.Controls.Add(this.lookInGroup);
            this.Controls.Add(this.optionsGroup);
            this.Controls.Add(this.replaceTextCheckBox);
            this.Controls.Add(this.replaceTextCombo);
            this.Controls.Add(this.findTextCombo);
            this.Controls.Add(this.findTextLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(1200, 327);
            this.MinimumSize = new System.Drawing.Size(396, 327);
            this.Name = "SearchDialog";
            this.ShowInTaskbar = false;
            this.Text = "Find and Replace";
            this.Activated += new System.EventHandler(this.SearchDialog_Activated);
            this.Load += new System.EventHandler(this.SearchDialog_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchDialog_KeyUp);
            this.optionsGroup.ResumeLayout(false);
            this.optionsGroup.PerformLayout();
            this.lookInGroup.ResumeLayout(false);
            this.lookInGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label findTextLabel;
        private System.Windows.Forms.ComboBox findTextCombo;
        private System.Windows.Forms.ComboBox replaceTextCombo;
        private System.Windows.Forms.CheckBox replaceTextCheckBox;
        private System.Windows.Forms.GroupBox optionsGroup;
        private System.Windows.Forms.CheckBox searchUp;
        private System.Windows.Forms.CheckBox matchWordStart;
        private System.Windows.Forms.CheckBox matchWholeWord;
        private System.Windows.Forms.CheckBox matchCase;
        private System.Windows.Forms.CheckBox regex;
        private System.Windows.Forms.GroupBox lookInGroup;
        private System.Windows.Forms.RadioButton selection;
        private System.Windows.Forms.RadioButton allDoc;
        private System.Windows.Forms.RadioButton currentDoc;
        private System.Windows.Forms.Button findNext;
        private System.Windows.Forms.Button findAll;
        private System.Windows.Forms.Button replaceAll;
        private System.Windows.Forms.Button replace;
        private System.Windows.Forms.Button HiddenCloseButton;
    }
}