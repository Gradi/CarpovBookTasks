using System.Collections.Generic;
using Milan.Expressions;

namespace Milan.Statements
{
    public class IfStatement : Statement
    {
        public ComparisonExpression Condition { get; }

        public IReadOnlyCollection<Statement> IfTrueStatements { get; }

        public IReadOnlyCollection<Statement>? ElseStatements { get; }

        public IfStatement
            (
                ComparisonExpression condition,
                IReadOnlyCollection<Statement> ifTrueStatements,
                IReadOnlyCollection<Statement>? elseStatements = null
            )
        {
            Condition = condition;
            IfTrueStatements = ifTrueStatements;
            ElseStatements = elseStatements;
        }
    }
}
