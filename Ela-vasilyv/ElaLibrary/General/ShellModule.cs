using System;
using System.Diagnostics;
using Ela.Linking;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
	public sealed class ShellModule : ForeignModule
	{
		#region Construction
		public ShellModule()
		{

		}
		#endregion


		#region Methods
		public override void Initialize()
		{
			Add<String,String,ElaUnit>("exec", Exec);
			Add<String,String,ElaUnit>("cmd", Command);
		}


		public ElaUnit Exec(string args, string file)
		{
			var pi = new ProcessStartInfo(file);
			pi.UseShellExecute = true;
			pi.Arguments = args;
			
			using (var p = Process.Start(pi)) { }

			return ElaUnit.Instance;
		}


		public ElaUnit Command(string args, string cmd)
		{
			return Exec("/C " + cmd + " " + args, "cmd.exe");
		}
		#endregion
	}
}
