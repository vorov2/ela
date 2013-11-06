using System;
using System.Collections.Generic;
using Ela.CodeModel;

namespace Ela.Compilation
{
	internal sealed class CodeWriter
	{
		#region Construction
		private FastList<Op> ops;
		private FastList<Int32> opData;
		private FastList<Int32> labels;
		private FastList<Int32> fixups;
		private FastStack<StackSize> locals;

		private sealed class StackSize
		{
			internal int Counter;
			internal int Max;
		}

		internal CodeWriter(FastList<Op> ops, FastList<Int32> opData)
		{
			this.ops = ops;
			this.opData = opData;
			labels = new FastList<Int32>();
			fixups = new FastList<Int32>();
			locals = new FastStack<StackSize>();
		}
		#endregion


		#region Methods
		internal void Duplicate(int start, int finish)
		{
			var sh = Offset - start;
			ops.AddRange(ops, start, finish);
			opData.AddRange(opData, start, finish);

			var nf = new FastList<Int32>();

			foreach (var i in fixups)
			{
				if (i >= start && i <= finish)
				{
					var l = labels[opData[i]];
					var nl = DefineLabel();

					opData[sh + i] = nl.GetIndex();
					labels[nl.GetIndex()] = l + sh;

					nf.Add(sh + i);
				}
			}

			fixups.AddRange(nf);
		}


		internal void CompileOpList()
		{
			foreach (var i in fixups)
				opData[i] = labels[opData[i]];
			
			fixups.Clear();
			labels.Clear();
		}


		internal void StartFrame(int init)
		{
			locals.Push(new StackSize { Counter = init, Max = init });
		}


		internal int FinishFrame()
		{
			var ret = locals.Pop();
			return ret.Max;
		}


		internal Op GetOpCode(int offset)
		{
			return ops[offset];
		}


		internal int GetOpCodeData(int offset)
		{
			return opData[offset];
		}


		internal Label DefineLabel()
		{
			var lab = new Label(labels.Count);
			labels.Add(Label.EmptyLabel);
			return lab;
		}


		internal void MarkLabel(Label label)
		{
			labels[label.GetIndex()] = ops.Count;
		}


		internal void Emit(Op op, Label label)
		{
			if (!label.IsEmpty())
			{
				fixups.Add(ops.Count);
				Emit(op, label.GetIndex());
			}
			else
				Emit(op, 0);
		}


		internal void Emit(Op op, int data)
		{
			ops.Add(op);
			var size = OpStackHelper.Op[(Int32)op];

			var ss = locals.Peek();
			ss.Counter += size;

			if (ss.Counter > ss.Max)
				ss.Max = ss.Counter;

			opData.Add(data);
		}


		internal void Emit(Op op)
		{
			Emit(op, 0);
		}
		#endregion


		#region Properties
		internal int Offset
		{
			get { return ops.Count; }
		}
		#endregion
	}
}