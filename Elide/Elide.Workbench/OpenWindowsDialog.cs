using System;
using System.Windows.Forms;
using Elide.Core;

namespace Elide.Environment
{
    public partial class OpenWindowsDialog : Form
    {
        public OpenWindowsDialog()
        {
            InitializeComponent();
        }

        private void OpenWindowsDialog_Load(object sender, EventArgs e)
        {
            BuildList();
        }

        private void windowsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (windowsList.SelectedIndex != -1)
            {
                var d = (Document)windowsList.SelectedItem;
                activate.Enabled = App.Document() != d;
                close.Enabled = true;
                save.Enabled = d.IsDirty;
            }
            else
                activate.Enabled = close.Enabled = save.Enabled = false;
        }

        private void activate_Click(object sender, EventArgs e)
        {
            App.GetService<IDocumentService>().SetActiveDocument((Document)windowsList.SelectedItem);
            windowsList_SelectedIndexChanged(windowsList, EventArgs.Empty);
        }

        private void save_Click(object sender, EventArgs e)
        {
            App.GetService<IFileService>().Save((Document)windowsList.SelectedItem);
            BuildList();
        }

        private void close_Click(object sender, EventArgs e)
        {
            App.GetService<IFileService>().Close((Document)windowsList.SelectedItem);
            BuildList();
        }

        private void BuildList()
        {
            windowsList.Items.Clear();
            App.GetService<IDocumentService>().EnumerateDocuments().ForEach(d => windowsList.Items.Add(d));
            windowsList.SelectedIndex = -1;
            windowsList_SelectedIndexChanged(windowsList, EventArgs.Empty);
        }
        
        public IApp App { get; set; }
    }
}
