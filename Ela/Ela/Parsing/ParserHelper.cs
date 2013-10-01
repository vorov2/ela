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

        private ElaExpression ValidateDoBlock(ElaExpression exp)
        {
            if (exp.Type == ElaNodeType.Juxtaposition && ((ElaJuxtaposition)exp).Parameters[0] == null)
            {
                var ext = ((ElaJuxtaposition)exp).Parameters[1];
                var ctx = default(ElaContext);

                if (ext.Type == ElaNodeType.Context)
                {
                    ctx = (ElaContext)ext;
                    ext = ctx.Expression;
                }

                var eqt = new ElaJuxtaposition { Spec = true };
                eqt.SetLinePragma(exp.Line, exp.Column);
                eqt.Target = new ElaNameReference(t) { Name = ">>=" };
                eqt.Parameters.Add(ext);

                var jux = new ElaJuxtaposition();
                jux.SetLinePragma(exp.Line, exp.Column);
                jux.Target = new ElaNameReference { Name = "point" };
                jux.Parameters.Add(new ElaUnitLiteral());
                eqt.Parameters.Add(new ElaLambda { Left = new ElaPlaceholder(), Right = jux });

                if (ctx != null)
                {
                    ctx.Expression = eqt;
                    exp = ctx;
                }
                else
                    exp = eqt;
            }

            var root = exp;

            while (true)
            {
                if (exp.Type == ElaNodeType.Juxtaposition)
                {
                    var juxta = (ElaJuxtaposition)exp;

                    if (juxta.Parameters.Count == 2)
                    {
                        juxta.Parameters[0] = Reduce(juxta.Parameters[0], juxta);
                        juxta.Parameters[1] = Reduce(juxta.Parameters[1], juxta);
                        exp = juxta.Parameters[1];
                    }
                    else
                        break;
                }
                else if (exp.Type == ElaNodeType.LetBinding)
                {
                    var lb = (ElaLetBinding)exp;
                    lb.Expression = Reduce(lb.Expression, lb);
                    exp = lb.Expression;
                }
                else if (exp.Type == ElaNodeType.Lambda)
                {
                    var lb = (ElaLambda)exp;
                    lb.Right = Reduce(lb.Right, lb);

                    if (lb.Left.Type != ElaNodeType.NameReference &&
                        lb.Left.Type != ElaNodeType.Placeholder)
                    {
                        var em = new ElaMatch();
                        em.SetLinePragma(lb.Left.Line, lb.Left.Column);
                        em.Expression = new ElaNameReference { Name = "$x01" };
                        em.Entries = new ElaEquationSet();

                        var eq1 = new ElaEquation();
                        eq1.SetLinePragma(lb.Left.Line, lb.Left.Column);
                        eq1.Left = lb.Left;
                        eq1.Right = lb.Right;
                        em.Entries.Equations.Add(eq1);

                        var eq2 = new ElaEquation();
                        eq2.SetLinePragma(lb.Left.Line, lb.Left.Column);
                        eq2.Left = new ElaNameReference { Name = "$x02" };
                        var errExp = new ElaJuxtaposition();
                        errExp.SetLinePragma(lb.Left.Line, lb.Left.Column);
                        errExp.Target = new ElaNameReference { Name = "failure" };
                        errExp.Parameters.Add(new ElaNameReference { Name = "$x02" });
                        eq2.Right = errExp;
                        em.Entries.Equations.Add(eq2);

                        lb.Left = new ElaNameReference { Name = "$x01" };
                        lb.Right = em;
                        exp = lb;
                    }
                    else
                        exp = lb.Right;
                }
                else
                    break;
            }

            var ret = new ElaLazyLiteral { Expression = root };
            ret.SetLinePragma(root.Line, root.Column);
            return ret;
        }

        private ElaExpression Reduce(ElaExpression exp, ElaExpression parent)
        {
            if (exp == null)
            {
                errors.AddErr(parent.Line, parent.Column, ElaParserError.InvaliDoEnd);
                return new ElaUnitLiteral();
            }

            if (exp.Type == ElaNodeType.Juxtaposition)
            {
                var juxta = (ElaJuxtaposition)exp;

                if (juxta.Parameters[0] == null)
                    return juxta.Parameters[1];
                else if (juxta.Parameters.Count > 1 &&
                    juxta.Parameters[1] != null &&
                    juxta.Parameters[1].Type == ElaNodeType.Lambda &&
                    ((ElaLambda)juxta.Parameters[1]).Right == null)
                    return juxta.Parameters[0];
                else
                    return juxta;
            }
            else
                return exp;
        }

        private void ProcessDoBlock(ElaExpression cexp1, ElaExpression cexp2, ref ElaExpression rootExp)
        {
            var eqt = default(ElaJuxtaposition);
            var lam = default(ElaLambda);
            var letb = default(ElaLetBinding);

            if (cexp2 == null)
            {
                cexp2 = cexp1;
                cexp1 = new ElaPlaceholder();
            }

            if (rootExp.Type == ElaNodeType.Juxtaposition)
                eqt = (ElaJuxtaposition)rootExp;
            else if (rootExp.Type == ElaNodeType.Lambda)
            {
                lam = (ElaLambda)rootExp;
                eqt = lam.Right as ElaJuxtaposition;
            }
            else if (rootExp.Type == ElaNodeType.LetBinding)
            {
                letb = (ElaLetBinding)rootExp;
                eqt = letb.Expression as ElaJuxtaposition;
            }
            else if (rootExp.Type != ElaNodeType.None)
            {
                eqt = new ElaJuxtaposition { Spec = true };
                eqt.SetLinePragma(cexp1.Line, cexp1.Column);
                eqt.Parameters.Add(null);
                eqt.Parameters.Add(rootExp);
            }

            if (eqt != null && !eqt.Spec)
            {
                var eqt2 = new ElaJuxtaposition();
                eqt2.SetLinePragma(eqt.Line, eqt.Column);
                eqt2.Target = new ElaNameReference(t) { Name = ">>-" };
                eqt2.Parameters.Add(null);
                eqt2.Parameters.Add(eqt);
                eqt = eqt2;
            }

            if (eqt != null && eqt.Parameters.Count == 2)
            {
                if (eqt.Parameters[0] == null)
                {
                    eqt.Target = new ElaNameReference(t) { Name = ">>=" };
                        
                    var lambda = new ElaLambda();
                    lambda.SetLinePragma(cexp2.Line, cexp2.Column);
                    lambda.Left = new ElaPlaceholder();
                        
                    var lambda2 = new ElaLambda();
                    lambda2.SetLinePragma(cexp2.Line, cexp2.Column);
                    lambda2.Left = cexp1;

                    var eqt1 = new ElaJuxtaposition { Spec = true };
                    eqt1.SetLinePragma(cexp2.Line, cexp2.Column);
                    eqt1.Target = new ElaNameReference(t) { Name = ">>=" };
                    eqt1.Parameters.Add(cexp2);
                    eqt1.Parameters.Add(lambda2);
                    lambda.Right = eqt1;
                        
                    eqt.Parameters[0] = eqt.Parameters[1];
                    eqt.Parameters[1] = lambda;
                    rootExp = lambda2;
                }
                else
                {
                    throw new Exception("Unable to process do-notation.");
                }
            }
            else
            {
                if (eqt == null)
                {
                    eqt = new ElaJuxtaposition { Spec = true };
                    eqt.SetLinePragma(cexp1.Line, cexp1.Column);

                    if (lam != null)
                        lam.Right = eqt;
                    else if (letb != null)
                        letb.Expression = eqt;
                }

                eqt.Target = new ElaNameReference(t) { Name = ">>=" };
                eqt.Parameters.Add(cexp2);

                var lambda = new ElaLambda();
                lambda.SetLinePragma(cexp2.Line, cexp2.Column);
                lambda.Left = cexp1;

                eqt.Parameters.Add(lambda);
                rootExp = lambda;
            }
        }

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
            if (TrimLast(ref val, 'l', 'L'))
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
			else
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
            if (TrimLast(ref val, 'd', 'D'))
			{
				var res = default(Double);
				
				if (!Double.TryParse(val, NumberStyles.Float, Culture.NumberFormat, out res))
					AddError(ElaParserError.InvalidRealSyntax);
				
				return new ElaLiteralValue(res);
			}
            else
            {
                var res = default(Single);

                if (!Single.TryParse(val.Trim('f', 'F'), NumberStyles.Float, Culture.NumberFormat, out res))
                    AddError(ElaParserError.InvalidRealSyntax);

                return new ElaLiteralValue(res);
            }
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
		
		
		private bool TrimLast(ref string val, char cl, char cu)
		{			
			var lc = val[val.Length - 1];			
			
			if (lc == cl || lc == cu)
			{
				val = val.Remove(val.Length - 1, 1);
				return true;
			}
			else
				return false;
		}

		public ElaProgram Program { get; private set; }
	}
}
