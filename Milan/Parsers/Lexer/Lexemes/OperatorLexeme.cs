using Milan.Expressions.Enums;

namespace Milan.Parsers.Lexer.Lexemes
{
    public class OperatorLexeme : Lexeme
    {
        public OperationType Type { get; }

        public OperatorLexeme(OperationType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type switch
            {
                OperationType.Plus => "+",
                OperationType.Minus => "-",
                OperationType.Multiply => "*",
                OperationType.Divide => "/",
                OperationType.Modulo => "%",
                var type => type.ToString()
            };
        }

        protected override bool InnerEquals(Lexeme other) => Type == ((OperatorLexeme)other).Type;

        protected override int InnerGetHashCode() => Type.GetHashCode();
    }
}
