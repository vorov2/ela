using System;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
    public sealed class ConModule : ForeignModule
    {
        public ConModule()
        {

        }
        
        public override void Initialize()
        {
            Add<ElaValue,ElaUnit>("write", Write);
            Add<ElaValue,ElaUnit>("writen", WriteLine);
            Add<String>("readn", ReadLine);
        }
        
        public ElaUnit Write(ElaValue val)
        {
            Console.Write(val.AsString());
            return ElaUnit.Instance;
        }
        
        public ElaUnit WriteLine(ElaValue val)
        {
            Console.WriteLine(val.AsString());
            return ElaUnit.Instance;
        }
        
        public string ReadLine()
        {
            var ln = Console.ReadLine();
            return ln != null ? ln.Trim('\0') : String.Empty;
        }
    }
}
