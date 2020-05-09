using System;
using System.Collections;
using System.Collections.Generic;
using Milan.Parsers.Lexer.Lexemes;

namespace Milan.Parsers.Lexer
{
    public class LexemeEnumerable : ILexemeEnumerable
    {
        private readonly IEnumerable<Lexeme> _innerEnumerable;

        public LexemeEnumerable(IEnumerable<Lexeme> innerEnumerable)
        {
            _innerEnumerable = innerEnumerable ?? throw new ArgumentNullException(nameof(innerEnumerable));
        }

        IEnumerator<Lexeme> IEnumerable<Lexeme>.GetEnumerator() => _innerEnumerable.GetEnumerator();

        ILexemeEnumerator ILexemeEnumerable.GetEnumerator() => new LexemeEnumerator(_innerEnumerable.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => _innerEnumerable.GetEnumerator();

        private class LexemeEnumerator : ILexemeEnumerator
        {
            private readonly IEnumerator<Lexeme> _innerEnumerator;
            private readonly Stack<Lexeme> _returnedLexemes;

            public LexemeEnumerator(IEnumerator<Lexeme> innerEnumerator)
            {
                _innerEnumerator = innerEnumerator;
                _returnedLexemes = new Stack<Lexeme>();
                Current = null!;
            }

            public Lexeme Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_returnedLexemes.TryPop(out var lexeme))
                {
                    Current = lexeme;
                    return true;
                }
                if (_innerEnumerator.MoveNext())
                {
                    Current = _innerEnumerator.Current;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _returnedLexemes.Clear();
                _innerEnumerator.Reset();
            }

            public void Return(Lexeme lexeme) => _returnedLexemes.Push(lexeme);

            public void Return() => Return(Current);

            public void Dispose()
            {
                _returnedLexemes.Clear();
                _innerEnumerator.Dispose();
            }
        }
    }
}
