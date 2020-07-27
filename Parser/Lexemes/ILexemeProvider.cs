using LanguageExt;

namespace Parser.Lexemes
{


    public interface ILexemeProvider
    {
        /// <summary>
        /// Returns future lexeme or None if pointer is past the valid position
        /// </summary>
        Option<Lexeme> LookAhead { get; }

        /// <summary>
        /// Returns a valid lexeme.
        /// if pointer is MaxPosition+1 it throws exception
        /// </summary>
        Lexeme Current { get; }
        /// <summary>
        /// Takes 1 position forward. If this is the max position, it takes it MaxPosition+1. At this point no read can be performed
        /// </summary>
        /// <returns></returns>
        bool Next();
        /// <summary>
        /// Takes the lexeme to the begining, irrespective of the valid or invalid inout
        /// </summary>
        void Reset();
        /// <summary>
        /// Takes 1 position back, if pointer is at 0, it stays at 0
        /// </summary>
        /// <returns></returns>
        bool Back();

        bool IsSafeToRead { get; }
        bool IsConsumed { get; }
    }
}