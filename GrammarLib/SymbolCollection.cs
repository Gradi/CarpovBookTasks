using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GrammarLib
{
    public class SymbolCollection : IReadOnlyCollection<Symbol>, IEquatable<SymbolCollection>
    {
        private readonly Symbol[] _symbols;

        public int Count => _symbols.Length;

        public SymbolCollection(IEnumerable<Symbol> symbols)
        {
            _symbols = symbols?.ToArray() ?? throw new ArgumentNullException(nameof(symbols));
        }

        public SymbolCollection(params Symbol[] symbols) : this((IEnumerable<Symbol>)symbols) {}

        public IEnumerator<Symbol> GetEnumerator() => ((IEnumerable<Symbol>)_symbols).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(SymbolCollection other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (ReferenceEquals(this, other))
                return true;
            if (Count != other.Count)
                return false;

            return this.Zip(other, (l, r) => l == r).All(i => i);
        }

        public override bool Equals(object obj) => obj is SymbolCollection symColl && Equals(symColl);

        public override int GetHashCode()
        {
            HashCode code = new HashCode();
            foreach (var symbol in _symbols)
            {
                code.Add(symbol);
            }
            return code.ToHashCode();
        }

        public override string ToString() => string.Join(string.Empty, this);

        public static bool operator==(SymbolCollection? left, SymbolCollection? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator!=(SymbolCollection? left, SymbolCollection? right) => !(left == right);
    }
}
