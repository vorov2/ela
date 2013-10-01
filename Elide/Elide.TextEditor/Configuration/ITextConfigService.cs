using System;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.TextEditor.Configuration
{
    public interface ITextConfigService : IExtService
    {
        TextConfig GetConfig(string key);
    }
}
