using Milan.Expressions.Enums;

namespace Milan.Expressions
{
    public static class ExpressionFactory
    {
        public static Expression Constant(Constant constant) => new ConstantExpression(constant);

        public static ComparisonExpression Comparison(ComparisonType type, Expression left, Expression right) => new ComparisonExpression(type, left, right);
        public static ComparisonExpression Less(Expression left, Expression right) => Comparison(ComparisonType.Less, left, right);
        public static ComparisonExpression LessEqual(Expression left, Expression right) => Comparison(ComparisonType.LessEqual, left, right);
        public static ComparisonExpression Equal(Expression left, Expression right) => Comparison(ComparisonType.Equal, left, right);
        public static ComparisonExpression NotEqual(Expression left, Expression right) => Comparison(ComparisonType.NotEqual, left, right);
        public static ComparisonExpression Greater(Expression left, Expression right) => Comparison(ComparisonType.Greater, left, right);
        public static ComparisonExpression GreaterEqual(Expression left, Expression right) => Comparison(ComparisonType.GreaterEqual, left, right);

        public static Expression Identifier(Identifier identifier) => new IdentifierExpression(identifier);

        public static Expression Read() => new ReadExpression();

        public static Expression Math(OperationType type, Expression left, Expression right) => new MathExpression(type, left, right);
        public static Expression Plus(Expression left, Expression right) => Math(OperationType.Plus, left, right);
        public static Expression Minus(Expression left, Expression right) => Math(OperationType.Minus, left, right);
        public static Expression Multiply(Expression left, Expression right) => Math(OperationType.Multiply, left, right);
        public static Expression Divide(Expression left, Expression right) => Math(OperationType.Divide, left, right);
        public static Expression Modulo(Expression left, Expression right) => Math(OperationType.Modulo, left, right);
    }
}
