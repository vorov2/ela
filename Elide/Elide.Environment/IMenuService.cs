using System;
using Elide.Core;

namespace Elide.Environment
{
    public interface IMenuService : IService
    {
        IMenuBuilder<T> CreateMenuBuilder<T>() where T : new();

        IMenuBuilder<T> CreateMenuBuilder<T>(T menu) where T : new();
    }
}
