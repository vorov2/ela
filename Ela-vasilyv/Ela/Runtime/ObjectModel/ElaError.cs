using System;
using System.Collections.Generic;
using Ela.Debug;

namespace Ela.Runtime.ObjectModel
{
	internal sealed class ElaError : ElaString
	{
		internal ElaError(ElaRuntimeError code) : base(Strings.GetError(code))
		{
            Code = code;
		}

		internal ElaError(ElaRuntimeError code, params object[] args) : base(Strings.GetError(code, args))
		{
            Code = code;
		}
        		
		internal ElaError(string message) : base(message)
		{

		}

		internal string Message
		{
			get
			{
                return base.Value;
			}
		}
        
		internal ElaRuntimeError Code { get; private set; }

		internal int CodeOffset { get; set; }

		internal int Module { get; set; }

		internal Stack<StackPoint> Stack { get; set; }
	}
}
