using System;
using Elide.Environment.Configuration;

namespace Elide.ElaCode.Configuration
{
    [Serializable]
    public sealed class CompilerConfig : Config
    {
        public CompilerConfig()
        {
            Prelude = "prelude";
            Optimize = true;
            ShowHints = true;
        }

        public bool GenerateDebugInfo { get; set; }
        
        public bool NoWarnings { get; set; }
        
        public bool Optimize { get; set; }
        
        public string Prelude { get; set; }
        
        public bool ShowHints { get; set; }

        public bool WarningsAsErrors { get; set; }
    }
}
