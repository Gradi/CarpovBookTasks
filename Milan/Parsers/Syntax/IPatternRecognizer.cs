using Milan.Parsers.Lexer;

namespace Milan.Parsers.Syntax
{
    public interface IPatternRecognizer<T>
    {
        Result<T> Recognize(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers);
    }
}
