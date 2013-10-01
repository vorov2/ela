using System;
using System.Collections.Generic;
using System.Text;

namespace Ela.CodeModel
{
    public sealed class ElaProgram
    {
        public ElaProgram()
        {
            TopLevel = new ElaEquationSet();
            Includes = new List<ElaModuleInclude>();
        }

        public override string ToString()
        {
            return Code;
        }

        public ElaEquationSet TopLevel { get; private set; }

        public List<ElaModuleInclude> Includes { get; internal set; }

        public ElaTypeClass Classes { get; internal set; }

        public ElaNewtype Types { get; internal set; }

        public ElaClassInstance Instances { get; internal set; }

        public string Code
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var i in Includes)
                {
                    i.ToString(sb, 0);
                    sb.AppendLine("\r\n");
                }

                if (Types != null)
                {
                    Types.ToString(sb, 0);
                    sb.AppendLine("\r\n");
                }

                if (Classes != null)
                {
                    Classes.ToString(sb, 0);
                    sb.AppendLine("\r\n");
                }

                if (Instances != null)
                {
                    Instances.ToString(sb, 0);
                    sb.AppendLine("\r\n");
                }

                TopLevel.ToString(sb, 0);
                return sb.ToString();
            }
        }
    }
}
