using System;
using System.IO;
using System.Text;
using Milan.Expressions.Enums;
using Milan.Extensions;
using Milan.Parsers.Lexer.Lexemes;
using StateMachinesBuilder;

namespace Milan.Parsers.Lexer
{
    internal class LexerMachine : SMachine<char, Lexeme>
    {
        private StringBuilder _inputBuffer;
        private char? _previousComparisonSymbol;

        public LexerMachine()
        {
            _inputBuffer = new StringBuilder();
            _previousComparisonSymbol = null;
        }

        protected override Action<Option<char>> GetStartState() => Start;

        protected override void Reset()
        {
            _inputBuffer = new StringBuilder();
            _previousComparisonSymbol = null;
        }

        private void Start(Option<char> input)
        {
            if (!input.HasValue)
            {
                SetEndState(End);
                return;
            }
            char ch = input.Value;
            if (char.IsWhiteSpace(ch))
                return;

            Action<Option<char>> nextState;
            if (char.IsLetter(ch))
                nextState = IdentifierOrKeyword;
            else if (char.IsDigit(ch))
                nextState = ConstantState;
            else if (ch == ':')
                nextState = Assigment;
            else if (ch == '<' || ch == '>' || ch == '=')
                nextState = ComparisonState;
            else if (ch == ';')
                nextState = Semicolon;
            else if (ch == '(')
                nextState = LeftBracket;
            else if (ch == ')')
                nextState = RightBracket;
            else if (IsMathOperationChar(ch))
                nextState = MathOperation;
            else
                throw new ArgumentOutOfRangeException($"Unexpected character value : \"{ch}\"'.", null as Exception);

            Return(ch);
            SetNextState(nextState);
        }

        private void IdentifierOrKeyword(Option<char> input)
        {
            if (!input.HasValue || !char.IsLetterOrDigit(input.Value))
            {
                var name = _inputBuffer.ToString();
                _inputBuffer.Clear();
                YieldResult(name.IsValidKeyword() ? LexemeFactory.Keyword(name) : LexemeFactory.Identifier(name));
                if (input.HasValue)
                {
                    Return(input.Value);
                    SetNextState(Start);
                }
                else
                    SetEndState(End);
            }
            else
            {
                _inputBuffer.Append(input.Value);
            }
        }

        private void ConstantState(Option<char> input)
        {
            if (!input.HasValue || !char.IsDigit(input.Value))
            {
                if (!int.TryParse(_inputBuffer.ToString(), out var constant))
                    throw new ArgumentException($"Can't convert {_inputBuffer} to int.");
                _inputBuffer.Clear();
                YieldResult(LexemeFactory.Constant(constant));
                if (input.HasValue)
                {
                    Return(input.Value);
                    SetNextState(Start);
                }
                else
                    SetEndState(End);
            }
            else
            {
                _inputBuffer.Append(input.Value);
            }
        }

        private void Assigment(Option<char> input)
        {
            if (!input.HasValue)
                ThrowUnexpectedEof("assigment lexeme");
            if (input.Value == ':')
                return;
            if (input.Value == '=')
            {
                YieldResult(LexemeFactory.Assigment());
                SetNextState(Start);
            }
            else
                ThrowUnexpectedChar(input.Value);
        }

        private void ComparisonState(Option<char> input)
        {
            if (!_previousComparisonSymbol.HasValue)
            {
                if (input.Value == '=')
                {
                    YieldResult(LexemeFactory.Comparison(ComparisonType.Equal));
                    SetNextState(Start);
                }
                else
                {
                    _previousComparisonSymbol = input.Value;
                }
            }
            else
            {
                ComparisonType type;
                if (!input.HasValue)
                {
                    type = _previousComparisonSymbol.Value switch
                    {
                        '<' => ComparisonType.Less,
                        '>' => ComparisonType.Greater,
                        _ => ThrowUnexpectedChar<ComparisonType>(_previousComparisonSymbol.Value)
                    };
                }
                else if (_previousComparisonSymbol.Value == '<')
                {
                    switch (input.Value)
                    {
                        case '>':
                            type = ComparisonType.NotEqual;
                            break;
                        case '=':
                            type = ComparisonType.LessEqual;
                            break;
                        default:
                            type = ComparisonType.Less;
                            Return(input.Value);
                            break;
                    }
                }
                else if (_previousComparisonSymbol.Value == '>')
                {
                    switch (input.Value)
                    {
                        case '=':
                            type = ComparisonType.GreaterEqual;
                            break;
                        default:
                            type = ComparisonType.Greater;
                            Return(input.Value);
                            break;
                    }
                }
                else
                    throw new InvalidOperationException($"Unexpected comparison operator: {_previousComparisonSymbol.Value}{input.Value}");
                _previousComparisonSymbol = null;
                YieldResult(LexemeFactory.Comparison(type));
                SetNextState(Start);
            }
        }

        private void Semicolon(Option<char> input)
        {
            YieldResult(LexemeFactory.Semicolon());
            SetNextState(Start);
        }

        private void LeftBracket(Option<char> input)
        {
            YieldResult(LexemeFactory.LeftBracket());
            SetNextState(Start);
        }

        private void RightBracket(Option<char> input)
        {
            YieldResult(LexemeFactory.RightBracket());
            SetNextState(Start);
        }

        private void MathOperation(Option<char> input)
        {
            var type = input.Value switch
            {
                '+' => OperationType.Plus,
                '-' => OperationType.Minus,
                '*' => OperationType.Multiply,
                '/' => OperationType.Divide,
                '%' => OperationType.Modulo,
                _ => ThrowUnexpectedChar<OperationType>(input.Value)
            };
            YieldResult(LexemeFactory.Operator(type));
            SetNextState(Start);
        }

        private void End()
        {
            if (_previousComparisonSymbol.HasValue)
                ThrowUnexpectedEof("comparison lexeme");
            if (_inputBuffer.Length != 0)
                ThrowUnexpectedEof("identifier/constant lexeme");
        }

        private static bool IsMathOperationChar(char ch)
        {
            return ch == '+' ||
                   ch == '-' ||
                   ch == '*' ||
                   ch == '/' ||
                   ch == '%';
        }

        private static void ThrowUnexpectedChar(char ch)
        {
            throw new ArgumentException($"Unexpected characted or it's position: \"{ch}\".");
        }

        private static T ThrowUnexpectedChar<T>(char ch)
        {
            ThrowUnexpectedChar(ch);
#pragma warning disable 8603
            return default(T);
#pragma warning restore 8603
        }

        private static void ThrowUnexpectedEof(string state)
        {
            throw new EndOfStreamException($"Unexpected eof when being in \"{state}\" state.");
        }
    }
}
