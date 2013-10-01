using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eladoc.Lexers
{
    public struct StyledToken
    {
        public StyledToken(int position, int length, ElaStyle styleKey)
        {
            Position = position;
            Length = length;
            StyleKey = styleKey;
        }

        public readonly int Position;

        public readonly int Length;

        public readonly ElaStyle StyleKey;
    }
}
