using System;
using Ela.Compilation;
using Ela.Runtime;

namespace Ela.Linking
{
	public sealed class IntrinsicFrame : CodeFrame
	{
		#region Construction
		internal IntrinsicFrame(ElaValue[] mem)
		{
			Memory = mem;
		}
		#endregion


		#region Properties
		internal ElaValue[] Memory { get; private set; }
		#endregion
	}
}
