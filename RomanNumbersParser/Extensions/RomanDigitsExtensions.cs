namespace RomanNumbersParser.Extensions
{
    internal static class RomanDigitsExtensions
    {
        public static bool IsPowerOf10(this RomanDigit digit)
        {
            return digit == RomanDigits.I ||
                   digit == RomanDigits.X ||
                   digit == RomanDigits.C ||
                   digit == RomanDigits.M;
        }

        public static bool IsOneFifthOrTenthOf(this RomanDigit left, RomanDigit right)
        {
            return (right.NumericValue / 5) == left.NumericValue ||
                   (right.NumericValue / 10) == left.NumericValue;
        }
    }
}
