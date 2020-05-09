using Milan.Parsers.Lexer;

namespace Milan.Parsers.Syntax
{
    public abstract class BasePatternRecognizer<T> : IPatternRecognizer<T>
    {
        public abstract Result<T> Recognize(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers);

        protected bool IsEmptyEnumerator(ILexemeEnumerator lexemes, out Result<T> result)
        {
            if (!lexemes.MoveNext())
            {
                result = Fail("Unexpected end of lexeme enumerator");
                return true;
            }
            result = default;
            return false;
        }

        protected Result<T> Fail(string message) => Result<T>.Empty($"{GetType().Name}: {message}");

        protected Result<T> Fail<TOther>(Result<TOther> failedOtherResult) => Fail(failedOtherResult.Error);
    }
}
