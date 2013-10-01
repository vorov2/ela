using System;
using System.Collections.Generic;
using Elide.Scintilla;

namespace Elide.EilCode.Lexer
{
	partial class Parser
	{
		internal List<StyledToken> Markers = new List<StyledToken>();

		internal int ErrorCount { get; set; }

		internal int Add(int position, int length, TextStyle style)
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
