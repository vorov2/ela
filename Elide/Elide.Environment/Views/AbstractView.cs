using System;
using Elide.Core;

namespace Elide.Environment.Views
{
    public abstract class AbstractView : IView
    {
        protected AbstractView()
        {
            
        }

        public virtual void Initialize(IApp app)
        {
            App = app;
        }

        public virtual void Activate()
        {

        }

        public virtual void Deactivate()
        {

        }

        public virtual void Dispose()
        {

        }

        public event EventHandler<ViewEventArgs> ContentChanged;
        protected virtual void OnContentChanged(ViewEventArgs e)
        {
            var h = ContentChanged;

            if (h != null)
                h(this, e);
        }

        public abstract object Control { get; }

        protected IApp App { get; private set; }
    }
}
