namespace Parser.Lexemes
{
    public enum LexemeType :byte
    {
        Unknown = 0,
        Alpha = 10,
        Digit =20,
        Symbol = 30,
        Space = 40,
        Punctuation =50
    }


    public readonly struct Lexeme
    {
        public readonly char data;
        public readonly LexemeType type;

        internal Lexeme(char data, LexemeType type) : this() => (this.data, this.type) = (data, type);
        public void Deconstruct(out char dt, out LexemeType type)
        {
            dt = this.data;
            type = this.type;
        }
    }
}