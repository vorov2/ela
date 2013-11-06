using System;
using System.Collections.Generic;

namespace Ela.Compilation
{
	internal sealed class LabelMap
	{
		internal LabelMap()
		{
			
		}
        
		internal LabelMap(LabelMap old)
		{
			FunStart = old.FunStart;
			FunctionName = old.FunctionName;
			FunctionParameters = old.FunctionParameters;
		}

        internal LabelMap Clone(int context)
        {
            return new LabelMap
            {
                FunStart = FunStart,
                BindingName = BindingName,
                FunctionName = FunctionName,
                FunctionParameters = FunctionParameters,
                FunctionScope = FunctionScope,
                Context = context
            };
        }

        internal bool HasContext
        {
            get { return Context != null; }
        }
		
        internal Label FunStart { get; set; }

        internal string BindingName { get; set; }

		internal string FunctionName { get; set; }

        internal int FunctionParameters { get; set; }

		internal Scope FunctionScope { get; set; }

        internal int? Context { get; private set; }
	}
}