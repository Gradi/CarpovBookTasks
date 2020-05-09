using System;
using System.IO;
using Milan.Helpers;

namespace Milan.Parsers.Lexer
{
    public static class LexemeParser
    {
        public static ILexemeEnumerable Parse(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            return new LexemeEnumerable(new LexerMachine().Run(new CharReader(reader)));
        }

        public static ILexemeEnumerable Parse(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            return new LexemeEnumerable(new LexerMachine().Run(new CharReader(new StringReader(source), disposeReader:true)));
        }
    }
}
