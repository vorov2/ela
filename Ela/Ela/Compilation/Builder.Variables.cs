using System;
using Ela.CodeModel;
using System.Collections.Generic;

namespace Ela.Compilation
{
    //This part is responsible for adding and seeking variables.
    internal sealed partial class Builder
    {
        //Variables indexers
        private FastStack<Int32> counters = new FastStack<Int32>(); //Scope based index stack
        private int currentCounter; //Global indexer
                
        //Flags used by GetVariable method
        [Flags]
        private enum GetFlags
        {
            None = 0x00,
            Local = 0x02, //Look for variable in current module only, ignore externals
            NoError = 0x04  //Don't generate an error if variable is not found
        }

        //Resume variable indexer from top level (if we are in interactive mode)
        private void ResumeIndexer()
        {
            currentCounter = frame.Layouts.Count > 0 ? frame.Layouts[0].Size : 0;
        }

        //This method should be used always instead of direct emitting Pushvar/Pushloc op codes.
        //It first checks if a given variable is an external and in such a case generates
        //a different op typeId. For addr it uses PushVar(int) to generate an appropriate op typeId.
        private void PushVar(ScopeVar sv)
        {
            if ((sv.Flags & ElaVariableFlags.External) == ElaVariableFlags.External)
                cw.Emit(Op.Pushext, sv.Address);
            else
                PushVar(sv.Address);
        }

        //This method emits either Pushvar or Pushloc.
        private void PushVar(int address)
        {
            if ((address & Byte.MaxValue) == 0)
                cw.Emit(Op.Pushloc, address >> 8);
            else
                cw.Emit(Op.Pushvar, address);
        }

        //This method emits either Popvar or Poploc. This method should always be used instead of
        //directly emitting Popvar/Poploc op codes.
        private void PopVar(int address)
        {
            if ((address & Byte.MaxValue) == 0)
                cw.Emit(Op.Poploc, address >> 8);
            else
                cw.Emit(Op.Popvar, address);
        }

        //A simplified version of EmitSpecName, when module ID of a name is not needed.
        private ScopeVar EmitSpecName(string ns, string specName, ElaExpression exp, ElaCompilerError err)
        {
            int _;
            return EmitSpecName(ns, specName, exp, err, out _);
        }

        //This method is used to generate Push* op typeId for a special name prefixed by $$ or $$$.
        //Such names are used for hidden variables that contains unique codes of types and classes.
        //This method can emit a qualified name (with a module prefix) as well as a short name.
        //It also generates an appropriate error if a name is not found.
        //Specifying ElaCompilerError.None would force this method to not generate any error messages.
        private ScopeVar EmitSpecName(string ns, string specName, ElaExpression exp, ElaCompilerError err, out int modId)
        {
            if (ns != null)
            {
                var v = GetVariable(ns, exp.Line, exp.Column);

                //A prefix (ns) is not a module alias which is an error
                if ((v.Flags & ElaVariableFlags.Module) != ElaVariableFlags.Module && err != ElaCompilerError.None)
                    AddError(ElaCompilerError.InvalidQualident, exp, ns);

                ModuleReference mr;
                var lh = -1;

                if (frame.References.TryGetValue(ns, out mr))
                    lh = mr.LogicalHandle;

                var extVar = default(ScopeVar);
                var mod = lh > -1 && lh < refs.Count ? refs[lh] : null;

                //A name (or even module) not found, generate an error
                if ((mod == null || !mod.GlobalScope.Locals.TryGetValue(specName, out extVar)) &&
                    !options.IgnoreUndefined && err != ElaCompilerError.None)
                {
                    //No need to apped alias if we want to generate UndefinedName. That would be misleading.
                    if (err == ElaCompilerError.UndefinedName)
                        AddError(err, exp, specName.TrimStart('$'));
                    else
                        AddError(err, exp, ns + "." + specName.TrimStart('$'));
                }

                if ((extVar.Flags & ElaVariableFlags.Private) == ElaVariableFlags.Private && err != ElaCompilerError.None)
                    AddError(ElaCompilerError.PrivateNameInModule, exp, specName.TrimStart('$'), ns);

                modId = lh;
                extVar = new ScopeVar(extVar.Flags | ElaVariableFlags.External, lh | (extVar.Address << 8), extVar.Data);
                PushVar(extVar);
                return extVar;
            }
            else
            {
                //Without a qualident it is pretty straightforward
                var a = GetVariable(specName, CurrentScope, GetFlags.NoError, exp.Line, exp.Column);

                if (a.IsEmpty() && !options.IgnoreUndefined && err != ElaCompilerError.None)
                    AddError(err, exp, specName.TrimStart('$'));

                modId = (a.Flags & ElaVariableFlags.External) == ElaVariableFlags.External ? a.Address & Byte.MaxValue : -1;
                PushVar(a);
                return a;
            }
        }

        //Basic add variable routine. This method is called directly when an unnamed service
        //variable is needed.
        private int AddVariable()
        {
            var ret = 0 | currentCounter << 8;
            currentCounter++;
            return ret;
        }

        //A main method to add named variables.
        private int AddVariable(string name, ElaExpression exp, ElaVariableFlags flags, int data)
        {
            //Hiding is allowed in parameter list
            if ((flags & ElaVariableFlags.Parameter) == ElaVariableFlags.Parameter)
                CurrentScope.Locals.Remove(name);
            else if (CurrentScope.Locals.ContainsKey(name))
            {
                //But for other cases hiding in the same scope is not allowed
                CurrentScope.Locals.Remove(name);
                AddError(ElaCompilerError.NoHindingSameScope, exp, name.TrimStart('$'));
            }

            CurrentScope.Locals.Add(name, new ScopeVar(flags, currentCounter, data));

            //Additional debug info is only generated when a debug flag is set.
            if (debug && exp != null)
            {
                cw.Emit(Op.Nop);
                AddVarPragma(name, currentCounter, cw.Offset, flags, data);
                AddLinePragma(exp);
            }

            return AddVariable();
        }

        //Main method to query a variable that starts to search a variable in
        //the current scope.
        private ScopeVar GetVariable(string name, int line, int col)
        {
            return GetVariable(name, CurrentScope, GetFlags.None, line, col);
        }


        //Main method to query a variable that starts to search a variable in
        //the current scope.
        private ScopeVar GetGlobalVariable(string name, GetFlags flags, int line, int col)
        {
            var cur = CurrentScope;
            var shift = 0;
            var var = ScopeVar.Empty;

            //Rolls the scopes to find global
            while (cur.Parent != null)
            {
                if (cur.Function)
                    shift++;

                cur = cur.Parent;
            }

            if (globalScope.Locals.TryGetValue(name, out var))
            {
                var.Address = shift | var.Address << 8;
                return var;
            }
            
            //If this flag is set we don't need to go further
            if ((flags & GetFlags.Local) == GetFlags.Local)
            {
                if (!options.IgnoreUndefined && (flags & GetFlags.NoError) != GetFlags.NoError)
                    AddError(ElaCompilerError.UndefinedName, line, col, name);

                return ScopeVar.Empty;
            }

            return GetExternVariable(name, flags, line, col);
        }

        //This method allows to specify a scope from which to start search.
        private ScopeVar GetVariable(string name, Scope startScope, GetFlags getFlags, int line, int col)
        {
            var cur = startScope;
            var shift = 0;
            var var = ScopeVar.Empty;

            //Walks the scopes recursively to look for a variable
            do
            {
                if (cur.Locals.TryGetValue(name, out var))
                {
                    var.Address = shift | var.Address << 8;

                    if ((var.Flags & ElaVariableFlags.NoInit) == ElaVariableFlags.NoInit &&
                        (var.Flags & ElaVariableFlags.Function) != ElaVariableFlags.Function &&
                        (var.Flags & ElaVariableFlags.ClassFun) != ElaVariableFlags.ClassFun &&
                        (var.Flags & ElaVariableFlags.TypeFun) != ElaVariableFlags.TypeFun)
                        cleans.Replace(false);

                    return var;
                }

                if (cur.Function)
                    shift++;

                var = ScopeVar.Empty;
                cur = cur.Parent;
            }
            while (cur != null);
            
            //If this flag is set we don't need to go further
            if ((getFlags & GetFlags.Local) == GetFlags.Local)
            {
                if (!options.IgnoreUndefined && (getFlags & GetFlags.NoError) != GetFlags.NoError)
                    AddError(ElaCompilerError.UndefinedName, line, col, name);

                return ScopeVar.Empty;
            }

            return GetExternVariable(name, getFlags, line, col);
        }

        //Directily searches a name in an export list.
        private ScopeVar GetExternVariable(string name, GetFlags getFlags, int line, int col)
        {
            var vk = default(ExportVarData);

            //Looks for variable in export list
            if (exports.FindVariable(name, out vk))
            {
                var flags = vk.Flags;
                var data = vk.Data;
                                
                //All externals are added to the list of 'latebounds'. These names are then verified by a linker.
                //This verification is used only when a module is deserialized from an object file.
                frame.LateBounds.Add(new LateBoundSymbol(name, vk.ModuleHandle | vk.Address << 8, data, (Int32)flags, line, col));
                return new ScopeVar(flags | ElaVariableFlags.External, vk.ModuleHandle | vk.Address << 8, data);
            }
            else
            {
                if (!options.IgnoreUndefined && (getFlags & GetFlags.NoError) != GetFlags.NoError)
                    AddError(ElaCompilerError.UndefinedName, line, col, name);

                return ScopeVar.Empty;
            }
        }

        //Looks for a scope where a given name is defined. Returns the first scope that matches.
        private Scope GetScope(string name)
        {
            var cur = CurrentScope;

            do
            {
                if (cur.Locals.ContainsKey(name))
                    return cur;

                cur = cur.Parent;
            }
            while (cur != null);

            return null;
        }

        //This method checks if a given name (qualified or not) is a type or not a type.
        private bool NameExists(string prefix, string name)
        {
            if (prefix == null)
            {
                var sv = GetVariable(name, globalScope, GetFlags.NoError, 0, 0);
                return !sv.IsEmpty();
            }
            else
            {
                CodeFrame _;
                var sv = FindByPrefix(prefix, name, out _);
                return !sv.IsEmpty();
            }
        }
    }
}
