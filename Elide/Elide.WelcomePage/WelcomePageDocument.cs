using System;
using Elide.Environment;

namespace Elide.WelcomePage
{
    public sealed class WelcomePageDocument : Document
    {
        public static readonly Document Instance = new WelcomePageDocument();

        internal WelcomePageDocument() : base("Welcome to Elide!")
        {

        }

        public override void Dispose()
        {
            //Nothing to dispose
        }

        public override bool IsDirty
        {
            get { return false; }
            set { }
        }
    }
}
