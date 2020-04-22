using System.Linq;
using Milan.Parsers.Lexer.Lexemes;

namespace Milan.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerFirstLetter(this string str)
        {
            if (string.IsNullOrWhiteSpace(str) || char.IsLower(str[0]))
                return str;

            var chars = str.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }

        public static bool IsValidKeyword(this string str) => !string.IsNullOrWhiteSpace(str) && KeywordLexeme.AllKeywords.Contains(str);
    }
}
