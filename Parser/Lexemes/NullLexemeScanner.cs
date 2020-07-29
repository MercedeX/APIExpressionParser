using System;

using LanguageExt;

namespace Parser.Lexemes
{
    /// <summary>
    /// Implements the <see langword="null"/>pattern, so that instead of returning null, we return an instance of this class.
    /// all the methods are unusable and do nothing. Calling Current will result in an InvalidOperationException.
    /// Calling a child from NullLexemeScanner will return the instance of the same class. 
    /// </summary>
    public sealed class NullLexemeScanner :ILexemeScanner, ILexemeSubScanner, ILexemeTransaction
    {
        NullLexemeScanner() { }
        /// <inheritdoc />
        public Option<Lexeme> LookAhead => Option<Lexeme>.None;
        /// <inheritdoc />
        public Lexeme Current => throw new InvalidOperationException("There is no more input available to scan");
        /// <inheritdoc />
        public bool Next() => false;
        /// <inheritdoc />
        public void Reset() { }
        /// <inheritdoc />
        public bool Back() => false;
        /// <inheritdoc />
        public bool IsSafeToRead => false;


        public static ILexemeScanner Default => new NullLexemeScanner();
        /// <inheritdoc />
        public ILexemeScanner GetSubScanner() => this;
        /// <inheritdoc />
        public void Commit() { }
    }
}