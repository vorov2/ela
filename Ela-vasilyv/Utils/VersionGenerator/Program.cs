using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VersionGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var changeList = args[0];
			var versionFile = args[1];

			using (var sr = File.OpenText(changeList))
			{
				var line = sr.ReadLine();

				if (line != null)
				{
					var arr = line.Split(':');

					if (arr.Length == 3)
					{
						var v = arr[0].Trim();

						using (var sr2 = new StreamWriter(File.Open(versionFile, FileMode.Create)))
						{
							sr2.Write(String.Format(@"internal static class Const 
{{
	internal const string Version = ""{0}"";
}}
", v));
						}
					}
				}
			}

		}
	}
}
