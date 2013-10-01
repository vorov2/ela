using System;
using Ela;
using Ela.Runtime;
using Ela.Parsing;
using Ela.Compilation;
using Ela.Linking;
using ElaConsole.Options;

namespace ElaConsole
{
    internal enum InteractiveAction
    {
        None,
        Reset,
        EvalFile
    }

	internal sealed class InteractiveCommands
	{
		private ElaMachine machine;
		private MessageHelper helper;
        private ElaOptions opts;

		internal InteractiveCommands(ElaMachine machine, MessageHelper helper, ElaOptions opts)
		{
			this.machine = machine;
			this.helper = helper;
            this.opts = opts;
		}


        internal InteractiveAction ProcessCommand(string command)
		{
            Data = null;
			var idx = command.IndexOf(' ');
			var arg = default(String);
			var cmd = default(String);

			if (idx > -1)
			{
				arg = command.Substring(idx).Trim(' ', '"');
				cmd = command.Substring(1, idx - 1);
			}
			else
				cmd = command.Substring(1);

			switch (cmd)
			{
				case "help":
					PrintHelp();
					break;
				case "clear":
					Console.Clear();
					break;
				case "exit":
					Environment.Exit(0);
					break;
                case "reset":
                    Console.WriteLine("Session reseted.");
                    return InteractiveAction.Reset;
                case "eval":
                    Data = arg;
                    return InteractiveAction.EvalFile;
                case "ml":
                    opts.Multiline = !opts.Multiline;
					Console.WriteLine();
					Console.WriteLine("Multiline mode is {0}.", opts.Multiline ? "on" : "off");
                    break;
				default:
					Console.WriteLine();
					Console.WriteLine("Unrecognized interactive command '{0}'.", cmd);
					break;
			}

            return InteractiveAction.None;
		}

        internal string Data { get; private set; }

		internal void PrintHelp()
		{
			helper.PrintHelp();
		}
	}
}
