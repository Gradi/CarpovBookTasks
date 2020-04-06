using System;
using RomanNumbersParser.Extensions;

namespace RomanNumbersParser
{
    public class RomanNumbersParser
    {
        private const int MaximumRepetitionAllowed = 2; // 1 digit + 2 more or 3 digits in a row

        private readonly string _input;

        private int _accumulator;

        private RomanDigit _currentDigit;

        private RomanDigit _previousDigit;
        private int _duplicationCount;

        private RomanDigit? _prepreviousDigit;
        private RomanDigit? _nextDigit;

        private RomanNumbersParser(string input)
        {
            _input = input;

            _accumulator = 0;
            _currentDigit = default;
            _previousDigit = default;
            _duplicationCount = 0;
            _prepreviousDigit = null;
            _nextDigit = null;
        }

        private int Parse()
        {
            for (int i = 0; i < _input.Length; ++i)
            {
                _currentDigit = GetRomanDigit(_input[i]);
                if (i + 1 < _input.Length)
                {
                    _nextDigit = GetRomanDigit(_input[i + 1]);
                }
                else
                {
                    _nextDigit = null;
                }

                if (i == 0)
                {
                    _accumulator = _currentDigit.NumericValue;
                }
                else
                {
                    if (_previousDigit == _currentDigit)
                    {
                        _duplicationCount += 1;
                    }
                    else
                    {
                        _duplicationCount = 0;
                    }
                    CheckDigit();
                }

                if (i > 0)
                {
                    _prepreviousDigit = _previousDigit;
                }

                _previousDigit = _currentDigit;

            }

            return  _accumulator;
        }

        private void CheckDigit()
        {
            if (_duplicationCount > 0)
            {
                if (!_currentDigit.IsPowerOf10())
                    throw new ArgumentException($"Digit '{_currentDigit}' can't duplicate.");
                if (_duplicationCount > MaximumRepetitionAllowed)
                {
                    throw new ArgumentException($"Duplication of '{_currentDigit}'. Digits can duplicate maximum {MaximumRepetitionAllowed + 1} times in a row.");
                }
            }

            if (_previousDigit < _currentDigit)
            {
                if (!_previousDigit.IsPowerOf10())
                {
                    throw new ArgumentException($"If digit is followed by smaller number smaller number must in power of 10 ('{_previousDigit}{_currentDigit}')");
                }
                if (!_previousDigit.IsOneFifthOrTenthOf(_currentDigit))
                {
                    throw new ArgumentException($"If digit is followed by smaller number smaller number must be 1/5 or 1/10 of bigger one ('{_previousDigit}{_currentDigit}').");
                }
                if (_prepreviousDigit.HasValue && (_prepreviousDigit.Value.NumericValue / _previousDigit.NumericValue) < 10)
                {
                    throw new ArgumentException($"Digit before small digit which is following bigger digit must be at least ten times bigger " +
                                                $"{_prepreviousDigit.Value}{_previousDigit}{_currentDigit}");
                }
                if (_nextDigit.HasValue && _nextDigit.Value > _previousDigit)
                {
                    throw new ArgumentException($"Digit after digit which if followed by a smaller number must be less than small number ('{_previousDigit}{_currentDigit}{_nextDigit.Value}')");
                }

                _accumulator -= _previousDigit.NumericValue;
                _accumulator += _currentDigit.NumericValue - _previousDigit.NumericValue;
                _duplicationCount -= 1; // In inputs like IXXXX it is okay to have 4 X'es. This line adds 1 to duplication threshold.
            }
            else
            {
                _accumulator += _currentDigit.NumericValue;
            }
        }

        private RomanDigit GetRomanDigit(char character)
        {
            if (!RomanDigits.IsValidRomanSymbol(character))
                throw new ArgumentException($"Char '{character}' is not roman digit.");
            return RomanDigits.RomanDigitsByChars[character];
        }

        public static int Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return 0;

            return new RomanNumbersParser(str).Parse();
        }

    }
}
