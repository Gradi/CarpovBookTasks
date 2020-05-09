using System;

namespace Milan.Parsers.Syntax.Exceptions
{
    public class SyntaxParseException : Exception
    {
        public SyntaxParseException(string message, Exception? innerException = null) : base(message, innerException) {}
    }
}
