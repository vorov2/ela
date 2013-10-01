using System;
using Ela.Linking;

namespace Ela.Library.General
{
    public sealed class CharModule : ForeignModule
    {
        public CharModule()
        {

        }

        public override void Initialize()
        {
            Add<Char,Char>("lower", ToLower);
            Add<Char,Char>("upper", ToUpper);
            Add<Char,Boolean>("isLower", IsLower);
            Add<Char,Boolean>("isUpper", IsUpper);
            Add<Char,Boolean>("isControl", IsControl);
            Add<Char,Boolean>("isDigit", IsDigit);
            Add<Char,Boolean>("isLetter", IsLetter);
            Add<Char,Boolean>("isLetterOrDigit", IsLetterOrDigit);
            Add<Char,Boolean>("isNumber", IsNumber);
            Add<Char,Boolean>("isPunctuation", IsPunctuation);
            Add<Char,Boolean>("isSeparator", IsSeparator);
            Add<Char,Boolean>("isSurrogate", IsSurrogate);
            Add<Char,Char,Boolean>("isSurrogatePair", IsSurrogatePair);
            Add<Char,Boolean>("isSymbol", IsSymbol);
            Add<Char,Boolean>("isWhiteSpace", IsWhiteSpace);
        }
        
        public char ToLower(char ch)
        {
            return Char.ToLower(ch);
        }
        
        public char ToUpper(char ch)
        {
            return Char.ToUpper(ch);
        }
        
        public bool IsUpper(char ch)
        {
            return Char.IsUpper(ch);
        }

        public bool IsLower(char ch)
        {
            return Char.IsLower(ch);
        }
        
        public bool IsControl(char ch)
        {
            return Char.IsControl(ch);
        }

        public bool IsDigit(char ch)
        {
            return Char.IsDigit(ch);
        }
        
        public bool IsLetter(char ch)
        {
            return Char.IsLetter(ch);
        }

        public bool IsLetterOrDigit(char ch)
        {
            return Char.IsLetterOrDigit(ch);
        }

        public bool IsNumber(char ch)
        {
            return Char.IsNumber(ch);
        }

        public bool IsPunctuation(char ch)
        {
            return Char.IsPunctuation(ch);
        }
        
        public bool IsSeparator(char ch)
        {
            return Char.IsSeparator(ch);
        }
        
        public bool IsSurrogate(char ch)
        {
            return Char.IsSurrogate(ch);
        }

        public bool IsSurrogatePair(char hs, char ls)
        {
            return Char.IsSurrogatePair(hs, ls);
        }

        public bool IsSymbol(char ch)
        {
            return Char.IsSymbol(ch);
        }

        public bool IsWhiteSpace(char ch)
        {
            return Char.IsWhiteSpace(ch);
        }
    }
}
