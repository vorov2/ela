using System;
using System.Collections.Generic;
using Ela.CodeModel;

namespace Ela.Parsing
{
	public sealed class ParserResult : TranslationResult
	{
		internal ParserResult(ElaProgram prog, bool success, IEnumerable<ElaMessage> messages) : base(success, messages)
		{
			Program = prog;
		}

		public ElaProgram Program { get; private set; }
	}
}
