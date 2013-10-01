using System;
using Ela.CodeModel;

namespace Ela.Compilation
{
    //This part contains compilation logic of built-in functions. Normally these are
    //functions that correspond directly to Ela machine op codes.
    internal sealed partial class Builder
    {
        //Compiles built-in as function in place. It is compiled in such a manner each time
        //it is referenced. But normally its body is just one or two op codes, so this is not a problem.
        private void CompileBuiltin(ElaBuiltinKind kind, ElaExpression exp, LabelMap map, string name)
        {
            StartSection();

            //Here we determine the number of parameters based on the function kind.
            var pars = BuiltinParams(kind);
            cw.StartFrame(pars);
            var funSkipLabel = cw.DefineLabel();
            cw.Emit(Op.Br, funSkipLabel);
            var address = cw.Offset;
            pdb.StartFunction(name, address, pars);

            AddLinePragma(exp);
            //Gets the actual typeId for built-in
            CompileBuiltinInline(kind, exp, map, Hints.None);

            cw.Emit(Op.Ret);
            frame.Layouts.Add(new MemoryLayout(currentCounter, cw.FinishFrame(), address));
            EndSection();
            pdb.EndFunction(frame.Layouts.Count - 1, cw.Offset);

            cw.MarkLabel(funSkipLabel);
            cw.Emit(Op.PushI4, pars);
            cw.Emit(Op.Newfun, frame.Layouts.Count - 1);
        }
        
        //This method can be called directly when a built-in is inlined.
        private void CompileBuiltinInline(ElaBuiltinKind kind, ElaExpression exp, LabelMap map, Hints hints)
        {
            switch (kind)
            {
                case ElaBuiltinKind.Seq:
                    cw.Emit(Op.Force);
                    cw.Emit(Op.Pop);
                    break;
                case ElaBuiltinKind.ForwardPipe:
                    cw.Emit(Op.Swap);
                    cw.Emit(Op.Call);
                    break;
                case ElaBuiltinKind.BackwardPipe:
                    cw.Emit(Op.Call);
                    break;
                case ElaBuiltinKind.LogicalOr:
                    {
                        var left = AddVariable();
                        var right = AddVariable();
                        PopVar(left);
                        PopVar(right);
                        
                        PushVar(left);
                        var termLab = cw.DefineLabel();
                        var exitLab = cw.DefineLabel();
                        cw.Emit(Op.Brtrue, termLab);

                        PushVar(right);
                        cw.Emit(Op.Br, exitLab);
                        cw.MarkLabel(termLab);
                        cw.Emit(Op.PushI1_1);
                        cw.MarkLabel(exitLab);
                        cw.Emit(Op.Nop);
                    }
                    break;
                case ElaBuiltinKind.LogicalAnd:
                    {
                        var left = AddVariable();
                        var right = AddVariable();
                        PopVar(left);
                        PopVar(right);
                        
                        PushVar(left);
                        var termLab = cw.DefineLabel();
                        var exitLab = cw.DefineLabel();
                        cw.Emit(Op.Brfalse, termLab);

                        PushVar(right);
                        cw.Emit(Op.Br, exitLab);
                        cw.MarkLabel(termLab);
                        cw.Emit(Op.PushI1_0);
                        cw.MarkLabel(exitLab);
                        cw.Emit(Op.Nop);
                    }
                    break;
                case ElaBuiltinKind.Head:
                    cw.Emit(Op.Head);
                    break;
                case ElaBuiltinKind.Tail:
                    cw.Emit(Op.Tail);
                    break;
                case ElaBuiltinKind.IsNil:
                    cw.Emit(Op.Isnil);
                    break;
                case ElaBuiltinKind.Negate:
                    cw.Emit(Op.Neg);
                    break;
                case ElaBuiltinKind.Length:
                    cw.Emit(Op.Len);
                    break;
                case ElaBuiltinKind.Force:
                    cw.Emit(Op.Force);
                    break;
                case ElaBuiltinKind.Not:
                    cw.Emit(Op.Not);
                    break;
                case ElaBuiltinKind.Show:
                    cw.Emit(Op.Show);
                    break;
                case ElaBuiltinKind.Concat:
                    cw.Emit(Op.Concat);
                    break;
                case ElaBuiltinKind.Add:
                    cw.Emit(Op.Add);
                    break;
                case ElaBuiltinKind.Divide:
                    cw.Emit(Op.Div);
                    break;
                case ElaBuiltinKind.Quot:
                    cw.Emit(Op.Quot);
                    break;
                case ElaBuiltinKind.Multiply:
                    cw.Emit(Op.Mul);
                    break;
                case ElaBuiltinKind.Power:
                    cw.Emit(Op.Pow);
                    break;
                case ElaBuiltinKind.Remainder:
                    cw.Emit(Op.Rem);
                    break;
                case ElaBuiltinKind.Modulus:
                    cw.Emit(Op.Mod);
                    break;
                case ElaBuiltinKind.Subtract:
                    cw.Emit(Op.Sub);
                    break;
                case ElaBuiltinKind.ShiftRight:
                    cw.Emit(Op.Shr);
                    break;
                case ElaBuiltinKind.ShiftLeft:
                    cw.Emit(Op.Shl);
                    break;
                case ElaBuiltinKind.Greater:
                    cw.Emit(Op.Cgt);
                    break;
                case ElaBuiltinKind.Lesser:
                    cw.Emit(Op.Clt);
                    break;
                case ElaBuiltinKind.Equal:
                    cw.Emit(Op.Ceq);
                    break;
                case ElaBuiltinKind.NotEqual:
                    cw.Emit(Op.Cneq);
                    break;
                case ElaBuiltinKind.GreaterEqual:
                    cw.Emit(Op.Cgteq);
                    break;
                case ElaBuiltinKind.LesserEqual:
                    cw.Emit(Op.Clteq);
                    break;
                case ElaBuiltinKind.BitwiseAnd:
                    cw.Emit(Op.AndBw);
                    break;
                case ElaBuiltinKind.BitwiseOr:
                    cw.Emit(Op.OrBw);
                    break;
                case ElaBuiltinKind.BitwiseXor:
                    cw.Emit(Op.Xor);
                    break;
                case ElaBuiltinKind.Cons:
                    cw.Emit(Op.Cons);
                    break;
                case ElaBuiltinKind.BitwiseNot:
                    cw.Emit(Op.NotBw);
                    break;
                case ElaBuiltinKind.GetValue:
                    cw.Emit(Op.Pushelem);
                    break;
                case ElaBuiltinKind.GetValueR:
                    cw.Emit(Op.Swap);
                    cw.Emit(Op.Pushelem);
                    break;
                case ElaBuiltinKind.GetField:
                    cw.Emit(Op.Pushfld);
                    break;
                case ElaBuiltinKind.HasField:
                    cw.Emit(Op.Hasfld);
                    break;
                /* Api */
                case ElaBuiltinKind.Api1:
                    cw.Emit(Op.Api, 1);
                    break;
                case ElaBuiltinKind.Api2:
                    cw.Emit(Op.Api, 2);
                    break;
                case ElaBuiltinKind.Api3:
                    cw.Emit(Op.Api, 3);
                    break;
                case ElaBuiltinKind.Api4:
                    cw.Emit(Op.Api, 4);
                    break;
                case ElaBuiltinKind.Api5:
                    cw.Emit(Op.Api, 5);
                    break;
                case ElaBuiltinKind.Api6:
                    cw.Emit(Op.Api, 6);
                    break;
                case ElaBuiltinKind.Api7:
                    cw.Emit(Op.Api, 7);
                    break;
                case ElaBuiltinKind.Api8:
                    cw.Emit(Op.Api, 8);
                    break;
                case ElaBuiltinKind.Api9:
                    cw.Emit(Op.Api, 9);
                    break;
                case ElaBuiltinKind.Api10:
                    cw.Emit(Op.Api, 10);
                    break;
                case ElaBuiltinKind.Api11:
                    cw.Emit(Op.Api, 11);
                    break;
                case ElaBuiltinKind.Api12:
                    cw.Emit(Op.Api, 12);
                    break;
                case ElaBuiltinKind.Api13:
                    cw.Emit(Op.Api, 13);
                    break;
                case ElaBuiltinKind.Api14:
                    cw.Emit(Op.Api, 14);
                    break;
                case ElaBuiltinKind.Api15:
                    cw.Emit(Op.Api, 15);
                    break;
                case ElaBuiltinKind.Api16:
                    cw.Emit(Op.Api, 16);
                    break;
                case ElaBuiltinKind.Api101:
                    cw.Emit(Op.Api2, 101);
                    break;
                case ElaBuiltinKind.Api102:
                    cw.Emit(Op.Api2, 102);
                    break;
                case ElaBuiltinKind.Api103:
                    cw.Emit(Op.Api2, 103);
                    break;
                case ElaBuiltinKind.Api104:
                    cw.Emit(Op.Api2, 104);
                    break;
                case ElaBuiltinKind.Api105:
                    cw.Emit(Op.Api2, 105);
                    break;
                case ElaBuiltinKind.Api106:
                    cw.Emit(Op.Api2, 106);
                    break;
                case ElaBuiltinKind.Api107:
                    cw.Emit(Op.Api2, 107);
                    break;
            }
        }

        //Determines the number of built-in parameters based on its kind.
        private int BuiltinParams(ElaBuiltinKind kind)
        {
            return kind >= ElaBuiltinKind.Seq ? 2 : 1;
        }
    }
}
