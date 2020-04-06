using System;

namespace RomanNumbersParser
{
    public readonly struct RomanDigit : IEquatable<RomanDigit>, IComparable<RomanDigit>
    {
        public readonly char Symbol;
        public readonly int NumericValue;

        public RomanDigit(char  symbol, int numericValue)
        {
            Symbol = symbol;
            NumericValue = numericValue;
        }

        public bool Equals(RomanDigit other)
        {
            return Symbol == other.Symbol &&
                   NumericValue == other.NumericValue;
        }

        public int CompareTo(RomanDigit other) => NumericValue.CompareTo(other.NumericValue);

        public override bool Equals(object obj) => obj is RomanDigit digit && Equals(digit);

        public override int GetHashCode() => (Symbol, NumericValue).GetHashCode();

        public override string ToString() => Symbol.ToString();

        public static bool operator==(RomanDigit left, RomanDigit right) => left.Equals(right);
        public static bool operator!=(RomanDigit left, RomanDigit right) => !left.Equals(right);

        public static bool operator<(RomanDigit left, RomanDigit right) => left.CompareTo(right) < 0;
        public static bool operator<=(RomanDigit left, RomanDigit right) => left.CompareTo(right) <= 0;
        public static bool operator>(RomanDigit left, RomanDigit right) => left.CompareTo(right) > 0;
        public static bool operator>=(RomanDigit left, RomanDigit right) => left.CompareTo(right) >= 0;

        public static bool operator==(RomanDigit left, int number) => left.NumericValue == number;
        public static bool operator!=(RomanDigit left, int number) => left.NumericValue != number;

        public static bool operator<(RomanDigit left, int number) => left.NumericValue < number;
        public static bool operator<=(RomanDigit left, int number) => left.NumericValue <= number;
        public static bool operator>(RomanDigit left, int number) => left.NumericValue > number;
        public static bool operator>=(RomanDigit left, int number) => left.NumericValue >= number;
    }
}
