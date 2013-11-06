using System;
using Ela.CodeModel;

namespace Ela.Compilation
{
	internal sealed partial class Builder
	{
		#region Main
		private void CompileGenerator(ElaGenerator s, LabelMap map, Hints hints)
		{
			StartScope(false, s.Line, s.Column);
			var iter = cw.DefineLabel();
			var breakExit = cw.DefineLabel();
			var newMap = new LabelMap(map);

			var addr = -1;

			if (s.Pattern.Type == ElaNodeType.NameReference)
				addr = AddVariable(s.Pattern.GetName(), s.Pattern, ElaVariableFlags.None, -1);
			else
				addr = AddVariable();

			var serv = AddVariable();
			CompileExpression(s.Target, map, Hints.None, s);
			cw.Emit(Op.Dup);
            PopVar(serv);
			cw.Emit(Op.Isnil);
			cw.Emit(Op.Brtrue, breakExit);

			cw.MarkLabel(iter);
			PushVar(serv);

            cw.Emit(Op.Isnil);
            cw.Emit(Op.Brtrue, breakExit);
            PushVar(serv);
            cw.Emit(Op.Head);
            PopVar(addr);
            PushVar(serv);
            cw.Emit(Op.Tail);
            PopVar(0 | ((addr >> 8) + 1) << 8);

			if (s.Pattern.Type != ElaNodeType.NameReference)
                CompilePattern(addr, s.Pattern, iter, false /*allowBang*/, false /*forceStrict*/);

			if (s.Guard != null)
			{
				CompileExpression(s.Guard, map, Hints.None, s);
				cw.Emit(Op.Brfalse, iter);
			}

			if (s.Body != null)
			{
				CompileExpression(s.Body, newMap, Hints.Scope, s);

				if (s.Body.Type != ElaNodeType.Generator)
					cw.Emit(Op.Cons);
			}

			cw.Emit(Op.Br, iter);
			cw.MarkLabel(breakExit);
			EndScope();

			cw.Emit(Op.Nop);
		}
		#endregion


		#region Lazy
		private void CompileLazyList(ElaGenerator s, LabelMap map, Hints hints)
		{
            var fun = CompileRecursiveFor(s, map, hints, -1, -1);
			CompileExpression(s.Target, map, Hints.None, s);
			PushVar(fun);
			cw.Emit(Op.Call);
		}


		private int CompileRecursiveFor(ElaGenerator s, LabelMap map, Hints hints, int parent, int parentTail)
		{
			var funAddr = AddVariable();
			StartSection();
			StartScope(true, s.Line, s.Column);
			cw.StartFrame(1);
			var funSkipLabel = cw.DefineLabel();
			cw.Emit(Op.Br, funSkipLabel);
			var address = cw.Offset;

			var exitLab = cw.DefineLabel();
			var endLab = cw.DefineLabel();
			var iterLab = cw.DefineLabel();
			var head = AddVariable();
			var tail = AddVariable();

            var sys = AddVariable();
            cw.Emit(Op.Dup);
            PopVar(sys);
            cw.Emit(Op.Isnil);
            cw.Emit(Op.Brtrue, endLab);
            PushVar(sys);
            cw.Emit(Op.Head);
            PopVar(head);
            PushVar(sys);
            cw.Emit(Op.Tail);
            PopVar(tail);

			if (s.Pattern.Type == ElaNodeType.NameReference)
			{
				var addr = AddVariable(s.Pattern.GetName(), s.Pattern, ElaVariableFlags.None, -1);
				PushVar(head);
                PopVar(addr);
			}
			else
                CompilePattern(head, s.Pattern, iterLab, false /*allowBang*/, false /*forceStrict*/);

			if (s.Guard != null)
			{
				CompileExpression(s.Guard, map, Hints.None, s);
				cw.Emit(Op.Brfalse, iterLab);
			}

			if (s.Body.Type == ElaNodeType.Generator)
			{
				var f = (ElaGenerator)s.Body;
				var child = CompileRecursiveFor(f, map, hints, funAddr, tail);
				CompileExpression(f.Target, map, Hints.None, f);
                PushVar(child);
				cw.Emit(Op.Call);
                cw.Emit(Op.Br, exitLab);//
			}
			else
			{
				PushVar(tail);
				PushVar(1 | (funAddr >> 8) << 8);
				cw.Emit(Op.LazyCall);
				CompileExpression(s.Body, map, Hints.None, s);
				cw.Emit(Op.Cons);
				cw.Emit(Op.Br, exitLab);
			}

			cw.MarkLabel(iterLab);
			PushVar(tail);
			PushVar(1 | (funAddr >> 8) << 8);
			cw.Emit(Op.Call);
            cw.Emit(Op.Br, exitLab);//

			cw.MarkLabel(endLab);

			if (parent == -1)
				cw.Emit(Op.Newlist);
			else
			{
				PushVar(1 | (parentTail >> 8) << 8);
				PushVar(2 | (parent >> 8) << 8);
				cw.Emit(Op.Call);
			}

			cw.MarkLabel(exitLab);
			cw.Emit(Op.Ret);
			frame.Layouts.Add(new MemoryLayout(currentCounter, cw.FinishFrame(), address));
			EndSection();
			EndScope();

			cw.MarkLabel(funSkipLabel);
			cw.Emit(Op.PushI4, 1);
			cw.Emit(Op.Newfun, frame.Layouts.Count - 1);
            PopVar(funAddr);
			return funAddr;
		}
		#endregion
	}
}