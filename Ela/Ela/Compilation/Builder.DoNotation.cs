using Ela.CodeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ela.Compilation
{
    //This part contains compilation logic for do-notation (desugaring).
    internal sealed partial class Builder
    {
        //This method rewrites a do-notation into a chain of bind (>>=) function calls.
        private void CompileDoNotation(ElaDoNotation exp, LabelMap map, Hints hints)
        {
            var root = default(ElaExpression);
            var bind = default(ElaLetBinding);
            var current = default(ElaLambda);

            for (var i = 0; i < exp.Statements.Count; i++)
            {
                var ce = exp.Statements[i];
                var last = i == exp.Statements.Count - 1;

                if (ce.Type != ElaNodeType.LetBinding)
                {
                    var ass = ce.Type == ElaNodeType.DoAssignment ? (ElaDoAssignment)ce : null;
                    var juxta = MakeBind(ass != null ? ass.Right : ce);

                    if (root == null)
                    {
                        root = juxta;

                        if (bind != null)
                        {
                            bind.Expression = root;
                            root = bind;
                            bind = null;
                        }
                    }
                    else
                    {
                        current.Right = juxta;

                        if (bind != null)
                        {
                            bind.Expression = juxta;
                            current.Right = bind;
                            bind = null;
                        }
                    }

                    if (!last)
                    {
                        var lam = new ElaLambda();
                        lam.SetLinePragma(ce.Line, ce.Column);
                        lam.Left = ass != null ? ass.Left : new ElaPlaceholder();
                        juxta.Parameters[1] = lam;
                        current = lam;

                        if (lam.Left.Type != ElaNodeType.NameReference && lam.Left.Type != ElaNodeType.Placeholder)
                        {
                            var jux = new ElaJuxtaposition { Target = new ElaNameReference { Name = "failure" } };
                            jux.Parameters.Add(new ElaNameReference { Name = "$v" });

                            var jux2 = new ElaJuxtaposition();
                            jux2.Parameters.Add(new ElaNameReference { Name = "$v" });
                            lam.Next = new ElaEquation { Left = jux2, Right = jux };
                        }
                    }
                    else
                    {
                        if (current != null)
                        {
                            if (current.Right.Type == ElaNodeType.LetBinding)
                            {
                                var lb = (ElaLetBinding)current.Right;
                                lb.Expression = juxta.Parameters[0];
                                current.Right = lb;
                            }
                            else
                                current.Right = juxta.Parameters[0];
                        }
                        else if (root.Type == ElaNodeType.Juxtaposition)
                        {
                            var jroot = (ElaJuxtaposition)root;
                            //root = jroot.Parameters[0];
                            jroot.Parameters[1] = MakeFake();
                        }
                        else if (root.Type == ElaNodeType.LetBinding)
                        {
                            var lroot = (ElaLetBinding)root;

                            if (lroot.Expression.Type == ElaNodeType.Juxtaposition)
                            {
                                var jroot = (ElaJuxtaposition)lroot.Expression;
                                jroot.Parameters[1] = MakeFake();
                                //lroot.Expression = jroot.Parameters[0];
                            }
                        }
                    }
                }
                else
                {
                    var letb = (ElaLetBinding)ce;

                    if (bind != null)
                        bind.Equations.Equations.AddRange(letb.Equations.Equations);
                    else
                        bind = letb;
                }
            }

            CompileExpression(root, map, hints, null);
        }

        private ElaLambda MakeFake()
        {
            var juxta = new ElaJuxtaposition
            {
                Target = new ElaNameReference { Name = "point" }
            };
            juxta.Parameters.Add(new ElaUnitLiteral());

            return new ElaLambda
            {
                Left = new ElaPlaceholder(),
                Right = juxta
            };
        }

        private ElaJuxtaposition MakeBind(ElaExpression exp)
        {
            var ret = new ElaJuxtaposition
            {
                Target = new ElaNameReference { Name = ">>=" }
            };
            ret.Parameters.Add(exp);
            ret.Parameters.Add(null);

            ret.SetLinePragma(exp.Line, exp.Column);
            ret.Target.SetLinePragma(exp.Line, exp.Column);

            return ret;
        }
    }
}
