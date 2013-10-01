using System;
using Elide.Core;

namespace Elide.CodeEditor
{
    public interface IOutlinerService : IService
    {
        void Outline(CodeDocument doc);

        void ClearOutline(CodeDocument doc);
    }
}
