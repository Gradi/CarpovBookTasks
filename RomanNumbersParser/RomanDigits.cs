using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RomanNumbersParser
{
    public static class RomanDigits
    {
        /// <summary>
        /// One
        /// </summary>
        public static readonly RomanDigit I = new RomanDigit('I', 1);

        /// <summary>
        /// Five
        /// </summary>
        public static readonly RomanDigit V = new RomanDigit('V', 5);

        /// <summary>
        /// Ten
        /// </summary>
        public static readonly RomanDigit X = new RomanDigit('X', 10);

        /// <summary>
        /// Fifty
        /// </summary>
        public static readonly RomanDigit L = new RomanDigit('L', 50);

        /// <summary>
        /// One hundred
        /// </summary>
        public static readonly RomanDigit C = new RomanDigit('C', 100);

        /// <summary>
        /// Five hundred
        /// </summary>
        public static readonly RomanDigit D = new RomanDigit('D', 500);

        /// <summary>
        /// One thousand
        /// </summary>
        public static readonly RomanDigit M = new RomanDigit('M', 1000);

        public static readonly IReadOnlyCollection<RomanDigit> AllDigits = Array.AsReadOnly(new RomanDigit[]
        {
            I, V, X, L, C, D, M
        });

        public static readonly IReadOnlyDictionary<char, RomanDigit> RomanDigitsByChars =
            new ReadOnlyDictionary<char, RomanDigit>(new Dictionary<char, RomanDigit>
        {
            { I.Symbol, I },
            { V.Symbol, V },
            { X.Symbol, X },
            { L.Symbol, L },
            { C.Symbol, C },
            { D.Symbol, D },
            { M.Symbol, M }
        });

        public static bool IsValidRomanSymbol(char symbol) => RomanDigitsByChars.ContainsKey(symbol);
    }
}
