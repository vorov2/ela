using System;
using Ela.CodeModel;

namespace Ela.Compilation
{
    //This part is responsible for compilation of arithmetic ranges.
	internal sealed partial class Builder
	{
        //An entry method for range generation. Implementation of ranges are not provided by a compiler,
        //instead a particular data type implement ranges by providing an instance of Enum class. Functions
        //succ, enumFrom and enumFromTo are explicitly invoked by this method.
		private void CompileRange(ElaExpression parent, ElaRange range, LabelMap map, Hints hints)
		{
			AddLinePragma(range);

            var snd = AddVariable();
            var fst = AddVariable();

            //Compile the first element which should always be present.
            CompileExpression(range.First, map, Hints.None, range);
            PopVar(fst);

            //If the second element is missing we will calculate it using 'succ'.
            if (range.Second == null)
            {
                var sv = GetFunction("succ", range);
                PushVar(fst);
                PushVar(sv);
                cw.Emit(Op.Call);
                PopVar(snd);
            }
            else
            {
                CompileExpression(range.Second, map, Hints.None, range);
                PopVar(snd);
            }

            //If a last element is missing we need to generate an infinite range
            //using 'enumFrom' function.
            if (range.Last == null)
            {
                var sv = GetFunction("enumFrom", range);
                PushVar(snd);                
                PushVar(fst);
                PushVar(sv);
                cw.Emit(Op.Call);
                cw.Emit(Op.Call);
            }
            else
            {
                //An ordinary strict range.
                var sv = GetFunction("enumFromTo", range);
                PushVar(snd);                
                PushVar(fst);
                CompileExpression(range.Last, map, Hints.None, range);
                PushVar(sv);
                cw.Emit(Op.Call);
                cw.Emit(Op.Call);
                cw.Emit(Op.Call);
            }
		}

        //Finds a function from a Enum class - either enumFrom, enumFromTo or succ.
        private ScopeVar GetFunction(string name, ElaRange range)
        {
            var sv = GetGlobalVariable(name, GetFlags.NoError, range.Line, range.Column);
            var args = name == "enumFrom" ? 2 : name == "enumFromTo" ? 3 : 1;

            //Do some validation to ensure that a found function at least "looks like"
            //a correct one - e.g. is a class member and have a correct number of arguments.
            if (!options.IgnoreUndefined && (sv.IsEmpty() || (sv.Flags & ElaVariableFlags.ClassFun) != ElaVariableFlags.ClassFun || args != sv.Data))
                AddError(
                    name == "enumFrom" ? ElaCompilerError.FromEnumNotFound :
                    name == "enumFromTo" ? ElaCompilerError.FromEnumToNotFound :
                    ElaCompilerError.SuccNotFound, 
                    range);

            return sv;
        }
        
        //This is an old typeId that was used to optimize small ranges (to generate them in-place).
        //It is currently not used, but in future it may be rejuvenated.
		private bool TryOptimizeRange(ElaRange range, LabelMap map, Hints hints)
		{
			if (range.First.Type != ElaNodeType.Primitive ||
				(range.Second != null && range.Second.Type != ElaNodeType.Primitive) ||
				range.Last.Type != ElaNodeType.Primitive)
				return false;

			var fst = (ElaPrimitive)range.First;
			var snd = range.Second != null ? (ElaPrimitive)range.Second : null;
			var lst = (ElaPrimitive)range.Last;

			if (fst.Value.LiteralType != ElaTypeCode.Integer ||
				(snd != null && snd.Value.LiteralType != ElaTypeCode.Integer) ||
				lst.Value.LiteralType != ElaTypeCode.Integer)
				return false;

			var fstVal = fst.Value.AsInteger();
			var sndVal = snd != null ? snd.Value.AsInteger() : fstVal + 1;
			var lstVal = lst.Value.AsInteger();
			var step = sndVal - fstVal;

			if (Math.Abs((fstVal - lstVal) / step) > 20)
				return false;

			cw.Emit(Op.Newlist);

			if (snd != null)
			{
				cw.Emit(Op.PushI4, fstVal);
				fstVal = sndVal;
				cw.Emit(Op.Cons);
			}

			for (;;)
			{
				cw.Emit(Op.PushI4, fstVal);
				cw.Emit(Op.Cons);
				fstVal += step;

				if (step > 0)
				{
					if (fstVal > lstVal)
						break;
				}
				else if (fstVal < lstVal)
					break;
			}

			cw.Emit(Op.Genfin);
			return true;
		}
	}
}