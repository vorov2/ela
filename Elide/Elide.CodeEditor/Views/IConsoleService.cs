using System;
using Elide.Core;

namespace Elide.CodeEditor.Views
{
    public interface IConsoleService : IService
    {
        void StartSession(ConsoleSessionInfo sessionInfo);

        void EndSession(object printValue);
    }
}
