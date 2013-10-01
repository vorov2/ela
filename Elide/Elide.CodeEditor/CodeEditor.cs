using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Core;
using Elide.Environment;
using Elide.TextEditor;
using Elide.Scintilla.ObjectModel;
using Elide.Environment.Configuration;
using Elide.Scintilla;
using Elide.CodeEditor.Infrastructure;
using System.Windows.Forms;

namespace Elide.CodeEditor
{
    public abstract class CodeEditor<T> : AbstractTextEditor<T>, ICodeEditor where T : CodeDocument
    {
        private ICodeLexer lexer;

        protected CodeEditor(string editorKey) : base(editorKey)
        {
            
        }

        protected override void InternalInitialize()
        {
            base.InternalInitialize();
            
            var sci = GetScintilla();
            sci.TextChanged += TextChanged;
            sci.MouseDwell += MouseDwell;
            sci.MouseDwellEnd += (o,e) => sci.HideCallTip();
            sci.FoldNeeded += FoldNeeded;
            sci.CharAdded += CharAdded;
            sci.StyleNeeded += Lex;
            this.lexer = App.GetService<ICodeLexerService>().GetLexer(EditorKey);
            UpdateCodeEditorSettings();
        }

        private void CharAdded(object sender, KeyPressEventArgs e)
        {
            var cfg = GetConfig();

            if (cfg == null)
                return;

            if ((e.KeyChar == ' ' && cfg.ShowAutocompleteOnSpace) ||
                (e.KeyChar == '.' && cfg.ShowAutocompleteOnModule) ||
                (cfg.ShowAutocompleteOnChars && cfg.AutocompleteChars != null && cfg.AutocompleteChars.IndexOf(e.KeyChar) != -1))
                ShowAutocomplete(GetScintilla().CaretPosition - 1);
        }

        protected override void ConfigUpdated(Config config)
        {
            UpdateCodeEditorSettings(config);
        }

        protected virtual void UpdateCodeEditorSettings(Config config)
        {
            var cfg = config as CodeEditorConfig;

            if (cfg != null)
            {
                var sci = GetScintilla();

                sci.MatchBraces = cfg.MatchBraces;
                sci.RestyleDocument();

                var svc = (BackgroundCompilerService)App.GetService<IBackgroundCompilerService>();
                svc.EnableBackgroundCompilation = cfg.EnableBackgroundCompilation;
                svc.HighlightErrors = cfg.HighlightErrors;

                if ((!svc.HighlightErrors || !svc.EnableBackgroundCompilation) && App.Document() is CodeDocument)
                    App.GetService<IOutlinerService>().ClearOutline((CodeDocument)App.Document());
            }
        }

        protected virtual void UpdateCodeEditorSettings()
        {
            var cfg = GetConfig();
            UpdateCodeEditorSettings(cfg as CodeEditorConfig);            
        }

        private void FoldNeeded(object sender, FoldNeededEventArgs e)
        {
            var cfg = GetConfig();

            if (cfg != null && cfg.EnableFolding)
                FoldNeeded(e);
        }

        private void TextChanged(object sender, EventArgs e)
        {
            var d = Doc();

            if (d != null)
                d.Version++;
        }

        private void MouseDwell(object sender, DwellEventArgs e)
        {
            CallTipRequested(e.X, e.Y, e.Position);
        }

        protected virtual void CallTipRequested(int x, int y, int position)
        {

        }


        protected override void OnDocumentCreateInstance(T doc)
        {
            doc.CodeEditor = App.EditorInfo(doc);
        }

        protected abstract CodeEditorConfig GetConfig();

        protected virtual void ShowAutocomplete(int position)
        {

        }
        
        protected virtual void FoldNeeded(FoldNeededEventArgs e)
        {

        }
        
        private void Lex(object sender, StyleNeededEventArgs e)
        {
            if (lexer != null)
                foreach (var t in lexer.Parse(e.Text))
                    e.AddStyleItem(t.Position, t.Length, t.StyleKey);
        }

        private CodeDocument Doc()
        {
            return App.Document() as CodeDocument;
        }
    }
}
