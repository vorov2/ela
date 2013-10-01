using System;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    public interface IConfigDialogService : IService
    {
        bool ShowConfigDialog();
    }
}
