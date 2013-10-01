using System;
using Elide.Environment.Configuration;

namespace Elide.ElaCode.Configuration
{
    [Serializable]
    public class EilGeneratorConfig : Config
    {
        public EilGeneratorConfig()
        {
            GenerateInDebugMode = true;
            IncludeCodeOffsets = true;
        }

        public bool IncludeCodeOffsets { get; set; }

        public bool GenerateInDebugMode { get; set; }
    }
}
