using System;
using System.Collections.Generic;

namespace Elide.Core
{
    public interface IApp : IExtService
    {
        void Run();

        IService GetService(Type serviceType);

        IEnumerable<String> GetCommandLineArguments();
    }
}
