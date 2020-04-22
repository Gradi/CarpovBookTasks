using Milan.Expressions.Enums;

namespace Milan.Expressions
{
    public static class ExpressionFactory
    {
        public static Expression Constant(Constant constant) => new ConstantExpression(constant);

        public static Expression Less(Expression left, Expression right) => new ComparisonExpression(ComparisonType.Less, left, right);
        public static Expression LessEqual(Expression left, Expression right) => new ComparisonExpression(ComparisonType.LessEqual, left, right);
        public static Expression Equal(Expression left, Expression right) => new ComparisonExpression(ComparisonType.Equal, left, right);
        public static Expression NotEqual(Expression left, Expression right) => new ComparisonExpression(ComparisonType.NotEqual, left, right);
        public static Expression Greater(Expression left, Expression right) => new ComparisonExpression(ComparisonType.Greater, left, right);
        public static Expression GreaterEqual(Expression left, Expression right) => new ComparisonExpression(ComparisonType.GreaterEqual, left, right);

        public static Expression Identifer(Identifier identifier) => new IdentiferExpression(identifier);

        public static Expression Read() => new ReadExpression();

        public static Expression Plus(Expression left, Expression right) => new MathExpression(OperationType.Plus, left, right);
        public static Expression Minus(Expression left, Expression right) => new MathExpression(OperationType.Minus, left, right);
        public static Expression Multiply(Expression left, Expression right) => new MathExpression(OperationType.Multiply, left, right);
        public static Expression Divide(Expression left, Expression right) => new MathExpression(OperationType.Divide, left, right);
        public static Expression Modulo(Expression left, Expression right) => new MathExpression(OperationType.Modulo, left, right);
    }
}
