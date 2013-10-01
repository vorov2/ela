using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Environment.Editors;
using Elide.Workbench.Configuration;
using Elide.Environment.Views;

namespace Elide.Workbench
{
    partial class MainForm
    {
        private void BuildDocumentTabMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
            var fs = App.GetService<IFileService>();
            var docs = App.GetService<IDocumentService>();
            Action openDir = () =>
            {
                var d = App.Document();

                if (d != null && d.FileInfo != null && d.FileInfo.Exists)
                {
                    var fi = d.FileInfo;
                    System.Diagnostics.Process.Start(fi.DirectoryName);
                }
            };

            var cm = builder
                .Item("Save Document", fs.Save, () => App.Document() != null && App.Document().IsDirty)
                .Separator()
                .Item("Close", fs.Close, () => App.Document() != null)
                .Item("Close All Other", fs.CloseAllOther, () => docs.EnumerateDocuments().Count() > 1)
                .Separator()
                .Item("Copy Full Path", () => Clipboard.SetText(App.Document().FileInfo.FullName), () => App.Document() != null && App.Document().FileInfo != null)
                .Item("Open Containing Directory", openDir, () => App.Document() != null && App.Document().FileInfo != null)
                .Finish();
            DocumentContainer.ContextMenuStrip = cm;
        }

        private void BuildMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<MenuStrip>(mainMenu);
            var fs = App.GetService<IFileService>();
            var ps = App.GetService<IPrintService>();
            var docs = App.GetService<IDocumentService>();
            Func<Boolean> hasDocs = () => docs.EnumerateDocuments().Any();
            Func<String,Action> openLink = s => () => App.GetService<IBrowserService>().OpenLink(new Uri(s));

            builder
                .Menu("&File")
                    .Menu("&New")
                        .Item(App.EditorInfo(EditorFlags.Main).DisplayName, "Ctrl+N", fs.NewMainFile)
                        .Items(BuildEditorList)
                        .CloseMenu()
                    .Item("&Open...", "Ctrl+O", fs.OpenFile)
                    .Separator()
                    .Item("&Close", fs.Close, hasDocs)
                    .Item("Cl&ose All", fs.CloseAll, hasDocs)
                    .Separator()
                    .Item("&Save", "Ctrl+S", fs.Save, () => docs.GetActiveDocument() != null && docs.GetActiveDocument().IsDirty)
                    .Item("Save &As...", fs.SaveAs, hasDocs)
                    .Item("Save A&ll", "Ctrl+Shift+S", fs.SaveAll, () => docs.EnumerateDocuments().Any(t => t.IsDirty))
                    .Separator()
                    .Item("Page Set&up...", () => ps.PageSetup(App.Document()), () => ps.IsPrintAvailable(App.Document()))
                    .Item("Print Pre&view...", () => ps.PrintPreview(App.Document()), () => ps.IsPrintAvailable(App.Document()))
                    .Item("&Print...", "Ctrl+Shift+P", () => ps.Print(App.Document()), () => ps.IsPrintAvailable(App.Document()))
                    .Separator()
                        .Menu("Recent &Files")
                        .ItemsDynamic(BuildRecentList)
                        .CloseMenu()
                    .Separator()
                    .Item("E&xit", Close)
                    .CloseMenu()
                .Menu("&View")
                    .Menu("&Output Windows")
                        .Items(BuildOutputList)
                        .CloseMenu()
                    .Separator()
                    .Items(BuildViewList)
                    .Separator()
                    .Item("Show Welcome Page", () => fs.NewFile("WelcomePage"))
                    .CloseMenu()
                .Menu("&Tools")
                    .Item("Options", () => App.GetService<IConfigDialogService>().ShowConfigDialog())
                    .CloseMenu()
                .Menu("&Window")
                    .Item("Close &Current", "Ctrl+F4", fs.Close, hasDocs)
                    .Item("Close All &Other", fs.CloseAllOther, () => docs.EnumerateDocuments().Count() > 1)
                    .Item("Close &All", fs.CloseAll, hasDocs)
                    .Separator()
                    .ItemsDynamic(BuildWindowList)
                    .Item("&Windows...", () => new OpenWindowsDialog { App = App }.ShowDialog(this), () => docs.EnumerateDocuments().Count() > 0)
                    .CloseMenu()
                .Menu("&Help")
                    .Items(BuildHelpList)
                    .Separator()
                    .Item("Ela &Home Page", openLink("http://elalang.net/"))
                    .Item("Ela Google &Code", openLink("http://elalang.googlecode.com/"))
                    .Separator()
                    .Item("&Submit a bug", openLink("http://code.google.com/p/elalang/issues/entry?template=Defect%20report%20from%20user"))
                    .Item("&Request a feature", openLink("http://code.google.com/p/elalang/issues/entry?template=Enhancement"))
                    .Separator()
                    .Item("&About Elide...", ShowAbout)
                .Finish();
        }

        private void ShowAbout()
        {
            var dlg = new AboutDialog { App = App };
            dlg.ShowDialog(this);
        }

        private void BuildWindowList(IMenuBuilder<MenuStrip> builder)
        {
            var srv = App.GetService<IDocumentService>();
            var seq = srv.EnumerateDocuments().Take(10).ToList();

            for (var i = 0; i < seq.Count; i++)
            {
                var doc = seq[i];
                builder.Item("&" + (i + 1) + ". " + seq[i], () => srv.SetActiveDocument(doc));
            }
        }

        private void BuildRecentList(IMenuBuilder<MenuStrip> builder)
        {
            var con = App.Config<WorkbenchConfig>();
            var recFiles = con.RecentFiles;

            if (recFiles == null || recFiles.Count == 0)
                builder.Item("[None]", null, () => false);
            else
            {
                for (var i = 0; i < recFiles.Count; i++)
                {
                    var idx = i;
                    builder.Item("&" + (i + 1) + ". " + recFiles[i], () =>
                        {
                            var fi = new FileInfo(recFiles[idx]);

                            if (!fi.Exists)
                            {
                                if (App.GetService<IDialogService>().ShowWarningDialog(
                                    "File '{0}' doesn't exist. Do you want to remove it from the recently used list?", fi.FullName))
                                    recFiles.RemoveAt(i);
                            }
                            else
                                App.GetService<IFileService>().OpenFile(fi);
                        });
                }
            }
        }

        private void BuildEditorList(IMenuBuilder<MenuStrip> builder)
        {
            App.GetService<IEditorService>().EnumerateInfos("editors").OfType<EditorInfo>()
                .Where(e => !e.Flags.Set(EditorFlags.Main) && !e.Flags.Set(EditorFlags.Hidden))
                .ForEach(e => builder.Item(e.DisplayName, () => App.GetService<IFileService>().NewFile(e.Key)));
        }

        private void BuildOutputList(IMenuBuilder<MenuStrip> builder)
        {
            var svc = App.GetService<IViewService>();
            svc.EnumerateInfos("views")
                .OfType<ViewInfo>()
                .Where(v => v.ViewType == ViewType.Output)
                .ForEach(v => builder.Item(v.Title, v.Shortcut, () => { if (svc.IsViewActive(v.Key)) svc.CloseView(v.Key); else svc.OpenView(v.Key); }, 
                    null, () => svc.IsViewActive(v.Key)));
        }

        private void BuildViewList(IMenuBuilder<MenuStrip> builder)
        {
            var svc = App.GetService<IViewService>();
            svc.EnumerateInfos("views")
                .OfType<ViewInfo>()
                .Where(v => v.ViewType == ViewType.Default)
                .ForEach(v => builder.Item(v.Title, v.Shortcut, () => { if (svc.IsViewActive(v.Key)) svc.CloseView(v.Key); else svc.OpenView(v.Key); }, 
                    null, () => svc.IsViewActive(v.Key)));
        }

        private void BuildHelpList(IMenuBuilder<MenuStrip> builder)
        {
            var svc = App.GetService<IViewService>();
            svc.EnumerateInfos("views")
                .OfType<ViewInfo>()
                .Where(v => v.ViewType == ViewType.Help)
                .ForEach(v => builder.Item(v.Title, v.Shortcut, () => { if (svc.IsViewActive(v.Key)) svc.CloseView(v.Key); else svc.OpenView(v.Key); },
                    null, () => svc.IsViewActive(v.Key)));
        }
    }
}
