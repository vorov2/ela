using System.Windows.Forms;
namespace Elide.TextWorkbench.Configuration
{
    partial class TextEditorConfigPage
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
            this.configsCombo = new Elide.Forms.DefaultComboBox();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.useTabs = new System.Windows.Forms.CheckBox();
            this.tabSizeCombo = new Elide.Forms.DefaultComboBox();
            this.multipleSelections = new System.Windows.Forms.CheckBox();
            this.mutipleSelectionsTyping = new System.Windows.Forms.CheckBox();
            this.multipleSelectionsPaste = new System.Windows.Forms.CheckBox();
            this.virtualSpace = new System.Windows.Forms.CheckBox();
            this.selBackgroundCombo = new Elide.Forms.ColorPicker();
            this.selTransparencyLabel = new System.Windows.Forms.Label();
            this.wrapIndicator = new System.Windows.Forms.CheckBox();
            this.eol = new System.Windows.Forms.CheckBox();
            this.whiteSpace = new System.Windows.Forms.CheckBox();
            this.indentationGuides = new System.Windows.Forms.CheckBox();
            this.tabSizeLabel = new System.Windows.Forms.Label();
            this.indentModeLabel = new System.Windows.Forms.Label();
            this.indentModeCombo = new Elide.Forms.DefaultComboBox();
            this.indentSizeLabel = new System.Windows.Forms.Label();
            this.indentSizeCombo = new Elide.Forms.DefaultComboBox();
            this.longLineIndicator = new System.Windows.Forms.CheckBox();
            this.longLineIndicatorCombo = new Elide.Forms.DefaultComboBox();
            this.selBackgroundLabel = new System.Windows.Forms.Label();
            this.selColorCombo = new Elide.Forms.ColorPicker();
            this.selColorLabel = new System.Windows.Forms.Label();
            this.caretLineColorLabel = new System.Windows.Forms.Label();
            this.caretLineColorCombo = new Elide.Forms.ColorPicker();
            this.caretLineTransparencyLabel = new System.Windows.Forms.Label();
            this.caretLine = new System.Windows.Forms.CheckBox();
            this.selColor = new System.Windows.Forms.CheckBox();
            this.caretLineTransparencyCombo = new Elide.Forms.DefaultComboBox();
            this.selTransparencyCombo = new Elide.Forms.DefaultComboBox();
            this.lineEndingsCombo = new Elide.Forms.DefaultComboBox();
            this.lineEndingsLabel = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.miscLabel = new System.Windows.Forms.Label();
            this.tabIndentsLabel = new System.Windows.Forms.Label();
            this.wrapCombo = new Elide.Forms.DefaultComboBox();
            this.wrapLabel = new System.Windows.Forms.Label();
            this.selectionsPage = new System.Windows.Forms.TabPage();
            this.selStylingLabel = new System.Windows.Forms.Label();
            this.multipleSelectionsLabel = new System.Windows.Forms.Label();
            this.helpPage = new System.Windows.Forms.TabPage();
            this.longLineLabel = new System.Windows.Forms.Label();
            this.guidesLabel = new System.Windows.Forms.Label();
            this.longLineColumnLabel = new System.Windows.Forms.Label();
            this.caretPage = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.careLineLabel = new System.Windows.Forms.Label();
            this.stickyCaret = new System.Windows.Forms.CheckBox();
            this.blinkLabel = new System.Windows.Forms.Label();
            this.blinkCombo = new Elide.Forms.DefaultComboBox();
            this.caretWidthLabel = new System.Windows.Forms.Label();
            this.caretWidthCombo = new Elide.Forms.DefaultComboBox();
            this.caretStyleLabel = new System.Windows.Forms.Label();
            this.caretStyleCombo = new Elide.Forms.DefaultComboBox();
            this.caretColorLabel = new System.Windows.Forms.Label();
            this.caretColorCombo = new Elide.Forms.ColorPicker();
            this.tabControl.SuspendLayout();
            this.generalPage.SuspendLayout();
            this.selectionsPage.SuspendLayout();
            this.helpPage.SuspendLayout();
            this.caretPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // configsCombo
            // 
            this.configsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configsCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.configsCombo.FormattingEnabled = true;
            this.configsCombo.Location = new System.Drawing.Point(15, 26);
            this.configsCombo.Name = "configsCombo";
            this.configsCombo.Size = new System.Drawing.Size(447, 21);
            this.configsCombo.TabIndex = 1;
            this.configsCombo.SelectedIndexChanged += new System.EventHandler(this.configsCombo_SelectedIndexChanged);
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
            // useTabs
            // 
            this.useTabs.AutoSize = true;
            this.useTabs.Checked = true;
            this.useTabs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.useTabs.Location = new System.Drawing.Point(12, 73);
            this.useTabs.Name = "useTabs";
            this.useTabs.Size = new System.Drawing.Size(152, 17);
            this.useTabs.TabIndex = 2;
            this.useTabs.Text = "&Use tabs for indentation";
            this.useTabs.ThreeState = true;
            this.useTabs.UseVisualStyleBackColor = true;
            this.useTabs.CheckedChanged += new System.EventHandler(this.useTabs_CheckedChanged);
            // 
            // tabSizeCombo
            // 
            this.tabSizeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tabSizeCombo.Enabled = false;
            this.tabSizeCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.tabSizeCombo.FormattingEnabled = true;
            this.tabSizeCombo.Location = new System.Drawing.Point(12, 46);
            this.tabSizeCombo.Name = "tabSizeCombo";
            this.tabSizeCombo.Size = new System.Drawing.Size(129, 21);
            this.tabSizeCombo.TabIndex = 4;
            this.tabSizeCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // multipleSelections
            // 
            this.multipleSelections.AutoSize = true;
            this.multipleSelections.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.multipleSelections.Location = new System.Drawing.Point(12, 30);
            this.multipleSelections.Name = "multipleSelections";
            this.multipleSelections.Size = new System.Drawing.Size(160, 17);
            this.multipleSelections.TabIndex = 9;
            this.multipleSelections.Text = "&Enable multiple selections";
            this.multipleSelections.ThreeState = true;
            this.multipleSelections.UseVisualStyleBackColor = true;
            this.multipleSelections.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // mutipleSelectionsTyping
            // 
            this.mutipleSelectionsTyping.AutoSize = true;
            this.mutipleSelectionsTyping.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.mutipleSelectionsTyping.Location = new System.Drawing.Point(12, 49);
            this.mutipleSelectionsTyping.Name = "mutipleSelectionsTyping";
            this.mutipleSelectionsTyping.Size = new System.Drawing.Size(203, 17);
            this.mutipleSelectionsTyping.TabIndex = 10;
            this.mutipleSelectionsTyping.Text = "Allow &typing in multiple selections";
            this.mutipleSelectionsTyping.ThreeState = true;
            this.mutipleSelectionsTyping.UseVisualStyleBackColor = true;
            this.mutipleSelectionsTyping.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // multipleSelectionsPaste
            // 
            this.multipleSelectionsPaste.AutoSize = true;
            this.multipleSelectionsPaste.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.multipleSelectionsPaste.Location = new System.Drawing.Point(12, 68);
            this.multipleSelectionsPaste.Name = "multipleSelectionsPaste";
            this.multipleSelectionsPaste.Size = new System.Drawing.Size(198, 17);
            this.multipleSelectionsPaste.TabIndex = 11;
            this.multipleSelectionsPaste.Text = "Allow &paste in multiple selections";
            this.multipleSelectionsPaste.ThreeState = true;
            this.multipleSelectionsPaste.UseVisualStyleBackColor = true;
            this.multipleSelectionsPaste.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // virtualSpace
            // 
            this.virtualSpace.AutoSize = true;
            this.virtualSpace.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.virtualSpace.Location = new System.Drawing.Point(12, 87);
            this.virtualSpace.Name = "virtualSpace";
            this.virtualSpace.Size = new System.Drawing.Size(128, 17);
            this.virtualSpace.TabIndex = 12;
            this.virtualSpace.Text = "Enable &virtual space";
            this.virtualSpace.ThreeState = true;
            this.virtualSpace.UseVisualStyleBackColor = true;
            this.virtualSpace.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
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
            this.selBackgroundCombo.Location = new System.Drawing.Point(12, 150);
            this.selBackgroundCombo.Name = "selBackgroundCombo";
            this.selBackgroundCombo.SelectedColor = null;
            this.selBackgroundCombo.Size = new System.Drawing.Size(129, 21);
            this.selBackgroundCombo.TabIndex = 26;
            this.selBackgroundCombo.SelectedValueChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // selTransparencyLabel
            // 
            this.selTransparencyLabel.AutoSize = true;
            this.selTransparencyLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selTransparencyLabel.Location = new System.Drawing.Point(296, 134);
            this.selTransparencyLabel.Name = "selTransparencyLabel";
            this.selTransparencyLabel.Size = new System.Drawing.Size(126, 13);
            this.selTransparencyLabel.TabIndex = 30;
            this.selTransparencyLabel.Text = "Selec&tion transparency:";
            // 
            // wrapIndicator
            // 
            this.wrapIndicator.AutoSize = true;
            this.wrapIndicator.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.wrapIndicator.Location = new System.Drawing.Point(12, 87);
            this.wrapIndicator.Name = "wrapIndicator";
            this.wrapIndicator.Size = new System.Drawing.Size(171, 17);
            this.wrapIndicator.TabIndex = 17;
            this.wrapIndicator.Text = "&Indicators for wrapped lines";
            this.wrapIndicator.ThreeState = true;
            this.wrapIndicator.UseVisualStyleBackColor = true;
            this.wrapIndicator.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // eol
            // 
            this.eol.AutoSize = true;
            this.eol.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.eol.Location = new System.Drawing.Point(12, 68);
            this.eol.Name = "eol";
            this.eol.Size = new System.Drawing.Size(127, 17);
            this.eol.TabIndex = 16;
            this.eol.Text = "Visible line &endings";
            this.eol.ThreeState = true;
            this.eol.UseVisualStyleBackColor = true;
            this.eol.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // whiteSpace
            // 
            this.whiteSpace.AutoSize = true;
            this.whiteSpace.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.whiteSpace.Location = new System.Drawing.Point(12, 49);
            this.whiteSpace.Name = "whiteSpace";
            this.whiteSpace.Size = new System.Drawing.Size(124, 17);
            this.whiteSpace.TabIndex = 15;
            this.whiteSpace.Text = "Visible &white space";
            this.whiteSpace.ThreeState = true;
            this.whiteSpace.UseVisualStyleBackColor = true;
            this.whiteSpace.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // indentationGuides
            // 
            this.indentationGuides.AutoSize = true;
            this.indentationGuides.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.indentationGuides.Location = new System.Drawing.Point(12, 30);
            this.indentationGuides.Name = "indentationGuides";
            this.indentationGuides.Size = new System.Drawing.Size(157, 17);
            this.indentationGuides.TabIndex = 14;
            this.indentationGuides.Text = "Show indentation &guides";
            this.indentationGuides.ThreeState = true;
            this.indentationGuides.UseVisualStyleBackColor = true;
            this.indentationGuides.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // tabSizeLabel
            // 
            this.tabSizeLabel.AutoSize = true;
            this.tabSizeLabel.Enabled = false;
            this.tabSizeLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.tabSizeLabel.Location = new System.Drawing.Point(9, 30);
            this.tabSizeLabel.Name = "tabSizeLabel";
            this.tabSizeLabel.Size = new System.Drawing.Size(50, 13);
            this.tabSizeLabel.TabIndex = 3;
            this.tabSizeLabel.Text = "&Tab size:";
            // 
            // indentModeLabel
            // 
            this.indentModeLabel.AutoSize = true;
            this.indentModeLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.indentModeLabel.Location = new System.Drawing.Point(293, 30);
            this.indentModeLabel.Name = "indentModeLabel";
            this.indentModeLabel.Size = new System.Drawing.Size(76, 13);
            this.indentModeLabel.TabIndex = 7;
            this.indentModeLabel.Text = "Indent &mode:";
            // 
            // indentModeCombo
            // 
            this.indentModeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.indentModeCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.indentModeCombo.FormattingEnabled = true;
            this.indentModeCombo.Location = new System.Drawing.Point(296, 46);
            this.indentModeCombo.Name = "indentModeCombo";
            this.indentModeCombo.Size = new System.Drawing.Size(129, 21);
            this.indentModeCombo.TabIndex = 8;
            this.indentModeCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // indentSizeLabel
            // 
            this.indentSizeLabel.AutoSize = true;
            this.indentSizeLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.indentSizeLabel.Location = new System.Drawing.Point(153, 30);
            this.indentSizeLabel.Name = "indentSizeLabel";
            this.indentSizeLabel.Size = new System.Drawing.Size(66, 13);
            this.indentSizeLabel.TabIndex = 5;
            this.indentSizeLabel.Text = "Indent &size:";
            // 
            // indentSizeCombo
            // 
            this.indentSizeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.indentSizeCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.indentSizeCombo.FormattingEnabled = true;
            this.indentSizeCombo.Location = new System.Drawing.Point(156, 46);
            this.indentSizeCombo.Name = "indentSizeCombo";
            this.indentSizeCombo.Size = new System.Drawing.Size(129, 21);
            this.indentSizeCombo.TabIndex = 6;
            this.indentSizeCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // longLineIndicator
            // 
            this.longLineIndicator.AutoSize = true;
            this.longLineIndicator.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.longLineIndicator.Location = new System.Drawing.Point(12, 175);
            this.longLineIndicator.Name = "longLineIndicator";
            this.longLineIndicator.Size = new System.Drawing.Size(159, 17);
            this.longLineIndicator.TabIndex = 18;
            this.longLineIndicator.Text = "Enable l&ong line indicator";
            this.longLineIndicator.ThreeState = true;
            this.longLineIndicator.UseVisualStyleBackColor = true;
            this.longLineIndicator.CheckedChanged += new System.EventHandler(this.longLineIndicator_CheckedChanged);
            // 
            // longLineIndicatorCombo
            // 
            this.longLineIndicatorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.longLineIndicatorCombo.Enabled = false;
            this.longLineIndicatorCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.longLineIndicatorCombo.FormattingEnabled = true;
            this.longLineIndicatorCombo.Location = new System.Drawing.Point(12, 149);
            this.longLineIndicatorCombo.Name = "longLineIndicatorCombo";
            this.longLineIndicatorCombo.Size = new System.Drawing.Size(129, 21);
            this.longLineIndicatorCombo.TabIndex = 19;
            this.longLineIndicatorCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // selBackgroundLabel
            // 
            this.selBackgroundLabel.AutoSize = true;
            this.selBackgroundLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selBackgroundLabel.Location = new System.Drawing.Point(9, 134);
            this.selBackgroundLabel.Name = "selBackgroundLabel";
            this.selBackgroundLabel.Size = new System.Drawing.Size(123, 13);
            this.selBackgroundLabel.TabIndex = 25;
            this.selBackgroundLabel.Text = "Sele&ction background:";
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
            this.selColorCombo.Location = new System.Drawing.Point(156, 150);
            this.selColorCombo.Name = "selColorCombo";
            this.selColorCombo.SelectedColor = null;
            this.selColorCombo.Size = new System.Drawing.Size(129, 21);
            this.selColorCombo.TabIndex = 29;
            this.selColorCombo.SelectedValueChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // selColorLabel
            // 
            this.selColorLabel.AutoSize = true;
            this.selColorLabel.Enabled = false;
            this.selColorLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selColorLabel.Location = new System.Drawing.Point(153, 134);
            this.selColorLabel.Name = "selColorLabel";
            this.selColorLabel.Size = new System.Drawing.Size(86, 13);
            this.selColorLabel.TabIndex = 27;
            this.selColorLabel.Text = "Selection c&olor:";
            // 
            // caretLineColorLabel
            // 
            this.caretLineColorLabel.AutoSize = true;
            this.caretLineColorLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretLineColorLabel.Location = new System.Drawing.Point(9, 29);
            this.caretLineColorLabel.Name = "caretLineColorLabel";
            this.caretLineColorLabel.Size = new System.Drawing.Size(88, 13);
            this.caretLineColorLabel.TabIndex = 20;
            this.caretLineColorLabel.Text = "Caret l&ine color:";
            // 
            // caretLineColorCombo
            // 
            this.caretLineColorCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.caretLineColorCombo.DropDownHeight = 320;
            this.caretLineColorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.caretLineColorCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretLineColorCombo.FormattingEnabled = true;
            this.caretLineColorCombo.IntegralHeight = false;
            this.caretLineColorCombo.ItemHeight = 15;
            this.caretLineColorCombo.Location = new System.Drawing.Point(12, 45);
            this.caretLineColorCombo.Name = "caretLineColorCombo";
            this.caretLineColorCombo.SelectedColor = null;
            this.caretLineColorCombo.Size = new System.Drawing.Size(129, 21);
            this.caretLineColorCombo.TabIndex = 22;
            this.caretLineColorCombo.SelectedValueChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // caretLineTransparencyLabel
            // 
            this.caretLineTransparencyLabel.AutoSize = true;
            this.caretLineTransparencyLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretLineTransparencyLabel.Location = new System.Drawing.Point(151, 29);
            this.caretLineTransparencyLabel.Name = "caretLineTransparencyLabel";
            this.caretLineTransparencyLabel.Size = new System.Drawing.Size(128, 13);
            this.caretLineTransparencyLabel.TabIndex = 23;
            this.caretLineTransparencyLabel.Text = "Caret line tr&ansparency:";
            // 
            // caretLine
            // 
            this.caretLine.AutoSize = true;
            this.caretLine.Checked = true;
            this.caretLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.caretLine.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretLine.Location = new System.Drawing.Point(12, 70);
            this.caretLine.Name = "caretLine";
            this.caretLine.Size = new System.Drawing.Size(125, 17);
            this.caretLine.TabIndex = 21;
            this.caretLine.Text = "&Highlight caret line";
            this.caretLine.ThreeState = true;
            this.caretLine.UseVisualStyleBackColor = true;
            this.caretLine.CheckedChanged += new System.EventHandler(this.caretLine_CheckedChanged);
            // 
            // selColor
            // 
            this.selColor.AutoSize = true;
            this.selColor.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selColor.Location = new System.Drawing.Point(12, 177);
            this.selColor.Name = "selColor";
            this.selColor.Size = new System.Drawing.Size(211, 17);
            this.selColor.TabIndex = 28;
            this.selColor.Text = "&Use different text color in selections";
            this.selColor.ThreeState = true;
            this.selColor.UseVisualStyleBackColor = true;
            this.selColor.CheckedChanged += new System.EventHandler(this.selColor_CheckedChanged);
            // 
            // caretLineTransparencyCombo
            // 
            this.caretLineTransparencyCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.caretLineTransparencyCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretLineTransparencyCombo.FormattingEnabled = true;
            this.caretLineTransparencyCombo.Location = new System.Drawing.Point(154, 45);
            this.caretLineTransparencyCombo.Name = "caretLineTransparencyCombo";
            this.caretLineTransparencyCombo.Size = new System.Drawing.Size(129, 21);
            this.caretLineTransparencyCombo.TabIndex = 24;
            this.caretLineTransparencyCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // selTransparencyCombo
            // 
            this.selTransparencyCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selTransparencyCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.selTransparencyCombo.FormattingEnabled = true;
            this.selTransparencyCombo.ItemHeight = 13;
            this.selTransparencyCombo.Location = new System.Drawing.Point(299, 150);
            this.selTransparencyCombo.Name = "selTransparencyCombo";
            this.selTransparencyCombo.Size = new System.Drawing.Size(128, 21);
            this.selTransparencyCombo.TabIndex = 31;
            this.selTransparencyCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // lineEndingsCombo
            // 
            this.lineEndingsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lineEndingsCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lineEndingsCombo.FormattingEnabled = true;
            this.lineEndingsCombo.Location = new System.Drawing.Point(12, 135);
            this.lineEndingsCombo.Name = "lineEndingsCombo";
            this.lineEndingsCombo.Size = new System.Drawing.Size(129, 21);
            this.lineEndingsCombo.TabIndex = 33;
            this.lineEndingsCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // lineEndingsLabel
            // 
            this.lineEndingsLabel.AutoSize = true;
            this.lineEndingsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lineEndingsLabel.Location = new System.Drawing.Point(9, 120);
            this.lineEndingsLabel.Name = "lineEndingsLabel";
            this.lineEndingsLabel.Size = new System.Drawing.Size(76, 13);
            this.lineEndingsLabel.TabIndex = 32;
            this.lineEndingsLabel.Text = "Line endin&gs:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.generalPage);
            this.tabControl.Controls.Add(this.selectionsPage);
            this.tabControl.Controls.Add(this.helpPage);
            this.tabControl.Controls.Add(this.caretPage);
            this.tabControl.Location = new System.Drawing.Point(15, 63);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(447, 284);
            this.tabControl.TabIndex = 34;
            // 
            // generalPage
            // 
            this.generalPage.Controls.Add(this.miscLabel);
            this.generalPage.Controls.Add(this.tabIndentsLabel);
            this.generalPage.Controls.Add(this.wrapCombo);
            this.generalPage.Controls.Add(this.wrapLabel);
            this.generalPage.Controls.Add(this.lineEndingsCombo);
            this.generalPage.Controls.Add(this.lineEndingsLabel);
            this.generalPage.Controls.Add(this.indentSizeCombo);
            this.generalPage.Controls.Add(this.indentModeCombo);
            this.generalPage.Controls.Add(this.indentSizeLabel);
            this.generalPage.Controls.Add(this.tabSizeLabel);
            this.generalPage.Controls.Add(this.useTabs);
            this.generalPage.Controls.Add(this.indentModeLabel);
            this.generalPage.Controls.Add(this.tabSizeCombo);
            this.generalPage.Location = new System.Drawing.Point(4, 22);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalPage.Size = new System.Drawing.Size(439, 258);
            this.generalPage.TabIndex = 0;
            this.generalPage.Text = "General";
            this.generalPage.UseVisualStyleBackColor = true;
            // 
            // miscLabel
            // 
            this.miscLabel.AutoSize = true;
            this.miscLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.miscLabel.Location = new System.Drawing.Point(9, 103);
            this.miscLabel.Name = "miscLabel";
            this.miscLabel.Size = new System.Drawing.Size(153, 13);
            this.miscLabel.TabIndex = 37;
            this.miscLabel.Text = "Line endings and wrapping:";
            // 
            // tabIndentsLabel
            // 
            this.tabIndentsLabel.AutoSize = true;
            this.tabIndentsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabIndentsLabel.Location = new System.Drawing.Point(9, 13);
            this.tabIndentsLabel.Name = "tabIndentsLabel";
            this.tabIndentsLabel.Size = new System.Drawing.Size(165, 13);
            this.tabIndentsLabel.TabIndex = 36;
            this.tabIndentsLabel.Text = "Tabs and indentation settings:";
            // 
            // wrapCombo
            // 
            this.wrapCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wrapCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.wrapCombo.FormattingEnabled = true;
            this.wrapCombo.Location = new System.Drawing.Point(156, 135);
            this.wrapCombo.Name = "wrapCombo";
            this.wrapCombo.Size = new System.Drawing.Size(129, 21);
            this.wrapCombo.TabIndex = 35;
            this.wrapCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // wrapLabel
            // 
            this.wrapLabel.AutoSize = true;
            this.wrapLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.wrapLabel.Location = new System.Drawing.Point(154, 120);
            this.wrapLabel.Name = "wrapLabel";
            this.wrapLabel.Size = new System.Drawing.Size(65, 13);
            this.wrapLabel.TabIndex = 34;
            this.wrapLabel.Text = "&Wrap lines:";
            // 
            // selectionsPage
            // 
            this.selectionsPage.Controls.Add(this.selStylingLabel);
            this.selectionsPage.Controls.Add(this.multipleSelectionsLabel);
            this.selectionsPage.Controls.Add(this.multipleSelectionsPaste);
            this.selectionsPage.Controls.Add(this.virtualSpace);
            this.selectionsPage.Controls.Add(this.selTransparencyCombo);
            this.selectionsPage.Controls.Add(this.mutipleSelectionsTyping);
            this.selectionsPage.Controls.Add(this.selTransparencyLabel);
            this.selectionsPage.Controls.Add(this.multipleSelections);
            this.selectionsPage.Controls.Add(this.selColorCombo);
            this.selectionsPage.Controls.Add(this.selColorLabel);
            this.selectionsPage.Controls.Add(this.selBackgroundCombo);
            this.selectionsPage.Controls.Add(this.selColor);
            this.selectionsPage.Controls.Add(this.selBackgroundLabel);
            this.selectionsPage.Location = new System.Drawing.Point(4, 22);
            this.selectionsPage.Name = "selectionsPage";
            this.selectionsPage.Padding = new System.Windows.Forms.Padding(3);
            this.selectionsPage.Size = new System.Drawing.Size(439, 258);
            this.selectionsPage.TabIndex = 1;
            this.selectionsPage.Text = "Selections";
            this.selectionsPage.UseVisualStyleBackColor = true;
            // 
            // selStylingLabel
            // 
            this.selStylingLabel.AutoSize = true;
            this.selStylingLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.selStylingLabel.Location = new System.Drawing.Point(9, 117);
            this.selStylingLabel.Name = "selStylingLabel";
            this.selStylingLabel.Size = new System.Drawing.Size(95, 13);
            this.selStylingLabel.TabIndex = 38;
            this.selStylingLabel.Text = "Selection styling:";
            // 
            // multipleSelectionsLabel
            // 
            this.multipleSelectionsLabel.AutoSize = true;
            this.multipleSelectionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.multipleSelectionsLabel.Location = new System.Drawing.Point(9, 13);
            this.multipleSelectionsLabel.Name = "multipleSelectionsLabel";
            this.multipleSelectionsLabel.Size = new System.Drawing.Size(152, 13);
            this.multipleSelectionsLabel.TabIndex = 37;
            this.multipleSelectionsLabel.Text = "Multiple selections settings:";
            // 
            // helpPage
            // 
            this.helpPage.Controls.Add(this.longLineLabel);
            this.helpPage.Controls.Add(this.guidesLabel);
            this.helpPage.Controls.Add(this.longLineColumnLabel);
            this.helpPage.Controls.Add(this.whiteSpace);
            this.helpPage.Controls.Add(this.indentationGuides);
            this.helpPage.Controls.Add(this.longLineIndicator);
            this.helpPage.Controls.Add(this.wrapIndicator);
            this.helpPage.Controls.Add(this.longLineIndicatorCombo);
            this.helpPage.Controls.Add(this.eol);
            this.helpPage.Location = new System.Drawing.Point(4, 22);
            this.helpPage.Name = "helpPage";
            this.helpPage.Padding = new System.Windows.Forms.Padding(3);
            this.helpPage.Size = new System.Drawing.Size(439, 258);
            this.helpPage.TabIndex = 3;
            this.helpPage.Text = "Visual Help";
            this.helpPage.UseVisualStyleBackColor = true;
            // 
            // longLineLabel
            // 
            this.longLineLabel.AutoSize = true;
            this.longLineLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.longLineLabel.Location = new System.Drawing.Point(9, 117);
            this.longLineLabel.Name = "longLineLabel";
            this.longLineLabel.Size = new System.Drawing.Size(59, 13);
            this.longLineLabel.TabIndex = 39;
            this.longLineLabel.Text = "Long line:";
            // 
            // guidesLabel
            // 
            this.guidesLabel.AutoSize = true;
            this.guidesLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.guidesLabel.Location = new System.Drawing.Point(9, 13);
            this.guidesLabel.Name = "guidesLabel";
            this.guidesLabel.Size = new System.Drawing.Size(90, 13);
            this.guidesLabel.TabIndex = 38;
            this.guidesLabel.Text = "Guides settings:";
            // 
            // longLineColumnLabel
            // 
            this.longLineColumnLabel.AutoSize = true;
            this.longLineColumnLabel.Enabled = false;
            this.longLineColumnLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.longLineColumnLabel.Location = new System.Drawing.Point(9, 133);
            this.longLineColumnLabel.Name = "longLineColumnLabel";
            this.longLineColumnLabel.Size = new System.Drawing.Size(99, 13);
            this.longLineColumnLabel.TabIndex = 20;
            this.longLineColumnLabel.Text = "Long line &column:";
            // 
            // caretPage
            // 
            this.caretPage.Controls.Add(this.label2);
            this.caretPage.Controls.Add(this.careLineLabel);
            this.caretPage.Controls.Add(this.stickyCaret);
            this.caretPage.Controls.Add(this.blinkLabel);
            this.caretPage.Controls.Add(this.blinkCombo);
            this.caretPage.Controls.Add(this.caretWidthLabel);
            this.caretPage.Controls.Add(this.caretWidthCombo);
            this.caretPage.Controls.Add(this.caretStyleLabel);
            this.caretPage.Controls.Add(this.caretStyleCombo);
            this.caretPage.Controls.Add(this.caretColorLabel);
            this.caretPage.Controls.Add(this.caretColorCombo);
            this.caretPage.Controls.Add(this.caretLineTransparencyCombo);
            this.caretPage.Controls.Add(this.caretLine);
            this.caretPage.Controls.Add(this.caretLineTransparencyLabel);
            this.caretPage.Controls.Add(this.caretLineColorLabel);
            this.caretPage.Controls.Add(this.caretLineColorCombo);
            this.caretPage.Location = new System.Drawing.Point(4, 22);
            this.caretPage.Name = "caretPage";
            this.caretPage.Padding = new System.Windows.Forms.Padding(3);
            this.caretPage.Size = new System.Drawing.Size(439, 258);
            this.caretPage.TabIndex = 2;
            this.caretPage.Text = "Caret";
            this.caretPage.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(9, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Caret settings:";
            // 
            // careLineLabel
            // 
            this.careLineLabel.AutoSize = true;
            this.careLineLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.careLineLabel.Location = new System.Drawing.Point(9, 13);
            this.careLineLabel.Name = "careLineLabel";
            this.careLineLabel.Size = new System.Drawing.Size(103, 13);
            this.careLineLabel.TabIndex = 39;
            this.careLineLabel.Text = "Caret line settings:";
            // 
            // stickyCaret
            // 
            this.stickyCaret.AutoSize = true;
            this.stickyCaret.Checked = true;
            this.stickyCaret.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stickyCaret.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.stickyCaret.Location = new System.Drawing.Point(12, 202);
            this.stickyCaret.Name = "stickyCaret";
            this.stickyCaret.Size = new System.Drawing.Size(104, 17);
            this.stickyCaret.TabIndex = 33;
            this.stickyCaret.Text = "Use &sticky caret";
            this.stickyCaret.ThreeState = true;
            this.stickyCaret.UseVisualStyleBackColor = true;
            this.stickyCaret.CheckedChanged += new System.EventHandler(this.CheckBoxCheckedChanged);
            // 
            // blinkLabel
            // 
            this.blinkLabel.AutoSize = true;
            this.blinkLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.blinkLabel.Location = new System.Drawing.Point(9, 160);
            this.blinkLabel.Name = "blinkLabel";
            this.blinkLabel.Size = new System.Drawing.Size(103, 13);
            this.blinkLabel.TabIndex = 32;
            this.blinkLabel.Text = "Caret &blink period:";
            // 
            // blinkCombo
            // 
            this.blinkCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blinkCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.blinkCombo.FormattingEnabled = true;
            this.blinkCombo.Location = new System.Drawing.Point(12, 176);
            this.blinkCombo.Name = "blinkCombo";
            this.blinkCombo.Size = new System.Drawing.Size(129, 21);
            this.blinkCombo.TabIndex = 31;
            this.blinkCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // caretWidthLabel
            // 
            this.caretWidthLabel.AutoSize = true;
            this.caretWidthLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretWidthLabel.Location = new System.Drawing.Point(293, 117);
            this.caretWidthLabel.Name = "caretWidthLabel";
            this.caretWidthLabel.Size = new System.Drawing.Size(70, 13);
            this.caretWidthLabel.TabIndex = 30;
            this.caretWidthLabel.Text = "Caret &width:";
            // 
            // caretWidthCombo
            // 
            this.caretWidthCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.caretWidthCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretWidthCombo.FormattingEnabled = true;
            this.caretWidthCombo.Location = new System.Drawing.Point(296, 133);
            this.caretWidthCombo.Name = "caretWidthCombo";
            this.caretWidthCombo.Size = new System.Drawing.Size(129, 21);
            this.caretWidthCombo.TabIndex = 29;
            this.caretWidthCombo.SelectedIndexChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // caretStyleLabel
            // 
            this.caretStyleLabel.AutoSize = true;
            this.caretStyleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretStyleLabel.Location = new System.Drawing.Point(151, 117);
            this.caretStyleLabel.Name = "caretStyleLabel";
            this.caretStyleLabel.Size = new System.Drawing.Size(63, 13);
            this.caretStyleLabel.TabIndex = 28;
            this.caretStyleLabel.Text = "Caret &style:";
            // 
            // caretStyleCombo
            // 
            this.caretStyleCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.caretStyleCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretStyleCombo.FormattingEnabled = true;
            this.caretStyleCombo.Location = new System.Drawing.Point(154, 133);
            this.caretStyleCombo.Name = "caretStyleCombo";
            this.caretStyleCombo.Size = new System.Drawing.Size(129, 21);
            this.caretStyleCombo.TabIndex = 27;
            this.caretStyleCombo.SelectedIndexChanged += new System.EventHandler(this.caretStyleCombo_SelectedIndexChanged);
            // 
            // caretColorLabel
            // 
            this.caretColorLabel.AutoSize = true;
            this.caretColorLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretColorLabel.Location = new System.Drawing.Point(9, 117);
            this.caretColorLabel.Name = "caretColorLabel";
            this.caretColorLabel.Size = new System.Drawing.Size(66, 13);
            this.caretColorLabel.TabIndex = 25;
            this.caretColorLabel.Text = "Caret &color:";
            // 
            // caretColorCombo
            // 
            this.caretColorCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.caretColorCombo.DropDownHeight = 320;
            this.caretColorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.caretColorCombo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.caretColorCombo.FormattingEnabled = true;
            this.caretColorCombo.IntegralHeight = false;
            this.caretColorCombo.ItemHeight = 15;
            this.caretColorCombo.Location = new System.Drawing.Point(12, 133);
            this.caretColorCombo.Name = "caretColorCombo";
            this.caretColorCombo.SelectedColor = null;
            this.caretColorCombo.Size = new System.Drawing.Size(129, 21);
            this.caretColorCombo.TabIndex = 26;
            this.caretColorCombo.SelectedValueChanged += new System.EventHandler(this.ComboSelectedIndexChanged);
            // 
            // TextEditorConfigPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.configsCombo);
            this.Controls.Add(this.settingsLabel);
            this.Name = "TextEditorConfigPage";
            this.Size = new System.Drawing.Size(474, 362);
            this.Load += new System.EventHandler(this.TextEditorConfigPage_Load);
            this.tabControl.ResumeLayout(false);
            this.generalPage.ResumeLayout(false);
            this.generalPage.PerformLayout();
            this.selectionsPage.ResumeLayout(false);
            this.selectionsPage.PerformLayout();
            this.helpPage.ResumeLayout(false);
            this.helpPage.PerformLayout();
            this.caretPage.ResumeLayout(false);
            this.caretPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Elide.Forms.DefaultComboBox configsCombo;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.CheckBox useTabs;
        private Elide.Forms.DefaultComboBox tabSizeCombo;
        private System.Windows.Forms.CheckBox multipleSelections;
        private System.Windows.Forms.CheckBox mutipleSelectionsTyping;
        private System.Windows.Forms.CheckBox multipleSelectionsPaste;
        private System.Windows.Forms.CheckBox virtualSpace;
        private Elide.Forms.ColorPicker selBackgroundCombo;
        private System.Windows.Forms.Label selTransparencyLabel;
        private System.Windows.Forms.CheckBox wrapIndicator;
        private System.Windows.Forms.CheckBox eol;
        private System.Windows.Forms.CheckBox whiteSpace;
        private System.Windows.Forms.CheckBox indentationGuides;
        private System.Windows.Forms.Label tabSizeLabel;
        private System.Windows.Forms.Label indentModeLabel;
        private Elide.Forms.DefaultComboBox indentModeCombo;
        private System.Windows.Forms.Label indentSizeLabel;
        private Elide.Forms.DefaultComboBox indentSizeCombo;
        private System.Windows.Forms.CheckBox longLineIndicator;
        private Elide.Forms.DefaultComboBox longLineIndicatorCombo;
        private System.Windows.Forms.Label selBackgroundLabel;
        private Elide.Forms.ColorPicker selColorCombo;
        private System.Windows.Forms.Label selColorLabel;
        private System.Windows.Forms.Label caretLineColorLabel;
        private Elide.Forms.ColorPicker caretLineColorCombo;
        private System.Windows.Forms.Label caretLineTransparencyLabel;
        private System.Windows.Forms.CheckBox caretLine;
        private System.Windows.Forms.CheckBox selColor;
        private Elide.Forms.DefaultComboBox caretLineTransparencyCombo;
        private Elide.Forms.DefaultComboBox selTransparencyCombo;
        private Elide.Forms.DefaultComboBox lineEndingsCombo;
        private System.Windows.Forms.Label lineEndingsLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage generalPage;
        private System.Windows.Forms.TabPage selectionsPage;
        private System.Windows.Forms.TabPage caretPage;
        private System.Windows.Forms.TabPage helpPage;
        private System.Windows.Forms.Label longLineColumnLabel;
        private System.Windows.Forms.Label caretColorLabel;
        private Elide.Forms.ColorPicker caretColorCombo;
        private System.Windows.Forms.Label caretWidthLabel;
        private Elide.Forms.DefaultComboBox caretWidthCombo;
        private System.Windows.Forms.Label caretStyleLabel;
        private Elide.Forms.DefaultComboBox caretStyleCombo;
        private System.Windows.Forms.Label blinkLabel;
        private Elide.Forms.DefaultComboBox blinkCombo;
        private System.Windows.Forms.CheckBox stickyCaret;
        private Elide.Forms.DefaultComboBox wrapCombo;
        private System.Windows.Forms.Label wrapLabel;
        private System.Windows.Forms.Label tabIndentsLabel;
        private System.Windows.Forms.Label miscLabel;
        private System.Windows.Forms.Label multipleSelectionsLabel;
        private System.Windows.Forms.Label selStylingLabel;
        private System.Windows.Forms.Label guidesLabel;
        private System.Windows.Forms.Label longLineLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label careLineLabel;
    }
}
