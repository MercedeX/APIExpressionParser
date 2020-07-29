using Parser.Machines;

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
        public readonly Operators @operator;
        public readonly long @long;
        public readonly double @decimal;


        internal Token(TokenType type, string value) => (this.type, this.value, this.@operator, this.@long, this.@decimal) = (type, value, Operators.Unknown, 0, 0);
        internal Token(TokenType type, Operators value) => (this.type, this.value, this.@operator, this.@long, this.@decimal) = (type, string.Empty, value, 0, 0.0);

        /// <inheritdoc />
        public Token(TokenType type, long @long) : this()
        {
            this.type = type;
            this.@long = @long;
        }
        public Token(TokenType type, double data) : this()
        {
            this.type = type;
            this.@decimal = data;
        }

        public void Deconstruct(out TokenType type, out string value) => (type, value) = (this.type, this.value);
        public void Deconstruct(out TokenType type, out Operators op) => (type, op) = (this.type, this.@operator);
        public void Deconstruct(out TokenType type, out long num) => (type, num) = (this.type, this.@long);
        public void Deconstruct(out TokenType type, out double num) => (type, num) = (this.type, this.@decimal);
    }
}
