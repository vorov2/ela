using System;
using System.Collections.Generic;
using System.IO;
using Ela.Compilation;
using Ela.Debug;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using Ela.Runtime.Classes;
using Ela.CodeModel;

namespace Ela.Runtime
{
    public sealed class ElaMachine : IDisposable
    {
        #region Construction
        internal ElaValue[][] modules;
        private readonly CodeAssembly asm;
        private int lastOffset;

        public ElaMachine(CodeAssembly asm)
        {
            this.asm = asm;
            var frame = asm.GetRootModule();
            MainThread = new WorkerThread(asm);
            var lays = frame.Layouts[0];
            modules = new ElaValue[asm.ModuleCount][];
            var mem = new ElaValue[lays.Size];
            modules[0] = mem;
            MainThread.CallStack.Push(new CallPoint(0, new EvalStack(lays.StackSize, false), mem, FastList<ElaValue[]>.Empty));
            BuildFunTable();
        }
        #endregion


        #region InternalTypes
        internal const int UNI = (Int32)ElaTypeCode.Unit;
        internal const int INT = (Int32)ElaTypeCode.Integer;
        internal const int REA = (Int32)ElaTypeCode.Single;
        internal const int BYT = (Int32)ElaTypeCode.Boolean;
        internal const int CHR = (Int32)ElaTypeCode.Char;
        internal const int LNG = (Int32)ElaTypeCode.Long;
        internal const int DBL = (Int32)ElaTypeCode.Double;
        internal const int STR = (Int32)ElaTypeCode.String;
        internal const int LST = (Int32)ElaTypeCode.List;
        internal const int RES = (Int32)ElaTypeCode.__Reserved;
        internal const int TUP = (Int32)ElaTypeCode.Tuple;
        internal const int REC = (Int32)ElaTypeCode.Record;
        internal const int FUN = (Int32)ElaTypeCode.Function;
        internal const int OBJ = (Int32)ElaTypeCode.Object;
        internal const int MOD = (Int32)ElaTypeCode.Module;
        internal const int LAZ = (Int32)ElaTypeCode.Lazy;
        internal const int RES2 = (Int32)ElaTypeCode.__Reserved2;
        internal const int RES3 = (Int32)ElaTypeCode.__Reserved3;

        private Class[] cls;
        #endregion


        #region Public Methods
        public void Dispose()
        {
            var ex = default(Exception);

            foreach (var m in asm.EnumerateForeignModules())
            {
                try
                {
                    m.Close();
                }
                catch (Exception e)
                {
                    ex = e;
                }
            }

            if (ex != null)
                throw ex;
        }


        public string PrintValue(ElaValue val)
        {
            if (val.Ref == null)
                return "_|_";

            if (val.TypeCode == ElaTypeCode.Lazy)
                val = ((ElaLazy)val.Ref).Force();
                        
            var ctx = new ExecutionContext();
            var ret = cls[val.TypeId].Show(val, ctx);

            if (ctx.Failed && ctx.Fun == null)
                return val.ToString();
            else if (ctx.Failed)
            {
                try
                {
                    var rval = ctx.Fun.Call(val);
                    return rval.DirectGetString();
                }
                catch
                {
                    return val.ToString();
                }
            }
            else
                return ret;
        }


        public ExecutionResult Run()
        {
            MainThread.Offset = MainThread.Offset == 0 ? 0 : MainThread.Offset;
            lastOffset = MainThread.Module.Ops.Count;
            var ret = default(ElaValue);

            try
            {
                ret = Execute(MainThread);
            }
            catch (ElaException)
            {
                throw;
            }
#if !DEBUG
            catch (NullReferenceException ex)
            {
                var err = new ElaError(ElaRuntimeError.BottomReached);
                throw CreateException(Dump(err, MainThread), ex);
            }
#endif
            catch (OutOfMemoryException)
            {
                throw Exception("OutOfMemory");
            }
#if !DEBUG
            catch (Exception ex)
            {
                var op = MainThread.Module != null && MainThread.Offset > 0 &&
                    MainThread.Offset - 1 < MainThread.Module.Ops.Count ?
                    MainThread.Module.Ops[MainThread.Offset - 1].ToString() : String.Empty;
                throw Exception("CriticalError", ex, MainThread.Offset - 1, op);
            }
#endif

            var evalStack = MainThread.CallStack[0].Stack;

            if (evalStack.Count > 0)
                throw Exception("StackCorrupted");

            return new ExecutionResult(ret);
        }


        public ExecutionResult Run(int offset)
        {
            MainThread.Offset = offset;
            return Run();
        }


        public ExecutionResult Resume()
        {
            RefreshState();
            return Run(lastOffset);
        }


        public void Recover()
        {
            if (modules.Length > 0)
            {
                var m = modules[0];

                for (var i = 0; i < m.Length; i++)
                {
                    var v = m[i];

                    if (v.Ref == null)
                        m[i] = new ElaValue(ElaUnit.Instance);
                }
            }
        }


        public void RefreshState()
        {
            if (modules.Length > 0)
            {
                if (modules.Length != asm.ModuleCount)
                {
                    var mods = new ElaValue[asm.ModuleCount][];

                    for (var i = 0; i < modules.Length; i++)
                        mods[i] = modules[i];

                    modules = mods;
                }

                var mem = modules[0];
                var frame = asm.GetRootModule();
                var arr = new ElaValue[frame.Layouts[0].Size];
                Array.Copy(mem, 0, arr, 0, mem.Length);
                modules[0] = arr;
                MainThread.SwitchModule(0);
                MainThread.CallStack.Clear();
                var cp = new CallPoint(0, new EvalStack(frame.Layouts[0].StackSize, false), arr, FastList<ElaValue[]>.Empty);
                MainThread.CallStack.Push(cp);
            }

            if (asm != null)
                BuildFunTable();
        }


        private void BuildFunTable()
        {
            cls = asm.Cls.ToArray();
        }

        public ElaValue GetVariableByHandle(int moduleHandle, int varHandle)
        {
            var mod = default(ElaValue[]);

            try
            {
                mod = modules[moduleHandle];
            }
            catch (IndexOutOfRangeException)
            {
                throw Exception("InvalidModuleHandle");
            }

            try
            {
                return mod[varHandle];
            }
            catch (IndexOutOfRangeException)
            {
                throw Exception("InvalidVariableAddress");
            }
        }
        #endregion


        #region Execute
        private ElaValue Execute(WorkerThread thread)
        {
            var callStack = thread.CallStack;
            var evalStack = callStack.Peek().Stack;
            var frame = thread.Module;
            var ops = thread.Module.Ops.GetRawArray();
            var opData = thread.Module.OpData.GetRawArray();
            var locals = callStack.Peek().Locals;
            var captures = callStack.Peek().Captures;

            var ctx = thread.Context;
            var left = default(ElaValue);
            var right = default(ElaValue);
            var i4 = 0;

        CYCLE:
            {
                #region Body
                var op = ops[thread.Offset];
                var opd = opData[thread.Offset];
                thread.Offset++;

                switch (op)
                {
                    #region Stack Operations
                    case Op.Pushext:
                        evalStack.Push(modules[frame.HandleMap[opd & Byte.MaxValue]][opd >> 8]);
                        break;
                    case Op.Pushloc:
                        evalStack.Push(locals[opd]);
                        break;
                    case Op.Pushvar:
                        right = captures[captures.Count - (opd & Byte.MaxValue)][opd >> 8];
                        evalStack.Push(right);
                        break;
                    case Op.Pushstr:
                        evalStack.Push(new ElaValue(frame.Strings[opd]));
                        break;
                    case Op.Pushstr_0:
                        evalStack.Push(ElaString.Empty);
                        break;
                    case Op.PushI4:
                        evalStack.Push(opd);
                        break;
                    case Op.PushI4_0:
                        evalStack.Push(0);
                        break;
                    case Op.PushI1_0:
                        evalStack.Push(new ElaValue(false));
                        break;
                    case Op.PushI1_1:
                        evalStack.Push(new ElaValue(true));
                        break;
                    case Op.PushR4:
                        evalStack.Push(new ElaValue(opd, ElaSingle.Instance));
                        break;
                    case Op.PushCh:
                        evalStack.Push(new ElaValue((Char)opd));
                        break;
                    case Op.Pushunit:
                        evalStack.Push(new ElaValue(ElaUnit.Instance));
                        break;
                    case Op.Pushelem:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[right.TypeId].GetValue(right, left, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Pushfld:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[right.TypeId].GetField(right, left, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Hasfld:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[right.TypeId].HasField(right, left, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Pop:
                        evalStack.PopVoid();
                        break;
                    case Op.Poploc:
                        locals[opd] = evalStack.Pop();
                        break;
                    case Op.Popvar:
                        captures[captures.Count - (opd & Byte.MaxValue)][opd >> 8] = evalStack.Pop();
                        break;
                    case Op.Dup:
                        evalStack.Dup();
                        break;
                    case Op.Swap:
                        right = evalStack.PopFast();
                        left = evalStack.Peek();
                        evalStack.Replace(right);
                        evalStack.Push(left);
                        break;
                    #endregion

                    #region Object Operations
                    case Op.Show:
                        {
                            right = evalStack.Pop();
                            
                            if (right.TypeId == LAZ)
                                right = right.Ref.Force(right, ctx);

                            if (ctx.Failed)
                            {
                                evalStack.Push(right);
                                ExecuteFail(ctx.Error, thread, evalStack);
                                goto SWITCH_MEM;
                            }

                            evalStack.Push(new ElaValue(cls[right.TypeId].Show(right, ctx)));

                            if (ctx.Failed)
                            {
                                evalStack.Replace(right);
                                ExecuteFail(ctx.Error, thread, evalStack);
                                goto SWITCH_MEM;
                            }
                        }
                        break;
                    case Op.Reccons:
                        right = evalStack.Pop();
                        left = evalStack.Pop();
                        ((ElaRecord)evalStack.Peek().Ref).SetField(opd, right.DirectGetString(), left);
                        break;
                    case Op.Tupcons:
                        right = evalStack.Pop();
                        ((ElaTuple)evalStack.Peek().Ref).Values[opd] = right;
                        break;
                    case Op.Cons:
                        left = evalStack.Pop();
                        right = evalStack.Peek();
                        evalStack.Replace(right.Ref.Cons(left, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Genfin:
                        right = evalStack.Peek();
                        evalStack.Replace(right.Ref.GenerateFinalize(ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Tail:
                        right = evalStack.Peek();

                        if (right.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        evalStack.Replace(cls[right.TypeId].Tail(right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Head:
                        right = evalStack.Peek();

                        if (right.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        evalStack.Replace(cls[right.TypeId].Head(right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Isnil:
                        right = evalStack.Peek();

                        if (right.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        evalStack.Replace(new ElaValue(cls[right.TypeId].IsNil(right, ctx)));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Len:
                        right = evalStack.Peek();

                        if (right.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        evalStack.Replace(cls[right.TypeId].GetLength(right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }

                        break;
                    case Op.Force:
                        if (evalStack.Peek().TypeId == LAZ)
                        {
                            right = evalStack.Peek();
                            evalStack.Replace(right.Ref.Force(right, ctx));

                            if (ctx.Failed)
                            {
                                evalStack.Replace(right);
                                ExecuteThrow(thread, evalStack);
                                goto SWITCH_MEM;
                            }
                        }
                        break;
                    case Op.Untag:
                        right = evalStack.Peek();

                        if (right.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        evalStack.Replace(right.Ref.Untag(asm, ctx, opd));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    #endregion

                    #region Goto Operations
                    case Op.Skiptag:
                        left = evalStack.Pop();
                        right = evalStack.Pop();

                        if (right.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        if (left.I4 == right.Ref.GetTag())
                        {
                            thread.Offset++;
                            break;
                        }

                        if (ctx.Failed)
                        {
                            evalStack.Push(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Brtrue:
                        right = evalStack.PopFast();

                        if (right.Ref.True(right, ctx))
                            thread.Offset = opd;

                        if (ctx.Failed)
                        {
                            evalStack.Push(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Brfalse:
                        right = evalStack.PopFast();

                        if (right.Ref.False(right, ctx))
                            thread.Offset = opd;

                        if (ctx.Failed)
                        {
                            evalStack.Push(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Br:
                        thread.Offset = opd;
                        break;
                    #endregion

                    #region Binary Operations
                    case Op.AndBw:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].BitwiseAnd(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.OrBw:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].BitwiseOr(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Xor:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].BitwiseXor(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Shl:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].ShiftLeft(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Shr:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].ShiftRight(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Concat:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ)
                            left = left.Ref.Force(left, ctx);

                        evalStack.Replace(cls[left.TypeId].Concatenate(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Add:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Add(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Sub:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Subtract(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Div:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Divide(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Quot:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(left.Ref.Quot(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Mul:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Multiply(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Pow:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Power(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Rem:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Remainder(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Mod:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.Ref.TypeId == LAZ || right.Ref.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Modulus(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Cgt:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Greater(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Clt:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Lesser(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Ceq:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].Equal(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Cneq:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].NotEqual(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Cgteq:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].GreaterEqual(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Clteq:
                        left = evalStack.Pop();
                        right = evalStack.Peek();

                        if (left.TypeId == LAZ || right.TypeId == LAZ)
                        {
                            left = left.Ref.Force(left, ctx);
                            right = right.Ref.Force(right, ctx);
                        }

                        evalStack.Replace(cls[left.TypeId].LesserEqual(left, right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Ctype:
                        left = evalStack.Pop();
                        right = evalStack.Peek();
                        evalStack.Replace(right.TypeId == LAZ || right.TypeId == left.I4);
                        break;
                    #endregion

                    #region Unary Operations
                    case Op.Not:
                        right = evalStack.Peek();
                        right = right.Ref.Force(right, ctx);

                        if (right.TypeId == BYT)
                        {
                            evalStack.Replace(right.I4 != 1);
                            break;
                        }
                        else if (right.TypeId != LAZ)
                        {
                            InvalidType(right, thread, evalStack, "bool");
                            goto SWITCH_MEM;
                        }

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Neg:
                        right = evalStack.Peek();

                        if (right.Ref.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        evalStack.Replace(cls[right.TypeId].Negate(right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.NotBw:
                        right = evalStack.Peek();

                        if (right.TypeId == LAZ)
                            right = right.Ref.Force(right, ctx);

                        evalStack.Replace(cls[right.TypeId].BitwiseNot(right, ctx));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    #endregion

                    #region CreateNew Operations
                    case Op.Newlazy:
                        evalStack.Push(new ElaValue(new ElaLazy((ElaFunction)evalStack.Pop().Ref)));
                        break;
                    case Op.Newfun:
                        {
                            var lst = new FastList<ElaValue[]>(captures);
                            lst.Add(locals);
                            var fun = new ElaFunction(opd, thread.ModuleHandle, evalStack.Pop().I4, lst, this);
                            evalStack.Push(new ElaValue(fun));
                        }
                        break;
                    case Op.Newfunc:
                        {
                            var fn = new ElaFunTable(frame.Strings[opd], evalStack.Pop().I4, evalStack.Pop().I4, 0, this);
                            evalStack.Push(new ElaValue(fn));
                        }
                        break;
                    case Op.Newconst:
                        {
                            var cs = new ElaConstant(frame.Strings[opd]);
                            evalStack.Push(new ElaValue(cs));
                        }
                        break;
                    case Op.Disp:
                        left = evalStack.Pop();
                        right = evalStack.Peek();
                        
                        if (left.TypeId == UNI)
                            evalStack.Replace(((ElaConstant)right.Ref).GetConstantValue(this, ctx, callStack.Peek().Context));
                        else
                            evalStack.Replace(((ElaConstant)right.Ref).GetConstantValue(this, ctx, left.I4));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Addmbr:
                        {
                            var typ = evalStack.Pop().I4;
                            left = evalStack.Pop();
                            right = evalStack.Pop();

                            if (right.TypeId != FUN && left.TypeId != OBJ)
                            {
                                InvalidType(right, thread, evalStack, TCF.GetShortForm(ElaTypeCode.Function));
                                goto SWITCH_MEM;
                            }

                            if (left.TypeId == INT)
                                cls[typ].AddFunction((ElaBuiltinKind)left.I4, (ElaFunction)right.Ref);
                            else if (left.TypeId == FUN)
                                ((ElaFunTable)left.Ref).AddFunction(typ, (ElaFunction)right.Ref);
                            else
                                ((ElaConstant)left.Ref).AddConstant(typ, right);
                        }
                        break;
                    case Op.Newlist:
                        evalStack.Push(ElaList.Empty);
                        break;
                    case Op.Newrec:
                        evalStack.Push(new ElaRecord(opd));
                        break;
                    case Op.Newtup:
                        evalStack.Push(new ElaTuple(opd));
                        break;
                    case Op.Newtup_2:
                        {
                            var tup = new ElaTuple(2);
                            tup.Values[1] = evalStack.Pop();
                            tup.Values[0] = evalStack.Peek();
                            evalStack.Replace(tup);
                        }
                        break;
                    case Op.NewI8:
                        {
                            right = evalStack.Pop();
                            left = evalStack.Peek();
                            var conv = new Conv();
                            conv.I4_1 = left.I4;
                            conv.I4_2 = right.I4;
                            evalStack.Replace(new ElaValue(conv.I8));
                        }
                        break;
                    case Op.NewR8:
                        {
                            right = evalStack.Pop();
                            left = evalStack.Peek();
                            var conv = new Conv();
                            conv.I4_1 = left.I4;
                            conv.I4_2 = right.I4;
                            evalStack.Replace(new ElaValue(conv.R8));
                        }
                        break;
                    case Op.Newmod:
                        {
                            i4 = frame.HandleMap[opd];
                            evalStack.Push(new ElaModule(i4, this));
                        }
                        break;
                    #endregion

                    #region Thunk Operations
                    case Op.Flip:
                        {
                            right = evalStack.Peek();

                            if (right.TypeId == LAZ)
                                right = right.Ref.Force(right, ctx);

                            if (ctx.Failed)
                            {
                                ExecuteThrow(thread, evalStack);
                                goto SWITCH_MEM;
                            }

                            if (right.TypeId != FUN)
                            {
                                evalStack.PopVoid();
                                ExecuteFail(new ElaError(ElaRuntimeError.ExpectedFunction, TCF.GetShortForm(right.TypeCode)), thread, evalStack);
                                goto SWITCH_MEM;
                            }

                            var fun = (ElaFunction)right.Ref;
                            fun = fun.Captures != null ? fun.CloneFast() : fun.Clone();
                            fun.Flip = !fun.Flip;
                            evalStack.Replace(new ElaValue(fun));
                        }
                        break;
                    case Op.Call:
                        if (Call(evalStack.Pop().Ref, thread, evalStack, CallFlag.None, null))
                            goto SWITCH_MEM;
                        break;
                    case Op.LazyCall:
                        {
                            var o = (ElaFunction)evalStack.Pop().Ref;

                            if (!o.table)
                            {
                                o = o.CloneFast();
                                o.LastParameter = evalStack.Pop();
                            }
                            else
                                o = ((ElaFunTable)o).GetFunction(evalStack.Pop(), ctx, thread.CallStack.Peek().Context);
                            
                            evalStack.Push(new ElaValue(-1, new ElaLazy(o)));
                        }
                        break;
                    case Op.Callt:
                        {
                            var funObj = evalStack.Peek().Ref;

                            if (callStack.Peek().Thunk != null || !funObj.CanTailCall())
                            {
                                if (Call(evalStack.Pop().Ref, thread, evalStack, CallFlag.None, null))
                                    goto SWITCH_MEM;
                                break;
                            }

                            var cp = callStack.Pop();
                            evalStack.PopVoid();

                            if (Call(funObj, thread, evalStack, CallFlag.NoReturn, (int?)cp.Context))
                                goto SWITCH_MEM;
                            else
                                callStack.Push(cp);
                        }
                        break;
                    case Op.Ret:
                        {
                            var cc = callStack.Pop();

                            if (cc.Thunk != null)
                            {
                                cc.Thunk.Value = evalStack.Pop();
                                thread.Offset = callStack.Peek().BreakAddress;

                                if (cc.Thunk.Value.TypeId == LAZ)
                                {
                                    cc.Thunk.Value = cc.Thunk.Value.Ref.Force(cc.Thunk.Value, ctx);

                                    if (ctx.Failed)
                                    {
                                        thread.Offset++;
                                        ExecuteThrow(thread, evalStack);
                                        goto SWITCH_MEM;
                                    }
                                }

                                goto SWITCH_MEM;
                            }

                            if (callStack.Count > 0)
                            {
                                var om = callStack.Peek();
                                om.Stack.Push(evalStack.Pop());

                                if (om.BreakAddress == 0)
                                    return default(ElaValue);
                                else
                                {
                                    thread.Offset = om.BreakAddress;
                                    goto SWITCH_MEM;
                                }
                            }
                            else
                                return evalStack.Pop();
                        }
                    #endregion

                    #region Misc
                    case Op.Nop:
                        break;
                    case Op.Ctx:
                        callStack.Peek().Context = evalStack.Pop().I4;
                        break;
                    case Op.Api:
                        right = evalStack.Pop();
                        evalStack.Push(ApiCall(opd, right, new ElaValue(), evalStack, thread));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Api2:
                        left = evalStack.Pop();
                        right = evalStack.Pop();
                        evalStack.Push(ApiCall(opd, left, right, evalStack, thread));

                        if (ctx.Failed)
                        {
                            evalStack.Replace(right);
                            evalStack.Push(left);
                            ExecuteThrow(thread, evalStack);
                            goto SWITCH_MEM;
                        }
                        break;
                    case Op.Newtype:
                        {
                            var tid = evalStack.Pop().I4;
                            left = evalStack.Pop();
                            right = evalStack.Pop();
                            var t = asm.Types[tid];                            
                            evalStack.Push(new ElaValue(new ElaUserTypeVariadic(t.TypeName, t.TypeCode, left.I4, ((ElaTuple)right.Ref).Values)));
                        }
                        break;
                    case Op.Newtype0:
                        {
                            var tid = evalStack.Pop().I4;
                            left = evalStack.Pop();
                            var t = asm.Types[tid];
                            evalStack.Push(new ElaValue(new ElaUserType0(t.TypeName, t.TypeCode, left.I4)));
                        }
                        break;
                    case Op.Newtype1:
                        {
                            var tid = evalStack.Pop().I4;
                            left = evalStack.Pop();
                            right = evalStack.Pop();
                            var t = asm.Types[tid];
                            evalStack.Push(new ElaValue(new ElaUserType1(t.TypeName, t.TypeCode, left.I4, right)));
                        }
                        break;
                    case Op.Newtype2:
                        {
                            var tid = evalStack.Pop().I4;
                            left = evalStack.Pop();
                            var last = evalStack.Pop();
                            right = evalStack.Pop();
                            var t = asm.Types[tid];
                            evalStack.Push(new ElaValue(new ElaUserType2(t.TypeName, t.TypeCode, left.I4, right, last)));
                        }
                        break;
                    case Op.Traitch:
                        {
                            left = evalStack.Pop();
                            right = evalStack.Pop();

                            if (right.TypeId == LAZ)
                                right = right.Ref.Force(right, ctx);

                            if (ctx.Failed)
                            {
                                evalStack.Push(right);
                                evalStack.Push(left);
                                ExecuteThrow(thread, evalStack);
                                goto SWITCH_MEM;
                            }

                            long cc = (Int64)left.I4;
                            long tc = (Int64)right.TypeId << 32;
                            evalStack.Push(Assembly.Instances.ContainsKey(cc | tc));
                        }
                        break;
                    case Op.Ctorid:
                        evalStack.Push(thread.Module.InternalConstructors[opd].Code);
                        break;
                    case Op.Typeid:
                        evalStack.Push(thread.Module.InternalTypes[frame.Strings[opd]]);
                        break;
                    case Op.Classid:
                        evalStack.Push(thread.Module.InternalClasses[frame.Strings[opd]].Code);
                        break;
                    case Op.Throw:
                        {
                            right = evalStack.Pop();
                            var msg = right.ToString();

                            if (ctx.Failed)
                            {
                                evalStack.Push(right);
                                ExecuteThrow(thread, evalStack);
                            }
                            else
                                ExecuteFail(new ElaError(msg), thread, evalStack);

                            goto SWITCH_MEM;
                        }
                    case Op.Rethrow:
                        {
                            var err = (ElaError)evalStack.Pop().Ref;
                            ExecuteFail(err, thread, evalStack);
                            goto SWITCH_MEM;
                        }
                    case Op.Failwith:
                        {
                            ExecuteFail((ElaRuntimeError)opd, thread, evalStack);
                            goto SWITCH_MEM;
                        }
                    case Op.Stop:
                        if (callStack.Count > 1)
                        {
                            callStack.Pop();
                            var modMem = callStack.Peek();
                            thread.Offset = modMem.BreakAddress;
                            var om = thread.Module;
                            var omh = thread.ModuleHandle;
                            thread.SwitchModule(modMem.ModuleHandle);
                            right = evalStack.Pop();

                            if (modMem.BreakAddress == 0)
                                return right;
                            else
                                goto SWITCH_MEM;
                        }
                        return evalStack.Count > 0 ? evalStack.Pop() : new ElaValue(ElaUnit.Instance);
                    case Op.Runmod:
                        {
                            var hdl = frame.HandleMap[opd];

                            if (modules[hdl] == null)
                            {
                                var frm = asm.GetModule(hdl);

                                if (frm is IntrinsicFrame)
                                    modules[hdl] = ((IntrinsicFrame)frm).Memory;
                                else
                                {
                                    i4 = frm.Layouts[0].Size;
                                    var loc = new ElaValue[i4];
                                    modules[hdl] = loc;
                                    callStack.Peek().BreakAddress = thread.Offset;
                                    callStack.Push(new CallPoint(hdl, new EvalStack(frm.Layouts[0].StackSize, false), loc, FastList<ElaValue[]>.Empty));
                                    thread.SwitchModule(hdl);
                                    thread.Offset = 0;
                                    goto SWITCH_MEM;
                                }
                            }
                        }
                        break;
                    case Op.Start:
                        callStack.Peek().CatchMark = opd;
                        callStack.Peek().StackOffset = evalStack.Count;
                        break;
                    case Op.Leave:
                        callStack.Peek().CatchMark = null;
                        break;
                    #endregion
                }
                #endregion
            }
            goto CYCLE;

        SWITCH_MEM:
            {
                var mem = callStack.Peek();
                thread.SwitchModule(mem.ModuleHandle);
                locals = mem.Locals;
                captures = mem.Captures;
                ops = thread.Module.Ops.GetRawArray();
                opData = thread.Module.OpData.GetRawArray();
                frame = thread.Module;
                evalStack = mem.Stack;
            }
            goto CYCLE;

        }
        #endregion


        #region Operations
        internal ElaValue ApiCall(int code, ElaValue left, ElaValue right, EvalStack stack, WorkerThread thread)
        {
            if (left.TypeId == LAZ)
                left = left.Ref.Force(left, thread.Context);

            if (right.Ref != null && right.TypeId == LAZ)
                right = right.Ref.Force(right, thread.Context);

            if (thread.Context.Failed)
                return new ElaValue(ElaObject.ElaInvalidObject.Instance);

            switch ((Api)code)
            {
                case Api.Classes:
                    if (left.TypeId != INT)
                    {
                        thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                        break;
                    }
                    break;
                case Api.IsAlgebraic:
                    return new ElaValue(left.TypeId > SysConst.MAX_TYP);
                case Api.CurrentContext:
                    var ic = thread.CallStack.Peek().Context;

                    if (ic == 0)
                        thread.Context.Fail(ElaRuntimeError.ZeroContext);
                    else
                        return new ElaValue(ic);
                    break;
                case Api.ListToString:
                    {
                        if (left.TypeId != LST)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.List), left);
                            break;
                        }

                        var sb = new System.Text.StringBuilder();

                        foreach (var v in ((ElaList)left.Ref).Reverse())
                            sb.Append(v.AsString());

                        return new ElaValue(sb.ToString());
                    }
                case Api.LazyList:
                    return new ElaValue(left.Ref is ElaLazyList);
                case Api.ListLength:
                    return new ElaValue(((ElaList)left.Ref).GetLength());
                case Api.ReverseList:
                    if (!thread.Context.Failed)
                    {
                        if (left.TypeId != LST)
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.List), left);
                        else
                            return new ElaValue(((ElaList)left.Ref).Reverse());
                    }
                    break;
                case Api.ConsName:
                    {
                        var cid = left.Ref.GetTag();

                        if (cid >= 0)
                            return new ElaValue(asm.Constructors[cid].Name);

                        thread.Context.NotAlgebraicType(left);
                    }
                    break;
                case Api.ConsParamNumber:
                    {
                        var cid = 0;

                        if (left.TypeId == INT)
                            cid = left.I4;
                        else
                            cid = left.Ref.GetTag();

                        if (cid >= 0)
                        {
                            var pars = asm.Constructors[cid].Parameters;
                            return new ElaValue(pars != null ? pars.Count : 0);
                        }

                        thread.Context.NotAlgebraicType(left);
                    }
                    break;
                case Api.ConsIndex:
                    {
                        if (left.TypeId <= SysConst.MAX_TYP)
                            thread.Context.NotAlgebraicType(left);

                        var cons = asm.Types[left.Ref.TypeId].Constructors;
                        var r = cons.IndexOf(left.Ref.GetTag());

                        if (r < 0)
                            thread.Context.Fail(new ElaError(ElaRuntimeError.InvalidConstructor, 
                                asm.Constructors[left.Ref.GetTag()].Name,
                                left.Ref.GetTypeName()));

                        return new ElaValue(r);
                    }
                case Api.ConsCode:
                    {
                        if (left.TypeId <= SysConst.MAX_TYP)
                            thread.Context.NotAlgebraicType(left);

                        return new ElaValue(left.Ref.GetTag());
                    }
                case Api.TypeName:
                    return new ElaValue(left.GetTypeName());
                case Api.TypeCode:
                    return new ElaValue(left.Ref.TypeId);
                case Api.TypeConsNumber:
                    if (left.TypeId != INT)
                        thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                    else if (left.I4 < 0 || left.I4 >= asm.Types.Count)
                        thread.Context.InvalidTypeCode(left);
                    else
                        return new ElaValue(asm.Types[left.I4].Constructors.Count);
                    break;
                case Api.ConsInfix:
                    {
                        var cid = left.Ref.GetTag();

                        if (cid >= 0)
                        {
                            var r = asm.Constructors[cid];
                            return new ElaValue(Format.IsSymbolic(r.Name));
                        }

                        thread.Context.NotAlgebraicType(right);
                    }
                    break;
                case Api.ConsParamIndex:
                    {
                        if (left.TypeId != STR)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.String), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.I4 < 0 || right.I4 >= asm.Constructors.Count)
                        {
                            thread.Context.InvalidConstructorCode(right);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        var r = asm.Constructors[right.I4].Parameters.IndexOf(left.DirectGetString());

                        if (r < 0)
                            thread.Context.IndexOutOfRange(left, right);

                        return new ElaValue(r);
                    }
                case Api.ConsParamExist:
                    {
                        if (left.TypeId != STR)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.String), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.I4 < 0 || right.I4 >= asm.Constructors.Count)
                        {
                            thread.Context.InvalidConstructorCode(right);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        var r = asm.Constructors[right.I4].Parameters.IndexOf(left.DirectGetString());
                        return new ElaValue(r >= 0);
                    }
                case Api.ConsParamValue:
                    {
                        if (left.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.Ref.TypeId > SysConst.MAX_TYP)
                            return ((ElaUserType)right.Ref).Untag(asm, thread.Context, left.I4);

                        thread.Context.NotAlgebraicType(right);
                    }
                    break;
                case Api.ConsParamName:
                    {
                        if (left.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.I4 < 0 || right.I4 >= asm.Constructors.Count)
                        {
                            thread.Context.InvalidConstructorCode(right);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        var pars = asm.Constructors[right.I4].Parameters;

                        if (left.I4 < 0 || left.I4 >= pars.Count)
                            thread.Context.IndexOutOfRange(left, right);
                        else
                            return new ElaValue(pars[left.I4]);

                        thread.Context.NotAlgebraicType(right);
                    }
                    break;
                case Api.ConsDefault:
                    {
                        if (left.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (left.I4 < 0 || left.I4 >= asm.Constructors.Count)
                            thread.Context.InvalidConstructorCode(left);
                        else
                        {
                            var cd = asm.Constructors[left.I4];

                            if (cd.Parameters == null)
                                return modules[cd.ModuleId][cd.ConsAddress];
                            else
                                thread.Context.Fail(ElaRuntimeError.UnableCreateConstructor, cd.Name, "Constructor has parameters");
                        }
                    }
                    break;
                case Api.RecordField:
                    {
                        if (right.TypeId != REC)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Record), right);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        var rec = (ElaRecord)right.Ref;
                        return rec.GetKey(left, thread.Context);
                    }
                case Api.ConsCreate:
                    {
                        if (left.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (left.I4 < 0 || left.I4 >= asm.Constructors.Count)
                            thread.Context.InvalidConstructorCode(left);
                        else
                        {
                            var cd = asm.Constructors[left.I4];
                            return modules[cd.ModuleId][cd.ConsAddress];
                        }
                    }
                    break;
                case Api.ConsCodeByIndex:
                    {
                        if (left.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (right.TypeId != INT)
                        {
                            thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), right);
                            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                        }

                        if (left.I4 < 0 || left.I4 >= asm.Types.Count)
                            thread.Context.InvalidTypeCode(left);
                        else
                        {
                            var td = asm.Types[left.I4];

                            if (right.I4 < 0 || right.I4 >= td.Constructors.Count)
                                thread.Context.IndexOutOfRange(right, new ElaValue(new ElaUserType0(td.TypeName, td.TypeCode, -1)));
                            else
                                return new ElaValue(td.Constructors[right.I4]);
                        }
                    }
                    break;
                case Api.ConsNameIndex:
                    if (left.TypeId != INT)
                    {
                        thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.Integer), left);
                        return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                    }

                    if (right.TypeId != STR)
                    {
                        thread.Context.InvalidType(TCF.GetShortForm(ElaTypeCode.String), right);
                        return new ElaValue(ElaObject.ElaInvalidObject.Instance);
                    }

                    if (left.I4 < 0 || left.I4 >= asm.Types.Count)
                        thread.Context.InvalidTypeCode(left);
                    else
                    {
                        var td = asm.Types[left.I4];
                        var name = right.DirectGetString();

                        for (var i = 0; i < td.Constructors.Count; i++)
                        {
                            var cd = asm.Constructors[td.Constructors[i]];

                            if (cd.Name == name)
                                return new ElaValue(i);
                        }

                        return new ElaValue(-1);
                    }
                    break;                
            }

            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
        }

        internal ElaValue CallPartial(ElaFunction fun, ElaValue arg)
        {
            if (fun.AppliedParameters < fun.Parameters.Length)
            {
                fun = fun.Clone();
                fun.Parameters[fun.AppliedParameters++] = arg;
                return new ElaValue(fun);
            }

            var t = MainThread.Clone();

            if (fun.ModuleHandle != t.ModuleHandle)
                t.SwitchModule(fun.ModuleHandle);

            var layout = t.Module.Layouts[fun.Handle];
            var stack = new EvalStack(layout.StackSize, false);

            stack.Push(arg);

            if (fun.AppliedParameters > 0)
                for (var i = 0; i < fun.AppliedParameters; i++)
                    stack.Push(fun.Parameters[fun.AppliedParameters - i - 1]);

            var cp = new CallPoint(fun.ModuleHandle, stack, new ElaValue[layout.Size], fun.Captures);
            t.CallStack.Push(cp);
            t.Offset = layout.Address;
            return Execute(t);
        }


        internal ElaValue Call(ElaFunction fun, ElaValue[] args)
        {
            var t = MainThread.Clone();

            if (fun.ModuleHandle != t.ModuleHandle)
                t.SwitchModule(fun.ModuleHandle);

            var layout = t.Module.Layouts[fun.Handle];
            var stack = new EvalStack(layout.StackSize, false);
            var len = args.Length;

            for (var i = 0; i < len; i++)
                stack.Push(args[len - i - 1]);

            if (fun.AppliedParameters > 0)
            {
                for (var i = 0; i < fun.AppliedParameters; i++)
                    stack.Push(fun.Parameters[fun.AppliedParameters - i - 1]);
            }

            if (len + fun.AppliedParameters != fun.Parameters.Length + 1)
            {
                InvalidParameterNumber(fun.Parameters.Length + 1, len, t, stack);
                return default(ElaValue);
            }

            var cp = new CallPoint(fun.ModuleHandle, stack, new ElaValue[layout.Size], fun.Captures);
            t.CallStack.Push(cp);
            t.Offset = layout.Address;
            return Execute(t);

        }


        private bool Call(ElaObject fun, WorkerThread thread, EvalStack stack, CallFlag cf, int? ctx)
        {
            if (fun.TypeId != FUN)
            {
                var rf = fun.Force(new ElaValue(fun), thread.Context).Ref;

                if (rf.TypeId == FUN)
                {
                    fun = rf;
                    goto goon;
                }

                if (thread.Context.Failed)
                {
                    stack.Push(new ElaValue(fun));
                    ExecuteThrow(thread, stack);
                    return true;
                }

                InvalidType(new ElaValue(rf), thread, stack, "fun");
                return true;
            }

        goon:

            var natFun = (ElaFunction)fun;

            if (natFun.table)
            {
                natFun = ((ElaFunTable)fun).GetFunction(stack.Peek(), thread.Context,
                    ctx == null ? thread.CallStack.Peek().Context : ctx.Value);

                if (thread.Context.Failed)
                {
                    stack.Push(new ElaValue(fun));
                    ExecuteThrow(thread, stack);
                    return true;
                }
            }

            if (natFun.Captures != null)
            {
                if (natFun.AppliedParameters < natFun.Parameters.Length)
                {
                    var newFun = natFun.CloneFast();
                    newFun.Parameters[natFun.AppliedParameters] = stack.Peek();
                    newFun.AppliedParameters++;
                    stack.Replace(new ElaValue(newFun));
                    return false;
                }

                if (natFun.AppliedParameters == natFun.Parameters.Length)
                {
                    if (natFun.ModuleHandle != thread.ModuleHandle)
                        thread.SwitchModule(natFun.ModuleHandle);

                    var p = cf != CallFlag.AllParams ? stack.Pop() : natFun.LastParameter;

                    var mod = thread.Module;
                    var layout = mod.Layouts[natFun.Handle];

                    var newLoc = new ElaValue[layout.Size];

                    //If CallFalg.NoReturn is set than this is a tail call. For a tail call we
                    //can reuse an already allocated eval stack array.
                    var newStack = new EvalStack(layout.StackSize, stack.tail && cf == CallFlag.NoReturn);
                    var newMem = new CallPoint(natFun.ModuleHandle, newStack, newLoc, natFun.Captures);
                    newMem.Context = ctx != null ? ctx.Value : thread.CallStack.Peek().Context;

                    if (cf != CallFlag.NoReturn)
                        thread.CallStack.Peek().BreakAddress = thread.Offset;

                    if (p.Ref != null)
                        newStack.Push(p);

                    for (var i = 0; i < natFun.Parameters.Length; i++)
                        newStack.Push(natFun.Parameters[natFun.Parameters.Length - i - 1]);

                    if (natFun.Flip)
                    {
                        var right = newStack.Pop();
                        var left = newStack.Peek();
                        newStack.Replace(right);
                        newStack.Push(left);
                    }

                    thread.CallStack.Push(newMem);
                    thread.Offset = layout.Address;
                    return true;
                }

                InvalidParameterNumber(natFun.Parameters.Length + 1, natFun.Parameters.Length + 2, thread, stack);
                return true;
            }

            if (natFun.AppliedParameters == natFun.Parameters.Length)
                return CallExternal(thread, stack, natFun);
            else if (natFun.AppliedParameters < natFun.Parameters.Length)
            {
                var newFun = natFun.Clone();
                newFun.Parameters[newFun.AppliedParameters] = stack.Peek();
                newFun.AppliedParameters++;
                stack.Replace(new ElaValue(newFun));
                return true;
            }

            InvalidParameterNumber(natFun.Parameters.Length + 1, natFun.Parameters.Length + 2, thread, stack);
            return true;
        }


        private bool CallExternal(WorkerThread thread, EvalStack stack, ElaFunction funObj)
        {
            try
            {
                var arr = new ElaValue[funObj.Parameters.Length + 1];

                if (funObj.AppliedParameters > 0)
                    Array.Copy(funObj.Parameters, arr, funObj.Parameters.Length);

                arr[funObj.Parameters.Length] = stack.Pop();

                if (funObj.Flip)
                {
                    var x = arr[0];
                    arr[0] = arr[1];
                    arr[1] = x;
                }

                stack.Push(funObj.Call(arr));
            }
            catch (ElaRuntimeException ex)
            {
                thread.LastException = ex;
                ExecuteFail(ex.ErrorObj ?? new ElaError(ex.Error, ex.Arguments), thread, stack);
                return true;
            }
            catch (Exception ex)
            {
                thread.LastException = ex;
                ExecuteFail(new ElaError(ElaRuntimeError.CallFailed, ex.Message), thread, stack);
                return true;
            }

            return false;
        }
        #endregion


        #region Exceptions
        private void ExecuteFail(ElaRuntimeError err, WorkerThread thread, EvalStack stack)
        {
            thread.Context.Fail(err);
            ExecuteThrow(thread, stack);
        }


        private void ExecuteFail(ElaError err, WorkerThread thread, EvalStack stack)
        {
            thread.Context.Fail(err);
            ExecuteThrow(thread, stack);
        }


        private void ExecuteThrow(WorkerThread thread, EvalStack stack)
        {
            if (thread.Context.Thunk != null)
            {
                thread.Context.Failed = false;
                var t = thread.Context.Thunk;
                thread.Context.Thunk = null;
                var fn = t.Function;
                thread.Offset--;

                if (fn != null)
                {
                    Call(t.Function, thread, stack, CallFlag.AllParams, null);
                    thread.CallStack.Peek().Thunk = t;
                }
                else
                    stack.Push(t.Value);

                return;
            }
            else if (thread.Context.Fun != null)
            {
                thread.Context.Failed = false;
                var f = thread.Context.Fun.Clone();
                thread.Context.Fun = null;
                var args = thread.Context.DefferedArgs;

                if (f.Parameters.Length + 1 - f.AppliedParameters > args)
                {
                    f.Parameters[f.AppliedParameters] = stack.Pop();
                    f.AppliedParameters++;
                    stack.Push(new ElaValue(f));
                }
                else if (args == 1)
                    f.LastParameter = stack.Pop();
                else if (args == 2)
                {
                    f.Parameters[f.AppliedParameters] = stack.Pop();
                    f.AppliedParameters += 1;
                    f.LastParameter = stack.Pop();
                }

                Call(f, thread, stack, CallFlag.AllParams, null);
                return;
            }

            var err = thread.Context.Error;
            thread.Context.Reset();
            var callStack = thread.CallStack;
            var cm = default(int?);
            var i = 0;

            if (callStack.Count > 0)
            {
                do
                    cm = callStack[callStack.Count - (++i)].CatchMark;
                while (cm == null && i < callStack.Count);
            }

            if (cm == null)
                throw CreateException(Dump(err, thread), thread.LastException);
            else
            {
                var c = 1;

                while (c++ < i)
                    callStack.Pop();

                var curStack = callStack.Peek().Stack;
                curStack.Clear(callStack.Peek().StackOffset);
                curStack.Push(new ElaValue(Dump(err, thread)));
                thread.Offset = cm.Value;
            }
        }


        private ElaError Dump(ElaError err, WorkerThread thread)
        {
            if (err.Stack != null)
                return err;

            err.Module = thread.ModuleHandle;
            err.CodeOffset = thread.Offset;
            var st = new Stack<StackPoint>();

            for (var i = 0; i < thread.CallStack.Count; i++)
            {
                var cm = thread.CallStack[i];
                st.Push(new StackPoint(cm.BreakAddress, cm.ModuleHandle));
            }

            err.Stack = st;
            return err;
        }


        private ElaCodeException CreateException(ElaError err, Exception ex)
        {
            var deb = new ElaDebugger(asm);
            var mod = asm.GetModule(err.Module);
            var cs = deb.BuildCallStack(err.CodeOffset, mod, mod.File, err.Stack);

            var fi = cs.File;

            if (fi != null && StringComparer.OrdinalIgnoreCase.Equals(fi.Extension, ".elaobj"))
            {
                var nfi = new FileInfo(fi.FullName.Replace(fi.Extension, ".ela"));

                if (nfi.Exists)
                    fi = nfi;
            }

            return new ElaCodeException(err.Message.Replace("\0", ""), err.Code, fi, cs.Line, cs.Column, cs, err, ex);
        }


        private ElaMachineException Exception(string message, Exception ex, params object[] args)
        {
            return new ElaMachineException(Strings.GetMessage(message, args), ex);
        }


        private ElaMachineException Exception(string message)
        {
            return Exception(message, null);
        }


        private void InvalidType(ElaValue val, WorkerThread thread, EvalStack evalStack, string type)
        {
            ExecuteFail(new ElaError(ElaRuntimeError.InvalidType, type, val.GetTypeName()), thread, evalStack);
        }


        private void InvalidParameterNumber(int pars, int passed, WorkerThread thread, EvalStack evalStack)
        {
            if (passed == 0)
                ExecuteFail(new ElaError(ElaRuntimeError.CallWithNoParams, pars), thread, evalStack);
            else if (passed > pars)
                ExecuteFail(new ElaError(ElaRuntimeError.TooManyParams), thread, evalStack);
            else if (passed < pars)
                ExecuteFail(new ElaError(ElaRuntimeError.TooFewParams), thread, evalStack);
        }
        #endregion


        #region Properties
        public CodeAssembly Assembly { get { return asm; } }

        internal WorkerThread MainThread { get; private set; }
        #endregion
    }
}