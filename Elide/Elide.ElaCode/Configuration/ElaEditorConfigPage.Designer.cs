namespace Elide.ElaCode.Configuration
{
    partial class ElaEditorConfigPage
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
            this.braces = new System.Windows.Forms.CheckBox();
            this.folding = new System.Windows.Forms.CheckBox();
            this.helpLabel = new System.Windows.Forms.Label();
            this.highlight = new System.Windows.Forms.CheckBox();
            this.compile = new System.Windows.Forms.CheckBox();
            this.optionsLabel = new System.Windows.Forms.Label();
            this.space = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chars = new System.Windows.Forms.CheckBox();
            this.charsBox = new System.Windows.Forms.TextBox();
            this.exportList = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // braces
            // 
            this.braces.AutoSize = true;
            this.braces.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.braces.Location = new System.Drawing.Point(15, 47);
            this.braces.Name = "braces";
            this.braces.Size = new System.Drawing.Size(94, 17);
            this.braces.TabIndex = 13;
            this.braces.Text = "&Match braces";
            this.braces.UseVisualStyleBackColor = true;
            this.braces.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // folding
            // 
            this.folding.AutoSize = true;
            this.folding.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.folding.Location = new System.Drawing.Point(15, 28);
            this.folding.Name = "folding";
            this.folding.Size = new System.Drawing.Size(130, 17);
            this.folding.TabIndex = 12;
            this.folding.Text = "Enable code &folding";
            this.folding.UseVisualStyleBackColor = true;
            this.folding.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.helpLabel.Location = new System.Drawing.Point(12, 10);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(67, 13);
            this.helpLabel.TabIndex = 11;
            this.helpLabel.Text = "Visual help:";
            // 
            // highlight
            // 
            this.highlight.AutoSize = true;
            this.highlight.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.highlight.Location = new System.Drawing.Point(15, 110);
            this.highlight.Name = "highlight";
            this.highlight.Size = new System.Drawing.Size(243, 17);
            this.highlight.TabIndex = 16;
            this.highlight.Text = "&Highlight errors and warnings as you type";
            this.highlight.UseVisualStyleBackColor = true;
            this.highlight.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // compile
            // 
            this.compile.AutoSize = true;
            this.compile.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.compile.Location = new System.Drawing.Point(15, 91);
            this.compile.Name = "compile";
            this.compile.Size = new System.Drawing.Size(270, 17);
            this.compile.TabIndex = 15;
            this.compile.Text = "Enable background &compilation of Ela modules";
            this.compile.UseVisualStyleBackColor = true;
            this.compile.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // optionsLabel
            // 
            this.optionsLabel.AutoSize = true;
            this.optionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.optionsLabel.Location = new System.Drawing.Point(12, 73);
            this.optionsLabel.Name = "optionsLabel";
            this.optionsLabel.Size = new System.Drawing.Size(138, 13);
            this.optionsLabel.TabIndex = 14;
            this.optionsLabel.Text = "Background compilation:";
            // 
            // space
            // 
            this.space.AutoSize = true;
            this.space.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.space.Location = new System.Drawing.Point(15, 173);
            this.space.Name = "space";
            this.space.Size = new System.Drawing.Size(223, 17);
            this.space.TabIndex = 18;
            this.space.Text = "Show autocomplete window on space";
            this.space.UseVisualStyleBackColor = true;
            this.space.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Autocomplete:";
            // 
            // chars
            // 
            this.chars.AutoSize = true;
            this.chars.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.chars.Location = new System.Drawing.Point(15, 192);
            this.chars.Name = "chars";
            this.chars.Size = new System.Drawing.Size(350, 17);
            this.chars.TabIndex = 19;
            this.chars.Text = "Show autocomplete window when one of the chars is entered:";
            this.chars.UseVisualStyleBackColor = true;
            this.chars.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // charsBox
            // 
            this.charsBox.Location = new System.Drawing.Point(15, 211);
            this.charsBox.Name = "charsBox";
            this.charsBox.Size = new System.Drawing.Size(350, 20);
            this.charsBox.TabIndex = 20;
            this.charsBox.TextChanged += new System.EventHandler(this.charsBox_TextChanged);
            // 
            // exportList
            // 
            this.exportList.AutoSize = true;
            this.exportList.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.exportList.Location = new System.Drawing.Point(15, 154);
            this.exportList.Name = "exportList";
            this.exportList.Size = new System.Drawing.Size(236, 17);
            this.exportList.TabIndex = 21;
            this.exportList.Text = "&List module export list on entering dot (.)";
            this.exportList.UseVisualStyleBackColor = true;
            this.exportList.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // ElaEditorConfigPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.exportList);
            this.Controls.Add(this.charsBox);
            this.Controls.Add(this.chars);
            this.Controls.Add(this.space);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.highlight);
            this.Controls.Add(this.compile);
            this.Controls.Add(this.optionsLabel);
            this.Controls.Add(this.braces);
            this.Controls.Add(this.folding);
            this.Controls.Add(this.helpLabel);
            this.Name = "ElaEditorConfigPage";
            this.Size = new System.Drawing.Size(453, 410);
            this.Load += new System.EventHandler(this.ElaEditorConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox braces;
        private System.Windows.Forms.CheckBox folding;
        private System.Windows.Forms.Label helpLabel;
        private System.Windows.Forms.CheckBox highlight;
        private System.Windows.Forms.CheckBox compile;
        private System.Windows.Forms.Label optionsLabel;
        private System.Windows.Forms.CheckBox space;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chars;
        private System.Windows.Forms.TextBox charsBox;
        private System.Windows.Forms.CheckBox exportList;
    }
}
