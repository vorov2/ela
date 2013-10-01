using System;
using Elide.Core;

namespace Elide.Main
{
    public interface IDaemonService : IService
    {
        void RegisterDaemon(IDaemon daemon);

        void RemoveDaemon(IDaemon daemon);
    }
}
