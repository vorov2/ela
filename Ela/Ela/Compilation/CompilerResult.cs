using System;
using System.Collections.Generic;

namespace Ela.Compilation
{
	public sealed class CompilerResult : TranslationResult
	{
		#region Construction
		internal CompilerResult(CodeFrame frame, bool success, IEnumerable<ElaMessage> messages) : base(success, messages)
		{
			CodeFrame = frame;
		}
		#endregion


		#region Properties
		public CodeFrame CodeFrame { get; private set; }
		#endregion
	}
}