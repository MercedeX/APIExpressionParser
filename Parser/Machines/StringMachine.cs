using System.Text;

using LanguageExt;

using Parser.Lexemes;

namespace Parser.Machines
{
    public class StringMachine :MachineBase
    {
        /// <inheritdoc />
        public StringMachine(ILexemeScanner scanner) : base(scanner)
        {
        }

        enum States { Started, Middle, Finished, Error }
        /// <inheritdoc />
        public override Option<Token> Get()
        {
            if(_scanner.IsSafeToRead)
            {
                var sb = new StringBuilder();
                var current = States.Started;

                do
                {
                    var (ch, type) = _scanner.Current;
                    switch(type)
                    {
                        case LexemeType.Alpha:
                        case LexemeType.Digit:
                        case LexemeType.Symbol:
                        case LexemeType.Unknown:
                        case LexemeType.Space:
                            if(current == States.Middle)
                            {
                                sb.Append(ch);
                            }

                            break;

                        case LexemeType.Punctuation:
                            current = (current == States.Started) ? States.Middle : States.Finished;
                            break;

                        default:
                            current = States.Error;
                            break;
                    }

                    if(current <= States.Finished)
                    {
                        _scanner.Next();
                    }
                }
                while(current != States.Error && current != States.Finished && _scanner.IsSafeToRead);

                if(current == States.Finished)
                {
                    return new Token(TokenType.String, sb.ToString());
                }
            }


            return Option<Token>.None;
        }
    }
}