using System;
using System.Linq;
using System.Text;
using LanguageExt;
using Parser.Lexemes;
using static LanguageExt.Prelude;


namespace Parser.Machines
{

    public interface IMachine
    {
        void Next();
    }

    public class IdentifierMachine : IMachine
    {
        readonly ILexemeProvider _provider;

        public IdentifierMachine(ILexemeProvider provider)
        {
            var tmp = provider ?? throw new ArgumentNullException(nameof(provider));
            if (tmp is ILexemeParent child)
            {
                var tmp1 = child.GetChild();
                if (tmp1.IsSome)
                {
                    _provider = tmp1.First();
                }
                else throw new Exception("Cannot retrieve child lexeme provider");
            }
            else _provider = provider;
        }


        enum States { Started, LetterFound, LetterOrDigitFound, DotFound, Finished, Error };


        /// <summary>
        /// THis function is a state machine. it should be called only when the lexeme is a character that is valid for this machine 
        /// it advances the provider only when the current charcter is a valid one. if it is not then it exits 
        ///  </summary>
        /// <param name="provider">Lexeme Provider</param>
        /// <returns> a valid character or nothing</returns>
        public Option<string> Get()
        {
            var token = Option<string>.None;

            if (!_provider.IsSafeToRead)
                return token;

            while(_provider.Current.type == LexemeType.Space)
            {
                if(!_provider.Next())  // its not safe to read further, quit now!
                    return token;
            }
            

            // If reached this point:
            // 1. It's safe to read,
            // 2. the next input is anythign but space

            var sb = new StringBuilder();
            var (current, future) = (States.Started, States.Started);
            Predicate<States> canContinue = (s0)=> s0 != States.Error && s0!= States.Finished;


            do
            {
                var (dt, type) = _provider.Current;
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
                                future = States.Finished;
                                break;
                        }
                        break;


                    case States.Finished:
                    case States.Error:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var safe = _provider.Next();
                if(!safe)
                {
                    current = (future != States.Error) ? States.Finished : States.Error;
                }
                else
                    current = future;
            }
            while(canContinue(current));

            if(future != States.Error)
                token = GetToken(sb.ToString());

            return token;

            Option<string> GetToken(string txt)
            {
                var reserved = new[] {"and", "or", "not"};
                var t0 = txt.Trim();
                if (reserved.Any(x => string.Compare(x, t0, StringComparison.CurrentCultureIgnoreCase) == 0))
                    return Option<string>.None;

                return t0;
            }
        }

        public void Next()
        {
            if(_provider is ILexemeTransaction tran)
                tran.Commit();
        }
    }

}
