using System;
using Elide.Core;

namespace Elide.Environment
{
    public interface IPrintService : IService
    {
        bool IsPrintAvailable(Document doc);

        void Print(Document doc);

        void PageSetup(Document doc);

        void PrintPreview(Document doc);
    }
}
