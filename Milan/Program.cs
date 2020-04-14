using System;
using System.Collections.Generic;
using Milan.Statements;

namespace Milan
{
    public class Program
    {
        public IReadOnlyCollection<Constant> Constants { get; }

        public IReadOnlyCollection<Identifier> Identifiers { get; }

        public IReadOnlyCollection<Statement> Statements { get; }

        public Program
            (
                IReadOnlyCollection<Constant> constants,
                IReadOnlyCollection<Identifier> identifiers,
                IReadOnlyCollection<Statement> statements
            )
        {
            Constants = constants ?? throw new ArgumentNullException(nameof(constants));
            Identifiers = identifiers ?? throw new ArgumentNullException(nameof(identifiers));
            Statements = statements ?? throw new ArgumentNullException(nameof(statements));
        }
    }
}
