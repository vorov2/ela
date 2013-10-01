using System;
using System.Collections.Generic;

namespace Ela
{
	public abstract class TranslationResult
	{
		protected TranslationResult(bool success, IEnumerable<ElaMessage> messages)
		{
			Success = success;
			Messages = messages;
		}
		
        public bool Success { get; private set; }

		public IEnumerable<ElaMessage> Messages { get; private set; }
	}
}
