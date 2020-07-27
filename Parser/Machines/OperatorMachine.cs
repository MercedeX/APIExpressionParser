using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using Parser.Lexemes;
using static LanguageExt.Prelude;

namespace Parser.Machines
{
    public enum Operators { AND =1, OR, NOT, Equals, NotEquals, LessThan, LessThanOrEqual, GreaterThan, GreaterThanOrEqual };


    public class OperatorMachine
    {
        readonly ILexemeProvider _provider;

        public OperatorMachine(ILexemeProvider provider) => _provider = provider;

        
        enum States { Start, A, AN, O, N, NO, DoubleOp, Completion, Finished, Error };


        /// <summary>
        /// Gets the operator or None if no operator found. 
        /// </summary>
        /// <returns></returns>
        public Option<Operators> Get()
        {
            var token = Option<Operators>.None;

            while(_provider.Current.type == LexemeType.Space)
            {
                if(!_provider.Next())  // its not safe to read further, quit now!
                    return token;
            }

            var current = States.Start;
            var sb = new StringBuilder();

            do
            {
                var (dt, type) = _provider.Current;
                var next = States.Error;

                switch(current)
                {
                    case States.Start:
                        //Determine what it is
                        if(dt == 'A' || dt == 'a') next = States.A;
                        else if(dt == 'O' || dt == 'o') next = States.O;
                        else if(dt == 'N' || dt == 'n') next = States.N;
                        else if(dt == '>' || dt == '<') next = States.DoubleOp;
                        else if(dt == '=') next = States.Completion;
                        else next = States.Error;
                        break;

                    case States.A:
                        if(dt == 'N' || dt == 'n') next = States.AN;
                        else next = States.Error;
                        break;

                    case States.AN:
                        if(dt == 'D' || dt == 'd') next = States.Completion;
                        break;

                    case States.O:
                        if(dt == 'R' || dt == 'r') next = States.Completion;
                        else next = States.Error;
                        break;

                    case States.N:
                        if(dt == 'O' || dt == 'o') next = States.NO;
                        else next = States.Error;
                        break;

                    case States.NO:
                        if(dt == 'T' || dt == 't') next = States.Completion;
                        else next = States.Error;
                        break;

                    case States.Completion:
                        if(type == LexemeType.Digit || type == LexemeType.Space || type == LexemeType.Punctuation ||
                            (type == LexemeType.Symbol && (dt == '(' || dt == ')')))
                            next = States.Finished;
                        else next = States.Error;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                current = next;
                if(next < States.Finished)
                    sb.Append(dt);
            } 
            while(current != States.Error && current!= States.Finished  && _provider.Next());

            if(current == States.Finished)
                token = GetToken(sb.ToString());

            return token;


            Option<Operators> GetToken(string text)
            {
                var table = new Dictionary<string, Operators>
                {
                    { "and", Operators.AND },
                    { "or", Operators.OR },
                    { "not", Operators.NOT },
                    { "=", Operators.Equals },
                    { "<", Operators.LessThan },
                    { "<=", Operators.LessThanOrEqual },
                    { ">", Operators.GreaterThan },
                    { ">=", Operators.GreaterThanOrEqual },
                    { "<>", Operators.NotEquals }
                };

                var t1 = text.ToLower();
                if(table.ContainsKey(t1))
                    return Some(table[t1]);


                return Option<Operators>.None;
            }
        }
    }
}