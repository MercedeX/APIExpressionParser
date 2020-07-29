using LanguageExt;
using Parser.Lexemes;

namespace Parser.Machines
{
    /// <summary>
    /// Absorbs the current input and moves pointer ahead 1 position
    /// </summary>
    public sealed class NullMachine: MachineBase
    {
        /// <inheritdoc />
        public NullMachine(ILexemeScanner scanner) : base(scanner)
        {
        }

        /// <inheritdoc />
        public override Option<Token> Get()
        {
            if (_scanner.IsSafeToRead)
                _scanner.Next();

            return Option<Token>.None;
        }


    }
}