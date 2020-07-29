using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Parser.Lexemes
{
    public class LexemeProvider :ILexemeProvider, ILexemeTransaction, ILexemeParent
    {
        readonly Memory<char> _data = Memory<char>.Empty;
        readonly int _maxPosition=0;
        readonly Option<Func<int, bool>> _mover = None;

        int _index;

        public LexemeProvider(in string input)
        {
            if(!string.IsNullOrEmpty(input))
            {
                _data = new Memory<char>(input.ToCharArray());
                _maxPosition = _data.Length-1;
                Reset();
            }
            else
                throw new NullReferenceException("Input string is empty. Lexeme provider must have non empty string");
        }

        public Option<Lexeme> LookAhead => ReadAtPosition(_index + 1);
        public Lexeme Current
        {
            get
            {
                var t0 = ReadAtPosition(_index);
                return t0.Match(x => x, () => throw new InvalidOperationException());
            }
        }
        public bool IsSafeToRead => _index <= _maxPosition;

        public bool Next()
        {
            var ret = false;

            switch(_index)
            {
                case int x when x < _maxPosition:
                    _index += 1;
                    ret = true;
                    break;

                case int x when x == _maxPosition:
                    _index += 1;
                    ret = false;
                    break;

                case int x when x > _maxPosition:
                    ret = false;
                    break;
            }
 
            return ret;
        }
        public void Reset() => _index = 0;
        public bool Back() {
            var ret = false;

            switch(_index)
            {
                case 0:
                    ret = false;
                    break;

                case int x when x < 0:
                    _index = 0;
                    ret = false;
                    break;

                case int x when x>0:
                    _index -= 1;
                        ret = true;
                    break;
            }

            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsValidCharacter(char ch) => !(char.IsControl(ch) || char.IsSurrogate(ch));
        Option<Lexeme> ReadAtPosition(int position)
        {
            var ret = Option<Lexeme>.None;

            if(position <= _maxPosition)
            {
                var ch = _data.Span[position];
                if (IsValidCharacter(ch))
                {
                    var type = DetermineType(ch);
                    ret= new Lexeme(ch, type);
                }
            }

            return ret;
        }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        static LexemeType DetermineType(char ch)
        {
            var ret = LexemeType.Unknown;

            if(char.IsLetter(ch))
                ret = LexemeType.Alpha;

            else if(char.IsDigit(ch))
                ret = LexemeType.Digit;
            
            else if(IsPunctuation(ch))
                ret = LexemeType.Punctuation;
            
            else if(IsSymbol(ch))
                ret = LexemeType.Symbol;
            
            else if(IsSpace(ch))
                ret = LexemeType.Space;

            return ret;

            //======================
            // Local Functions
            //======================
            bool IsPunctuation(char c) => c == '\'' || c == '"' || c == '`';
            bool IsSymbol(char c) => char.IsSymbol(ch) || ch == '(' || ch == ')';
            bool IsSpace(char c) => c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }


        //======================
        // Lexeme Parent
        //======================
        public void Commit()
        {
            _mover.Match(move =>
            {

                if (IsSafeToRead)
                {
                    int pos = _index;
                    var res = move(pos);
                    if (!res)
                        throw new Exception("Invalid position, cannot move parent");
                }
                else
                {
                    int pos = _index - 1;
                    var res = move(pos);
                }
            }, () => throw new Exception("No parent is present to commit"));
        }



        //======================
        // Lexeme Child 
        //======================
        private LexemeProvider(Memory<char> data, Func<int, bool> mover)
        {
            _mover = Some(mover);

            if(data.Length >0)
            {
                _data = data;
                _maxPosition = _data.Length - 1;
                Reset();
            }
            else
                throw new NullReferenceException("Input string is empty. Lexeme provider must have non empty string");
        }
        public Option<ILexemeProvider> GetChild()
        {
           var child = Option<ILexemeProvider>.None;

            if (IsSafeToRead)
            {
                var tmp = _data.Slice(_index);
                ILexemeProvider c0 = new LexemeProvider(tmp, (pos) =>
                {
                    if (pos < _maxPosition)
                    {
                        _index += pos;
                        return true;
                    }

                    return false;
                });
                child = Some(c0);
            }

            return child;
        }


    }
}