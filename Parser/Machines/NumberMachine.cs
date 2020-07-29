using System;
using System.Text;

using LanguageExt;

using Parser.Lexemes;

namespace Parser.Machines
{
    public class NumberMachine :MachineBase
    {
        /// <inheritdoc />
        public NumberMachine(ILexemeScanner scanner) : base(scanner)
        {
        }

        enum States { DigitsBeforeDot, RightAtDot, DigitsAfterDot, Finished, Error }

        /// <inheritdoc />
        public override Option<Token> Get()
        {
            if(_scanner.IsSafeToRead && _scanner.Current.type == LexemeType.Digit)
            {
                var sb = new StringBuilder();
                var current = States.DigitsAfterDot;

                var isFloat = false;

                do
                {
                    var (ch, type) = _scanner.Current;
                    var next = States.Error;

                    switch(type)
                    {
                        case LexemeType.Digit:
                            sb.Append(ch);
                            next = current;
                            break;

                        case LexemeType.Symbol when ch == '.':
                            if(current == States.DigitsBeforeDot)
                            {
                                sb.Append('.');
                                next = States.RightAtDot;
                                isFloat = true;
                            }
                            else
                            {
                                next = States.Error;
                            }
                            break;

                        case LexemeType.Symbol when ch == '(' || ch == ')':
                        case LexemeType.Alpha:
                        case LexemeType.Space:
                            if(current == States.RightAtDot)
                            {
                                next = States.Error;
                            }
                            else
                            {
                                next = States.Finished;
                            }

                            break;

                        default:
                            next = States.Error;
                            break;
                    }

                    current = next;
                    if(current != States.Error && current!=States.Finished)
                    {
                        _scanner.Next();
                    }
                }
                while(current != States.Finished && current != States.Error && _scanner.IsSafeToRead);

                if(current == States.Finished)
                {
                    var txt = sb.ToString();
                    var token = isFloat? new Token(TokenType.Number, Convert.ToDouble(txt)) : new Token(type: TokenType.Number, Convert.ToInt64(txt));
                    return token;
                }
            }

            return Option<Token>.None;
        }
    }
}