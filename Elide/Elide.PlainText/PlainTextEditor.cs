using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Elide.Configuration;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.PlainText.Configuration;
using Elide.PlainText.Images;
using Elide.Scintilla;
using Elide.TextEditor;

namespace Elide.PlainText
{
    public sealed class PlainTextEditor : AbstractTextEditor<PlainTextDocument>
    {
        public PlainTextEditor() : base("PlainText")
        {

        }

        public override bool TestDocumentType(FileInfo fileInfo)
        {
            return fileInfo != null && fileInfo.HasExtension("txt");
        }

        protected override void InternalInitialize()
        {
            var sci = GetScintilla();
            var srv = App.GetService<IStylesConfigService>();
            srv.EnumerateStyleItems("PlainText").UpdateStyles(sci);
            UpdateTextEditorSettings();

            sci.UseUnicodeLexing = true;
            sci.StyleNeeded += Lex;
            sci.LinkClicked += LinkClicked;
            sci.LinkDoubleClicked += LinkDoubleClicked;
        }

        private void LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (App.Config<PlainTextConfig>().SingleClickNavigation)
                OpenLink(e);
        }

        private void LinkDoubleClicked(object sender, LinkClickedEventArgs e)
        {
            if (!App.Config<PlainTextConfig>().SingleClickNavigation)
                OpenLink(e);
        }

        private void OpenLink(LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.Link);
            }
            catch { }
        }

        private void Lex(object sender, StyleNeededEventArgs e)
        {
            var con = App.Config<PlainTextConfig>();

            if (!con.HighlightBoldItalics && !con.HighlightHyperlinks)
                return;

            var lex = new PlainTextLexer
            {
                Hyperlinks = con.HighlightHyperlinks,
                Bold = con.HighlightBoldItalics,
                Italic = con.HighlightBoldItalics
            };
            lex.Parse(e.Text).ForEach(s => e.AddStyleItem(s.Position, s.Length, s.StyleKey));
        }

        protected override void ConfigUpdated(Config config)
        {
            
        }

        public override Image DocumentIcon
        {
            get { return Elide.Forms.Bitmaps.Load<NS>("Icon"); }
        }
    }
}
