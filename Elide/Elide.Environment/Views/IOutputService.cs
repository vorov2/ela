using System;
using Elide.Core;

namespace Elide.Environment.Views
{
    public interface IOutputService : IService
    {
        void WriteLine();

        void WriteLine(string text, params object[] args);

        void Write(string text, params object[] args);

        void WriteLine(OutputFormat format, string text, params object[] args);

        void Write(OutputFormat format, string text, params object[] args);

        void Clear();
    }
}
