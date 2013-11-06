using System;

namespace ElaConsole.Options
{
    public sealed class ElaArg
    {
        public ElaArg(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }

        public string Value { get; private set; }
    }
}
