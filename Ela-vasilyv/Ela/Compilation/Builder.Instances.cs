using System;
using System.Collections.Generic;
using Ela.CodeModel;

namespace Ela.Compilation
{
    //This Part is responsible for compilation of type class instances
    internal sealed partial class Builder
    {
        //An entry method for instance compilation
        private void CompileInstances(ElaClassInstance s, LabelMap map)
        {
            var ins = s;

            //First we need to compile default instances so that
            //other instances in this class will be able to use them
            while (ins != null)
            {
                if (ins.TypeName == null)
                    CompileInstanceBody(ins, map);

                ins = ins.And;
            }

            ins = s;

            //Now we compile specific instances
            while (ins != null)
            {
                if (ins.TypeName != null)
                    CompileInstanceBody(ins, map);

                ins = ins.And;
            }
        }


        //Main method for instance compilation
        private void CompileInstanceBody(ElaClassInstance s, LabelMap map)
        {
            //Obtain type class data
            var mod = default(CodeFrame);
            var mbr = default(ClassData);
            var modId = -1;
            var classId = ObtainTypeClass(s, out mbr, out modId, out mod);

            //It's OK if mbr (class members) is null. It is not null only if a class declaration is local.
            //However mod shouldn't be null in such a case (non-local class), and we will use it to obtain
            //class members. If mod==null and mbr==null, or mod doesn't have our class, than we really can't find it.
            var notFound = mbr == null && (mod == null || !mod.InternalClasses.TryGetValue(s.TypeClassName, out mbr));

            //Type class not found, nothing else to do
            if (notFound)
            {
                if (!options.IgnoreUndefined)
                    AddError(ElaCompilerError.UnknownClass, s, s.TypeClassName);
                else if (s.TypeName != null) //It is valid if this is a 'default' instance
                {
                    //Add an instance anyway - the IgnoreUndefine flag is usually set just to gather some module metadata,
                    //so we need to populate it anyway.
                    frame.InternalInstances.Add(new InstanceData(s.TypeName, s.TypeClassName, -1, modId, s.Line, s.Column));
                }
            }
            else
            {
                //Now we need to obtain a local ID of a module where type is defined
                var typeModuleCode = -1;
                var typeId = -1;

                //Type name is null if this is a default instance
                if (s.TypeName != null)
                {
                    typeId = ObtainType(s, out typeModuleCode);

                    //Add new instance registration information
                    frame.InternalInstances.Add(new InstanceData(s.TypeName, s.TypeClassName, typeModuleCode, modId, s.Line, s.Column));
                }

                //Fill a list of classMembers, this list is used in this method to validate
                //whether all members of a class have an implementation
                var classMembers = new List<String>(mbr.Members.Length);

                for (var i = 0; i < mbr.Members.Length; i++)
                    classMembers.Add(mbr.Members[i].Name);

                if (s.Where != null)
                    foreach (var b in s.Where.Equations)
                    {
                        var err = false;

                        //Patterns are now allowed in member bindings
                        if (!b.IsFunction() && b.Left.Type != ElaNodeType.NameReference)
                        {
                            AddError(ElaCompilerError.MemberNoPatterns, b.Left, FormatNode(b.Left));
                            err = true;
                        }

                        var name = b.GetLeftName();

                        //Only member functions can be declared inside instance
                        if (!classMembers.Contains(name))
                        {
                            AddError(ElaCompilerError.MemberInvalid, b, name, s.TypeClassName);
                            err = true;
                        }

                        //Compile member function                 
                        if (!err)
                        {
                            classMembers.Remove(name);

                            //It's OK if s.TypeName is null, this situation (default instance) is handled in this method
                            CompileInstanceMember(s.TypeClassName, s.TypeClassPrefix, b, map, s.TypePrefix, s.TypeName);                            
                        }
                    }

                //If this is not a default instance, we can try to autogenerate it.
                if (s.TypeName != null)
                    classMembers = TryGenerateMembers(classMembers, mod, s);

                //Not all of the members are implemented, which is an error if an instance is
                //not generated by a compiler or this is a 'default' instance.
                if (s.TypeName != null && !IsBuiltIn(classId, typeId) && classMembers.Count > 0)
                {
                    if (s.Where != null)
                        AddError(ElaCompilerError.MemberNotAll, s, s.TypeClassName, s.TypeClassName + " " + s.TypeName);
                    else
                    {
                        //If a 'where' clause is not specified at all we need to generate a better
                        //error message (most likely this instance appears in 'deriving' clause).
                        AddError(ElaCompilerError.UnableDerive, s, s.TypeClassName, s.TypeName);
                    }
                }
            }
        }

        //This method returns type class data including: type class metadata (ClassData), module local ID
        //(modId) and compiled module where class is defined.
        private int ObtainTypeClass(ElaClassInstance s, out ClassData mbr, out int modId, out CodeFrame mod)
        {
            mbr = null;
            modId = -1;
            mod = null;

            //If a type class prefix is not set we need to obtain a type class ID using a special '$$$*' variable
            //that is initialized during type class compilation
            if (s.TypeClassPrefix == null)
            {
                //We first check if a class definition is non-local
                if (!frame.InternalClasses.TryGetValue(s.TypeClassName, out mbr))
                {
                    var sv = GetVariable("$$$" + s.TypeClassName, CurrentScope, GetFlags.NoError, s.Line, s.Column);

                    if (sv.IsEmpty() && !options.IgnoreUndefined)
                    {
                        AddError(ElaCompilerError.UnknownClass, s, s.TypeClassName);
                        return -1;
                    }

                    //The trick is - here sv can be only an external name (we do check prior to this
                    //if a class is not local). If it is an external name that first byte contains a
                    //local index of a referenced module - that is exactly what we need here to obtain
                    //a compiled module frame (from refs array).
                    modId = sv.Address & Byte.MaxValue;

                    if (modId < refs.Count && modId >= 0)
                        mod = refs[modId];

                    return sv.Data;
                }
                else
                {
                    var sv = GetVariable("$$$" + s.TypeClassName, CurrentScope, GetFlags.NoError, s.Line, s.Column);
                    return sv.Data;
                }
            }
            else
            {
                //Type class prefix is set. The prefix itself should be local name (a module alias)
                var sv = GetVariable(s.TypeClassPrefix, s.Line, s.Column);

                if (sv.IsEmpty())
                    return -1;

                //A name was found but this is not a module alias
                if ((sv.Flags & ElaVariableFlags.Module) != ElaVariableFlags.Module)
                {
                    AddError(ElaCompilerError.InvalidQualident, s, s.TypeClassPrefix);
                    return -1;
                }

                //In this case we can look for a reference based on its alias (alias should be unique within
                //the current module).
                modId = frame.References[s.TypeClassPrefix].LogicalHandle;
                
                if (modId < refs.Count && modId >= 0)
                    mod = refs[modId];

                //Now we need to obtain 'data' of a type class variable - it might a type class
                //typeId if this type class is built-in.
                ScopeVar sv2;

                if (mod != null && mod.GlobalScope.Locals.TryGetValue("$$$" + s.TypeClassName, out sv2))
                    return sv.Data;
                else
                    return -1;
            }
        }

        //Here we obtain type information - local ID of a module where a type is defined.
        private int ObtainType(ElaClassInstance s, out int typeModuleCode)
        {
            typeModuleCode = -1;

            if (s.TypePrefix == null)
            {
                //First we check that type is not defined locally
                if (!frame.InternalTypes.ContainsKey(s.TypeName))
                {
                    var sv = GetVariable("$$" + s.TypeName, CurrentScope, GetFlags.NoError, s.Line, s.Column);

                    if (sv.IsEmpty() && !options.IgnoreUndefined)
                    {
                        AddError(ElaCompilerError.UndefinedType, s, s.TypeName);
                        return -1;
                    }

                    //The trick is - here sv can be only an external name (we do check prior to this
                    //if a type is not local). If it is an external name that first byte contains a
                    //local index of a referenced module - and typeModuleCode is effectly a local ID of a module
                    //where a type is declared
                    typeModuleCode = sv.Address & Byte.MaxValue;
                    return sv.Data;
                }
                else
                {
                    var sv = GetVariable("$$" + s.TypeName, CurrentScope, GetFlags.NoError, s.Line, s.Column);
                    return sv.Data;
                }
            }
            else
            {
                //TypePrefix is a local name that should correspond to a module alias
                var sv = GetVariable(s.TypePrefix, s.Line, s.Column);

                if (sv.IsEmpty())
                    return -1;

                //A name exists but this is not a module alias
                if ((sv.Flags & ElaVariableFlags.Module) != ElaVariableFlags.Module)
                {
                    AddError(ElaCompilerError.InvalidQualident, s, s.TypePrefix);
                    return -1;
                }

                //Obtain a local ID of a module based on TypePrefix (which is module alias
                //that should be unique within this module).
                var frm = frame.References[s.TypePrefix];
                typeModuleCode = frm.LogicalHandle;

                //Now we need to obtain type variable data - it may have the typeId of a type
                //if this is a built-in type.
                ScopeVar sv2;

                if (frm != null && typeModuleCode < refs.Count && typeModuleCode >= 0 &&
                    refs[typeModuleCode].GlobalScope.Locals.TryGetValue("$$" + s.TypeName, out sv2))
                    return sv2.Data;

                return -1;
            }
        }
                
        //Compile an instance member. Argument classPrefix (module alias) is null if a class is 
        //not prefixed, otherwise it should be used to lookup class members.
        private void CompileInstanceMember(string className, string classPrefix, ElaEquation s, LabelMap map, 
            string currentTypePrefix, string currentType)
        {
            //Obtain a 'table' function of a class
            var name = s.GetLeftName();
            var btVar = ObtainClassFunction(classPrefix, className, name, s.Line, s.Column);

            if (btVar.IsEmpty())
                AddError(ElaCompilerError.MemberInvalid, s, name, className);
            
            var builtin = (btVar.Flags & ElaVariableFlags.Builtin) == ElaVariableFlags.Builtin;
            var args = 0;

            //Here we need to understand how many arguments a class function has.
            //If our declaration has less arguments (allowed by Ela) we need to do
            //eta expansion.
            if (builtin)
                args = BuiltinParams((ElaBuiltinKind)btVar.Data);
            else
                args = btVar.Data;

            //First we check if this binding is simply a function reference
            if (!TryResolveInstanceBinding(args, s.Right, map))
            {
                //Eta expansion should be done if a member is not a function literal
                //(it can be a partially applied function) or if a number of arguments
                //doesn't match.
                if (!s.IsFunction())
                    EtaExpand(null, s.Right, map, args);
                else if (s.GetArgumentNumber() < args)
                    EtaExpand(null, s, map, args);
                else
                    CompileFunction(s);
            }

            AddLinePragma(s);

            //It is possible if we are compiling a default instance (that can be
            //used to automatically generate instances).
            if (currentType != null)
            {
                //Depending whether this is a built-in class or a different approach is
                //used to add a member function.
                if (!builtin)
                    PushVar(btVar);
                else
                    cw.Emit(Op.PushI4, (Int32)btVar.Data);

                //We need to obtain a 'real' global ID of the type that we are extending. This
                //is done using this helper method.
                EmitSpecName(currentTypePrefix, "$$" + currentType, s, ElaCompilerError.UndefinedType);

                //Finally adding a member function.
                cw.Emit(Op.Addmbr);
            }
            else
            {
                var nm = "$default$" + name;

                //We are double binding the same default member which is not allowed within 
                //the same module.
                if (globalScope.Locals.ContainsKey(nm))
                {
                    AddError(ElaCompilerError.DefaultMemberAlreadyExist, s, name, className);
                    globalScope.Locals.Remove(name);
                }

                var flags = ElaVariableFlags.None;
                var data = -1;

                if (s.Right.Type == ElaNodeType.Builtin)
                {
                    flags = ElaVariableFlags.Builtin; 
                    data = (Int32)((ElaBuiltin)s.Right).Kind;
                }

                var dv = AddVariable(nm, s, flags, data);
                PopVar(dv);
            }
        }

        //This method checks if an instance member binding is a function reference with a 
        //correct number of arguments.
        private bool TryResolveInstanceBinding(int args, ElaExpression exp, LabelMap map)
        {
            if (exp == null)
                return false;

            //This is not a function at all, but a polymorphic constant. We, however, cannot
            //execute it right away - as a result we need to defer its execution by creating 
            //a thunk. Code should not be executing when instances are run.
            if (args == 0)
            {
                CompileLazyExpression(exp, map, Hints.None);
                return true;
            }

            //A simple case - a direct name reference, need to check its type arguments
            if (exp.Type == ElaNodeType.NameReference)
            {
                var sv = GetVariable(exp.GetName(), CurrentScope, GetFlags.NoError, 0, 0);

                if ((sv.VariableFlags & ElaVariableFlags.Function) == ElaVariableFlags.Function && sv.Data == args)
                {
                    AddLinePragma(exp);
                    PushVar(sv);
                    return true;
                }
            }
            else if (exp.Type == ElaNodeType.FieldReference)
            {
                //A more complex case - this can be a qualified name (with a module alias)
                var fr = (ElaFieldReference)exp;

                if (fr.TargetObject.Type != ElaNodeType.NameReference)
                    return false;

                CodeFrame _;
                var sv = FindByPrefix(fr.TargetObject.GetName(), fr.FieldName, out _);

                if ((sv.VariableFlags & ElaVariableFlags.Function) == ElaVariableFlags.Function && sv.Data == args)
                {
                    AddLinePragma(exp);
                    PushVar(sv);
                    return true;
                }
            }
            
            return false;
        }

        //An instance might be missing a body. If this instance corresponds to
        //an auto-generated instance we are OK with this, otherwise we will generate
        //an error (on the caller side).
        private bool IsBuiltIn(int classId, int typeId)
        {
            var tc = (ElaTypeCode)typeId;
            var cc = (TypeClass)classId;

            if (tc == ElaTypeCode.None || typeId == -1 || cc == TypeClass.None || classId == -1 || classId == 0)
                return false;

            switch (cc)
            {
                case TypeClass.Eq:
                    return
                        tc == ElaTypeCode.Integer ||
                        tc == ElaTypeCode.Char ||
                        tc == ElaTypeCode.Boolean ||
                        tc == ElaTypeCode.Double ||
                        tc == ElaTypeCode.Single ||
                        tc == ElaTypeCode.Long ||
                        tc == ElaTypeCode.String ||
                        tc == ElaTypeCode.Unit ||
                        tc == ElaTypeCode.Function ||
                        tc == ElaTypeCode.Module ||
                        tc == ElaTypeCode.Tuple ||
                        tc == ElaTypeCode.Record ||
                        tc == ElaTypeCode.List;
                case TypeClass.Ord:
                    return
                        tc == ElaTypeCode.Integer ||
                        tc == ElaTypeCode.Long ||
                        tc == ElaTypeCode.Single ||
                        tc == ElaTypeCode.Double ||
                        tc == ElaTypeCode.Char ||
                        tc == ElaTypeCode.String;
                case TypeClass.Additive:
                case TypeClass.Modulo:
                case TypeClass.Ring:
                case TypeClass.Field:
                    return
                        tc == ElaTypeCode.Integer ||
                        tc == ElaTypeCode.Long ||
                        tc == ElaTypeCode.Single ||
                        tc == ElaTypeCode.Double;
                case TypeClass.Bit:
                    return
                        tc == ElaTypeCode.Integer ||
                        tc == ElaTypeCode.Long;
                case TypeClass.Seq:
                    return
                        tc == ElaTypeCode.List ||
                        tc == ElaTypeCode.String;
                case TypeClass.Len:
                case TypeClass.Ix:
                    return
                        tc == ElaTypeCode.String ||
                        tc == ElaTypeCode.Tuple ||
                        tc == ElaTypeCode.Record;
                case TypeClass.Name:
                    return
                        tc == ElaTypeCode.Record ||
                        tc == ElaTypeCode.Module;
                case TypeClass.Cat:
                    return
                        tc == ElaTypeCode.Char ||
                        tc == ElaTypeCode.String ||
                        tc == ElaTypeCode.Tuple ||
                        tc == ElaTypeCode.Record;
                case TypeClass.Show:
                    return
                        tc == ElaTypeCode.Integer ||
                        tc == ElaTypeCode.Long ||
                        tc == ElaTypeCode.Single ||
                        tc == ElaTypeCode.Double ||
                        tc == ElaTypeCode.Char ||
                        tc == ElaTypeCode.Boolean ||
                        tc == ElaTypeCode.String ||
                        tc == ElaTypeCode.Unit ||
                        tc == ElaTypeCode.Function ||
                        tc == ElaTypeCode.Module;
                default:
                    return false;
            }
        }
                
        //Looks for a 'table' function (a class function to which we would add instances).
        //It can be optionally qualified with a 'classPrefix' (which is a module alias).
        private ScopeVar ObtainClassFunction(string classPrefix, string className, string name, int line, int col)
        {
            var btVar = default(ScopeVar);

            if (classPrefix == null && !frame.InternalClasses.ContainsKey(className))
            {
                //We can't look for this name directly, because it can be shadowed. 
                //First, we need to obtain a class reference, which is used to obtain a local
                //module ID (encoded in address). Then we directly look for this name in the module.
                var cv = GetGlobalVariable("$$$" + className, GetFlags.NoError, line, col);
                var moduleHandle = cv.Address & Byte.MaxValue;

                if (moduleHandle < 0 || moduleHandle >= refs.Count ||
                    !refs[moduleHandle].GlobalScope.Locals.TryGetValue(name, out btVar))
                    btVar = ScopeVar.Empty;
                else
                {
                    //OK, found, but now we need to patch this variable, so it will be correctly
                    //encoded as an external name.
                    btVar = new ScopeVar(btVar.Flags | ElaVariableFlags.External,
                        moduleHandle | btVar.Address << 8, btVar.Data);
                }
            }
            else if (classPrefix == null)
                btVar = GetVariable(name, CurrentScope, GetFlags.NoError, line, col);
            else
            {
                CodeFrame _;
                btVar = FindByPrefix(classPrefix, name, out _);
            }

            return btVar;
        }

        //This function tries to autogenerate an instance (similar to derived in Haskell).
        //It can fully or partially generate it. A default instance (instance without a type) is
        //used for such purpose.
        private List<String> TryGenerateMembers(List<String> members, CodeFrame mod, ElaClassInstance inst)
        {
            if (members.Count == 0)
                return members;

            var newMem = new List<String>(members);

            for (var i = 0; i < newMem.Count; i++)
            {
                var m = newMem[i];
                var btVar = ObtainClassFunction(inst.TypeClassPrefix, inst.TypeClassName, m, inst.Line, inst.Column);

                //This is less likely but we better check this anyway
                if (btVar.IsEmpty())
                    AddError(ElaCompilerError.MemberInvalid, inst, m, inst.TypeClassName);

                var defVar = GetGlobalVariable("$default$" + m, GetFlags.NoError, inst.Line, inst.Column);

                //We dont' need to generate errors here, errors will be captured later.
                if (!defVar.IsEmpty())
                {
                    var builtin = (btVar.Flags & ElaVariableFlags.Builtin) == ElaVariableFlags.Builtin;

                    //Check if this member is implemented directly by compiler
                    if (!TryAutogenerate(defVar, inst))
                        PushVar(defVar);

                    if (!builtin)
                        PushVar(btVar);
                    else
                        cw.Emit(Op.PushI4, (Int32)btVar.Data);
                
                    EmitSpecName(inst.TypePrefix, "$$" + inst.TypeName, inst, ElaCompilerError.UndefinedType);
                    cw.Emit(Op.Addmbr);
                    members.Remove(m);
                }
            }

            return members;
        }

        //Some members may be generated directly by compiler. Here we check if this is the case for this
        //particular member.
        private bool TryAutogenerate(ScopeVar var, ElaClassInstance inst)
        {
            if ((var.Flags & ElaVariableFlags.Builtin) != ElaVariableFlags.Builtin)
                return false;

            switch ((ElaBuiltinKind)var.Data)
            {
                //Used to generate Bounded.maxBound constant
                case ElaBuiltinKind.GenMaxBound:
                    cw.Emit(Op.PushI4, 1);
                    //Obtain type ID, no errors, they are captured elsewhere
                    EmitSpecName(inst.TypePrefix, "$$" + inst.TypeName, inst, ElaCompilerError.None);
                    cw.Emit(Op.Api, (Int32)Api.TypeConsNumber);
                    cw.Emit(Op.Sub);

                    EmitSpecName(inst.TypePrefix, "$$" + inst.TypeName, inst, ElaCompilerError.None);
                    cw.Emit(Op.Api2, (Int32)Api.ConsCodeByIndex);
                    cw.Emit(Op.Api, (Int32)Api.ConsDefault);
                    return true;
                //Used to generate Bounded.minBound constant
                case ElaBuiltinKind.GenMinBound:                    
                case ElaBuiltinKind.GenDefault:
                    cw.Emit(Op.PushI4_0);
                    //Obtain type ID, no errors, they are captured elsewhere
                    EmitSpecName(inst.TypePrefix, "$$" + inst.TypeName, inst, ElaCompilerError.None);
                    cw.Emit(Op.Api2, (Int32)Api.ConsCodeByIndex);
                    cw.Emit(Op.Api, (Int32)Api.ConsDefault);
                    return true;
                default:
                    return false;
            }
        }
    }
}
