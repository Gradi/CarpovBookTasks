using System;
using System.Collections.Generic;
using System.IO;
using Milan.Parsers.Lexer;
using Milan.Parsers.Syntax.Exceptions;

namespace Milan.Parsers.Syntax
{
    public class SyntaxParser : IPatternRecognizerCollection
    {
        private readonly IDictionary<Type, object> _recognizersCache;

        private SyntaxParser()
        {
            _recognizersCache = new Dictionary<Type, object>();
        }

        public IPatternRecognizer<T> GetRecognizer<T>()
        {
            var type = typeof(T);
            if (!_recognizersCache.TryGetValue(type, out var recognizer))
            {
                recognizer = RecognizersFactory.Create<T>();
                _recognizersCache[type] = recognizer;
            }
            return (IPatternRecognizer<T>)recognizer;
        }

        private Program InnerParse(ILexemeEnumerable lexemes)
        {
            try
            {
                using var lexemeEnumerator = lexemes.GetEnumerator();
                var result = GetRecognizer<Program>().Recognize(lexemeEnumerator, new ProgramBuilder(), this);
                if (!result.HasValue)
                {
                    throw new SyntaxParseException($"Can't parse syntax: {result.Error}");
                }
                return result.Value;
            }
            catch(Exception exception)
            {
                throw new SyntaxParseException("Unexpected exception parsing syntax.", exception);
            }
        }

        public static Program Parse(ILexemeEnumerable lexemeEnumerable)
        {
            if (lexemeEnumerable == null)
                throw new ArgumentNullException(nameof(lexemeEnumerable));
            return new SyntaxParser().InnerParse(lexemeEnumerable);
        }

        public static Program Parse(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            return new SyntaxParser().InnerParse(LexemeParser.Parse(reader));
        }

        public static Program Parse(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));
            return new SyntaxParser().InnerParse(LexemeParser.Parse(source));
        }
    }
}
