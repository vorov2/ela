using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Forms;
using Elide.Scintilla.ObjectModel;
using Elide.TextEditor.Configuration;
using Elide.Environment.Configuration;

namespace Elide.TextWorkbench.Configuration
{
    public partial class TextEditorConfigPage : UserControl, IOptionPage
    {
        private bool noevents;

        private sealed class Value
        {
            public Value(string text, int value)
            {
                Text = text;
                Val = value;                
            }

            public override string ToString()
            {
                return Text;
            }

            public int Val;
            public string Text;
        }
        
        public TextEditorConfigPage()
        {
            InitializeComponent();
            InitializeControlValues();
        }

        private void InitializeControlValues()
        {
            noevents = true;
            tabSizeCombo.Populate(2, 3, 4, 5, 6, 7, 8);           
            indentSizeCombo.Populate(2, 3, 4, 5, 6, 7, 8);
            indentModeCombo.Populate(IndentMode.None, IndentMode.Block, IndentMode.Smart);
            caretLineTransparencyCombo.Populate(new Value("No", 255), new Value("Low", 200), new Value("Average", 150), new Value("High", 100), new Value("Very High", 50), new Value("Transparent", 10));
            selTransparencyCombo.Populate(new Value("No", 255), new Value("Low", 200), new Value("Average", 150), new Value("High", 100), new Value("Very High", 50), new Value("Transparent", 10));
            lineEndingsCombo.Populate(LineEndings.Windows, LineEndings.Mac, LineEndings.Unix);
            caretStyleCombo.Populate(CaretStyle.Line, CaretStyle.Block);
            caretWidthCombo.Populate(CaretWidth.Thin, CaretWidth.Medium, CaretWidth.Thick);
            wrapCombo.Populate(WordWrapMode.None, WordWrapMode.Char, WordWrapMode.Word);
            longLineIndicatorCombo.Populate(Enumerable.Range(10, 100).OfType<Object>().ToArray());
            blinkCombo.Populate(200, 500, 700, 1000, 1500, 2000);
            
            caretLineColorCombo.Populate();
            selBackgroundCombo.Populate();
            selColorCombo.Populate();
            caretColorCombo.Populate();
            noevents = false;
        }
        
        private void TextEditorConfigPage_Load(object sender, EventArgs e)
        {
            var srv = App.GetService<ITextConfigService>();
            configsCombo.Items.Add(new TextConfigInfo("Default", "Default", TextConfigOptions.None));

            configsCombo.Items.AddRange(srv.EnumerateInfos("textConfigs").OfType<Object>().ToArray());            
            configsCombo.SelectedIndex = 0;
        }

        private void useTabs_CheckedChanged(object sender, EventArgs e)
        {
            tabSizeLabel.Enabled = tabSizeCombo.Enabled = useTabs.Checked;
            ApplySettings();
        }

        private void caretLine_CheckedChanged(object sender, EventArgs e)
        {
            caretLineColorLabel.Enabled = caretLineColorCombo.Enabled = caretLineTransparencyLabel.Enabled = caretLineTransparencyCombo.Enabled = caretLine.Checked;
            ApplySettings();
        }

        private void selColor_CheckedChanged(object sender, EventArgs e)
        {
            selColorLabel.Enabled = selColorCombo.Enabled = selColor.Checked;
            ApplySettings();
        }

        private void longLineIndicator_CheckedChanged(object sender, EventArgs e)
        {
            longLineColumnLabel.Enabled = longLineIndicatorCombo.Enabled = longLineIndicator.Checked;
            ApplySettings();
        }

        private void caretStyleCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            caretWidthLabel.Enabled = caretWidthCombo.Enabled = caretStyleCombo.SelectedItem is CaretStyle
                && (CaretStyle)caretStyleCombo.SelectedItem == CaretStyle.Line;
            ApplySettings();
        }

        private void ApplySettings()
        {
            if (noevents)
                return;

            var cfg = CurrentConfig();
            cfg.CaretLineAlpha = GetValue<Int32>(caretLineTransparencyCombo);
            cfg.CaretColor = caretColorCombo.SelectedColor;
            cfg.CaretLineColor = caretLineColorCombo.SelectedColor;
            cfg.CaretLineVisible = GetValue(caretLine);
            cfg.CaretStyle = GetValue<CaretStyle>(caretStyleCombo);
            cfg.CaretWidth = GetValue<CaretWidth>(caretWidthCombo);
            cfg.CaretBlinkPeriod = GetValue<Int32>(blinkCombo);
            cfg.CaretSticky = GetValue(stickyCaret);
            cfg.IndentationGuides = GetValue(indentationGuides);
            cfg.LineEndings = GetValue<LineEndings>(lineEndingsCombo);
            cfg.LongLine = GetValue<Int32>(longLineIndicatorCombo);
            cfg.LongLineIndicator = GetValue(longLineIndicator);
            cfg.MultipleSelection = GetValue(multipleSelections);
            cfg.MultipleSelectionPaste = GetValue(multipleSelectionsPaste);
            cfg.MultipleSelectionTyping = GetValue(mutipleSelectionsTyping);
            cfg.SelectionBackColor = selBackgroundCombo.SelectedColor;
            cfg.SelectionForeColor = selColorCombo.SelectedColor;
            cfg.SelectionTransparency = GetValue<Int32>(selTransparencyCombo);
            cfg.TabSize = GetValue<Int32>(tabSizeCombo);
            cfg.IndentSize = GetValue<Int32>(indentSizeCombo);
            cfg.IndentMode = GetValue<IndentMode>(indentModeCombo);
            cfg.UseSelectionColor = GetValue(selColor);
            cfg.UseTabs = GetValue(useTabs);
            cfg.VirtualSpace = GetValue(virtualSpace);
            cfg.VisibleLineEndings = GetValue(eol);
            cfg.VisibleWhiteSpace = GetValue(whiteSpace);
            cfg.WordWrap = GetValue<WordWrapMode>(wrapCombo);
            cfg.WordWrapIndicator = GetValue(wrapIndicator);
        }

        private void configsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            noevents = true;
            var cfg = CurrentConfig();
            var def = DefaultConfig();           
            EnableDefault(cfg != def, def);

            var data = (TextConfigInfo)configsCombo.SelectedItem;

            if ((data.Options & TextConfigOptions.RestrictTabs) == TextConfigOptions.RestrictTabs)
                useTabs.Enabled = useTabs.Checked = false;
            else
                useTabs.Enabled = true;

            if ((data.Options & TextConfigOptions.RestrictWordWrap) == TextConfigOptions.RestrictWordWrap)
            {
                wrapCombo.Enabled = wrapLabel.Enabled = wrapIndicator.Enabled = wrapIndicator.Checked = false;
                wrapCombo.SelectedItem = wrapCombo.Tag;
            }
            else
                wrapCombo.Enabled = wrapLabel.Enabled = wrapIndicator.Enabled = true;

            Select(caretLineTransparencyCombo, cfg.CaretLineAlpha);
            caretLineColorCombo.SelectedColor = cfg.CaretLineColor;
            caretColorCombo.SelectedColor = cfg.CaretColor;
            Select<CaretStyle>(caretStyleCombo, cfg.CaretStyle);
            Select<CaretWidth>(caretWidthCombo, cfg.CaretWidth);
            Select(caretLine, cfg.CaretLineVisible);
            Select(stickyCaret, cfg.CaretSticky);
            Select(blinkCombo, cfg.CaretBlinkPeriod);
            Select(indentationGuides, cfg.IndentationGuides);
            Select<LineEndings>(lineEndingsCombo, cfg.LineEndings);
            Select(longLineIndicatorCombo, cfg.LongLine);
            Select(longLineIndicator, cfg.LongLineIndicator);
            Select(multipleSelections, cfg.MultipleSelection);
            Select(multipleSelectionsPaste, cfg.MultipleSelectionPaste);
            Select(mutipleSelectionsTyping, cfg.MultipleSelectionTyping);
            selBackgroundCombo.SelectedColor = cfg.SelectionBackColor;
            selColorCombo.SelectedColor = cfg.SelectionForeColor;
            Select(selTransparencyCombo, cfg.SelectionTransparency);
            Select(indentSizeCombo, cfg.IndentSize);
            Select<IndentMode>(indentModeCombo, cfg.IndentMode);
            Select(selColor, cfg.UseSelectionColor);

            if ((data.Options & TextConfigOptions.RestrictTabs) != TextConfigOptions.RestrictTabs)
            {
                Select(useTabs, cfg.UseTabs);
                Select(tabSizeCombo, cfg.TabSize);            
            }

            Select(virtualSpace, cfg.VirtualSpace);
            Select(eol, cfg.VisibleLineEndings);
            Select(whiteSpace, cfg.VisibleWhiteSpace);

            if ((data.Options & TextConfigOptions.RestrictWordWrap) != TextConfigOptions.RestrictWordWrap)
            {
                Select<WordWrapMode>(wrapCombo, cfg.WordWrap);
                Select(wrapIndicator, cfg.WordWrapIndicator);
            }

            noevents = false;
        }

        private void Select<T>(DefaultComboBox comboBox, T? value) where T : struct 
        {
            if (value == null)
                comboBox.SelectedItem = comboBox.DefaultItem;
            else
            {
                if (comboBox == selTransparencyCombo || comboBox == caretLineTransparencyCombo)
                {
                    var val = comboBox.Items.OfType<Value>().First(i => i.Val.Equals(value.Value));
                    comboBox.SelectedItem = val;
                }
                else
                    comboBox.SelectedItem = value.Value;
            }
        }

        private void Select(CheckBox checkBox, bool? flag)
        {
            checkBox.CheckState = flag == null ? CheckState.Indeterminate :
                (flag.Value ? CheckState.Checked : CheckState.Unchecked);
        }

        private bool? GetValue(CheckBox checkBox)
        {
            if (!checkBox.ThreeState)
                return checkBox.Checked;
            else
                return checkBox.CheckState == CheckState.Indeterminate ? (bool?)null :
                    (bool?)(checkBox.CheckState == CheckState.Checked);
        }

        private T? GetValue<T>(ComboBox comboBox) where T : struct
        {
            var sel = comboBox.SelectedItem;

            if (sel is DefaultItem || sel == null)
                return null;
            else if (sel is Value)
                return (T)(Object)((Value)sel).Val;
            else
                return (T)sel;
        }

        private void EnableDefault(bool enable, TextConfig def)
        {
            caretColorCombo.DefaultColor = def.CaretColor.Value;
            caretLineColorCombo.DefaultColor = def.CaretLineColor.Value;
            selBackgroundCombo.DefaultColor = def.SelectionBackColor.Value;
            selColorCombo.DefaultColor = def.SelectionForeColor.Value;

            var seq = GetControls();

            seq.Where(c => c is CheckBox).OfType<CheckBox>()
               .ForEach(c => c.ThreeState = enable);

            seq.Where(c => c is ColorPicker).OfType<ColorPicker>()
               .ForEach(c => c.SetShowDefault(enable));

            if (!enable)
                seq.Where(c => c is DefaultComboBox).OfType<DefaultComboBox>()
                   .ForEach(c => c.RemoveDefault());
            else
            {
                tabSizeCombo.AddDefault(def.TabSize.ToString());
                indentSizeCombo.AddDefault(def.IndentSize.ToString());
                indentModeCombo.AddDefault(def.IndentMode.ToString());
                longLineIndicatorCombo.AddDefault(def.LongLine.ToString());
                lineEndingsCombo.AddDefault(def.LineEndings.ToString());
                caretStyleCombo.AddDefault(def.CaretStyle.ToString());
                caretWidthCombo.AddDefault(def.CaretWidth.ToString());
                blinkCombo.AddDefault(def.CaretBlinkPeriod.ToString());
                caretLineTransparencyCombo.AddDefault(caretLineTransparencyCombo.Items.OfType<Object>().Where(o => o is Value).OfType<Value>().First(v => v.Val == def.CaretLineAlpha).Text);
                selTransparencyCombo.AddDefault(selTransparencyCombo.Items.OfType<Object>().Where(o => o is Value).OfType<Value>().First(v => v.Val == def.SelectionTransparency).Text);
                wrapCombo.AddDefault(def.WordWrap.ToString());
            }
        }

        private IEnumerable<Control> GetControls()
        {
            var ret = new List<Control>();

            foreach (var t in tabControl.TabPages.OfType<TabPage>())
                ret.AddRange(t.Controls.OfType<Control>());

            return ret;
        }

        private TextConfig CurrentConfig()
        {
            var k = ((TextConfigInfo)configsCombo.SelectedItem).Key;
            return k == "Default" ? DefaultConfig() : ((TextEditorsConfig)Config).Configs[k];
        }

        private TextConfig DefaultConfig()
        {
            return ((TextEditorsConfig)Config).Default;
        }

        private void ComboSelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private void CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            ApplySettings();
        }

        public IApp App { get; set; }

        public Config Config { get; set; }
    }
}
