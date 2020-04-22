using System;
using System.Collections.Generic;
using System.Linq;
using Milan.Extensions;
using Milan.Parsers.Lexer.Enums;

namespace Milan.Parsers.Lexer.Lexemes
{
    public class KeywordLexeme : Lexeme
    {
        public static readonly IReadOnlyCollection<string> AllKeywords;

        public static readonly IReadOnlyDictionary<string, KeywordType> KeywordMap;

        public KeywordType Type { get; }

        static KeywordLexeme()
        {
            KeywordMap = ((KeywordType[])Enum.GetValues(typeof(KeywordType)))
                .ToDictionary(k => k.ToString().ToLowerFirstLetter(),  k => k);
            AllKeywords = Array.AsReadOnly(KeywordMap.Keys.ToArray());
        }

        public KeywordLexeme(KeywordType type)
        {
            Type = type;
        }

        public override int GetHashCode() => Type.GetHashCode();

        public override string ToString() => Type.ToString().ToLowerFirstLetter();

        protected override bool InnerEquals(Lexeme other) => Type == ((KeywordLexeme)other).Type;
    }
}
