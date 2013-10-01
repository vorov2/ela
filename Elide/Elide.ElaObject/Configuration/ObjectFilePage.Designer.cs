namespace Elide.ElaObject.Configuration
{
    partial class ObjectFilePage
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
            this.settingsLabel = new System.Windows.Forms.Label();
            this.expand = new System.Windows.Forms.CheckBox();
            this.header = new System.Windows.Forms.CheckBox();
            this.format = new System.Windows.Forms.CheckBox();
            this.formatBox = new System.Windows.Forms.TextBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.offsets = new System.Windows.Forms.CheckBox();
            this.opcodes = new System.Windows.Forms.CheckBox();
            this.frameOpcodes = new System.Windows.Forms.CheckBox();
            this.limit = new System.Windows.Forms.CheckBox();
            this.limitTextBox = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.settingsLabel.Location = new System.Drawing.Point(12, 10);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(120, 13);
            this.settingsLabel.TabIndex = 0;
            this.settingsLabel.Text = "Presentation settings:";
            // 
            // expand
            // 
            this.expand.AutoSize = true;
            this.expand.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.expand.Location = new System.Drawing.Point(15, 28);
            this.expand.Name = "expand";
            this.expand.Size = new System.Drawing.Size(184, 17);
            this.expand.TabIndex = 1;
            this.expand.Text = "&Expand all tree nodes on open";
            this.expand.UseVisualStyleBackColor = true;
            this.expand.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.header.Location = new System.Drawing.Point(15, 47);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(220, 17);
            this.header.TabIndex = 2;
            this.header.Text = "Display object file information &header";
            this.header.UseVisualStyleBackColor = true;
            this.header.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // format
            // 
            this.format.AutoSize = true;
            this.format.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.format.Location = new System.Drawing.Point(15, 66);
            this.format.Name = "format";
            this.format.Size = new System.Drawing.Size(243, 17);
            this.format.TabIndex = 3;
            this.format.Text = "Use custom &format for information header";
            this.format.UseVisualStyleBackColor = true;
            this.format.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // formatBox
            // 
            this.formatBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.formatBox.Location = new System.Drawing.Point(15, 85);
            this.formatBox.Name = "formatBox";
            this.formatBox.Size = new System.Drawing.Size(445, 22);
            this.formatBox.TabIndex = 4;
            this.formatBox.Text = "Module %name%, object file format version %version%. Created by Ela %ela%";
            this.formatBox.TextChanged += new System.EventHandler(this.formatBox_TextChanged);
            // 
            // formatLabel
            // 
            this.formatLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.formatLabel.Location = new System.Drawing.Point(12, 110);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(448, 31);
            this.formatLabel.TabIndex = 5;
            this.formatLabel.Text = "Use the following macros in format string: %name% (module name), %version% (objec" +
                "t file version), %ela% (Ela compiler version), %date% (date stamp).";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Output settings:";
            // 
            // offsets
            // 
            this.offsets.AutoSize = true;
            this.offsets.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.offsets.Location = new System.Drawing.Point(15, 167);
            this.offsets.Name = "offsets";
            this.offsets.Size = new System.Drawing.Size(245, 17);
            this.offsets.TabIndex = 7;
            this.offsets.Text = "Display &offset for each op code instruction";
            this.offsets.UseVisualStyleBackColor = true;
            this.offsets.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // opcodes
            // 
            this.opcodes.AutoSize = true;
            this.opcodes.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.opcodes.Location = new System.Drawing.Point(15, 205);
            this.opcodes.Name = "opcodes";
            this.opcodes.Size = new System.Drawing.Size(268, 17);
            this.opcodes.TabIndex = 9;
            this.opcodes.Text = "Display &flat op codes listing as a separate node";
            this.opcodes.UseVisualStyleBackColor = true;
            this.opcodes.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // frameOpcodes
            // 
            this.frameOpcodes.AutoSize = true;
            this.frameOpcodes.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.frameOpcodes.Location = new System.Drawing.Point(15, 186);
            this.frameOpcodes.Name = "frameOpcodes";
            this.frameOpcodes.Size = new System.Drawing.Size(233, 17);
            this.frameOpcodes.TabIndex = 8;
            this.frameOpcodes.Text = "Display op codes for each &memory frame";
            this.frameOpcodes.UseVisualStyleBackColor = true;
            this.frameOpcodes.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // limit
            // 
            this.limit.AutoSize = true;
            this.limit.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.limit.Location = new System.Drawing.Point(15, 224);
            this.limit.Name = "limit";
            this.limit.Size = new System.Drawing.Size(242, 17);
            this.limit.TabIndex = 10;
            this.limit.Text = "Limit number of op codes in flat listing to:";
            this.limit.UseVisualStyleBackColor = true;
            this.limit.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // limitTextBox
            // 
            this.limitTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.errorProvider.SetIconPadding(this.limitTextBox, -20);
            this.limitTextBox.Location = new System.Drawing.Point(253, 220);
            this.limitTextBox.Name = "limitTextBox";
            this.limitTextBox.Size = new System.Drawing.Size(100, 22);
            this.limitTextBox.TabIndex = 11;
            this.limitTextBox.TextChanged += new System.EventHandler(this.limitTextBox_TextChanged);
            this.limitTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.limitTextBox_KeyPress);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // ObjectFilePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.limitTextBox);
            this.Controls.Add(this.limit);
            this.Controls.Add(this.frameOpcodes);
            this.Controls.Add(this.opcodes);
            this.Controls.Add(this.offsets);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.formatBox);
            this.Controls.Add(this.format);
            this.Controls.Add(this.header);
            this.Controls.Add(this.expand);
            this.Controls.Add(this.settingsLabel);
            this.Name = "ObjectFilePage";
            this.Load += new System.EventHandler(this.ObjectFilePage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.CheckBox expand;
        private System.Windows.Forms.CheckBox header;
        private System.Windows.Forms.CheckBox format;
        private System.Windows.Forms.TextBox formatBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox offsets;
        private System.Windows.Forms.CheckBox opcodes;
        private System.Windows.Forms.CheckBox frameOpcodes;
        private System.Windows.Forms.CheckBox limit;
        private System.Windows.Forms.TextBox limitTextBox;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
