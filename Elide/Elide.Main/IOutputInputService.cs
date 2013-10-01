using System;
using System.IO;
using Elide.Core;

namespace Elide.Main
{
    public interface IOutputInputService : IService
    {
        string ReadFileAsString(FileInfo fileInfo);

        void WriteStringToFile(FileInfo fileInfo, string text);
    }
}
