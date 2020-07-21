using System;
using System.Runtime.CompilerServices;
using LanguageExt;

namespace Parser
{
    public class LexemeProvider :ILexemeProvider
    {
        readonly Memory<char> _data;
        int _index;


        public LexemeProvider(string input)
        {
            if(!string.IsNullOrEmpty(input))
            {
                _data = input.ToCharArray();
                Reset();
            }
            else
                throw new NullReferenceException("Input string is empty. Lexeme provider must have non empty string");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsValidCharacter(char ch) => !(char.IsWhiteSpace(ch) || char.IsControl(ch) || char.IsSurrogate(ch));

        public Option<char> Future => GetAt(_index + 1);
        public char Current => GetAt(_index).Match(x => x, () => throw new InvalidOperationException());
        public bool Next()
        {
            var ret = false;
            var maxPosition = _data.Length-1;
            var idx = _index+1;

            if(idx < maxPosition)  // Just started, skip any non valid characters
            {
                do
                {
                    if(IsValidCharacter(_data.Span[idx]))
                    {
                        ret = true;
                        break;
                    }
                    else
                        idx++;
                }
                while(idx <= maxPosition);
                _index = idx > maxPosition ? maxPosition : idx;
            }
            else if(_index > maxPosition)
            {
                ret = false;
            }
            else
            {
                _index = maxPosition;
                ret = false;
            }

            return ret;
        }

        public void Reset()
        {
            _index = 0;

            while(!IsValidCharacter(_data.Span[_index]))
                _index+=1;
        }

        public bool Back() {
            var ret = false;
           
            if (_index == 0)
                ret = false;
            else if (_index > 0)
            {
                _index-=1;
                ret = true;
            }
            else // index < 0
            {
                _index =0;
                ret = false;
            }
            return ret;
        }


        Option<char> GetAt(int position)
        {
            var idx = position;

            if(idx < _data.Length - 1)
            {
                var ch = _data.Span[idx];
                var t0 = IsValidCharacter(ch) ? Prelude.Some(ch) : Prelude.None;
                return t0;
            }
                
            return Option<char>.None;
        }
    }
}