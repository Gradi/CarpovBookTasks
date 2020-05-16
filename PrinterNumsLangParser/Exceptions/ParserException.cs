using System;

namespace PrinterNumsLangParser.Exceptions
{
    public class ParserException : Exception
    {
        public string? Input { get; }

        internal ParserException(string message) : base(message) {}

        internal ParserException(string message, Exception innerException)
            : base(message, innerException) {}

        internal ParserException(string input, string message)
            : base ($"{message}. Input was: {input}.")
        {
            Input = input;
        }
    }
}
