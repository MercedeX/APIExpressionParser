using System;
using System.Collections.Generic;
using System.Text;

using LanguageExt;

using Parser.Lexemes;

namespace Parser.Machines
{
    public enum Operators { Unknown = 0, AND =1, OR, NOT, Equals, NotEquals, LessThan, LessThanOrEqual, GreaterThan, GreaterThanOrEqual };



    public class OperatorMachine :MachineBase
    {
        public OperatorMachine(ILexemeScanner provider) : base(provider) { }


        enum States { Start, A, AN, O, N, NO, DoubleOp, Completion, Finished, Error };

        /// <summary>
        /// Gets the operator or None if no operator found. 
        /// </summary>
        /// <returns></returns>
        public override Option<Token> Get()
        {
            var token = Option<Token>.None;

            if(!_scanner.IsSafeToRead)
            {
                return token;
            }

            var current = States.Start;
            var sb = new StringBuilder();

            do
            {

                var (dt, type) = _scanner.Current;
                var next = States.Error;

                switch(current)
                {
                    case States.Start:
                        //Determine what it is
                        if(dt == 'A' || dt == 'a')
                        {
                            next = States.A;
                        }
                        else if(dt == 'O' || dt == 'o')
                        {
                            next = States.O;
                        }
                        else if(dt == 'N' || dt == 'n')
                        {
                            next = States.N;
                        }
                        else if (dt == '>' || dt == '<') next = States.DoubleOp;
                        else if(dt == '=')
                        {
                            next = States.Completion;
                        }
                        else
                        {
                            next = States.Error;
                        }

                        break;

                    case States.A:
                        if(dt == 'N' || dt == 'n')
                        {
                            next = States.AN;
                        }
                        else
                        {
                            next = States.Error;
                        }

                        break;

                    case States.AN:
                        if(dt == 'D' || dt == 'd')
                        {
                            next = States.Completion;
                        }

                        break;

                    case States.O:
                        if(dt == 'R' || dt == 'r')
                        {
                            next = States.Completion;
                        }
                        else
                        {
                            next = States.Error;
                        }

                        break;

                    case States.N:
                        if(dt == 'O' || dt == 'o')
                        {
                            next = States.NO;
                        }
                        else
                        {
                            next = States.Error;
                        }

                        break;

                    case States.NO:
                        if(dt == 'T' || dt == 't')
                        {
                            next = States.Completion;
                        }
                        else
                        {
                            next = States.Error;
                        }

                        break;

                    case States.Completion:
                        if(type != LexemeType.Alpha)
                        {
                            next = States.Finished;
                        }
                        else
                        {
                            next = States.Error;
                        }

                        break;

                    default:
                        next = States.Error;
                        break;
                }
                current = next;
                if(next < States.Finished)
                {
                    sb.Append(dt);
                }

                if(current != States.Error && current != States.Finished)
                {
                    _scanner.Next();
                }
            }
            while(current != States.Error && current != States.Finished && _scanner.IsSafeToRead);

            if(current == States.Finished)
            {
                token = GetToken(sb.ToString().ToUpperInvariant());
            }
            else if(current == States.Error)
            {
                _scanner.Back();
            }

            return token;


            Option<Token> GetToken(string text)
            {
                var table = new Dictionary<string, Operators>
                {
                    { "AND", Operators.AND },
                    { "OR", Operators.OR },
                    { "NOT", Operators.NOT },
                    { "=", Operators.Equals },
                    { "<", Operators.LessThan },
                    { "<=", Operators.LessThanOrEqual },
                    { ">", Operators.GreaterThan },
                    { ">=", Operators.GreaterThanOrEqual },
                    { "<>", Operators.NotEquals }
                };

                if(table.ContainsKey(text))
                {
                    return new Token(TokenType.Operator, table[text]);
                }

                return Option<Token>.None;
            }
        }

    }
}