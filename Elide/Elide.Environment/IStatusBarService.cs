using System;
using Elide.Core;

namespace Elide.Environment
{
    public interface IStatusBarService : IService
    {
        void SetStatusString(StatusType type, string text, params object[] args);

        void ClearStatusString();
    }
}
