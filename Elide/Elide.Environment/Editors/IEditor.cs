using System;
using System.Drawing;
using System.IO;
using Elide.Core;

namespace Elide.Environment.Editors
{
    public interface IEditor
    {
        void Initialize(IApp app);

        bool TestDocumentType(FileInfo fileInfo);

        Document CreateDocument(string title);

        Document OpenDocument(FileInfo fileInfo);

        void OpenDocument(Document doc);

        void ReloadDocument(Document doc, bool silent);

        void Save(Document doc, FileInfo fileInfo);
        
        void CloseDocument(Document doc);

        Image DocumentIcon { get; }

        object Control { get; }

        object Menu { get; }
    }
}
