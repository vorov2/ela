using System;

namespace Ela.Compilation
{
	internal struct Label
	{
		#region Construction
		internal static readonly Label Empty = new Label(EmptyLabel);
		internal const int EmptyLabel = -1;
		private int index;

		internal Label(int index)
		{
			this.index = index;
		}
		#endregion


		#region Methods
		public override string ToString()
		{
			return index.ToString();
		}


		internal bool IsEmpty()
		{
			return index == EmptyLabel;
		}


		internal int GetIndex()
		{
			return index;
		}
		#endregion
	}
}