namespace Parser.Lexemes
{
    public interface ILexemeSubScanner
    {
        ILexemeScanner GetSubScanner();
    }
}