using Elide.Forms;
namespace Elide.Configuration
{
    partial class StylesPage
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
            this.settingsLabel = new System.Windows.Forms.Label();
            this.groupsCombo = new System.Windows.Forms.ComboBox();
            this.fontLabel = new System.Windows.Forms.Label();
            this.fontCombo = new Elide.Forms.FontPicker();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.sizeCombo = new Elide.Forms.IntPicker();
            this.itemsLabel = new System.Windows.Forms.Label();
            this.itemsList = new Elide.Forms.FlexListBox();
            this.foreLabel = new System.Windows.Forms.Label();
            this.forePicker = new Elide.Forms.ColorPicker();
            this.backLabel = new System.Windows.Forms.Label();
            this.backPicker = new Elide.Forms.ColorPicker();
            this.checkBold = new System.Windows.Forms.CheckBox();
            this.checkItalic = new System.Windows.Forms.CheckBox();
            this.sampleLabel = new System.Windows.Forms.Label();
            this.samplePanel = new Elide.Forms.FlatPanel();
            this.sample = new System.Windows.Forms.Label();
            this.checkUnderline = new System.Windows.Forms.CheckBox();
            this.samplePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.settingsLabel.Location = new System.Drawing.Point(12, 10);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(101, 13);
            this.settingsLabel.TabIndex = 0;
            this.settingsLabel.Text = "Show se&ttings for:";
            // 
            // groupsCombo
            // 
            this.groupsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupsCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.groupsCombo.FormattingEnabled = true;
            this.groupsCombo.Location = new System.Drawing.Point(15, 26);
            this.groupsCombo.Name = "groupsCombo";
            this.groupsCombo.Size = new System.Drawing.Size(447, 21);
            this.groupsCombo.TabIndex = 1;
            this.groupsCombo.SelectedIndexChanged += new System.EventHandler(this.groupsCombo_SelectedIndexChanged);
            // 
            // fontLabel
            // 
            this.fontLabel.AutoSize = true;
            this.fontLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.fontLabel.Location = new System.Drawing.Point(204, 79);
            this.fontLabel.Name = "fontLabel";
            this.fontLabel.Size = new System.Drawing.Size(233, 13);
            this.fontLabel.TabIndex = 2;
            this.fontLabel.Text = "&Font (bold type indicates fixed-width fonts):";
            // 
            // fontCombo
            // 
            this.fontCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontCombo.DropDownHeight = 500;
            this.fontCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fontCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.fontCombo.IntegralHeight = false;
            this.fontCombo.Location = new System.Drawing.Point(207, 95);
            this.fontCombo.Name = "fontCombo";
            this.fontCombo.SelectedFont = null;
            this.fontCombo.Size = new System.Drawing.Size(253, 23);
            this.fontCombo.TabIndex = 3;
            this.fontCombo.SelectedIndexChanged += new System.EventHandler(this.ControlUpdated);
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sizeLabel.Location = new System.Drawing.Point(204, 129);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(30, 13);
            this.sizeLabel.TabIndex = 4;
            this.sizeLabel.Text = "&Size:";
            // 
            // sizeCombo
            // 
            this.sizeCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.sizeCombo.DropDownHeight = 500;
            this.sizeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sizeCombo.FormattingEnabled = true;
            this.sizeCombo.IntegralHeight = false;
            this.sizeCombo.Location = new System.Drawing.Point(207, 145);
            this.sizeCombo.Name = "sizeCombo";
            this.sizeCombo.Size = new System.Drawing.Size(121, 23);
            this.sizeCombo.TabIndex = 5;
            this.sizeCombo.SelectedIndexChanged += new System.EventHandler(this.ControlUpdated);
            // 
            // itemsLabel
            // 
            this.itemsLabel.AutoSize = true;
            this.itemsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.itemsLabel.Location = new System.Drawing.Point(12, 63);
            this.itemsLabel.Name = "itemsLabel";
            this.itemsLabel.Size = new System.Drawing.Size(79, 13);
            this.itemsLabel.TabIndex = 6;
            this.itemsLabel.Text = "&Display items:";
            // 
            // itemsList
            // 
            this.itemsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.itemsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.itemsList.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.itemsList.FormattingEnabled = true;
            this.itemsList.IntegralHeight = false;
            this.itemsList.ItemHeight = 15;
            this.itemsList.Location = new System.Drawing.Point(15, 79);
            this.itemsList.Name = "itemsList";
            this.itemsList.Size = new System.Drawing.Size(172, 265);
            this.itemsList.TabIndex = 7;
            this.itemsList.SelectedIndexChanged += new System.EventHandler(this.itemsList_SelectedIndexChanged);
            // 
            // foreLabel
            // 
            this.foreLabel.AutoSize = true;
            this.foreLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.foreLabel.Location = new System.Drawing.Point(204, 187);
            this.foreLabel.Name = "foreLabel";
            this.foreLabel.Size = new System.Drawing.Size(95, 13);
            this.foreLabel.TabIndex = 8;
            this.foreLabel.Text = "Item fo&reground:";
            // 
            // forePicker
            // 
            this.forePicker.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.forePicker.DropDownHeight = 320;
            this.forePicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forePicker.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.forePicker.FormattingEnabled = true;
            this.forePicker.IntegralHeight = false;
            this.forePicker.Location = new System.Drawing.Point(207, 203);
            this.forePicker.Name = "forePicker";
            this.forePicker.Size = new System.Drawing.Size(121, 23);
            this.forePicker.TabIndex = 9;
            this.forePicker.SelectedIndexChanged += new System.EventHandler(this.ControlUpdated);
            // 
            // backLabel
            // 
            this.backLabel.AutoSize = true;
            this.backLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.backLabel.Location = new System.Drawing.Point(337, 187);
            this.backLabel.Name = "backLabel";
            this.backLabel.Size = new System.Drawing.Size(98, 13);
            this.backLabel.TabIndex = 10;
            this.backLabel.Text = "Item bac&kground:";
            // 
            // backPicker
            // 
            this.backPicker.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.backPicker.DropDownHeight = 320;
            this.backPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backPicker.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.backPicker.FormattingEnabled = true;
            this.backPicker.IntegralHeight = false;
            this.backPicker.Location = new System.Drawing.Point(341, 203);
            this.backPicker.Name = "backPicker";
            this.backPicker.Size = new System.Drawing.Size(121, 23);
            this.backPicker.TabIndex = 11;
            this.backPicker.SelectedIndexChanged += new System.EventHandler(this.ControlUpdated);
            // 
            // checkBold
            // 
            this.checkBold.AutoSize = true;
            this.checkBold.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.checkBold.Location = new System.Drawing.Point(208, 240);
            this.checkBold.Name = "checkBold";
            this.checkBold.Size = new System.Drawing.Size(50, 17);
            this.checkBold.TabIndex = 12;
            this.checkBold.Text = "&Bold";
            this.checkBold.ThreeState = true;
            this.checkBold.UseVisualStyleBackColor = true;
            this.checkBold.CheckStateChanged += new System.EventHandler(this.ControlUpdated);
            // 
            // checkItalic
            // 
            this.checkItalic.AutoSize = true;
            this.checkItalic.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.checkItalic.Location = new System.Drawing.Point(270, 240);
            this.checkItalic.Name = "checkItalic";
            this.checkItalic.Size = new System.Drawing.Size(50, 17);
            this.checkItalic.TabIndex = 13;
            this.checkItalic.Text = "&Italic";
            this.checkItalic.ThreeState = true;
            this.checkItalic.UseVisualStyleBackColor = true;
            this.checkItalic.CheckStateChanged += new System.EventHandler(this.ControlUpdated);
            // 
            // sampleLabel
            // 
            this.sampleLabel.AutoSize = true;
            this.sampleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sampleLabel.Location = new System.Drawing.Point(204, 272);
            this.sampleLabel.Name = "sampleLabel";
            this.sampleLabel.Size = new System.Drawing.Size(47, 13);
            this.sampleLabel.TabIndex = 15;
            this.sampleLabel.Text = "Sample:";
            // 
            // samplePanel
            // 
            this.samplePanel.Controls.Add(this.sample);
            this.samplePanel.Location = new System.Drawing.Point(208, 288);
            this.samplePanel.Name = "samplePanel";
            this.samplePanel.Padding = new System.Windows.Forms.Padding(2);
            this.samplePanel.Size = new System.Drawing.Size(254, 56);
            this.samplePanel.TabIndex = 16;
            this.samplePanel.WideRendering = true;
            // 
            // sample
            // 
            this.sample.BackColor = System.Drawing.Color.Transparent;
            this.sample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sample.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sample.Location = new System.Drawing.Point(2, 2);
            this.sample.Name = "sample";
            this.sample.Size = new System.Drawing.Size(250, 52);
            this.sample.TabIndex = 17;
            this.sample.Text = "AaBbCcXxYyZz";
            this.sample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkUnderline
            // 
            this.checkUnderline.AutoSize = true;
            this.checkUnderline.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.checkUnderline.Location = new System.Drawing.Point(332, 240);
            this.checkUnderline.Name = "checkUnderline";
            this.checkUnderline.Size = new System.Drawing.Size(77, 17);
            this.checkUnderline.TabIndex = 14;
            this.checkUnderline.Text = "Unde&rline";
            this.checkUnderline.ThreeState = true;
            this.checkUnderline.UseVisualStyleBackColor = true;
            this.checkUnderline.CheckStateChanged += new System.EventHandler(this.ControlUpdated);
            // 
            // StylesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.itemsList);
            this.Controls.Add(this.checkUnderline);
            this.Controls.Add(this.samplePanel);
            this.Controls.Add(this.sampleLabel);
            this.Controls.Add(this.checkItalic);
            this.Controls.Add(this.checkBold);
            this.Controls.Add(this.backPicker);
            this.Controls.Add(this.backLabel);
            this.Controls.Add(this.forePicker);
            this.Controls.Add(this.foreLabel);
            this.Controls.Add(this.itemsLabel);
            this.Controls.Add(this.sizeCombo);
            this.Controls.Add(this.sizeLabel);
            this.Controls.Add(this.fontCombo);
            this.Controls.Add(this.fontLabel);
            this.Controls.Add(this.groupsCombo);
            this.Controls.Add(this.settingsLabel);
            this.Name = "StylesPage";
            this.Size = new System.Drawing.Size(476, 360);
            this.Load += new System.EventHandler(this.StylesPage_Load);
            this.samplePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.ComboBox groupsCombo;
        private System.Windows.Forms.Label fontLabel;
        private FontPicker fontCombo;
        private System.Windows.Forms.Label sizeLabel;
        private IntPicker sizeCombo;
        private System.Windows.Forms.Label itemsLabel;
        private FlexListBox itemsList;
        private System.Windows.Forms.Label foreLabel;
        private Elide.Forms.ColorPicker forePicker;
        private System.Windows.Forms.Label backLabel;
        private Elide.Forms.ColorPicker backPicker;
        private System.Windows.Forms.CheckBox checkBold;
        private System.Windows.Forms.CheckBox checkItalic;
        private System.Windows.Forms.Label sampleLabel;
        private Elide.Forms.FlatPanel samplePanel;
        private System.Windows.Forms.Label sample;
        private System.Windows.Forms.CheckBox checkUnderline;
    }
}
