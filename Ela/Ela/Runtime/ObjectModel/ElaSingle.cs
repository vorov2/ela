using System;

namespace Ela.Runtime.ObjectModel
{
	internal sealed class ElaSingle : ElaObject
	{
		internal static readonly ElaSingle Instance = new ElaSingle();
		
		private ElaSingle() : base(ElaTypeCode.Single)
		{

		}
	}
}