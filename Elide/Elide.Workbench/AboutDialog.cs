using System;
using System.IO;
using System.Windows.Forms;
using Elide.Core;
using Elide.Main;
using System.Text;
using System.Diagnostics;

namespace Elide.Workbench
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        public IApp App { get; set; }

        private void AboutDialog_Load(object sender, EventArgs e)
        {
            Icon = WB.Form.Icon;
            header.Text = String.Format(header.Text, ElideInfo.Title);
            progInfo.Text = String.Format(progInfo.Text, ElideInfo.Title, ElideInfo.Version, ElideInfo.VersionType, 
                System.Environment.NewLine + ElideInfo.Copyright);
            infoBox.Text =
@"
This computer program and its source code are distributed under the terms of GNU GPL v2 license. Please refer to the text of the license for additional details. If a license file is not provided with your copy of the program you can obtain it from GNU site: http://www.gnu.org/licenses/gpl-2.0.html.

This computer program uses the following open source components:
• Scintilla editing component (http://scintilla.org)
• Parts of code from ScintillaNet (http://scintillanet.codeplex.com)
";

            var path = App.GetService<IPathService>().GetPath(PlatformPath.Root);
            var fi = new FileInfo(Path.Combine(path, "elide_log.txt"));

            if (fi.Exists)
            {
                try
                {
                    var sb = new StringBuilder();
                    var line = String.Empty;

                    using (var sr = fi.OpenText())
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.IndexOf("Release") == -1)
                            {
                                var nl = line.Substring(line.IndexOf(':') + 1);
                                var idx = nl.IndexOf(':');
                                var type = nl.Substring(0, idx);
                                sb.Append("• ");
                                sb.AppendFormat(@"\i {0}. \i0", type);
                                sb.Append(nl.Substring(idx + 1));
                                sb.Append(@" \par");
                            }
                            else
                            {
                                var ver = line.Substring(0, line.IndexOf(':'));
                                sb.Append(@" \par");
                                sb.AppendFormat(@"\b Release of {0}\b0", ver);
                                sb.Append(@" \par");
                            }
                        }

                        changeLog.Rtf = 
@"{\rtf1\ansi\ansicpg1251\deff0\deflang1049{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}}
\viewkind4\uc1\pard\f0\fs17 " +
                            sb.ToString() + " }\0";
                    }
                }
                catch { }
            }
        }

        private void infoBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch { }
        }
    }
}
