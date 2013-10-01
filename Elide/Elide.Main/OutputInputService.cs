using System;
using System.IO;
using Elide.Core;

namespace Elide.Main
{
    public sealed class OutputInputService : Service, IOutputInputService
    {
        public OutputInputService()
        {

        }

        public string ReadFileAsString(FileInfo fileInfo)
        {
            using (var stream = OpenFileRead(fileInfo))
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public void WriteStringToFile(FileInfo fileInfo, string text)
        {
            using (var sr = new StreamWriter(fileInfo.Open(FileMode.Create)))
                sr.Write(text);
        }

        private Stream OpenFileRead(FileInfo fileInfo)
        {
            try
            {
                return fileInfo.OpenRead();
            }
            catch (IOException)
            {
                return fileInfo.OpenRead();
            }
        }
    }
}
