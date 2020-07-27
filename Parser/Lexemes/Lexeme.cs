using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Parser.Lexemes
{
    public enum LexemeType :byte
    {
        Unknown = 0,
        Alpha = 10,
        Digit =20,
        Symbol = 30,
        Space = 40,
        Punctuation =50
    }


    public readonly struct Lexeme
    {
        public readonly char data;
        public readonly LexemeType type;

        internal Lexeme(char data) : this()
        {
            this.data = data;
            type = DetermineType(data);
        }

        public int AsInt() => Convert.ToInt32(data);
        public void Deconstruct(out char dt, out LexemeType type)
        {
            dt = this.data;
            type = this.type;
        }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        static LexemeType DetermineType(char ch)
        {
            if(char.IsLetter(ch)) 
                return LexemeType.Alpha;
            else if(char.IsDigit(ch)) 
                return LexemeType.Digit;
            else if(ch == '\'' || ch == '"' || ch == '`') 
                return LexemeType.Punctuation;
            else if(char.IsSymbol(ch)) 
                return LexemeType.Symbol;
            else if(char.IsWhiteSpace(ch)) 
                return LexemeType.Space;
            else 
                return LexemeType.Unknown;
        }
    }
}