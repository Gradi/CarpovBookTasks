using System.Collections.Generic;
using Milan.Expressions;

namespace Milan.Statements
{
    public class WhileStatement : Statement
    {
        public ComparisonExpression Condition { get; }

        public IReadOnlyCollection<Statement> Statements { get; }

        public WhileStatement(ComparisonExpression condition, IReadOnlyCollection<Statement> statements)
        {
            Condition = condition;
            Statements = statements;
        }
    }
}
