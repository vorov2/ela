using System;
using Elide.Core;

namespace Elide.Environment
{
    public interface IEnvironmentService : IExecService
    {
        object GetMainWindow();
    }
}
