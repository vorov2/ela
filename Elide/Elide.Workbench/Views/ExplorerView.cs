using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Elide.Core;
using Elide.Workbench;
using Elide.Forms;
using System.Drawing;
using Elide.Workbench.Images;
using Elide.Environment;
using Elide.Environment.Editors;
using System.Collections.Generic;
using Elide.Environment.Configuration;
using Elide.Workbench.Configuration;

namespace Elide.Workbench.Views
{
	public sealed class ExplorerView : Elide.Environment.Views.AbstractView
	{
		private ExplorerControl control;
        private LazyTreeView treeView;
        private ImageList imageList;
        private Dictionary<String,String> extMap;

        public ExplorerView()
		{
            extMap = new Dictionary<String,String>();
            control = new ExplorerControl();
            treeView = control.TreeView;

            imageList = new ImageList();
            imageList.ImageSize = new Size(16, 16);
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.TransparentColor = Color.Magenta;
            treeView.ImageList = imageList;

            imageList.Images.Add("Drive", Bitmaps.Load<NS>("Drive"));
            imageList.Images.Add("Folder", Bitmaps.Load<NS>("Folder"));
            imageList.Images.Add("File", Bitmaps.Load<NS>("File"));
            imageList.Images.Add("Computer", Bitmaps.Load<NS>("Computer"));
            imageList.Images.Add("Favorite", Bitmaps.Load<NS>("Favorite"));
            imageList.Images.Add("FilterFolder", Bitmaps.Load<NS>("FilterFolder"));
		}
		
		public override void  Initialize(IApp app)
        {
 	        base.Initialize(app);

            treeView.NodesNeeded += (o,e) => NodesNeeded(e.Node);
			treeView.NodeMouseDoubleClick += (o,e) =>
			{
                if (e.Node.Tag is FileInfo)
                    App.GetService<IFileService>().OpenFile((FileInfo)e.Node.Tag);
			};

            app.GetService<IConfigService>().ConfigUpdated += ConfigUpdated;
		}

        private void ConfigUpdated(object sender, ConfigEventArg e)
        {
            var cfg = e.Config as FileExplorerConfig;

            if (cfg != null)
                BuildTreeView(cfg);
        }

        public override void Activate()
        {
            if (treeView.Nodes.Count == 0)
                InitializeTreeView();

            treeView.Select();
        }

        private void InitializeTreeView()
        {
            var svc = App.GetService<IEditorService>();
            svc.EnumerateInfos("editors").OfType<EditorInfo>().ForEach(e =>
            {
                extMap.Add(e.FileExtension.ToLower(), e.Key);
                treeView.ImageList.Images.Add(e.Key, e.Instance.DocumentIcon);
            });

            BuildTreeView(App.Config<FileExplorerConfig>());
        }

        private void BuildTreeView(FileExplorerConfig cfg)
        {
            try
            {
                treeView.BeginUpdate();

                treeView.Nodes.Clear();

                if (cfg.Folders != null)
                    foreach (var f in cfg.Folders)
                    {
                        var nf = CreateNode(f.Name, "Favorite", f);
                        treeView.AddLazyNode(nf);
                    }

                var n = CreateNode("My Computer", "Computer", null);
                treeView.AddLazyNode(n);

                treeView.ContextMenuStrip = BuildMenu();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

		private void NodesNeeded(TreeNode node)
		{
			if (node.Tag is DriveInfo)
			{
				var d = (DriveInfo)node.Tag;
				ProcessDirectory(d.RootDirectory, node, null);
				
			}
            else if (node.Tag is DirectoryInfo)
            {
                var d = (DirectoryInfo)node.Tag;
                ProcessDirectory(d, node, null);
            }
            else if (node.Tag is FileExplorerConfig.FavoriteFolder)
            {
                var f = (FileExplorerConfig.FavoriteFolder)node.Tag;
                var d = new DirectoryInfo(f.Directory);

                if (d.Exists)
                    ProcessDirectory(d, node, f.Mask);
            }
            else
            {
                foreach (var d in DriveInfo.GetDrives())
                {
                    var n = CreateNode(d.Name, "Drive", d);
                    treeView.AddLazyNode(node, n);
                }
            }
		}
        
		private void ProcessDirectory(DirectoryInfo dir, TreeNode node, string mask)
		{
			try
			{
				treeView.BeginUpdate();
                var cfg = App.Config<FileExplorerConfig>();
                
                if (String.IsNullOrWhiteSpace(mask))
                    control.FilteredFolders.TryGetValue(dir.FullName.ToUpper(), out mask);

                var dirSeq = (IEnumerable<FileSystemInfo>)dir.GetFileSystemInfos();
                dirSeq = cfg.SortAscending ? dirSeq.OrderBy(d => d.Name) : dirSeq.OrderByDescending(d => d.Name);

                if (cfg.SortDirectoriesFirst)
                    dirSeq = dirSeq.OrderBy(d => d is FileInfo);
               
				foreach (var e in dirSeq)
				{
					if ((e is DirectoryInfo || mask == null || mask == "*.*" || Like(e.Name, mask)) &&
                        ((e.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden || cfg.ShowHiddenFilesFolders))
                    {
                        var isFolder = e is DirectoryInfo;
                        var img = "Folder";

                        if (!isFolder)
                        {
                            var fi = (FileInfo)e;

                            if (!extMap.TryGetValue(fi.Extension.ToLower(), out img))
                                img = "File";
                        }
                        else
                            img = control.FilteredFolders.ContainsKey(e.FullName.ToUpper()) ? "FilterFolder" : "Folder";

                        var n = CreateNode(e.Name, img, e);

                        if (isFolder)
                            treeView.AddLazyNode(node, n);
                        else
                        {
                            var fi = (FileInfo)e;            
                            node.Nodes.Add(n);
                        }
                    }
				}
			}
			finally
			{
				treeView.EndUpdate();
			}
		}

        private bool Like(string str, string pattern)
        {
            try 
            {
                return Microsoft.VisualBasic.CompilerServices.LikeOperator.LikeString(str, pattern, Microsoft.VisualBasic.CompareMethod.Binary);
            }
            catch 
            {
                return false;
            }
        }

        public ContextMenuStrip BuildMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
            return builder
                .Item("Refresh", () => control.Refresh(false))
                .Separator()
                .Item("Create File", CreateFile, () => treeView.SelectedNode != null && 
                    (treeView.SelectedNode.Tag is DirectoryInfo || treeView.SelectedNode.Tag is FileExplorerConfig.FavoriteFolder))
                .Item("Add to Favorite Folders", AddToFavorites, () => treeView.SelectedNode != null && treeView.SelectedNode.Tag is DirectoryInfo)
                .Item("Remove", RemoveFromFavorites, () => treeView.SelectedNode != null && treeView.SelectedNode.Tag is FileExplorerConfig.FavoriteFolder)
                .Item("Filter", null, SetFilter, () => treeView.SelectedNode != null && treeView.SelectedNode.Tag is DirectoryInfo, () =>
                    treeView.SelectedNode != null && treeView.SelectedNode.Tag is DirectoryInfo && control.FilteredFolders.ContainsKey(
                        ((DirectoryInfo)treeView.SelectedNode.Tag).FullName.ToUpper()))
                .Separator()
                .Item("Open File", null, OpenFile, () => treeView.SelectedNode != null && treeView.SelectedNode.Tag is FileInfo)
                .Item("Properties", ShowProperties, () => treeView.SelectedNode != null && treeView.SelectedNode.Tag is FileExplorerConfig.FavoriteFolder)
                .Finish();
        }

        private void RemoveFromFavorites()
        {
            if (treeView.SelectedNode != null)
            {
                var n = treeView.SelectedNode;
                var ff = n.Tag as FileExplorerConfig.FavoriteFolder;

                if (ff != null)
                {
                    App.Config<FileExplorerConfig>().Folders.Remove(ff);
                    n.Remove();
                }
            }
        }

        private void ShowProperties()
        {
            if (treeView.SelectedNode != null)
            {
                var n = treeView.SelectedNode;
                var ff = n.Tag as FileExplorerConfig.FavoriteFolder;

                if (ff != null)
                {
                    var dlg = new AddFolderDialog
                    {
                        Path = ff.Directory,
                        FolderName = ff.Name,
                        Mask = ff.Mask
                    };

                    if (dlg.ShowDialog(WB.Form) == DialogResult.OK)
                    {
                        ff.Directory = dlg.Path;
                        ff.Name = dlg.FolderName;
                        ff.Mask = dlg.Mask;
                        treeView.RefreshNode(n);
                        n.Text = ff.Name;
                    }
                }
            }
        }

        private void AddToFavorites()
        {
            if (treeView.SelectedNode != null)
            {
                var n = treeView.SelectedNode;
                var di = n.Tag as DirectoryInfo;

                if (di != null)
                {
                    var cfg = App.Config<FileExplorerConfig>();
                    var mask = default(String);
                    control.FilteredFolders.TryGetValue(di.FullName.ToUpper(), out mask);

                    cfg.Folders.Add(new FileExplorerConfig.FavoriteFolder
                    {
                        Directory = di.FullName,
                        Mask = mask,
                        Name = di.Name
                    });

                    BuildTreeView(cfg);
                }
             }
        }

        private void CreateFile()
        {
            if (treeView.SelectedNode != null)
            {
                var n = treeView.SelectedNode;
                var di = n.Tag as DirectoryInfo;

                if (di == null)
                {
                    var fv = n.Tag as FileExplorerConfig.FavoriteFolder;

                    if (fv == null || fv.Directory == null)
                        return;

                    di = new DirectoryInfo(fv.Directory);
                }
                                
                var fi = App.GetService<IDialogService>().ShowSaveDialog(null, di.FullName);

                if (fi != null)
                {
                    try 
                    { 
                        fi.CreateText().Dispose();
                        App.GetService<IFileService>().OpenFile(fi);

                        control.TreeView.RefreshNode(n);
                        n.Expand();
                    }
                    catch
                    {

                    }
                }
            }
        }
  
        private void SetFilter()
        {
            if (treeView.SelectedNode != null)
            {
                var n = treeView.SelectedNode;
                var di = n.Tag as DirectoryInfo;

                if (di != null)
                {
                    var filter = default(String);
                    var key = di.FullName.ToUpper();
                    control.FilteredFolders.TryGetValue(key, out filter);

                    var dlg = new AddFilterDialog { Filter = filter };

                    if (dlg.ShowDialog(WB.Form) == DialogResult.OK)
                    {
                        control.FilteredFolders.Remove(key);
                        var hasFilter = false;

                        if (hasFilter = !String.IsNullOrWhiteSpace(dlg.Filter))
                            control.FilteredFolders.Add(key, dlg.Filter);

                        n.ImageKey = n.SelectedImageKey = (hasFilter ? "FilterFolder" : "Folder");
                        control.Refresh(false);
                        treeView.Refresh();
                    }
                }
            }
        }

        private void OpenFile()
        {
            var fi = (FileInfo)treeView.SelectedNode.Tag;
            App.GetService<IFileService>().OpenFile(fi);
        }

        private TreeNode CreateNode(string text, string image, object tag)
        {
            return new TreeNode(text) { ImageKey = image, SelectedImageKey = image, Tag = tag };
        }

        public override object Control
        {
            get { return control; }
        }
	}
}
