using System;
using System.IO;
using Elide.CodeEditor;
using Elide.CodeEditor.Infrastructure;
using Elide.Core;
using Elide.ElaCode.Configuration;
using Elide.ElaCode.ObjectModel;
using Elide.ElaCode.Views;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Environment.Views;
using Elide.Scintilla;
using Elide.TextEditor;
using Ela.Linking;
using Ela;
using Ela.CodeModel;
using System.Windows.Forms;

namespace Elide.ElaCode
{
    internal sealed class ElaFunctions
    {
        private readonly IApp app;
        private readonly ScintillaControl sci;

        internal ElaFunctions(IApp app, ScintillaControl sci)
        {
            this.app = app;
            this.sci = sci;
        }

        public void Run()
        {
            var src = sci.Text;

            if (sci.HasSelections())
                src = src + "\r\n_=()\r\n" + sci.GetSelection().Text;

            var asm = app.GetService<ICodeBuilderService>().
                RunBuilder<CompiledAssembly>(src, app.Document(), BuildOptions.Output | BuildOptions.ErrorList);

            if (asm != null)
                app.GetService<ICodeRunnerService>().
                    RunCode(asm, ExecOptions.Annotation | ExecOptions.PrintResult | ExecOptions.Console | ExecOptions.ShowOutput);
        }

        public void RunSelected()
        {
            Func<String,CompiledAssembly> fun = s => app.GetService<ICodeBuilderService>().
                 RunBuilder<CompiledAssembly>(s, app.Document(), BuildOptions.Output | BuildOptions.TipError, ElaCodeBuilder.NoDebug, ElaCodeBuilder.NoWarnings);
            var sel = sci.HasSelections() ? sci.GetSelection().Text : sci.GetLine(sci.CurrentLine).Text;
            var src = sci.Text + "\r\n_=()\r\n" + sel.Trim();            
            var asm = fun(src);

            if (asm == null)
                asm = fun(sel);

            if (asm != null)
            {
                var svc = app.GetService<ICodeRunnerService>();

                if (!svc.RunCode(asm, ExecOptions.PrintResult | ExecOptions.TipResult | ExecOptions.TipError | ExecOptions.LimitTime))
                {
                    asm = fun(sel);

                    if (asm != null)
                        svc.RunCode(asm, ExecOptions.PrintResult | ExecOptions.TipResult | ExecOptions.TipError);
                }
            }
        }

        public void MakeObjectFile()
        {
            var asm = app.GetService<ICodeBuilderService>().
               RunBuilder<CompiledAssembly>(sci.Text, app.Document(), BuildOptions.Output | BuildOptions.ErrorList, ElaCodeBuilder.ForceRecompile, ElaCodeBuilder.NoDebug);

            if (asm != null)
            {
                var fi = app.GetService<IDialogService>().ShowSaveDialog(app.Document().Title.Replace(".ela", String.Empty) + ".elaobj");

                if (fi != null)
                {
                    var wr = new ObjectFileWriter(fi.ToModuleFileInfo());
                    wr.Write(asm.Assembly.GetRootModule());
                }
            }
        }

        public void GenerateEil()
        {
            var cfg = app.Config<EilGeneratorConfig>();
            var opts = cfg.GenerateInDebugMode ? new ExtendedOption[] { ElaCodeBuilder.ForceRecompile }
                : new ExtendedOption[] { ElaCodeBuilder.NoDebug, ElaCodeBuilder.ForceRecompile };

            var asm = app.GetService<ICodeBuilderService>().
                RunBuilder<CompiledAssembly>(sci.Text, app.Document(), BuildOptions.Output | BuildOptions.ErrorList, opts);

            if (asm != null)
            {                
                var frame = asm.Assembly.GetRootModule();
                var gen = new EilGeneratorHelper(app);
                gen.PrintOffsets = cfg.IncludeCodeOffsets;
                gen.IgnoreDebugInfo = !cfg.GenerateInDebugMode;
                var src = gen.Generate(frame);

                var editor = (EditorInfo)app.GetService<IEditorService>().GetInfo("editors", "EilCode");

                var fi = new FileInfo(app.Document().Title);
                var doc = editor.Instance.CreateDocument(fi.Name.Replace(fi.Extension, editor.FileExtension));
                app.GetService<IDocumentService>().AddDocument(doc);
                ((ITextEditor)editor.Instance).SetContent(doc, src);
            }
        }

        public void FindSymbol()
        {
            var w = sci.GetWordAt(sci.CurrentPosition);
            app.GetService<ISymbolSearchService>().SearchSymbol(w, new SymbolFinder(app));
        }

        public void Autocomplete()
        {
            Autocomplete(sci.CurrentPosition);
        }
        
        public void Autocomplete(int position)
        {
            var am = new AutocompleteManager(app, sci);
            am.DoComplete(position, app.Document() as CodeDocument);
        }

        public void EvaluateSelected()
        {
            app.GetService<IViewService>().OpenView("ElaInteractive"); 
            var view = (InteractiveView)app.GetService<IViewService>().GetView("ElaInteractive");
            view.ResetSession();
            var sel = sci.HasSelections() ? sci.GetSelection().Text : sci.GetLine(sci.CurrentLine).Text;
            var src = sci.Text + "\r\n_=()\r\n" + sel;
            view.PrintLine();

            if (!view.RunCode(src, fastFail: true, onlyErrors: true))
                view.RunCode(sel, fastFail: false, onlyErrors: false);
        }

        public void EvaluateCurrentModule()
        {
            app.GetService<IViewService>().OpenView("ElaInteractive");
            var view = (InteractiveView)app.GetService<IViewService>().GetView("ElaInteractive");
            view.ResetSession();
            view.PrintLine();
            view.RunCode(sci.Text, fastFail: false, onlyErrors: false);
        }

        //TODO: not done
        public void GotoDefinition()
        {
            var am = new AutocompleteManager(app, sci);
            var fi = am.FindModule(sci.CurrentPosition, app.Document() as CodeDocument);

            if (fi != null)
                app.GetService<IDocumentNavigatorService>().Navigate(new VirtualDocument(fi), 0, 0, false);
        }

        public void GenerateAst()
        {
            var sel = sci.HasSelections() ? sci.GetSelection().Text : sci.Text;
            var ast = app.GetService<ICodeParserService>().RunParser<ElaAst>(sel, app.Document());

            if (ast != null && ast.Root != null)
            {
                app.GetService<IViewService>().OpenView("ElaAST");
                var view = (AstView)app.GetService<IViewService>().GetView("ElaAST");
                var node = ast.Root as ElaProgram;
                var builder = new AstTreeBuilder(node);
                builder.Build((AstControl)view.Control);
            }
        }
    }
}
