using System;

namespace Milan
{
    public class Constant : IEquatable<Constant>
    {
        public int Value { get; }

        public Constant(int value)
        {
            Value = value;
        }

        public bool Equals(Constant other)
        {
            if (ReferenceEquals(other, null))
                throw new ArgumentNullException(nameof(other));
            return Value == other.Value;
        }

        public override bool Equals(object obj) => obj is Constant other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() =>  Value.ToString();

        public static bool operator==(Constant? left, Constant? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator!=(Constant? left, Constant? right) => !(left == right);
    }
}
