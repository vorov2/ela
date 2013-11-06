using System;
using System.Collections.Generic;
using Ela.CodeModel;
using Ela.Runtime;

namespace Ela.Compilation
{
    //This part contains compilation logic for built-in and user defined types.
    internal sealed partial class Builder
    {
        //An entry method for type compilation. Ensures that types ('type') are
        //always compiled before type extensions ('data').
        private void CompileTypes(ElaNewtype v, LabelMap map)
        {
            CompileHeaders(v);
            CompileTypeOnly(v, map);
            CompileDataOnly(v, map);
        }

        //Builds a list of attribute headers for types
        private void CompileHeaders(ElaNewtype v)
        {
            var t = v;
            var oldt = default(ElaNewtype);

            while (t != null)
            {
                if (t.Header)
                {
                    if (oldt == null || t.Extends != oldt.Extends || t.Opened != oldt.Opened)
                        AddError(ElaCompilerError.TypeHeaderNotConnected, t, t.Name);
                    else
                        oldt.Flags = t.Flags;
                }
                else
                    oldt = t;

                t = t.And;
            }
        }
        
        //This method only compiles types declared through 'type' keyword
        //and not type extensions ('data' declarations).
        private void CompileTypeOnly(ElaNewtype nt, LabelMap map)
        {
            var v = nt;

            while (v != null)
            {
                if (!v.Extends && !v.Header)
                    CompileTypeBody(v, map);

                v = v.And;
            }
        }

        //This method only compiles type extensions (declared through 'data').
        private void CompileDataOnly(ElaNewtype nt, LabelMap map)
        {
            var v = nt;

            while (v != null)
            {
                if (v.Extends && !v.Header)
                    CompileTypeBody(v, map);

                v = v.And;
            }
        }

        //Main method for type compilation
        private void CompileTypeBody(ElaNewtype v, LabelMap map)
        {
            //We need to obtain typeId for a type
            var tc = -1;
            var sca = -1;
            var flags = v.Flags;
                
            //A body may be null only if this is a built-in type
            if (!v.HasBody && v.Extends)
                AddError(ElaCompilerError.ExtendsNoDefinition, v, v.Name);
            else if (!v.HasBody)
            {
                tc = (Int32)TCF.GetTypeCode(v.Name);
                tc = tc == 0 ? -1 : tc;
                sca = AddVariable("$$" + v.Name, v, flags | ElaVariableFlags.ClosedType, tc);

                //OK, type is built-in
                if (tc > 0)
                {
                    //We add a special variable that contains a global type ID
                    cw.Emit(Op.PushI4, tc);
                    PopVar(sca);
                }
            }
            else
            {
                var tf = flags;

                if (!v.Opened)
                    tf |= ElaVariableFlags.ClosedType;

                sca = v.Extends ? AddVariable() : AddVariable("$$" + v.Name, v, tf, -1);
            }

            //Type is already declared within the same module (types from different
            //modules can shadow each, so this is perfectly OK).
            if (!v.Extends && frame.InternalTypes.ContainsKey(v.Name))
            {
                AddError(ElaCompilerError.TypeAlreadyDeclared, v, v.Name);
                frame.InternalTypes.Remove(v.Name);
            }
            
           if (v.Prefix != null && !v.Extends)
                AddError(ElaCompilerError.InvalidTypeDefinition, v);

            if (!v.Extends)
                frame.InternalTypes.Add(v.Name, tc);

            AddLinePragma(v);

            //-1 mean "this" module (considered by default).
            var typeModuleId = -1;

            //Add a type var for a non built-in type with a body
            if (tc == -1)
            {
                //Add a special variable with global type ID which will be calculated at run-time
                if (v.Extends)
                {
                    var sv = EmitSpecName(v.Prefix, "$$" + v.Name, v, ElaCompilerError.UndefinedType, out typeModuleId);

                    //We can only extend type that are explicitly declared as open
                    if ((sv.Flags & ElaVariableFlags.ClosedType) == ElaVariableFlags.ClosedType)
                        AddError(ElaCompilerError.UnableExtendOpenType, v, v.Name);
                }
                else
                    cw.Emit(Op.Typeid, AddString(v.Name));
                
                PopVar(sca);

                if (v.HasBody)
                {
                    for (var i = 0; i < v.Constructors.Count; i++)
                    {
                        var c = v.Constructors[i];
                        var cf = v.ConstructorFlags[i];
                        cf = cf == ElaVariableFlags.None ? flags : cf|flags;
                        CompileConstructor(v.Name, sca, c, cf, typeModuleId);
                    }
                }
            }
            else
                cw.Emit(Op.Nop);
        }

        //Generic compilation logic for a constructor, compiles both functions and constants.
        //Parameter sca is an address of variable that contains real type ID.
        private void CompileConstructor(string typeName, int sca, ElaExpression exp, ElaVariableFlags flags, int typeModuleId)
        {
            //Constructor name
            var name = String.Empty;

            switch (exp.Type)
            {
                case ElaNodeType.NameReference:
                    {
                        var n = (ElaNameReference)exp;
                        name = n.Name;

                        if (!n.Uppercase)
                            AddError(ElaCompilerError.InvalidConstructor, n, FormatNode(n));
                        else
                            CompileConstructorConstant(typeName, n, sca, flags, typeModuleId);
                    }
                    break;
                case ElaNodeType.Juxtaposition:
                    {
                        var n = (ElaJuxtaposition)exp;

                        if (n.Target.Type == ElaNodeType.NameReference)
                        {
                            var m = (ElaNameReference)n.Target;
                            name = m.Name;

                            //If a name is uppercase or if this is an infix/postfix/prefix constructor
                            //we assume that this is a correct case and is a constructor function
                            if (m.Uppercase || (Format.IsSymbolic(m.Name) && n.Parameters.Count <= 2))
                            {
                                CompileConstructorFunction(typeName, m.Name, n, sca, flags, typeModuleId);
                                break;
                            }   
                        }

                        AddError(ElaCompilerError.InvalidConstructor, exp, FormatNode(exp));
                    }
                    break;
                default:
                    AddError(ElaCompilerError.InvalidConstructor, exp, FormatNode(exp));
                    break;
            }

            //To prevent redundant errors
            CurrentScope.Locals.Remove("$$$$" + name);
            var ab = AddVariable("$$$$" + name, exp, flags, -1);
            cw.Emit(Op.Ctorid, frame.InternalConstructors.Count - 1);
            PopVar(ab);
        }

        //Compiles a simple parameterless constructor
        private void CompileConstructorConstant(string typeName, ElaNameReference exp, int sca, ElaVariableFlags flags, int typeModuleId)
        {
            frame.InternalConstructors.Add(new ConstructorData
            {
                TypeName = typeName,
                Name = exp.Name,
                Code = -1,
                TypeModuleId = typeModuleId
            });

            AddLinePragma(exp);
            cw.Emit(Op.Ctorid, frame.InternalConstructors.Count - 1);
            PushVar(sca);
            cw.Emit(Op.Newtype0);
            var a = AddVariable(exp.Name, exp, ElaVariableFlags.TypeFun|flags, 0);
            PopVar(a);
        }

        //Compiles a type constructor
        private void CompileConstructorFunction(string typeName, string name, ElaJuxtaposition juxta, int sca, ElaVariableFlags flags, int typeModuleId)
        {
            Label funSkipLabel;
            int address;
            LabelMap newMap;
            var pars = new List<String>();
            var len = juxta.Parameters.Count;

            AddLinePragma(juxta);
            CompileFunctionProlog(name, len, juxta.Line, juxta.Column, out funSkipLabel, out address, out newMap);
            
            var sys = new int[len];
            var types = new ScopeVar[len];
            var bangs = new bool[len];
            
            //Here we have to validate all constructor parameters
            for (var i = 0; i < len; i++)
            {
                var ce = juxta.Parameters[i];
                sys[i] = AddVariable();

                if (bangs[i] = IsBang(ce))
                    cw.Emit(Op.Force);

                PopVar(sys[i]);

                //This can be type a type constraint so we should compile here type check logic
                if (ce.Type == ElaNodeType.Juxtaposition)
                {
                    var jc = (ElaJuxtaposition)ce;

                    //First we check if a constraint is actually valid
                    if (IsInvalidConstructorParameterConstaint(jc))
                        AddError(ElaCompilerError.InvalidConstructorParameter, juxta, FormatNode(ce), name);                    
                    else if (jc.Target.Type == ElaNodeType.NameReference)
                    {
                        //A simple direct type reference
                        var nt = (ElaNameReference)jc.Target;
                        PushVar(sys[i]);
                        types[i] = TypeCheckConstructor(name, null, nt.Name, nt, false);
                        pars.Add(jc.Parameters[0].GetName());
                    }
                    else if (jc.Target.Type == ElaNodeType.FieldReference)
                    {
                        //A type is qualified with a module alias
                        var fr = (ElaFieldReference)jc.Target;
                        PushVar(sys[i]);
                        types[i] = TypeCheckConstructor(name, fr.TargetObject.GetName(), fr.FieldName, fr, false);
                        pars.Add(jc.Parameters[0].GetName());
                    }
                }
                else if (ce.Type == ElaNodeType.NameReference && !IsInvalidConstructorParameter(ce))
                {
                    pars.Add(ce.GetName());
                    types[i] = ScopeVar.Empty;
                }
                else
                    AddError(ElaCompilerError.InvalidConstructorParameter, juxta, FormatNode(ce), name);
            }

            frame.InternalConstructors.Add(new ConstructorData
            {
                TypeName = typeName,
                Name = name,
                Code = -1,
                Parameters = pars,
                TypeModuleId = typeModuleId
            });

            //For optimization purposes we use a simplified creation algorythm for constructors 
            //with 1 and 2 parameters
            if (len == 1)
                PushVar(sys[0]);                    
            else if (len == 2)
            {
                PushVar(sys[0]);
                PushVar(sys[1]);                    
            }
            else 
            {
                cw.Emit(Op.Newtup, len);

                for (var i = 0; i < len; i++)
                {
                    PushVar(sys[i]);
                    cw.Emit(Op.Tupcons, i);
                }
            }

            var ctid = frame.InternalConstructors.Count - 1;
            cw.Emit(Op.Ctorid, ctid);
            //Refering to captured name, need to recode its address
            PushVar((Byte.MaxValue & sca) + 1 | (sca << 8) >> 8);

            if (len == 1)
                cw.Emit(Op.Newtype1);
            else if (len == 2)
                cw.Emit(Op.Newtype2);
            else
                cw.Emit(Op.Newtype);

            CompileFunctionEpilog(name, len, address, funSkipLabel);
            var a = AddVariable(name, juxta, ElaVariableFlags.TypeFun|ElaVariableFlags.Function|flags, len);
            PopVar(a);

            //We need to add special variable that would indicate that a constructor parameter
            //should be strictly evaluated. Used when inlining constructors. This variable is for
            //metadata only, it is never initialized.
            for (var i = 0; i < bangs.Length; i++)
            {
                if (bangs[i])
                {
                    CurrentScope.Locals.Remove("$-!" + i + name); //To prevent redundant errors
                    AddVariable("$-!" + i + name, juxta, ElaVariableFlags.None, -1);
                }
            }

            //We need to add special variable that would store type check information.
            //This information is used when inlining constructors.
            for (var i = 0; i < types.Length; i++)
            {
                var sv = types[i];

                //There is a type constraint, we have to memoize it for inlining
                if (!sv.IsEmpty())
                {
                    CurrentScope.Locals.Remove("$-" + i + name); //To prevent redundant errors
                    var av = AddVariable("$-" + i + name, juxta, ElaVariableFlags.None, -1);

                    //This ScopeVar was obtained in a different scope, we have to patch it here
                    if ((sv.Flags & ElaVariableFlags.External) == ElaVariableFlags.External)
                        PushVar(sv); //No need to patch an external
                    else
                    {
                        //Roll up one scope
                        sv.Address = ((sv.Address & Byte.MaxValue) - 1) | (sv.Address >> 8) << 8;
                        PushVar(sv);
                    }

                    PopVar(av);
                }
            }

            //To prevent redundant errors
            CurrentScope.Locals.Remove("$-" + name); 
            CurrentScope.Locals.Remove("$--" + name);
            //We add special variables that can be used lately to inline this constructor call.
            var consVar = AddVariable("$-" + name, juxta, flags, len);
            var typeVar = AddVariable("$--" + name, juxta, flags, len);
            cw.Emit(Op.Ctorid, ctid);
            PopVar(consVar);
            PushVar(sca);
            PopVar(typeVar);
        }

        //Checks if we need to force a constructor parameter.
        private bool IsBang(ElaExpression exp)
        {
            if (exp.Type == ElaNodeType.NameReference && ((ElaNameReference)exp).Bang)
                return true;
            else if (exp.Type == ElaNodeType.Juxtaposition)
            {
                var j = (ElaJuxtaposition)exp;

                if (j.Parameters[0].Type == ElaNodeType.NameReference &&
                    ((ElaNameReference)j.Parameters[0]).Bang)
                    return true;

                return false;
            }

            return false;
        }

        //Checks if a constructor parameter name is invalid
        private bool IsInvalidConstructorParameter(ElaExpression exp)
        {
            if (exp.Type != ElaNodeType.NameReference)
                return false;

            var n = (ElaNameReference)exp;
            return n.Uppercase || Format.IsSymbolic(n.Name);
        }

        //Checks if a constructor parameter type constraint is invalid
        private bool IsInvalidConstructorParameterConstaint(ElaJuxtaposition n)
        {
            if (n.Parameters.Count != 1)
                return true;

            if (IsInvalidConstructorParameter(n.Parameters[0]))
                return true;

            if (n.Target.Type == ElaNodeType.NameReference)
                return !((ElaNameReference)n.Target).Uppercase;
            else if (n.Target.Type == ElaNodeType.FieldReference)
            {
                var f = (ElaFieldReference)n.Target;
                return f.TargetObject.Type != ElaNodeType.NameReference || !Char.IsUpper(f.FieldName[0]);
            }

            return true;
        }

        //Checks if a constructor is actually a list constructor which implementation can be optimized.
        private bool IsCons(ElaExpression exp, ElaNewtype t)
        {
            if (exp.Type != ElaNodeType.Juxtaposition)
                return false;

            var j = (ElaJuxtaposition)exp;

            //We check that a constructor is in the form 'a :: a'
            return j.Parameters.Count == 2 
                && j.Parameters[0].Type == ElaNodeType.NameReference
                && j.Parameters[1].Type == ElaNodeType.NameReference 
                && !Char.IsUpper(j.Parameters[0].GetName()[0])
                && !Char.IsUpper(j.Parameters[1].GetName()[0]);
        }

        //Returns a local index of a constructor
        private int ConstructorIndex(string name)
        {
            for (var i = 0; i < frame.InternalConstructors.Count; i++)
                if (name == frame.InternalConstructors[i].Name)
                    return i;

            return -1;
        }
    }
}
