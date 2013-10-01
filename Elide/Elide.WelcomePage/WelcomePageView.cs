using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Forms;
using Elide.Main;
using Elide.TextEditor;
using Elide.WelcomePage.Images;
using Elide.Workbench.Configuration;

namespace Elide.WelcomePage
{
    public partial class WelcomePageView : UserControl
    {
        public WelcomePageView()
        {
            InitializeComponent();
        }

        private void WelcomePageView_Load(object sender, EventArgs e)
        {
            BackColor = Color.White;
            BackgroundImage = Bitmaps.Load<NS>("WelcomePage");
            BackgroundImageLayout = ImageLayout.None;
            version.Text = String.Format(version.Text, ElideInfo.Version, ElideInfo.VersionType);
            dontShow.Checked = !App.Config<WorkbenchConfig>().ShowWelcomePage;
            App.GetService<IConfigService>().ConfigUpdated += (o,ev) =>
            {
                dontShow.Checked = !App.Config<WorkbenchConfig>().ShowWelcomePage;
            };
        }
        
        private void configure_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            App.GetService<IConfigDialogService>().ShowConfigDialog();
        }

        private void codeSamples_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            App.OpenView("CodeSamples");
        }

        private void newFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            App.GetService<IFileService>().NewFile("ElaCode");
            var editor = (ITextEditor)App.Editor();
            editor.SetContent(App.Document(), "open list\r\n\r\nmap (*2) [1,3..42]");
        }

        private void onlineConsoleLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://elalang.net/elac.aspx");
        }

        private void docsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            App.OpenView("Documentation");
        }

        private void codeSamplesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            App.OpenView("CodeSamples");
        }

        private void whatsNewLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fi = new FileInfo(Path.Combine(App.GetService<IPathService>().GetPath(PlatformPath.Docs), "whatsnew.htm"));
            App.GetService<IFileService>().OpenFile(fi);            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://www.codeproject.com/Articles/158068/Ela-functional-programming-language");
        }

        private void book_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://elalang.net/elabook.aspx");
        }

        private void homepageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://elalang.net");
        }

        private void rosettaLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("http://rosettacode.org/wiki/Ela");
        }

        private void ohlohLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://www.ohloh.net/p/ela");
        }

        private void groupLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://groups.google.com/forum/?fromgroups#!forum/elalang");
        }

        private void dontShow_CheckedChanged(object sender, EventArgs e)
        {
            App.Config<WorkbenchConfig>().ShowWelcomePage = !dontShow.Checked;
        }

        private void OpenLink(string link)
        {
            App.GetService<IBrowserService>().OpenLink(new Uri(link));
        }

        internal IApp App { get; set; }
    }
}
