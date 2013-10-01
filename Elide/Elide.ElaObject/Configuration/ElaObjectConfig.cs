using System;
using Elide.Environment.Configuration;

namespace Elide.ElaObject.Configuration
{
    [Serializable]
    public sealed class ElaObjectConfig : Config
    {
        public ElaObjectConfig()
        {
            DisplayHeader = true;
            CustomHeaderFormat = "Module %name%, object file format version %version%. Created by Ela %ela%";
            DisplayOffset = true;
            DisplayFrameOpCodes = true;
            DisplayFlatOpCodes = true;
            LimitOpCodes = true;
            OpCodeLimit = 1000;
        }

        public bool ExpandAllNodes { get; set; }

        public bool DisplayHeader { get; set; }

        public bool UseCustomHeaderFormat { get; set; }

        public string CustomHeaderFormat { get; set; }

        public bool DisplayOffset { get; set; }

        public bool DisplayFrameOpCodes { get; set; }

        public bool DisplayFlatOpCodes { get; set; }

        public bool LimitOpCodes { get; set; }

        public int OpCodeLimit { get; set; }
    }
}
