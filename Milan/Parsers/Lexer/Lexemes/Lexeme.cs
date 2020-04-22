using System;

namespace Milan.Parsers.Lexer.Lexemes
{
#pragma warning disable 659,660,661
    public abstract class Lexeme : IEquatable<Lexeme>
#pragma warning restore 659,660,661
    {
        private string _name;

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

        public override string ToString() => _name;

        protected virtual bool InnerEquals(Lexeme other) => true;

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
