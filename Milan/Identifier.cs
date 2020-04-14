using System;

namespace Milan
{
    public class Identifier : IEquatable<Identifier>
    {
        public string Name { get; }

        public Identifier(string name)
        {
            Name = name;
        }

        public bool Equals(Identifier other)
        {
            if (ReferenceEquals(other, null))
                throw new ArgumentNullException(nameof(other));
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj) => obj is Identifier other && Equals(other);

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;

        public static bool operator==(Identifier? left, Identifier? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator!=(Identifier? left, Identifier? right) => !(left == right);
    }
}
