using System;
using System.Windows.Forms;
using Elide.CodeEditor;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.ElaCode.Configuration
{
    public partial class ElaEditorConfigPage : UserControl, IOptionPage
    {
        private bool noevents;
        
        public ElaEditorConfigPage()
        {
            InitializeComponent();
        }

        private void ElaEditorConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            var cfg = GetConfig();
            folding.Checked = cfg.EnableFolding;
            braces.Checked = cfg.MatchBraces;
            compile.Checked = cfg.EnableBackgroundCompilation;
            highlight.Checked = cfg.HighlightErrors;
            space.Checked = cfg.ShowAutocompleteOnSpace;
            chars.Checked = cfg.ShowAutocompleteOnChars;
            charsBox.Text = cfg.AutocompleteChars;
            exportList.Checked = cfg.ShowAutocompleteOnModule;
            noevents = false;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            var cfg = GetConfig();
            highlight.Enabled = compile.Checked;
            charsBox.Enabled = chars.Checked;

            if (noevents)
                return;

            cfg.EnableFolding = folding.Checked;
            cfg.MatchBraces = braces.Checked;
            cfg.EnableBackgroundCompilation = compile.Checked;
            cfg.HighlightErrors = highlight.Checked;
            cfg.ShowAutocompleteOnSpace = space.Checked;
            cfg.ShowAutocompleteOnChars = chars.Checked;
            cfg.ShowAutocompleteOnModule = exportList.Checked;
        }

        private void charsBox_TextChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            GetConfig().AutocompleteChars = charsBox.Text;
        }

        private CodeEditorConfig GetConfig()
        {
            return (CodeEditorConfig)Config;
        }

        public Config Config { get; set; }

        public IApp App { get; set; }
    }
}
