using System;
using Milan.Expressions.Enums;
using Milan.Extensions;
using Milan.Parsers.Lexer.Enums;

namespace Milan.Parsers.Lexer.Lexemes
{
    public static class LexemeFactory
    {
        public static Lexeme Assigment() => new AssigmentLexeme();

        public static Lexeme Comparison(ComparisonType type) => new ComparisonLexeme(type);

        public static Lexeme Constant(int value) => new ConstantLexeme(value);

        public static Lexeme Identifier(string value) => new IdentifierLexeme(value);

        public static Lexeme Keyword(KeywordType type) => new KeywordLexeme(type);

        public static Lexeme Keyword(string keyword)
        {
            if (!keyword.IsValidKeyword())
            {
                throw new  ArgumentOutOfRangeException(nameof(keyword), $"String \"{keyword}\" is not valid keyword.");
            }
            return Keyword(KeywordLexeme.KeywordMap[keyword]);
        }

        public static Lexeme LeftBracket() => new LeftBracketLexeme();
        public static Lexeme RightBracket() => new RightBracketLexeme();

        public static Lexeme Operator(OperationType type) => new OperatorLexeme(type);

        public static Lexeme Semicolon() => new SemicolonLexeme();
    }
}
