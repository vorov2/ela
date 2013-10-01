using System;
using System.Linq;
using System.Collections.Generic;
using Elide.Environment.Configuration;

namespace Elide.Workbench.Configuration
{
    [Serializable]
    public sealed class FileExplorerConfig : Config
    {
        [Serializable]
        public sealed class FavoriteFolder
        {
            public FavoriteFolder() 
            {

            }

            public override string ToString()
            {
                return String.Format("{0}: {1} ({2})", Name, Directory, 
                    String.IsNullOrWhiteSpace(Mask) ? "*.*" : Mask);
            }

            public FavoriteFolder Clone()
            {
                return (FavoriteFolder)MemberwiseClone();
            }

            public string Name { get; set; }

            public string Mask { get; set; }

            public string Directory { get; set; }
        }

        public FileExplorerConfig()
        {
            SortDirectoriesFirst = true;
            SortAscending = true;
            Folders = new List<FavoriteFolder>();
        }

        public override Config Clone()
        {
            var cl = (FileExplorerConfig)MemberwiseClone();
            cl.Folders = new List<FavoriteFolder>(Folders.Select(f => f.Clone()));
            return cl;
        }

        public bool SortDirectoriesFirst { get; set; }

        public bool SortAscending { get; set; }

        public bool ShowHiddenFilesFolders { get; set; }

        public List<FavoriteFolder> Folders { get; set; }
    }
}
