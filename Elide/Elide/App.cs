using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;

namespace Elide
{
    internal sealed class App : AbstractApp
    {
        private readonly string[] args;

        public App(ExtList<ExtSection> sections, string[] args) : base(sections)
        {
            this.args = args;
        }

        protected override void OnUnload()
        {
            Application.Exit();
        }

        public override IEnumerable<String> GetCommandLineArguments()
        {
            return args;
        }
    }
}
