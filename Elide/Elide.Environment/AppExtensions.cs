using System;
using System.IO;
using Elide.Core;
using Elide.Environment.Configuration;
using Elide.Environment.Editors;
using Elide.Environment.Views;

namespace Elide.Environment
{
    public static class AppExtensions
    {
        public static void OpenView(this IApp app, string key)
        {
            app.GetService<IViewService>().OpenView(key);
        }

        public static void CloseView(this IApp app, string key)
        {
            app.GetService<IViewService>().CloseView(key);
        }

        public static Document Document(this IApp app)
        {
            return app.GetService<IDocumentService>().GetActiveDocument();
        }

        public static T Document<T>(this IApp app) where T : Document
        {
            return app.GetService<IDocumentService>().GetActiveDocument() as T;
        }

        public static T Config<T>(this IApp app) where T : Config
        {
            return (T)app.GetService<IConfigService>().QueryConfig(typeof(T));
        }

        public static EditorInfo EditorInfo(this IApp app, FileInfo fileInfo)
        {
            return app.GetService<IEditorService>().GetEditor(fileInfo);
        }

        public static EditorInfo EditorInfo(this IApp app, EditorFlags flags)
        {
            return app.GetService<IEditorService>().GetEditor(flags);
        }

        public static EditorInfo EditorInfo(this IApp app, Document doc)
        {
            return app.GetService<IEditorService>().GetEditor(doc.GetType());
        }

        public static IEditor Editor<T>(this IApp app) where T : Document
        {
            return app.GetService<IEditorService>().GetEditor(typeof(T)).Instance;
        }
        
        public static IEditor Editor(this IApp app)
        {
            var doc = AppExtensions.Document(app);
            return doc != null ? app.GetService<IEditorService>().GetEditor(doc.GetType()).Instance : null;
        }

        public static IEditor Editor(this IApp app, FileInfo fileInfo)
        {
            return app.GetService<IEditorService>().GetEditor(fileInfo).Instance;
        }

        public static IEditor Editor(this IApp app, Type docType)
        {
            return app.GetService<IEditorService>().GetEditor(docType).Instance;
        }
    }
}
