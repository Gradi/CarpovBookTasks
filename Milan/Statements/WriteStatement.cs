using Milan.Expressions;

namespace Milan.Statements
{
    public class WriteStatement : Statement
    {
        public Expression Value { get; }

        public WriteStatement(Expression value)
        {
            Value = value;
        }
    }
}
