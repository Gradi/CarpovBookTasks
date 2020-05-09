using System;
using System.Collections;
using System.Collections.Generic;

namespace Milan.Statements
{
    public class StatementCollection : IReadOnlyCollection<Statement>
    {
        private readonly IReadOnlyCollection<Statement> _statements;

        public int Count => _statements.Count;

        public StatementCollection(IReadOnlyCollection<Statement> statements)
        {
            _statements = statements ?? throw new ArgumentNullException(nameof(statements));
        }

        public IEnumerator<Statement> GetEnumerator() => _statements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
