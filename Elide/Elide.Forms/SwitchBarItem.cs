using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Elide.Forms
{
	public sealed class SwitchBarItem
	{
        public SwitchBarItem()
		{
            
		}
		
		public override string ToString() 
		{
			return Caption;
		}

		private string _caption;
		[Category("Main")]
		public string Caption
		{
			get { return _caption; }
			set
			{
				_caption = value;

				if (Parent != null)
					Parent.Refresh();
			}
		}

		[Browsable(false)]
		public object Tag { get; set; }

		internal int Left { get; set; }

		internal int Right { get; set; }

		internal SwitchBar Parent { get; set; }

		internal bool Visible { get; set; }
	}
}
