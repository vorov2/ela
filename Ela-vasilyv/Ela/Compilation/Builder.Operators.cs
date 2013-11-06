using System;
using Ela.CodeModel;

namespace Ela.Compilation
{
    //This part is responsible for compilation of Ela operators.
    //All entities treated as operators are special forms (not functions).
    //Ela has only three of them - logical AND, logical OR and sequencing ($).
    //Plus conditional if-then-else operator.
	internal sealed partial class Builder
	{
        //Compile conditional if-then-else operator
        private void CompileConditionalOperator(ElaCondition s, LabelMap map, Hints hints)
        {
            AddLinePragma(s);
            CompileExpression(s.Condition, map, Hints.Scope, s);
            var falseLab = cw.DefineLabel();
            cw.Emit(Op.Brfalse, falseLab);

            //Both the True and False parts may be the tail expressions
            //Also this whole operator can be used as a statement. Or can be compiled
            //in a situation when some of the referenced names are not initialized (Lazy)
            var left = (hints & Hints.Left) == Hints.Left ? Hints.Left : Hints.None;
            var tail = (hints & Hints.Tail) == Hints.Tail ? Hints.Tail : Hints.None;
            
            if (s.True != null)
                CompileExpression(s.True, map, left | tail | Hints.Scope, s);

            if (s.False != null)
            {
                var skipLabel = cw.DefineLabel();
                cw.Emit(Op.Br, skipLabel);
                cw.MarkLabel(falseLab);
                CompileExpression(s.False, map, left | tail | Hints.Scope, s);
                cw.MarkLabel(skipLabel);
                cw.Emit(Op.Nop);
            }
            else
            {
                AddError(ElaCompilerError.ElseMissing, s.True);
                AddHint(ElaCompilerHint.AddElse, s.True);
            }
        }

        //Compiles a sequencing operator
        private void CompileSeq(ElaExpression parent, ElaExpression left, ElaExpression right, LabelMap map, Hints hints)
        {
            var ut = hints;

            if ((ut & Hints.Left) == Hints.Left)
                ut ^= Hints.Left;

            //Sequence operators forces left expression, pops it and yields a value
            //of a right expression. Evaliation is done in a strict order.
            CompileExpression(left, map, Hints.None, parent);
            cw.Emit(Op.Force);
            cw.Emit(Op.Pop);
            CompileExpression(right, map, ut, parent);
        }

        //Compiles logical AND operator in a lazy manner.
        private void CompileLogicalAnd(ElaExpression parent, ElaExpression left, ElaExpression right, LabelMap map, Hints hints)
        {
            //Logical AND is executed in a lazy manner
            var exitLab = default(Label);
            var termLab = default(Label);
            var ut = hints;

            if ((ut & Hints.Left) == Hints.Left)
                ut ^= Hints.Left;

            if ((ut & Hints.Tail) == Hints.Tail)
                ut ^= Hints.Tail;
            
            CompileExpression(left, map, ut, parent);
            termLab = cw.DefineLabel();
            exitLab = cw.DefineLabel();
            cw.Emit(Op.Brfalse, termLab);
            CompileExpression(right, map, ut, parent);
            cw.Emit(Op.Br, exitLab);
            cw.MarkLabel(termLab);
            cw.Emit(Op.PushI1_0);
            cw.MarkLabel(exitLab);
            cw.Emit(Op.Nop);
        }

        //Compiles logical OR operator in a lazy manner.
        private void CompileLogicalOr(ElaExpression parent, ElaExpression left, ElaExpression right, LabelMap map, Hints hints)
        {
            //Logical OR is executed in a lazy manner
            var exitLab = default(Label);
            var termLab = default(Label);
            var ut = hints;

            if ((ut & Hints.Left) == Hints.Left)
                ut ^= Hints.Left;

            if ((ut & Hints.Tail) == Hints.Tail)
                ut ^= Hints.Tail;
            
            CompileExpression(left, map, ut, parent);
            termLab = cw.DefineLabel();
            exitLab = cw.DefineLabel();
            cw.Emit(Op.Brtrue, termLab);
            CompileExpression(right, map, ut, parent);
            cw.Emit(Op.Br, exitLab);
            cw.MarkLabel(termLab);
            cw.Emit(Op.PushI1_1);
            cw.MarkLabel(exitLab);
            cw.Emit(Op.Nop);
        }
	}
}