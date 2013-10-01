using System;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    public interface IOptionPage
    {
        Config Config { get; set; }

        IApp App { get; set; }
    }
}
