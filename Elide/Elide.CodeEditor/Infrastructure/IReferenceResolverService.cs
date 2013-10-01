using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IReferenceResolverService : IExtService
    {
        ICompiledUnit Resolve<T>(T reference, params ExtendedOption[] options) where T : IReference;
    }
}
