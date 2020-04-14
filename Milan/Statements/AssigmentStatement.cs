using Milan.Expressions;

namespace Milan.Statements
{
    public class AssigmentStatement : Statement
    {
        public Identifier Target { get; }

        public Expression Value { get; }

        public AssigmentStatement(Identifier target, Expression value)
        {
            Target = target;
            Value = value;
        }
    }
}
