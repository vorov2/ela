using System;
using System.Collections.Generic;
using Elide.Environment.Configuration;

namespace Elide.Workbench.Configuration
{
    [Serializable]
    public sealed class WorkbenchConfig : Config
    {
        public WorkbenchConfig()
        {
            RecentFilesMax = 10;
            AutoSavePeriod = 1000 * 60 * 5; //5 minutes
            FullFilePath = true;
            ShowWelcomePage = true;
            RememberOpenFiles = true;
            RememberOpenTools = true;
            DocumentTitleInWindowHeader = true;
        }

        public int RecentFilesMax { get; set; }

        public bool FullFilePath { get; set; }

        public bool StartWithBlank { get; set; }

        public bool RememberOpenFiles { get; set; }

        public bool RememberOpenTools { get; set; }

        public bool DocumentTitleInWindowHeader { get; set; }

        public bool EnableAutoSave { get; set; }

        public int AutoSavePeriod { get; set; }

        public bool ShowWelcomePage { get; set; }

        internal List<String> RecentFiles { get; set; }
    }
}
