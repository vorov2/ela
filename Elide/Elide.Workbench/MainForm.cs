using System;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using Elide.Forms.State;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Environment.Editors;
using Elide.Workbench.Configuration;
using Elide.TextEditor;
using Elide.Environment.Views;
using Elide.Workbench.ExceptionHandling;

namespace Elide.Workbench
{
    public partial class MainForm : StateForm
    {        
        public MainForm()
        {
            InitializeComponent();
            _outputPanelSize = 200;
            _toolPanelSize = 300;
        }

        internal void Initialize(IApp app)
        {
            topBorderPanel.BackColor = toolDock.BackColor = mainSplit.BackColor = UserColors.Background;
            mainSplit.SplitterDistance = mainSplit.ClientSize.Height - mainSplit.Panel2MinSize - mainSplit.SplitterWidth;
            
            App = app;
            documentContainer.SelectedDocumentFunc = App.Document;
            documentContainer.DocumentCaptionRedraw += (o,e) => UpdateWindowHeader();
            App.GetService<IDocumentService>().DocumentClosed += (o,e) => UpdateWindowHeader();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        } 

        private void MainForm_Load(object sender, EventArgs e)
        {
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            AppDomain.CurrentDomain.UnhandledException += (o,ev) => ExceptionManager.Process(App, ev.ExceptionObject as Exception);

            BuildMenu();
            BuildDocumentTabMenu();
            BuildViews();

            var con = App.Config<WorkbenchConfig>();
            var fs = App.GetService<IFileService>();

            if (con.ShowWelcomePage)
                fs.NewFile("WelcomePage");

            if (con.StartWithBlank)
                fs.NewFile("ElaCode");
            else if (con.RememberOpenFiles && LastSessionFiles != null)
            {
                LastSessionFiles
                    .Select(f => new FileInfo(f))
                    .Where(f => f.Exists)
                    .ForEach(f => fs.OpenFile(f));

                if (!String.IsNullOrEmpty(LastActiveDocument))
                {
                    var srv = App.GetService<IDocumentService>();
                    var actDoc = srv.GetOpenedDocument(new FileInfo(LastActiveDocument));

                    if (actDoc != null)
                        srv.SetActiveDocument(actDoc);
                }

                LastSessionFiles = null;
            }

            mainSplit.IsSplitterFixed = true;
            toolDock.Panel2Collapsed = true;

            if (App.Config<WorkbenchConfig>().RememberOpenTools)
            {
                if (OpenedTool != null)
                    App.GetService<IViewService>().OpenView(OpenedTool);

                if (OpenedOutput != null)
                    App.GetService<IViewService>().OpenView(OpenedOutput);
            }

            if (App.GetCommandLineArguments().Count() > 0)
            {
                var fi = new FileInfo(App.GetCommandLineArguments().First().Trim('"'));

                if (fi.Exists)
                    App.GetService<IFileService>().OpenFile(fi);
            }
        }

        private void BuildViews()
        {
            var svc = App.GetService<IViewService>();
            svc.EnumerateInfos("views")
                .OfType<ViewInfo>()
                .Where(t => t.ViewType == ViewType.Output)
                .ForEach(t =>
                {
                    var hdl = outputsBar.AddItem(t.Title, t);
                    svc.GetView(t.Key).ContentChanged += (o, e) => hdl(e.NewContent, e.ContentDescription);
                });

            svc.EnumerateInfos("views")
                .OfType<ViewInfo>()
                .Where(t => t.ViewType != ViewType.Output)
                .ForEach(t => toolWindow.Items.Add(new SwitchBarItem { Caption = t.Title, Tag = t }));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (App.Config<WorkbenchConfig>().RememberOpenTools)
            {
                OpenedOutput = outputsBar.SelectedTag != null ? ((ViewInfo)outputsBar.SelectedTag).Key : null;
                OpenedTool = toolWindow.SelectedIndex != -1 ? ((ViewInfo)toolWindow.Items[toolWindow.SelectedIndex].Tag).Key : null;
            }

            if (App.Config<WorkbenchConfig>().RememberOpenFiles)
            {
                LastSessionFiles = new List<String>();
                App.GetService<IDocumentService>().EnumerateDocuments().Where(d => d.FileInfo != null).ForEach(d => LastSessionFiles.Add(d.FileInfo.FullName));
                var doc = App.Document();

                if (doc != null && doc.FileInfo != null)
                    LastActiveDocument = doc.FileInfo.FullName;
            }

            var fs = App.GetService<IFileService>();
            var ds = App.GetService<IDocumentService>();
            var active = ds.GetActiveDocument();
            e.Cancel = ds.EnumerateDocuments()
                .OrderByDescending(d => d != active)
                .Any(d => !fs.Close(d));
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            App.Unload();
        }

        private void documentContainer_UserDocumentSelect(object sender, ObjectEventArgs e)
        {
            App.GetService<IDocumentService>().SetActiveDocument((Document)e.Data);
        }

        internal void UpdateWindowHeader()
        {
            if (App.Config<WorkbenchConfig>().DocumentTitleInWindowHeader)
            {
                var doc = App.Document();

                if (doc != null)
                    Text = String.Format("Elide - {0}", doc.Title + (doc.IsDirty ? "*" : String.Empty));
                else
                    Text = "Elide";
            }
            else
                Text = "Elide";
        }

        private void docs_InfoBarUpdate(object sender, EventArgs e)
        {
            var doc = App.Document();

            if (doc == null)
                return;

            var editor = App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance as ITextEditor;

            if (editor != null)
            {
                var upd = false;

                if (!documentContainer.InfoBarVisible)
                {
                    documentContainer.InfoBarVisible = true;
                    upd = true;
                }

                if (editor.CurrentLine + 1 != documentContainer.Line)
                {
                    documentContainer.Line = editor.CurrentLine + 1;
                    upd = true;
                }

                if (editor.CurrentColumn + 1 != documentContainer.Column)
                {
                    documentContainer.Column = editor.CurrentColumn + 1;
                    upd = true;
                }

                if (editor.Overtype != documentContainer.Overtype)
                {
                    documentContainer.Overtype = editor.Overtype;
                    upd = true;
                }

                if (upd)
                    documentContainer.Refresh();
            }
            else
            {
                if (documentContainer.InfoBarVisible)
                {
                    documentContainer.InfoBarVisible = false;
                    documentContainer.Refresh();
                }
            }
        }

        internal IApp App { get; private set; }

        private void outputsBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.GetService<IStatusBarService>().ClearStatusString();
            var rh = mainSplit.ClientSize.Height - mainSplit.SplitterWidth;

            if (outputsBar.SelectedIndex == -1)
            {
                _outputPanelSize = mainSplit.Panel2.Height;
                mainSplit.SetPanel2Size(mainSplit.Panel2MinSize);
                mainSplit.IsSplitterFixed = true;
            }
            else
            {
                if (mainSplit.Panel2.Height > 50) //50 - minimum panel size (with buttons).
                    _outputPanelSize = mainSplit.Panel2.Height;
                
                mainSplit.SetPanel2Size(_outputPanelSize);
                var inf = (ViewInfo)outputsBar.SelectedTag;
                var view = App.GetService<IViewService>().GetView(inf.Key);
                var ctl = (Control)view.Control;
                ctl.Dock = DockStyle.Fill;
                mainSplit.Panel2.Controls.Add(ctl);
                ctl.BringToFront();
                mainSplit.IsSplitterFixed = false;
                view.Activate();
            }
        }

        private void toolWindow_SelectedIndexChanged(object sender, SwitchBarEventArgs e)
        {
            App.GetService<IStatusBarService>().ClearStatusString();

            if (e.Item == null)
            {
                toolWindow.RemoveHostedControl();
                toolDock.Panel2Collapsed = true;
            }
            else
            {
                var svc = App.GetService<IViewService>();
                var inf = (ViewInfo)e.Item.Tag;
                svc.OpenView(inf.Key);
                var view = svc.GetView(inf.Key);
                toolWindow.RemoveHostedControl();
                toolWindow.AddHostedControl((Control)view.Control);
                toolDock.Panel2Collapsed = false;
                view.Activate();
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData("FileDrop") as string[];

            if (files != null && files.Length > 0)
                e.Effect = DragDropEffects.All;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData("FileDrop") as string[];

            if (files != null && files.Length > 0)
            {
                var fs = App.GetService<IFileService>();
                files.ForEach(f => fs.OpenFile(new FileInfo(f)));
            }
        }

        private void documentContainer_MoreDocumentsRequested(object sender, EventArgs e)
        {
            new OpenWindowsDialog { App = App }.ShowDialog(this);
        }

        public ActiveToolbar ActiveToolbar
        {
            get { return outputsBar; }
        }

        internal DocumentContainer DocumentContainer
        {
            get { return documentContainer; }
        }

        internal MenuStrip MenuStrip
        {
            get { return mainMenu; }
        }

        internal Panel DocumentPanel
        {
            get { return docPanel; }
        }

        internal ToolWindow ToolWindow
        {
            get { return toolWindow; }
        }

        #region State
        private int _outputPanelSize;
        [StateItem]
        public int OutputPanelSize
        {
            get { return mainSplit.IsSplitterFixed ? _outputPanelSize : mainSplit.Panel2.Height; }
            set {  _outputPanelSize = value;  }
        }

        private int _toolPanelSize;
        [StateItem]
        public int ToolPanelSize
        {
            get { return toolDock.IsSplitterFixed ? _toolPanelSize : toolDock.Panel2.Width; }
            set { _toolPanelSize = value; }
        }

        [StateItem]
        public List<String> LastSessionFiles { get; set; }

        [StateItem]
        public string LastActiveDocument { get; set; }

        [StateItem]
        public string OpenedTool { get; set; }

        [StateItem]
        public string OpenedOutput { get; set; }
        #endregion
    }
}
