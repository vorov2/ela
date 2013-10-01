using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Forms.State;
using Elide.Scintilla.ObjectModel;
using Elide.TextEditor;

namespace Elide.TextWorkbench.Dialogs
{
    public partial class SearchDialog : StateForm
    {
        private const int MAXITEMS = 50;

        public SearchDialog()
        {
            InitializeComponent();
        }

        private void replaceTextCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            replaceAll.Enabled = replace.Enabled = replaceTextCombo.Enabled = replaceTextCheckBox.Checked;
        }

        private void SearchDialog_Load(object sender, EventArgs e)
        {
            selection.Checked = IsTextSelected();
            currentDoc.Checked = !selection.Checked;
            App.GetService<IStatusBarService>().ClearStatusString();
        }

        private void SearchDialog_Activated(object sender, EventArgs e)
        {
            selection.Enabled = IsTextSelected();

            if (!selection.Enabled && selection.Checked)
                currentDoc.Checked = true;
        }

        private void findNext_Click(object sender, EventArgs e)
        {
            AddHistory(findTextCombo);
            var srv = App.GetService<ISearchService>();
            var doc = App.Document();
            srv.Search(findTextCombo.Text, doc, GetFlags(), GetScope());
        }

        private void findAll_Click(object sender, EventArgs e)
        {
            AddHistory(findTextCombo);
            var srv = App.GetService<ISearchService>();
            var doc = App.Document();
            srv.SearchAll(findTextCombo.Text, doc, GetFlags(), GetScope());
        }

        private void replace_Click(object sender, EventArgs e)
        {
            AddHistory(findTextCombo);
            AddHistory(replaceTextCombo);
            var srv = App.GetService<ISearchService>();
            var doc = App.Document();
            srv.Replace(findTextCombo.Text, replaceTextCombo.Text, doc, GetFlags(), GetScope());
        }

        private void replaceAll_Click(object sender, EventArgs e)
        {
            AddHistory(findTextCombo);
            AddHistory(replaceTextCombo);
            var srv = App.GetService<ISearchService>();
            var doc = App.Document();
            srv.ReplaceAll(findTextCombo.Text, replaceTextCombo.Text, doc, GetFlags(), GetScope());
        }
        
        private void SearchDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt || e.Control || e.Shift)
                return;

            if (e.KeyValue == 27) //Esc
                Close();
        }

        private SearchFlags GetFlags()
        {
            return SearchFlags.None
                | (matchCase.Checked ? SearchFlags.MatchCase : SearchFlags.None)
                | (matchWholeWord.Checked ? SearchFlags.WholeWord : SearchFlags.None)
                | (matchWordStart.Checked ? SearchFlags.WordStart : SearchFlags.None)
                | (regex.Checked ? SearchFlags.Regex : SearchFlags.None);
        }

        private SearchScope GetScope()
        {
            return currentDoc.Checked && searchUp.Checked ? SearchScope.WholeDocument :
                currentDoc.Checked && !searchUp.Checked ? SearchScope.Current :
                allDoc.Checked ? SearchScope.AllDocuments :
                selection.Checked ? SearchScope.Selection :
                SearchScope.None;
        }

        private bool IsTextSelected()
        {
            var te = (ITextEditor)App.Editor();
            return te.SelectionEnd != te.SelectionStart;
        }

        private void AddHistory(ComboBox combo)
        {
            if (combo.Items.IndexOf(combo.Text) != -1)
                return;

            if (combo.Items.Count == MAXITEMS)
                combo.Items.RemoveAt(combo.Items.Count - 1);

            combo.Items.Insert(0, combo.Text);
        }

        public bool ReplaceMode
        {
            get { return replaceTextCheckBox.Checked; }
            set
            {
                replaceTextCheckBox.Checked =
                    replaceAll.Enabled =
                    replace.Enabled =
                    replaceTextCombo.Enabled = value;
            }
        }

        public IApp App { get; set; }
        
        #region State
        [StateItem]
        public List<String> SearchStrings
        {
            get { return findTextCombo.Items.OfType<String>().ToList(); }
            set { value.ForEach(i => findTextCombo.Items.Add(i)); }
        }

        [StateItem]
        public List<String> ReplaceStrings
        {
            get { return replaceTextCombo.Items.OfType<String>().ToList(); }
            set { value.ForEach(i => replaceTextCombo.Items.Add(i)); }
        }

        [StateItem]
        public bool MatchCaseChecked
        {
            get { return matchCase.Checked; }
            set { 
                matchCase.Checked = value; 
            }
        }

        [StateItem]
        public bool MatchWholeWordChecked
        {
            get { return matchWholeWord.Checked; }
            set { matchWholeWord.Checked = value; }
        }

        [StateItem]
        public bool MatchWordStartChecked
        {
            get { return matchWordStart.Checked; }
            set { matchWordStart.Checked = value; }
        }

        [StateItem]
        public bool SearchUpChecked
        {
            get { return searchUp.Checked; }
            set { searchUp.Checked = value; }
        }

        [StateItem]
        public bool RegexChecked
        {
            get { return regex.Checked; }
            set { regex.Checked = value; }
        }
        #endregion
    }
}
