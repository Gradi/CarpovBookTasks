using System.Collections.Generic;
using Milan.Parsers.Lexer.Lexemes;

namespace Milan.Parsers.Lexer
{
    public interface ILexemeEnumerable : IEnumerable<Lexeme>
    {
        new ILexemeEnumerator GetEnumerator();
    }
}
