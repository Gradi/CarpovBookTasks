using Milan.Expressions;

namespace Milan.Statements
{
    public class IfStatement : Statement
    {
        public ComparisonExpression Condition { get; }

        public StatementCollection IfTrueStatements { get; }

        public StatementCollection? ElseStatements { get; }

        public IfStatement
            (
                ComparisonExpression condition,
                StatementCollection ifTrueStatements,
                StatementCollection? elseStatements = null
            )
        {
            Condition = condition;
            IfTrueStatements = ifTrueStatements;
            ElseStatements = elseStatements;
        }
    }
}
