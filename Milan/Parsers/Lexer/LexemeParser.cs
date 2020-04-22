using System;
using System.Collections.Generic;
using System.IO;
using Milan.Helpers;
using Milan.Parsers.Lexer.Lexemes;

namespace Milan.Parsers.Lexer
{
    public static class LexemeParser
    {
        public static IEnumerable<Lexeme> Parse(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            return new LexerMachine().Run(new CharReader(reader));
        }

        public static IEnumerable<Lexeme> Parse(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            return new LexerMachine().Run(new CharReader(new StringReader(source), disposeReader:true));
        }
    }
}
