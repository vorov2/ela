using System;
using Ela.CodeModel;
using Ela.Linking;

namespace Ela.Compilation
{
    //This part contains compilation logic for module includes.
    internal sealed partial class Builder
    {
        //All compiled referenced modules
        private FastList<CodeFrame> refs = new FastList<CodeFrame>();

        //An entry method for includes compilation
        private void CompileModuleIncludes(ElaProgram prog, LabelMap map)
        {
            var inc = prog.Includes;

            for (var i = 0; i < inc.Count; i++)
                CompileModuleInclude(inc[i], map);
        }

        //Compiles an 'open' module directive.
        private void CompileModuleInclude(ElaModuleInclude s, LabelMap map)
        {
            var modFrame = IncludeModule(
                s.Alias,             //alias
                s.Name,              //name
                s.DllName,           //dllname
                s.Path.ToArray(),    //path
                s.Line,              //line
                s.Column,            //col
                s.RequireQualified,  //qualified
                s,                   //ElaExpression
                true,                //add variable
                false);              //NoPrelude

            //Process explicit import list for a module if module is valid and has an import list
            if (s.HasImportList && modFrame != null)
            {
                var il = s.ImportList;

                for (var i = 0; i < il.Count; i++)
                {
                    var im = il[i];
                    var a = AddVariable(im.LocalName, im, im.Private ? ElaVariableFlags.Private : ElaVariableFlags.None, -1);
                    var sv = default(ScopeVar);

                    //Query a name from an import list directly in a module
                    if (!modFrame.GlobalScope.Locals.TryGetValue(im.Name, out sv))
                        AddError(ElaCompilerError.UndefinedNameInModule, im, s.Alias, im.Name);

                    AddLinePragma(im);
                    cw.Emit(Op.Pushext, (frame.HandleMap.Count - 1) | sv.Address << 8);
                    PopVar(a);
                }
            }
        }

        //Includes a module reference (used for user references, Prelude and Components module).
        private CodeFrame IncludeModule(string alias, string name, string dllName, string[] path,
            int line, int col, bool reqQual, ElaExpression exp, bool addVar, bool noPrelude)
        {
            var modIndex = frame.HandleMap.Count;
            var modRef = new ModuleReference(frame, name, dllName, path, line, col, reqQual, modIndex);
            modRef.NoPrelude = noPrelude;
            frame.AddReference(alias, modRef);

            //Handles are filled by linker for module indexing at run-time
            frame.HandleMap.Add(-1);

            if (exp != null)
                AddLinePragma(exp);

            cw.Emit(Op.Runmod, modIndex);
            var addr = -1;

            if (addVar)
            {
                //Create a variable and bind to it module instance
                cw.Emit(Op.Newmod, modIndex);
                addr = AddVariable(alias, exp, ElaVariableFlags.Module | ElaVariableFlags.Private, modIndex);
            }

            //An event is handled by linker and a module is compiled/deserialized
            var ev = new ModuleEventArgs(modRef);
            comp.OnModuleInclude(ev);
            refs.Add(ev.Frame);

            if (addr != -1)
                PopVar(addr);

            return ev.Frame;
        }

        //Includes Prelude module
        private void IncludePrelude(ElaProgram prog, bool addVariable)
        {
            IncludeModule(
                options.Prelude, //alias
                options.Prelude, //name
                null,            //dllname
                null,            //path
                0,               //line
                0,               //col
                false,           //qualified
                prog.TopLevel,   //ElaExpression
                addVariable,     //add variable
                true);           //NoPrelude
        }

        //Includes module with command line arguments
        private void IncludeArguments()
        {
            IncludeModule(
               ElaLinker.ARG_MODULE, //alias
               ElaLinker.ARG_MODULE, //name
               null,                 //dllname
               null,                 //path
               0,                    //line
               0,                    //col
               false,                //qualified
               null,                 //ElaExpression
               false,                //add variable
               true);                //NoPrelude
        }

        //Checks if a field reference is actually an exported name prefixed by a module
        //and if so emits a direct Pushexit op typeId. If this is not the case returns false
        //and the typeId gets compiled as a regular field reference.
        private bool TryOptimizeFieldReference(ElaFieldReference p)
        {
            if (p.TargetObject.Type != ElaNodeType.NameReference)
                return false;
            
            var mod = default(ModuleReference);
            var sv = GetVariable(p.TargetObject.GetName(), p.TargetObject.Line, p.TargetObject.Column);

            //Looks like a target object is not a module
            if ((sv.Flags & ElaVariableFlags.Module) != ElaVariableFlags.Module || 
                !frame.References.TryGetValue(p.TargetObject.GetName(), out mod))
                return false;

            var fieldSv = default(ScopeVar);

            //We have such a reference but looks like it couldn't be obtained
            //We don't need to handle this situation here, it is already reported by a linker
            if (refs[mod.LogicalHandle] == null)
                return false;

            //No such name, now captured statically
            if (!refs[mod.LogicalHandle].GlobalScope.Locals.TryGetValue(p.FieldName, out fieldSv) &&
                !options.IgnoreUndefined)
            {
                AddError(ElaCompilerError.UndefinedNameInModule, p, p.TargetObject.GetName(), p.FieldName);
                return false;
            }

            //Name is private, now captured statically
            if ((fieldSv.VariableFlags & ElaVariableFlags.Private) == ElaVariableFlags.Private)
            {
                AddError(ElaCompilerError.PrivateNameInModule, p, p.FieldName, p.TargetObject.GetName());
                return false;
            }

            AddLinePragma(p);
            cw.Emit(Op.Pushext, mod.LogicalHandle | fieldSv.Address << 8);
            return true;
        }

        //A generic method used to loop a prefixed var in a module.
        private ScopeVar FindByPrefix(string prefix, string var, out CodeFrame fr)
        {
            var sv = GetVariable(prefix, CurrentScope, GetFlags.NoError, 0, 0);
            var mod = default(ModuleReference);
            fr = null;
            
            //Looks like a target object is not a module
            if ((sv.Flags & ElaVariableFlags.Module) != ElaVariableFlags.Module || 
                !frame.References.TryGetValue(prefix, out mod))
                return ScopeVar.Empty;

            var fieldSv = default(ScopeVar);

            //We have such a reference but looks like it couldn't be obtained
            //We don't need to handle this situation here, it is already reported by a linker
            fr = refs[mod.LogicalHandle];

            if (fr == null)
                return ScopeVar.Empty;

            //No such name, now captured statically
            if (!refs[mod.LogicalHandle].GlobalScope.Locals.TryGetValue(var, out fieldSv) &&
                !options.IgnoreUndefined)
                return ScopeVar.Empty;

            //Name is private, now captured statically
            if ((fieldSv.VariableFlags & ElaVariableFlags.Private) == ElaVariableFlags.Private)
                return ScopeVar.Empty;

            return new ScopeVar(fieldSv.Flags|ElaVariableFlags.External, mod.LogicalHandle | fieldSv.Address << 8, fieldSv.Data);
        }
    }
}
