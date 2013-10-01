using System;
using Elide.Core;

namespace Elide.TextEditor
{
    public interface IGotoDialogService : IService
    {
        void ShowGotoDialog();
    }
}
