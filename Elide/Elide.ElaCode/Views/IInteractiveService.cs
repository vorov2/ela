using System;
using Elide.Core;

namespace Elide.ElaCode.Views
{
    public interface IInteractiveService : IService
    {
        void EvaluateSelected();

        void EvaluateCurrentModule();
    }
}
