using System;

namespace GrammarLib
{
    public abstract class Symbol : IEquatable<Symbol>
    {
        public string Value { get; }

        protected Symbol(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool Equals(Symbol symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            if (ReferenceEquals(this, symbol))
                return true;
            return GetType() == symbol.GetType() &&
                   Value == symbol.Value;
        }

        public override bool Equals(object obj) => obj is Symbol symbol && Equals(symbol);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

        public static bool operator==(Symbol left, Symbol right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator!=(Symbol left, Symbol right) => !(left == right);
    }
}
