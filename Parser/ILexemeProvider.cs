using LanguageExt;

namespace Parser
{
    public interface ILexemeProvider
    {
        Option<char> Future { get; }
        char Current { get; }
        bool Next();
        void Reset();
        bool Back();
    }
}