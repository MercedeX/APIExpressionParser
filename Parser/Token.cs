namespace Parser
{
    public enum TokenType
    {
        Unknown = 0,
        Identifier =1,
        Operator =2,
        String = 3,
        Number = 4,
        ChildExpressionBegins =5,
        ChildExpressionEnds = 6
    };


    public readonly struct Token
    {
        public readonly TokenType type;
        public readonly string value;

        internal Token(TokenType type, string value) => (this.type, this.value) = (type, value);
        public void Deconstruct(out TokenType type, out string value) => (type, value) = (this.type, this.value);
    }
}
