using System;
using System.Collections.Generic;
using Milan.Expressions;
using Milan.Expressions.Enums;
using Milan.Parsers.Lexer;
using Milan.Parsers.Lexer.Enums;
using Milan.Parsers.Lexer.Lexemes;

namespace Milan.Parsers.Syntax
{
    public class ExpressionPatternRecognizer : BasePatternRecognizer<Expression>
    {
        private readonly HashSet<OperationType> _operatorPriority0;
        private readonly HashSet<OperationType> _operatorPriority1;

        public ExpressionPatternRecognizer()
        {
            _operatorPriority0 = new HashSet<OperationType> { OperationType.Plus, OperationType.Minus };
            _operatorPriority1 = new HashSet<OperationType> { OperationType.Divide, OperationType.Multiply, OperationType.Modulo };
        }

        public override Result<Expression> Recognize(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            return ParseTerm(lexemes, programBuilder, recognizers, _operatorPriority0, (lex, pb, r) => ParseTerm(lex, pb, r, _operatorPriority1, ParseProduction));
        }

        private Result<Expression> ParseTerm(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers,
            HashSet<OperationType> allowedOperations, Func<ILexemeEnumerator, ProgramBuilder, IPatternRecognizerCollection, Result<Expression>> nextParser)
        {
            var leftExpr = nextParser(lexemes, programBuilder, recognizers);
            if (!leftExpr.HasValue)
                return Fail(leftExpr);

            Expression result = leftExpr.Value;

            while (lexemes.MoveNext())
            {
                if (lexemes.Current is OperatorLexeme operatorLexeme && allowedOperations.Contains(operatorLexeme.Type))
                {
                    var rightExpr = nextParser(lexemes, programBuilder, recognizers);
                    if (!rightExpr.HasValue)
                        return Fail(rightExpr);

                    result = new MathExpression(operatorLexeme.Type, result, rightExpr.Value);
                }
                else
                {
                    lexemes.Return();
                    break;
                }
            }
            return result;
        }

        private Result<Expression> ParseProduction(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            if (IsEmptyEnumerator(lexemes, out var emptyResult))
                return emptyResult;

            if (lexemes.Current is IdentifierLexeme identifierLexeme)
                return ExpressionFactory.Identifier(programBuilder.GetOrAddIdentifier(identifierLexeme.Name));

            if (lexemes.Current is ConstantLexeme constantLexeme)
                return ExpressionFactory.Constant(programBuilder.GetOrAddConstant(constantLexeme.Value));

            if (lexemes.Current is KeywordLexeme keywordLexeme)
            {
                if (keywordLexeme.Type == KeywordType.Read)
                    return ExpressionFactory.Read();
                else
                    return Fail($"Can't parse expression: Expected \"{KeywordType.Read}\" keyword, but found \"{keywordLexeme}\".");
            }

            if (lexemes.Current is LeftBracketLexeme)
            {
                var expression = Recognize(lexemes, programBuilder, recognizers);
                if (!expression.HasValue)
                    return Fail($"Can't parse expression after left opening bracket: {expression.Value}.");

                if (IsEmptyEnumerator(lexemes, out emptyResult))
                    return Fail(emptyResult);

                if (!(lexemes.Current is RightBracketLexeme))
                    return Fail("Can't parse expression: No right closing bracket.");

                return expression;
            }

            return Fail($"Can't parse expression: Unexpected lexeme \"{lexemes.Current}\".");
        }
    }
}
