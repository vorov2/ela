using System;
using System.Windows.Forms;

namespace Elide.HelpViewer
{
    public partial class HelpView : UserControl
    {
        public HelpView()
        {
            InitializeComponent();
        }

        private void WelcomePageView_Load(object sender, EventArgs e)
        {
            
        }

        public void SetContent(string content)
        {
            if (webBrowser.Document != null)
                webBrowser.Document.OpenNew(true);
            else
                webBrowser.Navigate("about:blank");

            webBrowser.DocumentText = content;
        }
    }
}
