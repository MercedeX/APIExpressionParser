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

        internal Token(TokenType type, string value) => (this.type, this.value, this.@operator) = (type, value, Operators.Unknown);
        internal Token(TokenType type, Operators value) => (this.type, this.value, this.@operator) = (type, string.Empty, value);
        
        public void Deconstruct(out TokenType type, out string value) => (type, value) = (this.type, this.value);
        public void Deconstruct(out TokenType type, out Operators op) => (type, op) = (this.type, this.@operator);

    }
}
