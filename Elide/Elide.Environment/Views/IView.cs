using System;
using Elide.Core;

namespace Elide.Environment.Views
{
    public interface IView : IDisposable
    {
        void Initialize(IApp app);

        void Activate();

        void Deactivate();

        object Control { get; }

        event EventHandler<ViewEventArgs> ContentChanged;
    }
}
