using System;
using Elide.Scintilla.Internal;
using Elide.Scintilla.ObjectModel;

namespace Elide.Scintilla
{
    public sealed class KeyboardManager
    {
        internal KeyboardManager(EditorRef @ref)
        {
            Ref = @ref;
        }
        
        public void AssignKey(SciKey key, SciModifier mod, SciCommand cmd)
        {
            Ref.Send(Sci.SCI_ASSIGNCMDKEY, GetKey(key, mod), (Int32)cmd);
        }

        public void ClearKey(SciKey key, SciModifier mod)
        {
            Ref.Send(Sci.SCI_CLEARCMDKEY, GetKey(key, mod));
        }
        
        public void ClearAllKeys()
        {
            Ref.Send(Sci.SCI_CLEARALLCMDKEYS);
        }
        
        public void ExecuteCommand(SciCommand cmd)
        {
            Ref.Send((Int32)cmd);
        }
        
        private int GetKey(SciKey key, SciModifier mod)
        {
            return (Int32)key | ((Int32)mod << 16);
        }

		internal EditorRef Ref { get; private set; }
    }
}
