using System;

namespace GrammarLib
{
    public class Production : IEquatable<Production>
    {
        public SymbolCollection Left { get; }

        public SymbolCollection Right { get; }

        public Production(SymbolCollection left, SymbolCollection right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public bool Equals(Production other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (ReferenceEquals(this, other))
                return true;

            return Left == other.Left &&
                   Right == other.Right;
        }

        public override bool Equals(object obj) => obj is Production prod && Equals(prod);

        public override int GetHashCode() => (Left, Right).GetHashCode();

        public override string ToString() => $"{Left} -> {Right}";

        public static bool operator==(Production? left, Production? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator!=(Production? left, Production? right) => !(left == right);
    }
}
