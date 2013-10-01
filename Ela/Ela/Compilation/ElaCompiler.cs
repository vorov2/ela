using System;
using Ela.CodeModel;
using Ela.Linking;

namespace Ela.Compilation
{
	public sealed class ElaCompiler : IElaCompiler
	{
		#region Construction
		public ElaCompiler()
		{
			
		}
		#endregion


		#region Methods
		public CompilerResult Compile(ElaProgram prog, CompilerOptions options, ExportVars builtins)
		{
			var frame = new CodeFrame();
			return Compile(prog, options, builtins, frame, new Scope(false, null));
		}


        public CompilerResult Compile(ElaProgram prog, CompilerOptions options, ExportVars builtins, CodeFrame frame, Scope globalScope)
		{
            Options = options;
            var helper = new Builder(frame, this, builtins, globalScope);
                
            try
            {
                helper.CompileUnit(prog);
            }
            catch (TerminationException)
            {
                //Nothing should be done here. This was thrown to stop compilation.      
            }
#if !DEBUG
            catch (Exception ex)
            {
                if (ex is ElaCompilerException)
                    throw;

                throw new ElaCompilerException(Strings.GetMessage("Ice", ex.Message), ex);
            }
#endif

            frame.Symbols = frame.Symbols == null ? helper.Symbols :
                helper.Symbols != null ? frame.Symbols.Merge(helper.Symbols) : frame.Symbols;
            frame.GlobalScope = globalScope;
            return new CompilerResult(frame, helper.Success, helper.Errors.ToArray());

		}

        public static int GetOpCodeSize(Op op)
        {
            return OpSizeHelper.OpSize[(Int32)op];
        }
		#endregion


		#region Properties
		internal CompilerOptions Options { get; private set; }
		#endregion


		#region Events
		public event EventHandler<ModuleEventArgs> ModuleInclude;
		internal void OnModuleInclude(ModuleEventArgs e)
		{
			var h = ModuleInclude;

			if (h != null)
				h(this, e);
		}
		#endregion
	}
}