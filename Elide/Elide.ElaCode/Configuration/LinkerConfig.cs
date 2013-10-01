using System;
using System.Collections.Generic;
using Elide.Environment.Configuration;

namespace Elide.ElaCode.Configuration
{
    [Serializable]
    public sealed class LinkerConfig : Config
    {
        public LinkerConfig()
        {
            Directories = new List<String>();
            LookupStartupDirectory = true;
            Directories.Add(@"%root%\lib");
        }

        public override Config Clone()
        {
            var cl = (LinkerConfig)MemberwiseClone();
            cl.Directories = new List<String>(Directories.ToArray());
            return cl;
        }

        public List<String> Directories { get; set; }
        
        public bool LookupStartupDirectory { get; set; }

        public bool ForceRecompile { get; set; }
        
        public bool NoWarnings { get; set; }
        
        public bool SkipTimeStampCheck { get; set; }
        
        public bool WarningsAsErrors { get; set; }
    }
}
