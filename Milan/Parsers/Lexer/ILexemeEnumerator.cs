using System.Collections.Generic;
using Milan.Parsers.Lexer.Lexemes;

namespace Milan.Parsers.Lexer
{
    public interface ILexemeEnumerator : IEnumerator<Lexeme>
    {
        void Return(Lexeme lexeme);

        void Return();
    }
}
