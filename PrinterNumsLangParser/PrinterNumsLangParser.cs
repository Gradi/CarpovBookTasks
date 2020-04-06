using System;
using System.Collections.Generic;
using PrinterNumsLangParser.Exceptions;

namespace PrinterNumsLangParser
{
    public class PrinterNumsLangParser
    {
        private readonly string _input;

        private State _state;

        private char _currentChar;

        private int? _numberBuf;

        private int? _startBuf;
        private int? _endBuf;

        private PrinterNumsLangParser(string input)
        {
            _input = input;
        }

        private IEnumerable<NumberRange> Parse()
        {
            Reset();

            NumberRange? newEntry = null;

            void HandleState()
            {
                switch (_state)
                {
                    case State.Start: Start(); break;
                    case State.Number: Number(); break;
                    case State.Dash: Dash(); break;
                    case State.Eof: Eof(); break;
                }
                if (_state == State.EntryComplete)
                {
                    if (!_startBuf.HasValue)
                        throw new ParserException(_input, $"Invalid state: Expected {nameof(_startBuf)} to have values.");
                    newEntry = new NumberRange(_startBuf.Value, _endBuf.GetValueOrDefault(_startBuf.Value));
                    Reset();
                }
            }

            foreach (char character in _input)
            {
                if (Char.IsWhiteSpace(character))
                    continue;

                _currentChar = character;
                HandleState();
                if (newEntry.HasValue)
                {
                    yield return newEntry.Value;
                    newEntry = null;
                }
            }
            _state = State.Eof;
            HandleState();
            if (newEntry.HasValue)
            {
                yield return newEntry.Value;
            }
        }

        private void Reset()
        {
            _state = State.Start;
            _currentChar = default;
            _numberBuf = null;
            _startBuf = null;
            _endBuf = null;
        }

        private void Start()
        {
            if (!Char.IsDigit(_currentChar))
                throw new ParserException(_input, $"Expected symbol to be digit, but found '{_currentChar}'.");
            _numberBuf = int.Parse(_currentChar.ToString());
            _state = State.Number;
        }

        private void Number()
        {
            if (Char.IsDigit(_currentChar))
            {
                _numberBuf *= 10;
                _numberBuf += int.Parse(_currentChar.ToString());
            }
            else if (_currentChar == '-')
            {
                _startBuf = _numberBuf.Value;
                _numberBuf = null;
                _state = State.Dash;
            }
            else if (_currentChar == ',')
            {
                _startBuf = _numberBuf.Value;
                _state = State.EntryComplete;
            }
            else
                ThrowUnexpectedSymbol();
        }

        private void Dash()
        {
            if (Char.IsDigit(_currentChar))
            {
                _numberBuf = _numberBuf.GetValueOrDefault();
                _numberBuf *= 10;
                _numberBuf += int.Parse(_currentChar.ToString());
            }
            else if (_currentChar == ',')
            {
                if (!_numberBuf.HasValue)
                    throw new ParserException(_input, "Unexpected comma after dash.");

                _endBuf = _numberBuf.Value;
                _state = State.EntryComplete;
            }
            else
                ThrowUnexpectedSymbol();
        }

        private void Eof()
        {
            if (!_numberBuf.HasValue)
                throw new ParserException(_input, "Unexpected eof.");
            if (!_startBuf.HasValue)
            {
                _startBuf = _numberBuf.Value;
            }
            else
            {
                _endBuf = _numberBuf.Value;
            }
            _state = State.EntryComplete;
        }

        private void ThrowUnexpectedSymbol()
        {
            throw new ParserException(_input, $"Unexpected symbol (or it's position): '{_currentChar}'.");
        }

        public static IEnumerable<NumberRange> Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException("Input is null or empty or whitespace", nameof(str));
            return new PrinterNumsLangParser(str).Parse();
        }

        private enum State
        {
            Start,
            Number,
            Dash,
            EntryComplete,
            Eof
        }
    }
}
