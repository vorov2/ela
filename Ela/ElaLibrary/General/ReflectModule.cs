using System;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using System.Collections.Generic;
using Ela.Runtime;

namespace Ela.Library.General
{
    public sealed class ReflectModule : ForeignModule
    {
        public ReflectModule()
        {

        }

        public override void Initialize()
        {
            Add<ElaFunction,Int32>("funArgsCount", FunArgsCount);
            Add<ElaFunction,String>("funName", FunName);
            Add<ElaFunction,Int32>("funAppliedArgsCount", FunAppliedArgsCount);
            Add<ElaFunction,ElaList>("funAppliedArgs", FunAppliedArgs);
            Add<ElaFunction,Int32>("funHandle", FunHandle);
            Add<ElaFunction,ElaModule>("funModule", FunModule);
            Add<ElaModule,Int32>("moduleHandle", ModuleHandle);
            Add<ElaModule,String>("moduleName", ModuleName);
            Add<ElaModule,ElaList>("moduleExportList", ModuleNames);
            Add<ElaModule,ElaList>("moduleReferences", ModuleReferences);
            Add<ElaModule,Int32>("asmModuleCount", AssemblyModuleCount);
            Add<ElaModule,ElaList>("asmModules", AssemblyModules);
            Add<ElaModule,ElaModule>("asmMainModule", AssemblyMainModule);
        }

        public ElaModule FunModule(ElaFunction fun)
        {
            return fun.GetFunctionModule();
        }

        public int FunHandle(ElaFunction fun)
        {
            return fun.Handle;
        }

        public int FunArgsCount(ElaFunction fun)
        {
            return fun.GetArgumentNumber();
        }

        public int FunAppliedArgsCount(ElaFunction fun)
        {
            return fun.GetAppliedArgumentNumber();
        }

        public string FunName(ElaFunction fun)
        {
            return fun.GetFunctionName();
        }

        public ElaList FunAppliedArgs(ElaFunction fun)
        {
            return ElaList.FromEnumerable(fun.GetAppliedArguments());
        }

        public int ModuleHandle(ElaModule mod)
        {
            return mod.Handle;
        }

        public string ModuleName(ElaModule mod)
        {
            return mod.GetModuleName();
        }

        public ElaList ModuleNames(ElaModule mod)
        {
            return ElaList.FromEnumerable(mod.GetVariables());
        }

        public ElaList ModuleReferences(ElaModule mod)
        {
            return ElaList.FromEnumerable(mod.GetReferences());
        }

        public int AssemblyModuleCount(ElaModule mod)
        {
            return mod.GetCurrentMachine().Assembly.ModuleCount;
        }

        public ElaModule AssemblyMainModule(ElaModule mod)
        {
            var vm = mod.GetCurrentMachine();
            return new ElaModule(0, vm);
        }

        public ElaList AssemblyModules(ElaModule mod)
        {
            var vm = mod.GetCurrentMachine();
            var asm = vm.Assembly;
            var list = new List<ElaValue>();

            for (var i = 0; i < asm.ModuleCount; i++)
                list.Add(new ElaValue(new ElaModule(i, vm)));

            return ElaList.FromEnumerable(list);
        }
    }
}
