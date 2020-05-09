using Milan.Expressions.Enums;

namespace Milan.Parsers.Lexer.Lexemes
{
    public class ComparisonLexeme : Lexeme
    {
        public ComparisonType Type { get; }

        public ComparisonLexeme(ComparisonType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type switch
            {
                ComparisonType.Less => "<",
                ComparisonType.LessEqual => "<=",
                ComparisonType.Equal => "=",
                ComparisonType.Greater => ">",
                ComparisonType.GreaterEqual => ">=",
                var type => type.ToString()
            };
        }

        protected override bool InnerEquals(Lexeme other) => Type == ((ComparisonLexeme)other).Type;

        protected override int InnerGetHashCode() => Type.GetHashCode();
    }
}
