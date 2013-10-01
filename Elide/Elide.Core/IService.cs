using System;

namespace Elide.Core
{
    public interface IService
    {
        void Initialize(IApp app);

        void Unload();
    }
}
