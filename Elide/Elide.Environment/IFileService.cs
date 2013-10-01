using System;
using System.IO;
using Elide.Core;

namespace Elide.Environment
{
    public interface IFileService : IService
    {
        void NewFile(string editorKey);

        void NewDefaultFile();

        void NewMainFile();

        void OpenFile();

        void OpenFile(FileInfo fi);

        void Save();

        void Save(Document doc);

        void Save(Document doc, FileInfo file);

        void SaveAll();

        void SaveAs();

        void SaveAs(Document doc);

        void Close();

        void CloseAll();

        void CloseAllOther();

        bool Close(Document doc);
    }
}
