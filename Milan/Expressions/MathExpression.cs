using Milan.Expressions.Enums;

namespace Milan.Expressions
{
    public class MathExpression : BinaryExpression
    {
        public OperationType Type { get; }

        public MathExpression(OperationType type,  Expression left, Expression right) : base(left, right)
        {
            Type = type;
        }
    }
}
