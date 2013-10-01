using System;
using Elide.Core;
using Ela.Compilation;
using Ela.Debug;

namespace Elide.ElaCode
{
    public sealed class EilGeneratorHelper
    {
        private readonly IApp app;

        public EilGeneratorHelper(IApp app)
        {
            this.app = app;
        }

        public string Generate(CodeFrame frame)
        {
            var gen = new EilGenerator(frame);
            gen.OmitOffsets = !PrintOffsets;
            gen.IgnoreDebugInfo = IgnoreDebugInfo;
            return gen.Generate();
        }

        public bool PrintOffsets { get; set; }

        public bool IgnoreDebugInfo { get; set; }
    }
}
