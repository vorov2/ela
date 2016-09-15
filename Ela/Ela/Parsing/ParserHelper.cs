using System;
using System.Collections.Generic;
using System.Reflection;
using Ela.CodeModel;
using System.Globalization;

namespace Ela.Parsing
{
    internal sealed partial class Parser
    {
        private static readonly ElaEquation unit = new ElaEquation();
        private Stack<ElaEquation> bindings = new Stack<ElaEquation>(new ElaEquation[] { null });

        private bool RequireEndBlock()
        {
            if (la.kind == _EBLOCK)
                return true;

            if (la.kind == _PIPE)
                return false;

            if (la.kind == _IN)
            {
                scanner.PopIndent();
                return false;
            }

            return true;
        }

        private ElaVariableFlags ProcessAttribute(string attribute, ElaVariableFlags flags)
        {
            if (attribute == "private")
                return flags | ElaVariableFlags.Private;
            else if (attribute == "qualified")
                return flags | ElaVariableFlags.Qualified;
            else if (attribute == "nowarnings")
                return flags | ElaVariableFlags.NoWarnings;
            else
            {
                AddError(ElaParserError.UnknownAttribute, attribute);
                return flags;
            }
        }

        private void BuildMask(ref int count, ref int mask, string val, string targ)
        {
            var flag = 0;

            if (val == targ)
                flag = 1;
            else if (val != "_")
                AddError(ElaParserError.InvalidFunctionSignature, val);

            mask |= flag << count;
            count++;
        }

        private void ProcessBinding(ElaEquationSet block, ElaEquation bid, ElaExpression left, ElaExpression right)
        {
            bid.Left = left;
            bid.Right = right;

            if (bindings.Peek() == unit)
            {
                block.Equations.Add(bid);
                return;
            }

            var fName = default(String);

            if (right != null && left.Type == ElaNodeType.Juxtaposition && !left.Parens)
            {
                var fc = (ElaJuxtaposition)left;

                if (fc.Target.Type == ElaNodeType.NameReference)
                    fName = fc.Target.GetName();
            }

            if (fName != null)
            {
                var lastB = bindings.Peek();

                if (lastB != null && ((ElaJuxtaposition)lastB.Left).Target.GetName() == fName)
                    lastB.Next = bid;
                else
                    block.Equations.Add(bid);

                bindings.Pop();
                bindings.Push(bid);
            }
            else
                block.Equations.Add(bid);
        }

        private ElaExpression GetOperatorFun(string op, ElaExpression left, ElaExpression right)
        {
            var fc = new ElaJuxtaposition(t) {
                Target = new ElaNameReference(t) { Name = op }
            };

            if (left != null)
                fc.Parameters.Add(left);
            else
                fc.FlipParameters = true;

            if (right != null)
                fc.Parameters.Add(right);

            return fc;
        }

        private ElaExpression GetPrefixFun(ElaExpression funexp, ElaExpression par, bool flip)
        {
            var fc = new ElaJuxtaposition(t) { Target = funexp };
            fc.Parameters.Add(par);
            fc.FlipParameters = flip;
            return fc;
        }

        private ElaLiteralValue ParseInt(string val)
        {
            var except = val.Length > 2 && Char.ToUpper(val[1]) == 'X' ?
                "ABCDEFabcdef" : null;

            var c = TrimLast(ref val, except);

            if (c == 'l' || c == 'L')
            {
                var res = default(Int64);

                if (!Int64.TryParse(val, out res))
                {
                    try
                    {
                        res = Convert.ToInt64(val, 16);
                    }
                    catch
                    {
                        AddError(ElaParserError.InvalidIntegerSyntax);
                    }
                }

                return new ElaLiteralValue(res);
            } 
            else if (c == 'd' || c == 'D')
            {
                var res = default(Double);

                if (!Double.TryParse(val, NumberStyles.Float, Culture.NumberFormat, out res))
                    AddError(ElaParserError.InvalidRealSyntax);

                return new ElaLiteralValue(res);
            }
            else if (c == 'f' || c == 'F')
            {
                var res = default(Single);

                if (!Single.TryParse(val, NumberStyles.Float, Culture.NumberFormat, out res))
                    AddError(ElaParserError.InvalidRealSyntax);

                return new ElaLiteralValue(res);
            }
            else if (c == '\0')
            {
                var res = default(Int32);

                if (!Int32.TryParse(val, out res))
                {
                    try
                    {
                        res = Convert.ToInt32(val, 16);
                    }
                    catch
                    {
                        AddError(ElaParserError.InvalidIntegerSyntax);
                    }
                }

                return new ElaLiteralValue(res);
            }
            else
                return new ElaLiteralValue(val, c);
        }

        private ElaLiteralValue ParseString(string val)
        {
            return new ElaLiteralValue(ReadString(val));
        }

        private ElaLiteralValue ParseChar(string val)
        {
            var str = ReadString(val);
            return new ElaLiteralValue(str[0]);
        }

        private ElaLiteralValue ParseReal(string val)
        {
            var c = TrimLast(ref val);

            if (c == 'd' || c == 'D')
            {
                var res = default(Double);
                
                if (!Double.TryParse(val, NumberStyles.Float, Culture.NumberFormat, out res))
                    AddError(ElaParserError.InvalidRealSyntax);
                
                return new ElaLiteralValue(res);
            }
            else if (c == 'f' || c == 'F' || c == '\0' || c == 'L' || c == 'l')
            {
                var res = default(Single);

                if (!Single.TryParse(val, NumberStyles.Float, Culture.NumberFormat, out res))
                    AddError(ElaParserError.InvalidRealSyntax);

                return new ElaLiteralValue(res);
            }
            else
               return new ElaLiteralValue(val, c);
        }

        private string ReadString(string val)
        {
            if (val.Length > 0) 
            {
                if (val[0] != '<')
                {
                    var res = EscapeCodeParser.Parse(ref val);

                    if (res > 0)
                        AddError(ElaParserError.InvalidEscapeCode, res);
                }
                else
                    val = val.Substring(2, val.Length - 4);
            }
            
            return val;
        }

        private char TrimLast(ref string val, string except = null)
        {
            var lc = val[val.Length - 1];

            if (Char.IsLetter(lc) && (except == null || except.IndexOf(lc) == -1))
            {
                val = val.Remove(val.Length - 1, 1);
                return lc;
            }

            return '\0';
        }

        public ElaProgram Program { get; private set; }
    }
}
