using Milan.Extensions;
using Milan.Parsers.Lexer;
using Milan.Parsers.Lexer.Enums;
using Milan.Parsers.Lexer.Lexemes;
using Milan.Statements;

namespace Milan.Parsers.Syntax
{
    public class ProgramPatternRecognizer : BasePatternRecognizer<Program>
    {
        public override Result<Program> Recognize(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return emptyResult;

            if (!(lexemes.Current is KeywordLexeme keywordLexeme))
                return Fail($"First lexeme is not \"{nameof(KeywordLexeme)}\", but \"{lexemes.Current?.GetType()}\"");

            if (keywordLexeme.Type != KeywordType.Begin)
                return Fail($"Expected program to start with \"{KeywordType.Begin}\" keyword, but actual keyword is \"{keywordLexeme}\"");

            var statementsCollection = recognizers.GetRecognizer<StatementCollection>().Recognize(lexemes, programBuilder, recognizers);
            if (!statementsCollection.HasValue)
                return Fail(statementsCollection);

            if (!lexemes.MoveNext())
                return Fail($"Expected \"{KeywordType.End}\" keyword, but found empty enumerator.");

            if (!(lexemes.Current is KeywordLexeme lastKeywordLexeme))
                return Fail($"Expected \"{nameof(KeywordLexeme)}\" to be the last lexeme, but found \"{lexemes.Current}\".");

            if (lastKeywordLexeme.Type != KeywordType.End)
                return Fail($"Expected last keyword to be \"{KeywordType.End}\", but found \"{lastKeywordLexeme}\".");

            if (lexemes.MoveNext())
                return Fail($"Got \"{lastKeywordLexeme}\" and some lexemes after final keyword.");

            programBuilder.Statements.AddRange(statementsCollection.Value);

            return programBuilder.Build();
        }
    }
}
