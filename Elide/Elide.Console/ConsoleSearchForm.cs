using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;

namespace Elide.Console
{
    public partial class ConsoleSearchForm : Form
    {
        private SearchResult lastResult;
        private string lastText;

        public ConsoleSearchForm()
        {
            InitializeComponent();
        }

        private void search_Click(object sender, EventArgs e)
        {
            App.GetService<IStatusBarService>().ClearStatusString();
            var sm = new SearchManager(Sci.GetText());
            var flags = caseSensitive.Checked ? SearchFlags.MatchCase : SearchFlags.None;
            lastResult = lastText != textBox.Text ? null : lastResult;

            lastResult = sm.Search(flags, textBox.Text, 
                lastResult != null ? lastResult.EndPosition : 0, Sci.GetTextLength());

            if (lastResult.Found)
            {
                lastText = textBox.Text;
                Sci.Select(lastResult.StartPosition, lastResult.EndPosition - lastResult.StartPosition, SelectionFlags.MakeOnlySelection | SelectionFlags.ScrollToCaret);
            }
            else
            {
                lastResult = null;
                App.GetService<IStatusBarService>().SetStatusString(StatusType.Warning, "There are no more occurences of '{0}' in the document.", textBox.Text);
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            App.GetService<IStatusBarService>().ClearStatusString();
            Close();
        }

        internal ScintillaControl Sci { get; set; }

        internal IApp App { get; set; }
    }
}
