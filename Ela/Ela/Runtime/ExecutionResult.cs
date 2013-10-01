using System;

namespace Ela.Runtime
{
	public sealed class ExecutionResult
	{
		#region Construction
		internal ExecutionResult(ElaValue val)
		{
			ReturnValue = val;
		}
		#endregion


		#region Properties
		public ElaValue ReturnValue { get; private set; }
		#endregion
	}
}