using Milan.Expressions;

namespace Milan.Statements
{
    public class WhileStatement : Statement
    {
        public ComparisonExpression Condition { get; }

        public StatementCollection Statements { get; }

        public WhileStatement(ComparisonExpression condition, StatementCollection statements)
        {
            Condition = condition;
            Statements = statements;
        }
    }
}
