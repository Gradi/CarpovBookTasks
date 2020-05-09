using System;

namespace Milan.Parsers.Lexer.Lexemes
{
    public abstract class Lexeme : IEquatable<Lexeme>
    {
        private readonly string _name;

        protected Lexeme(string? name = null)
        {
            _name = name ?? string.Empty;
        }

        public bool Equals(Lexeme other)
        {
            if (ReferenceEquals(other, null))
                throw new ArgumentNullException(nameof(other));
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            return InnerEquals(other);
        }

        public override bool Equals(object obj) => obj is Lexeme other && Equals(other);

        public override int GetHashCode()
        {
            HashCode code = new HashCode();
            code.Add(_name);
            code.Add(InnerGetHashCode());
            return code.ToHashCode();
        }

        public override string ToString() => _name;

        protected virtual bool InnerEquals(Lexeme other) => true;

        protected abstract int InnerGetHashCode();

        public static bool operator==(Lexeme left, Lexeme right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator!=(Lexeme left, Lexeme right) => !(left == right);
    }
}
