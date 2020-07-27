using LanguageExt;

namespace Parser.Lexemes
{
    public interface ILexemeParent
    {
        Option<ILexemeProvider> GetChild();
    }
}