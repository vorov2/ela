using System.Text;
using Elide.CodeEditor.Infrastructure;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class TypeClassMember : IClassMember
    {
        internal TypeClassMember(string name, int arguments, int signature)
        {
            Name = name;
            Components = arguments;
            Signature = signature;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            if (IsSymbolic(Name))
            {
                sb.Append('(');
                sb.Append(Name);
                sb.Append(')');
            }
            else
                sb.Append(Name);

            if (Signature != 0)
            {
                sb.Append(" ");

                for (var i = 0; i < Components; i++)
                {
                    if (i > 0)
                        sb.Append("->");

                    if ((Signature & (1 << i)) == (1 << i))
                        sb.Append("a");
                    else
                        sb.Append("_");
                }
            }

            return sb.ToString();
        }

        private static readonly char[] ops = new char[] { '!', '%', '&', '*', '+', '-', '.', ':', '/', '$', '\\', '<', '=', '>', '?', '@', '^', '|', '~' };

        private static bool IsSymbolic(string name)
        {
            return name.IndexOfAny(ops) != -1;
        }

        public string Name { get; private set; }

        public int Components { get; private set; }

        public int Signature { get; private set; }
    }
}
