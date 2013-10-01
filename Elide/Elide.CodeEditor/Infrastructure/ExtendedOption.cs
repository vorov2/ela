using System;

namespace Elide.CodeEditor.Infrastructure
{
    public struct ExtendedOption
    {
        private readonly bool set; 
        public readonly int Code;

        public ExtendedOption(int code, bool set)
        {
            Code = code;
            this.set = set;
        }

        public bool IsSet()
        {
            return set;
        }
    }
}
