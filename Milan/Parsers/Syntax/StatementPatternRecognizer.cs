using Milan.Expressions;
using Milan.Parsers.Lexer;
using Milan.Parsers.Lexer.Enums;
using Milan.Parsers.Lexer.Lexemes;
using Milan.Statements;

namespace Milan.Parsers.Syntax
{
    public class StatementPatternRecognizer : BasePatternRecognizer<Statement>
    {
        // I got a problem: Milan doesn't require to always write semicolons (but if you type several statements you must
        // put semicolons between them). So code like
        // begin
        //    write(123);
        // end
        // breaks parsing because of that last optional semicolon due to StatementCollectionPatternRecognizer finds semicolon and tries
        // to parse next statement which is missing.
        // That is why if i can't parse statement from the start i put lexeme back and return success(null).
        // StatementCollectionPatternRecognizer checks that and do not fail parsing allowing recognizers above to continue
        // parsing.
        public override Result<Statement> Recognize(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return emptyResult;

            if (lexemes.Current is IdentifierLexeme)
                return ParseAssigmentStatement(lexemes, programBuilder, recognizers);

            if (lexemes.Current is KeywordLexeme keyword)
            {
                switch (keyword.Type)
                {
                    case KeywordType.While: return ParseWhileStatement(lexemes, programBuilder, recognizers);
                    case KeywordType.If: return ParseIfStatement(lexemes, programBuilder, recognizers);
                    case KeywordType.Write: return ParseWriteStatement(lexemes, programBuilder, recognizers);
                }
            }

            lexemes.Return();
            return null!;
        }

        private Result<Statement> ParseAssigmentStatement(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            var identifer = new Identifier(((IdentifierLexeme)lexemes.Current).Name);

            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return emptyResult;

            if (!(lexemes.Current is AssigmentLexeme))
                return Fail($"Can't parse assigment statement: Expected \"{nameof(AssigmentLexeme)}\", but found \"{lexemes.Current}\".");

            var expression = recognizers.GetRecognizer<Expression>().Recognize(lexemes, programBuilder, recognizers);
            if (!expression.HasValue)
                return Fail($"Can't parse assigment statement: {expression.Error}");

            return new AssigmentStatement(programBuilder.GetOrAddIdentifier(identifer), expression.Value);
        }

        private Result<Statement> ParseWhileStatement(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            var comparisonExpression = recognizers.GetRecognizer<ComparisonExpression>().Recognize(lexemes, programBuilder, recognizers);
            if (!comparisonExpression.HasValue)
                return Fail($"Can't parse while statement: {comparisonExpression.Error}");

            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return emptyResult;

            if (!(lexemes.Current is KeywordLexeme doKeyword))
                return Fail($"Can't parse while statement: Expected \"{nameof(KeywordLexeme)}\", but found \"{lexemes.Current}\".");
            if (doKeyword.Type != KeywordType.Do)
                return Fail($"Can't parse while statement: Expected \"{KeywordType.Do}\" keyword, but found \"{doKeyword}\".");

            var statements = recognizers.GetRecognizer<StatementCollection>().Recognize(lexemes, programBuilder, recognizers);
            if (!statements.HasValue)
                return Fail($"Can't parse while body statements: {statements.Error}");

            if (IsEmptyEnumerator(lexemes, out emptyResult))
                return emptyResult;

            if (!(lexemes.Current is KeywordLexeme odKeyword))
                return Fail($"Can't parse while statement: Expected \"{nameof(KeywordLexeme)}\" keyword, but found \"{lexemes.Current}\".");
            if (odKeyword.Type != KeywordType.Od)
                return Fail($"Can't parse while statement: Expected \"{KeywordType.Od}\" keyword, but found \"{odKeyword}\".");

            return new WhileStatement(comparisonExpression.Value, statements.Value);
        }

        private Result<Statement> ParseIfStatement(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            var comparisonExpression = recognizers.GetRecognizer<ComparisonExpression>().Recognize(lexemes, programBuilder, recognizers);
            if (!comparisonExpression.HasValue)
                return Fail(comparisonExpression);

            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return Fail(emptyResult);

            if (!(lexemes.Current is KeywordLexeme thenKeyword))
                return Fail($"Can't parse if statement: Expected \"{nameof(KeywordLexeme)}\", but found \"{lexemes.Current}\".");
            if (thenKeyword.Type != KeywordType.Then)
                return Fail($"Can't parse if statement: Expected \"{KeywordType.Then}\", but found \"{thenKeyword}\".");

            var statements = recognizers.GetRecognizer<StatementCollection>().Recognize(lexemes, programBuilder, recognizers);
            if (!statements.HasValue)
                return Fail(statements);

            if (IsEmptyEnumerator(lexemes, out emptyResult))
                return Fail(emptyResult);
            if (!(lexemes.Current is KeywordLexeme keywordLexeme))
                return Fail($"Can't parse if statement: Expected \"{nameof(KeywordLexeme)}\", but found \"{lexemes.Current}\".");

            switch (keywordLexeme.Type)
            {
                case KeywordType.Fi: return new IfStatement(comparisonExpression.Value, statements.Value);
                case KeywordType.Else:
                    var elseStatements = recognizers.GetRecognizer<StatementCollection>().Recognize(lexemes, programBuilder, recognizers);
                    if (!elseStatements.HasValue)
                        return Fail(elseStatements);

                    if (IsEmptyEnumerator(lexemes, out emptyResult))
                        return Fail(emptyResult);

                    if (!(lexemes.Current is KeywordLexeme fiKeyword))
                        return Fail($"Can't parse else statement: Expected \"{nameof(KeywordLexeme)}\", but found \"{lexemes.Current}\".");
                    if (fiKeyword.Type != KeywordType.Fi)
                        return Fail($"Can't parse else statement: Expected \"{KeywordType.Fi}\", but found \"{fiKeyword}\".");

                    return new IfStatement(comparisonExpression.Value, statements.Value, elseStatements.Value);
                default:
                    return Fail($"Can't parse if statement: Expected \"{KeywordType.Fi}\" or \"{KeywordType.Then}\", but found \"{keywordLexeme}\".");
            }
        }

        private Result<Statement> ParseWriteStatement(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return Fail(emptyResult);

            if (!(lexemes.Current is LeftBracketLexeme))
                return Fail($"Can't parse write statement: Expected \"{nameof(LeftBracketLexeme)}\", but found \"{lexemes.Current}\".");

            var expression = recognizers.GetRecognizer<Expression>().Recognize(lexemes, programBuilder, recognizers);
            if (!expression.HasValue)
                return Fail(expression);

            if (IsEmptyEnumerator(lexemes, out emptyResult))
                return Fail(emptyResult);
            if (!(lexemes.Current is RightBracketLexeme))
                return Fail($"Can't parse write statement: Expected \"{nameof(RightBracketLexeme)}\", but found \"{lexemes.Current}\".");

            return new WriteStatement(expression.Value);
        }
    }
}
