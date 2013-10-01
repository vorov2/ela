using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IReferenceResolver<out T> where T : IReference
    {
        ICompiledUnit Resolve(IReference reference, params ExtendedOption[] options);

        IApp App { get; set; }
    }
}
