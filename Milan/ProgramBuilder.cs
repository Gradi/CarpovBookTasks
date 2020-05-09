using System.Collections.Generic;
using System.Linq;
using Milan.Statements;

namespace Milan
{
    public class ProgramBuilder
    {
        public ICollection<Constant> Constants { get; }

        public ICollection<Identifier> Identifiers { get; }

        public ICollection<Statement> Statements { get; }

        public ProgramBuilder()
        {
            Constants = new List<Constant>();
            Identifiers = new List<Identifier>();
            Statements = new List<Statement>();
        }

        public Constant GetOrAddConstant(Constant constant)
        {
            var existing = Constants.FirstOrDefault(c => c == constant);
            if (existing == null)
            {
                existing = constant;
                Constants.Add(constant);
            }
            return existing;
        }

        public Constant GetOrAddConstant(int value) => GetOrAddConstant(new Constant(value));

        public Identifier GetOrAddIdentifier(Identifier identifier)
        {
            var existing = Identifiers.FirstOrDefault(i => i == identifier);
            if (existing == null)
            {
                existing = identifier;
                Identifiers.Add(identifier);
            }
            return existing;
        }

        public Identifier GetOrAddIdentifier(string name) => GetOrAddIdentifier(new Identifier(name));

        public Program Build()
        {
            return new Program
                (
                    Constants.ToArray(),
                    Identifiers.ToArray(),
                    new StatementCollection(Statements.ToArray())
                );
        }
    }
}
