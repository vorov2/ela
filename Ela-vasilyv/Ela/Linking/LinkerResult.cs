using System;
using System.Collections.Generic;

namespace Ela.Linking
{
	public sealed class LinkerResult : TranslationResult
	{
		#region Construction
		internal LinkerResult(CodeAssembly asm, bool success, IEnumerable<ElaMessage> messages) :
			base(success, messages)
		{
			Assembly = asm;
		}
		#endregion


		#region Properties
		public CodeAssembly Assembly { get; private set; }
		#endregion
	}
}