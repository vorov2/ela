using System;

namespace Ela.Runtime
{
    public sealed class ExecutionResult
    {
        internal ExecutionResult(ElaValue val)
        {
            ReturnValue = val;
        }
        
        public ElaValue ReturnValue { get; private set; }
    }
}