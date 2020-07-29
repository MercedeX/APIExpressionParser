using Parser.Lexemes;

namespace Parser
{
    public class ExpressionParser
    {
        readonly ILexemeScanner _provider;


        public ExpressionParser(in string input)
        {
            _provider = new LexemeScanner(input);
        }

        public void Parse()
        {

        }
    }

    public class DetermineMachine
    {

    }
}
