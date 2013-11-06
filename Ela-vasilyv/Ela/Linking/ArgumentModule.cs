using System;
using Ela.Runtime;

namespace Ela.Linking
{
    internal sealed class ArgumentModule : ForeignModule
    {
        internal ArgumentModule()
        {

        }
        
        public override void Initialize()
        {
            
        }
        
        internal void AddArgument(string name, object value)
        {
            Add(name, ElaValue.FromObject(value));
        }
    }
}
