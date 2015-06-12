using System;
using Ela.Compilation;

namespace Ela.Linking
{
    public sealed class ModuleEventArgs : EventArgs
    {
        private CodeFrame frame;
        private ForeignModule foreign;

        internal ModuleEventArgs(ModuleReference mod)
        {
            Module = mod;
        }

        public void AddModule(CodeFrame frame)
        {
            this.frame = frame;
        }

        public void AddModule(ForeignModule mod)
        {
            this.foreign = mod;
        }

        internal CodeFrame GetFrame()
        {
            if (frame != null)
                return frame;
            else
            {
                foreign.Initialize();
                return foreign.Compile();
            }
        }
        
        public ModuleReference Module { get; private set; }

        internal bool HasModule
        {
            get { return frame != null || foreign != null; }
        }
    }
}
