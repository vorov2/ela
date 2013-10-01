using System;
using Ela.CodeModel;
using Ela.Debug;

namespace Ela.Compilation
{
    //This part contains methods that generate debug info.
    internal sealed partial class Builder
    {
        //Last location provided to AddLineSym method.
        //It is used by EndScope method to add location
        //of where scope ends.
        private int lastLine;
        private int lastColumn;

        //Symbols writer
        private DebugWriter pdb = new DebugWriter();

        //This one is refered by an ElaCompiler
        internal DebugInfo Symbols
        {
            get { return pdb != null ? pdb.Symbols : null; }
        }
		        
        //Called to create a first part of FunSym symbol.
        private void StartFun(string name, int pars)
        {
            cw.StartFrame(pars);
            pdb.StartFunction(name, cw.Offset, pars);
        }
        
        //Called when a function compilation is finished in
        //order to complete information for FunSym symbol.
        private int EndFun(int handle)
        {
            pdb.EndFunction(handle, cw.Offset);
            return cw.FinishFrame();
        }

        //Generates information about a lexical scope. Called
        //when a lexical scope starts.
        private void StartScope(bool fun, int line, int col)
        {
            CurrentScope = new Scope(fun, CurrentScope);

            if (debug)
                pdb.StartScope(cw.Offset, line, col);
        }
        
        //Called when a lexical scope ends.
        private void EndScope()
        {
            CurrentScope = CurrentScope.Parent != null ? CurrentScope.Parent : null;

            if (debug)
                pdb.EndScope(cw.Offset, lastLine, lastColumn);
        }
        
        //This one is called in conjunction with StartScope, when a lexical
        //scope if actually the one that exists in run-time as well (such as
        //function scope.
        private void StartSection()
        {
            counters.Push(currentCounter);
            currentCounter = 0;
        }
        
        //Called when a lexical scope ends.
        private void EndSection()
        {
            currentCounter = counters.Pop();
        }
        
        //Generates a line pragma and remembers a provided location
        //via lastLine and lastColumn class fields.
        private void AddLinePragma(ElaExpression exp)
        {
            lastLine = exp.Line;
            lastColumn = exp.Column;
            pdb.AddLineSym(cw.Offset, exp.Line, exp.Column);
        }
        
        //Only in effect when extended debug info mode is on.
        private void AddVarPragma(string name, int address, int offset, ElaVariableFlags flags, int data)
        {
            if (debug)
                pdb.AddVarSym(name, address, offset, (Int32)flags, data);
        }
    }
}
