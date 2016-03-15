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
                            root = jroot.Parameters[0];
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
