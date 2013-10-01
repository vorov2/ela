using System;
using System.IO;
using Elide.Core;
using Elide.Scintilla;
using Elide.TextEditor;
using Elide.Environment;
using Elide.Environment.Editors;

namespace Elide.TextWorkbench
{
    public sealed class DocumentNavigatorService : Service, IDocumentNavigatorService
    {
        public DocumentNavigatorService()
        {
            
        }

        public bool SetActive(FileInfo fileInfo)
        {
            if (fileInfo != null)
            {
                var docSrv = App.GetService<IDocumentService>();
                var realDoc = docSrv.GetOpenedDocument(fileInfo);

                if (realDoc != null)
                    return docSrv.SetActiveDocument(realDoc);
                else
                {
                    fileInfo.Refresh();

                    if (!fileInfo.Exists)
                        return false;

                    var editor = App.Editor(fileInfo);

                    try
                    {
                        realDoc = editor.OpenDocument(fileInfo);
                        docSrv.AddDocument(realDoc);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

        public bool SetActive(Document doc)
        {
            var docSrv = App.GetService<IDocumentService>();

            if (!docSrv.SetActiveDocument(doc))
                return SetActive(doc.FileInfo);
            else
                return true;
        }

        public bool Navigate(Document doc, int line, int col, int length, bool arrow)
        {
            if (SetActive(doc))
            {
                var editor = App.Editor() as ITextEditor;

                if (editor != null)
                {
                    editor.SelectText(line, col, length);

                    if (arrow)
                    {
                        var sci = editor.Control as ScintillaControl;

                        if (sci != null)
                            sci.PutArrow(line);
                    }

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool Navigate(Document doc, int start, int end, bool arrow)
        {
            if (SetActive(doc))
            {
                var editor = App.Editor() as ITextEditor;

                if (editor != null)
                {
                    editor.SelectText(start, end);

                    if (arrow)
                    {
                        var sci = editor.Control as ScintillaControl;                        

                        if (sci != null)
                            sci.PutArrow(sci.GetLineFromPosition(start));
                    }

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
