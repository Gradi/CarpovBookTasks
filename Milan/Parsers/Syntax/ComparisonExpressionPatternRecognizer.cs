using Milan.Expressions;
using Milan.Parsers.Lexer;
using Milan.Parsers.Lexer.Lexemes;

namespace Milan.Parsers.Syntax
{
    public class ComparisonExpressionPatternRecognizer : BasePatternRecognizer<ComparisonExpression>
    {
        public override Result<ComparisonExpression> Recognize(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            var expressionRecognizer = recognizers.GetRecognizer<Expression>();

            var leftExpression = expressionRecognizer.Recognize(lexemes, programBuilder, recognizers);
            if (!leftExpression.HasValue)
                return Fail(leftExpression);

            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return Fail(emptyResult);

            if (!(lexemes.Current is ComparisonLexeme comparisonLexeme))
                return Fail($"Can't parse comparison expression: Expected \"{nameof(ComparisonLexeme)}\", but found \"{lexemes.Current}\".");

            var rightExpression = expressionRecognizer.Recognize(lexemes, programBuilder, recognizers);
            if (!rightExpression.HasValue)
                return Fail(rightExpression);

            return new ComparisonExpression(comparisonLexeme.Type, leftExpression.Value, rightExpression.Value);
        }
    }
}
