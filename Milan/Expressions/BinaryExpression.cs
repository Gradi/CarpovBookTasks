namespace Milan.Expressions
{
    public abstract class BinaryExpression : Expression
    {
        public Expression LeftOperand { get; }

        public Expression RightOperand { get; }

        protected BinaryExpression(Expression leftOperand, Expression rightOperand)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }
    }
}
