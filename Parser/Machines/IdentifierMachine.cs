using System;
using System.Linq;
using System.Text;

using LanguageExt;

using Parser.Lexemes;


namespace Parser.Machines
{
    public class IdentifierMachine :MachineBase
    {
        public IdentifierMachine(ILexemeScanner scanner) : base(scanner) { }


        enum States { Started, LetterFound, LetterOrDigitFound, DotFound, Finished, Error };


        /// <summary>
        /// THis function is a state machine. it should be called only when the lexeme is a character that is valid for this machine 
        /// it advances the scanner only when the current charcter is a valid one. if it is not then it exits 
        ///  </summary>
        /// <returns> a valid character or nothing</returns>
        public override Option<Token> Get()
        {

            Predicate<States> canContinue = (s0)=> s0 != States.Error && s0!= States.Finished;
            var token = Option<Token>.None;
            var sb = new StringBuilder();


            // Starting symbol must be an alphabet. if not, quit now.
            if(!_scanner.IsSafeToRead)
            {
                return token;
            }

            if(_scanner.Current.type != LexemeType.Alpha)
            {
                return token;
            }


            // If reached this point:
            // 1. It's safe to read,
            // 2. the next input is anythign but space

            var (current, future) = (States.Started, States.Started);

            do
            {
                var (dt, type) = _scanner.Current;

                switch(current)
                {
                    case States.Started:
                        switch(type)
                        {
                            case LexemeType.Alpha:  //Must be a char or Error
                                sb.Append(dt);
                                future = States.LetterFound;
                                break;

                            default:
                                future = States.Error;
                                break;
                        }
                        break;

                    case States.LetterFound:
                        switch(type)
                        {
                            case LexemeType.Alpha:
                            case LexemeType.Digit:
                                sb.Append(dt);
                                future = States.LetterOrDigitFound;
                                break;
                            case LexemeType.Symbol when dt == '.':
                                sb.Append(dt);
                                future = States.DotFound;
                                break;
                            default:
                                future = States.Finished;
                                break;
                        }
                        break;

                    case States.LetterOrDigitFound:
                        switch(type)
                        {
                            case LexemeType.Alpha:
                            case LexemeType.Digit:
                                sb.Append(dt);
                                future = States.LetterOrDigitFound;
                                break;
                            case LexemeType.Symbol when dt == '.':
                                sb.Append(dt);
                                future = States.DotFound;
                                break;
                            default:
                                future = States.Finished;
                                break;
                        }
                        break;


                    case States.DotFound:
                        switch(type)
                        {
                            case LexemeType.Alpha:
                                sb.Append(dt);
                                future = States.LetterFound;
                                break;
                            default:
                                future = States.Error;
                                _scanner.Back(); // if . was found and no alphabet following it, move back.
                                break;
                        }
                        break;


                    case States.Finished:
                    case States.Error:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }


                if(future != States.Error && future != States.Finished)
                {
                    _ = _scanner.Next();
                    current = _scanner.IsSafeToRead ? future : States.Finished;
                }
                else
                {
                    current = future;
                }
            }
            while(canContinue(current));

            if(future != States.Error)
            {
                token = GetToken(sb.ToString());
            }

            return token;

            Option<Token> GetToken(string txt)
            {
                var reserved = new[] {"and", "or", "not"};

                if(reserved.All(x => string.Compare(x, txt, StringComparison.CurrentCultureIgnoreCase) != 0))
                {
                    return new Token(TokenType.Identifier, txt.Trim());
                }

                return Option<Token>.None;
            }
        }

    }

}
