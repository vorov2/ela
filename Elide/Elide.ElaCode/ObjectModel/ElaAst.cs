using Elide.CodeEditor.Infrastructure;
using System;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class ElaAst : IAst
    {
        internal ElaAst(object root)
        {
            Root = root;
        }

        public object Root { get; private set; }
    }
}
