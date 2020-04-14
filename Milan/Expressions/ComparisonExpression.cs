using Milan.Expressions.Enums;

namespace Milan.Expressions
{
    public class ComparisonExpression : BinaryExpression
    {
        public ComparisonType Type { get; }

        public ComparisonExpression(ComparisonType type, Expression left, Expression right) : base(left, right)
        {
            Type = type;
        }
    }
}
