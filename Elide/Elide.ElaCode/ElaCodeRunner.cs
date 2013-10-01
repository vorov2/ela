using System;
using Ela;
using Ela.Runtime;
using Elide.CodeEditor.Infrastructure;
using Elide.Core;
using Elide.ElaCode.ObjectModel;
using Elide.Environment;
using System.Threading;

namespace Elide.ElaCode
{
    public sealed class ElaCodeRunner : ICodeRunner<CompiledAssembly>
    {
        public ElaCodeRunner()
        {

        }
        
        public object Execute(ICompiledAssembly compiled, params ExtendedOption[] options)
        {
            var asm = (CompiledAssembly)compiled;

            try
            {
                var em = new ElaMachine(asm.Assembly);
                var res = em.Run().ReturnValue;
                return res.TypeCode == ElaTypeCode.Unit ? String.Empty : em.PrintValue(res);
            }
            catch (ElaCodeException ex)
            {
                if (ex.InnerException == null || !(ex.InnerException is ThreadAbortException))
                {
                    var mu = (CompiledUnit)asm.MainUnit;
                    var doc = default(Document);

                    if (!ex.Error.File.Exists)
                        doc = mu.Document;
                    else if (!ex.Error.File.HasExtension("elaobj"))
                        doc = new VirtualDocument(ex.Error.File);

                    throw new CodeException(doc, ex.Error.Line, ex);
                }

                return null;
            }
            catch (ElaException ex)
            {
                if (ex.InnerException == null || !(ex.InnerException is ThreadAbortException))
                {
                    var mu = (CompiledUnit)asm.MainUnit;
                    throw new CodeException(mu.Document, 0, ex);
                }

                return null;
            }
        }

        public IApp App { get; set; }
    }
}
