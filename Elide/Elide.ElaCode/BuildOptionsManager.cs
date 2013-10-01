using System;
using System.IO;
using System.Windows.Forms;
using Ela.Compilation;
using Ela.Linking;
using Elide.Core;
using Elide.ElaCode.Configuration;
using Elide.Environment;
using Elide.Main;

namespace Elide.ElaCode
{
    internal sealed class BuildOptionsManager
    {
        private IApp app;

        internal BuildOptionsManager(IApp app)
        {
            this.app = app;
        }

        public LinkerOptions CreateLinkerOptions()
        {
            var c = app.Config<LinkerConfig>();
            var opt = new LinkerOptions();
            var svc = app.GetService<IPathService>();

            c.Directories.ForEach(d => opt.CodeBase.Directories.Add(new DirectoryInfo(svc.ResolvePathMacros(d))));
            opt.CodeBase.LookupStartupDirectory = c.LookupStartupDirectory;
            opt.ForceRecompile = c.ForceRecompile;
            opt.NoWarnings = c.NoWarnings;
            opt.WarningsAsErrors = c.WarningsAsErrors;
            opt.SkipTimeStampCheck = c.SkipTimeStampCheck;
            return opt;
        }

        public CompilerOptions CreateCompilerOptions()
        {
            var c = app.Config<CompilerConfig>();
            var opt = new CompilerOptions();

            opt.GenerateDebugInfo = c.GenerateDebugInfo;
            opt.NoWarnings = c.NoWarnings;
            opt.Prelude = c.Prelude;
            opt.WarningsAsErrors = c.WarningsAsErrors;
            opt.Optimize = c.Optimize;
            opt.ShowHints = c.ShowHints;
            return opt;
        }
    }
}
