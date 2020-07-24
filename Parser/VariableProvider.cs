using LanguageExt;

using System;
using System.Linq;
using System.Text;

using static LanguageExt.Prelude;


namespace Parser
{
    #region  Dead Code
    //public enum Operator:byte
    //{
    //    Equals,
    //    NotEquals,
    //    GreaterThan,
    //    GreaterThanOrEqual,
    //    LessThan,
    //    LessThanOrEqual,
    //    Like
    //}
    //public enum Joiner: byte
    //{
    //    AND,
    //    OR
    //}
    //public enum ValueOrVariable:byte
    //{
    //    Value,
    //    Variable
    //}


    //public readonly struct Operand
    //{
    //    public readonly ValueOrVariable type;
    //    public readonly string value;
    //}


    //public readonly struct Expression1
    //{
    //    public readonly Operand leftOperand;
    //    public readonly Operator @operator;
    //    public readonly Operand rightOperand;
    //}

    //public readonly struct Expression2
    //{
    //    public readonly Operand leftOperand;
    //}

    //public readonly struct Expression3
    //{
    //    public readonly Expression1 leftExpression;
    //    public readonly Joiner @operator;
    //    public readonly Expression1 rightOperand;
    //}




    //internal class DataProvider
    //{
    //    readonly Memory<char> _data;
    //    int _nextIndex;

    //    public DataProvider(string data)
    //    {
    //        _data = data.ToCharArray();
    //        _nextIndex = 0;
    //    }

    //    public IEnumerable<Span<char>> GetToken()
    //    {
    //        var operators = new[]{'=', '>', '<', '(', ')' };
    //        var span = _data.Span;
    //        var curr = _nextIndex;
    //        var endPosition = _nextIndex;

    //        var nextToken = "";

    //        while(curr< _data.Length)
    //        {
    //            if(span[curr] == '@' || char.IsLetter(span[curr]))
    //            {
    //                nextToken = "variable";
    //                var (v0, idx)= ExtractVariable(_data.Slice(curr));
    //                curr += idx;
    //                yield return v0;
    //            }
    //            else if(char.IsDigit(span[curr]))
    //            {
    //                nextToken = "number";
    //                var (v0, idx) = ExtractNumber(_data.Slice(curr));
    //                curr += idx;
    //                yield return v0;
    //            }
    //            else if(operators.Contains(span[curr]))
    //            {
    //                nextToken = "operator";
    //                var (v0, idx) = ExtractOperator(_data.Slice(curr));
    //                curr += idx;
    //                yield return v0;
    //            }
    //            else if(char.IsWhiteSpace(span[curr]))
    //            {
    //                curr++;
    //            }
    //        }


    //        while(_nextIndex < _data.Length)
    //        {
    //            if(span[curr]=='@' || char.IsLetterOrDigit(span[curr])

    //        }




    //    }


    //}

    //internal class Tokenizer
    //{

    //} 
    #endregion

    public class VariableProvider
    {
        ILexemeProvider _provider;

        public VariableProvider(ILexemeProvider provider) => _provider = provider ?? throw new ArgumentNullException(nameof(provider));


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


            while(_provider.Current.type == Parser.LexemeType.Space)
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
                token = Some(sb.ToString());

            return token;
        }
    }




}
