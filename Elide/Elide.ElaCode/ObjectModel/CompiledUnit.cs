using System;
using System.Collections.Generic;
using System.Linq;
using Ela.CodeModel;
using Ela.Compilation;
using Ela.Debug;
using Elide.CodeEditor;
using Elide.CodeEditor.Infrastructure;
using Elide.Environment;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class CompiledUnit : ICompiledUnit
    {
        internal CompiledUnit(Document doc, CodeFrame codeFrame)
        {
            CodeFrame = codeFrame;
            Document = doc;

            if (codeFrame != null)
            {
                Globals = ExtractNames(codeFrame).ToList();
                Classes = codeFrame.Classes.Select(kv => new TypeClass(kv.Key, kv.Value, GetLocation("$$$" + kv.Key))).ToList();
                Instances = codeFrame.Instances.Select(i => new TypeClassInstance(i.Class, i.Type, new Location(i.Line, i.Column)));
                Types = codeFrame.Types.Select(s => new UserType(s, GetLocation("$$" + s)));
                References = codeFrame.References.Where(r => !r.Key.StartsWith("$__")).Select(r => new Reference(this, r.Value)).ToList();
            }
        }

        private Location GetLocation(string name)
        {
            var sv = CodeFrame.GlobalScope.GetVariable(name);

            if (CodeFrame.Symbols != null)
            {
                var dr = new DebugReader(CodeFrame.Symbols);
                var sym = dr.FindVarSym(sv.VariableAddress, 0);

                if (sym != null)
                {
                    var ln = dr.FindLineSym(sym.Offset);
                    
                    if (ln != null)
                        return new Location(ln.Line, ln.Column);
                }
            }

            return new Location(0, 0);
        }

        private IEnumerable<CodeName> ExtractNames(CodeFrame codeFrame)
        {
            if (codeFrame.Symbols == null)
                yield break;

            var dr = new DebugReader(codeFrame.Symbols);

            if (dr.EnumerateVarSyms().Count() > 0)
            {
                foreach (var v in dr.FindVarSyms(Int32.MaxValue, dr.GetScopeSymByIndex(0)))
                {
                    var flags = (ElaVariableFlags)v.Flags;

                    if (!v.Name.StartsWith("$"))
                    {
                        var ln = dr.FindLineSym(v.Offset);
                        yield return new CodeName(v.Name, (Int32)flags, ln.Line, ln.Column);
                    }
                }
            }
            else
            {
                foreach (var v in codeFrame.GlobalScope.EnumerateNames())
                {
                    var sv = codeFrame.GlobalScope.GetVariable(v);

                    if (!sv.VariableFlags.Set(ElaVariableFlags.Private) && !v.StartsWith("$"))
                        yield return new CodeName(v, (Int32)sv.VariableFlags, 0, 0);
                }
            }
        }

        internal CodeFrame CodeFrame { get; private set; }

        internal Document Document { get; private set; }

        public string Name
        {
            get { return CodeFrame.File.ShortName(); }
        }

        public IEnumerable<CodeName> Globals { get; private set; }

        public IEnumerable<IReference> References { get; private set; }

        public IEnumerable<IType> Types { get; private set; }

        public IEnumerable<IClass> Classes { get; private set; }

        public IEnumerable<IClassInstance> Instances { get; private set; }
    }
}
