using System;
using System.Collections.Generic;
using System.Linq;

using Parser.Lexemes;
using Parser.Machines;

namespace Parser
{
    public class Tokenizer
    {
        readonly ILexemeScanner _provider;


        public Tokenizer(in string input) => _provider = new LexemeScanner(input);

        public IEnumerable<Token> GetTokens()
        {
            var machineMap = new Dictionary<LexemeType, IEnumerable<Func<ILexemeScanner, IMachine>>>
            {
                {
                    LexemeType.Alpha,
                    new List<Func<ILexemeScanner, IMachine>> { x=> new IdentifierMachine(x), x=> new OperatorMachine(x)}
                },
                {
                    LexemeType.Symbol,
                    new List<Func<ILexemeScanner, IMachine>> {x=> new OperatorMachine(x) }
                },
                {
                    LexemeType.Digit,
                    new List<Func<ILexemeScanner, IMachine>> {x=> new NumberMachine(x)}
                },
                {
                    LexemeType.Punctuation,
                    new List<Func<ILexemeScanner, IMachine>> {x=>new StringMachine(x)}
                },
                {
                    LexemeType.Space,
                    new List<Func<ILexemeScanner, IMachine>>{x=> new NullMachine(x) }
                },
                {
                    LexemeType.Unknown,
                    new List<Func<ILexemeScanner, IMachine>>{ x=>new NullMachine(x) }
                }
            };

            _provider.Reset();
            while(_provider.IsSafeToRead)
            {
                (var ch, var type) = _provider.Current;
                var builders = machineMap[type];
                var found = false;

                foreach(var builder in builders)
                {
                    var m = builder(_provider);
                    var t0 = m.Get();
                    if (t0.IsSome)
                    {
                        m.Done();
                        found = true;
                        yield return t0.First();
                        break;
                    }

                }                    
                if(!found)
                    _provider.Next();
            }
        }

    }

  
}
