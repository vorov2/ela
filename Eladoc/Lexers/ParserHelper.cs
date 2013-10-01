using System;
using System.Collections.Generic;

namespace Eladoc.Lexers
{
	partial class Parser
	{
		internal List<StyledToken> Markers = new List<StyledToken>();
        
        internal bool ProcessTasks { get; set; }

		internal int ErrorCount { get; set; }

		internal int Add(int position, int length, ElaStyle style)
		{
			Markers.Add(new StyledToken(position, length, style));
			return Markers.Count - 1;
		}

		internal void Patch(int index, int length)
		{
            Markers[index] = new StyledToken(Markers[index].Position, length, Markers[index].StyleKey); 
		}
	}
}
