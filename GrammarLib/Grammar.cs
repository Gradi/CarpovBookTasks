using System;
using System.Collections.Generic;
using System.Linq;
using GrammarLib.Extensions;

namespace GrammarLib
{
    public class Grammar : IEquatable<Grammar>
    {
        public NonTerminal StartSymbol { get; }

        public SymbolCollection Terminals { get; }

        public SymbolCollection NonTerminals { get; }

        public SymbolCollection AllSymbols { get; }

        public IReadOnlyCollection<Production> Productions { get; }

        internal Grammar
            (
                NonTerminal startSymbol,
                IReadOnlyCollection<Production> productions
            )
        {
            StartSymbol = startSymbol ?? throw new ArgumentNullException(nameof(startSymbol));
            Productions = productions ?? throw new ArgumentNullException(nameof(productions));

            var allSymbols = productions
                .Select(p => p.Left.Concat(p.Right))
                .Flatten()
                .Append(StartSymbol)
                .Distinct()
                .ToList();
            Terminals = new SymbolCollection(allSymbols.OfType<Terminal>());
            NonTerminals = new SymbolCollection(allSymbols.OfType<NonTerminal>());
            AllSymbols = new SymbolCollection(allSymbols);

            if (!NonTerminals.Contains(startSymbol))
                throw new ArgumentException("Collection of non terminals doesn't contain start symbol/non terminal.");
        }

        public bool Equals(Grammar other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (ReferenceEquals(this, other))
                return true;
            if (Productions.Count != other.Productions.Count)
                return false;

            return StartSymbol == other.StartSymbol &&
                   Terminals == other.Terminals &&
                   NonTerminals == other.NonTerminals &&
                   Productions.Zip(other.Productions, (l, r) => l == r).All(i => i);
        }

        public override bool Equals(object obj) => obj is Grammar grammar && Equals(grammar);

        public override int GetHashCode()
        {
            HashCode code = new HashCode();
            code.Add(StartSymbol);
            code.Add(Terminals);
            code.Add(NonTerminals);
            foreach (var production in Productions)
            {
                code.Add(production);
            }
            return code.ToHashCode();
        }
    }
}
