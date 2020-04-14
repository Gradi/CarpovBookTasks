namespace Milan.Expressions
{
    public class ConstantExpression : Expression
    {
        public Constant Constant { get; }

        public ConstantExpression(Constant constant)
        {
            Constant = constant;
        }
    }
}
