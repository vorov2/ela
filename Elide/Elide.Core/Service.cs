using System;

namespace Elide.Core
{
    public abstract class Service : IService
    {
        protected Service()
        {

        }

        public virtual void Initialize(IApp app)
        {
            App = app;
        }

        public virtual void Unload()
        {

        }

        protected IApp App { get; private set; }
    }
}
