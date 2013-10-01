using System;
using System.IO;
using Elide.Core;
using Elide.Environment;

namespace Elide.TextEditor
{
    public interface IDocumentNavigatorService : IService
    {
        bool SetActive(FileInfo fileInfo);

        bool SetActive(Document doc);

        bool Navigate(Document doc, int line, int col, int length, bool arrow);

        bool Navigate(Document doc, int start, int end, bool arrow);
    }
}
