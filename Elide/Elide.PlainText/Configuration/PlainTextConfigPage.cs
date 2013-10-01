using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.PlainText.Configuration
{
    public partial class PlainTextConfigPage : UserControl, IOptionPage
    {
        private bool noevents;
        
        public PlainTextConfigPage()
        {
            InitializeComponent();
        }

        private void PlainTextConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            hyperlinks.Checked = Conf().HighlightHyperlinks;
            singleClickNavigation.Checked = Conf().SingleClickNavigation;
            boldItalics.Checked = Conf().HighlightBoldItalics;
            noevents = false;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            Conf().HighlightHyperlinks = hyperlinks.Checked;
            Conf().SingleClickNavigation = singleClickNavigation.Checked;
            Conf().HighlightBoldItalics = boldItalics.Checked;
        }

        private PlainTextConfig Conf()
        {
            return (PlainTextConfig)Config;
        }

        public IApp App { get; set; }

        public Config Config { get; set; }
    }
}
