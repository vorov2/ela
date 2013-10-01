using System;

namespace Elide.Forms
{
	public sealed class SwitchBarEventArgs : EventArgs
	{
		internal SwitchBarEventArgs(SwitchBarItem item)
		{
			Item = item;
		}
		
        public SwitchBarItem Item { get; private set; }
	}
}
