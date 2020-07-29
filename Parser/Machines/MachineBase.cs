using System;

using LanguageExt;

using Parser.Lexemes;

namespace Parser.Machines
{
    public abstract class MachineBase :IMachine
    {
        protected ILexemeScanner _scanner;

        protected MachineBase(ILexemeScanner scanner)
        {
            var tmp = scanner ?? throw new ArgumentNullException(nameof(scanner));

            if(tmp is ILexemeSubScanner child)
            {
                _scanner = child.GetSubScanner();
            }
            else
            {
                _scanner = scanner;
            }
        }

        /// <inheritdoc />
        public abstract Option<Token> Get();

        /// <inheritdoc />
        public void Done()
        {
            if(_scanner is ILexemeTransaction tran)
            {
                tran.Commit();
            }
            else
            {
                throw new InvalidOperationException("The provided scanner does not support ILexemeTransaction");
            }
        }


    }
}