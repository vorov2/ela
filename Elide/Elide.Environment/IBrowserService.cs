using System;
using Elide.Core;

namespace Elide.Environment
{
    public interface IBrowserService : IService
    {
        bool OpenLink(Uri url);
    }
}
