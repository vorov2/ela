using System;
using System.Collections.Generic;
using Ela.Debug;

namespace ElaConsole.Options
{
	internal sealed class ElaOptions
	{
		#region Construction
		public ElaOptions()
		{
			ReferencePaths = new List<String>();
			Arguments = new List<ElaArg>();
		}
		#endregion


		#region Console Options
        [CommandLineOption("version", "v")]
        public bool ShowVersion { get; set; }

		[CommandLineOption("prompt")]
		public string Prompt { get; set; }
		
		[CommandLineOption(true)]
		public string FileName { get; set; }

		[CommandLineOption("nologo", "n")]
		public bool NoLogo { get; set; }

		[CommandLineOption("eil")]
		public bool ShowEil { get; set; }
				
		[CommandLineOption("sym")]
		public SymTables ShowSymbols { get; set; }

		[CommandLineOption("inter")]
		public bool LunchInteractive { get; set; }

		[CommandLineOption("silent", "s")]
		public bool Silent { get; set; }
				
		[CommandLineOption("time", "t")]
		public bool ShowTime { get; set; }

		[CommandLineOption("help", "h")]
		public bool ShowHelp { get; set; }

		[CommandLineOption("compile", "c")]
        public bool Compile { get; set; }

        [CommandLineOption("parse", "p")]
        public bool ParseOnly { get; set; }

		[CommandLineOption("ml")]
		public bool Multiline { get; set; }

        [CommandLineOption("nlprompt")]
        public bool NewLinePrompt { get; set; }

		[CommandLineOption("out")]
		public string OutputFile { get; set; }

		[CommandLineOption("arg")]
		public List<ElaArg> Arguments { get; private set; }
		#endregion


		#region Compiler Options
		[CommandLineOption("nohints")]
        public bool SupressHints { get; set; }

        [CommandLineOption("strict")]
        public bool Strict { get; set; }
		
		[CommandLineOption("warnaserr")]
		public bool WarningsAsErrors { get; set; }

		[CommandLineOption("nowarn")]
		public bool NoWarnings { get; set; }
		
		[CommandLineOption("debug", "d")]
		public bool Debug { get; set; }

		[CommandLineOption("noOpt")]
		public bool DisableOptimization { get; set; }

		[CommandLineOption("prelude")]
		public string Prelude { get; set; }

        [CommandLineOption("noprelude")]
        public bool NoPrelude { get; set; }
		#endregion


		#region Linker Options
		[CommandLineOption("ref", "r")]
		public List<String> ReferencePaths { get; private set; }

		[CommandLineOption("recompile", "rc")]
		public bool ForceRecompile { get; set; }

		[CommandLineOption("skipDir")]
		public bool IgnoreStartupDirectory { get; set; }
	
		[CommandLineOption("skipCheck")]
		public bool SkipTimeStampCheck { get; set; }

		[CommandLineOption("linkNowarn")]
		public bool LinkerNoWarnings { get; set; }

		[CommandLineOption("linkWarnaserr")]
		public bool LinkerWarningsAsErrors { get; set; }
		#endregion
	}
}
