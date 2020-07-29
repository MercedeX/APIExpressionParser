using System;
using System.Collections.Generic;
using System.Text;
using Parser.Lexemes;

namespace Parser
{
    public class ExpressionParser
    {
        readonly ILexemeProvider _provider;


        public ExpressionParser(in string input)
        {
            _provider = new LexemeProvider(input);
        }
    }
}
