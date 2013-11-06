using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ela.Linking
{
	public sealed class LinkerOptions
	{
		public LinkerOptions()
		{
			CodeBase = new CodeBase();
        }

        public static LinkerOptions Default()
        {
            return new LinkerOptions
            {
                WarningsAsErrors = false,
                NoWarnings = false,
                SkipTimeStampCheck = false,
                ForceRecompile = false,
                CodeBase = new CodeBase()
            };
        }
		
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var pi in typeof(LinkerOptions).GetProperties())
                if (pi.Name != "CodeBase")
                    sb.AppendFormat("{0}={1};", pi.Name, pi.GetValue(this, null));

            sb.AppendFormat("LookupStartupDirectory={0}", CodeBase.LookupStartupDirectory);
            return sb.ToString();
        }
        
        public CodeBase CodeBase { get; private set; }

		public bool ForceRecompile { get; set; }

		public bool SkipTimeStampCheck { get; set; }

		public bool NoWarnings { get; set; }

		public bool WarningsAsErrors { get; set; }
	}
}
