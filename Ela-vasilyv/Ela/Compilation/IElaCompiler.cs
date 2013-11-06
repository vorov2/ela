using System;
using Ela.CodeModel;

namespace Ela.Compilation
{
	public interface IElaCompiler
	{
		CompilerResult Compile(ElaProgram prog, CompilerOptions options, ExportVars builtins);

        CompilerResult Compile(ElaProgram prog, CompilerOptions options, ExportVars builtins, CodeFrame frame, Scope globalScope);

		event EventHandler<ModuleEventArgs> ModuleInclude;
	}
}
