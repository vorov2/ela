using System;
using System.Collections.Generic;
using System.IO;
using Elide.Core;

namespace Elide.Environment
{
    public interface IDocumentService : IService
    {
        void AddDocument(Document doc);

        void RemoveDocument(Document doc);

        Document GetOpenedDocument(FileInfo fileInfo);

        Document GetActiveDocument();
        
        bool SetActiveDocument(Document doc);
        
        Document GetNextDocument(Document doc, Type docType);

        Document GetPreviousDocument(Document doc, Type docType);

        IEnumerable<Document> EnumerateDocuments();

        event EventHandler<DocumentEventArgs> ActiveDocumentChanged;

        event EventHandler<DocumentEventArgs> DocumentAdded;

        event EventHandler<DocumentEventArgs> DocumentRemoved;

        event EventHandler<DocumentEventArgs> DocumentClosed;
    }
}
