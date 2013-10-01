using System;
using System.Collections.Generic;
using System.IO;
using Elide.Core;

namespace Elide.Environment.Editors
{
    public interface IEditorService : IExtService, IExecService
    {
        EditorInfo GetEditor(FileInfo fileInfo);

        EditorInfo GetEditor(Type docType);

        EditorInfo GetEditor(EditorFlags flags);
    }
}
