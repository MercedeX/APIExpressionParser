using LanguageExt;

namespace Parser.Machines
{
    public interface IMachine
    {
        Option<Token> Get();
        void Done();
    }
}