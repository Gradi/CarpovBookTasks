using System;
using System.IO;
using System.Linq;
using Milan.Expressions.Enums;
using Milan.Parsers.Lexer;
using Milan.Parsers.Lexer.Lexemes;
using NUnit.Framework;
using LF = Milan.Parsers.Lexer.Lexemes.LexemeFactory;

namespace TestMilan.Tests.Parsers.Lexer
{
    [TestFixture]
    public class TestLexer
    {
        [Test]
        public void ThrowsOnInvalidInput()
        {
            Assert.That(() => LexemeParser.Parse(null as TextReader), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => LexemeParser.Parse(null as string), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => LexemeParser.Parse(""), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => LexemeParser.Parse("   "), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => LexemeParser.Parse("   \n\n\r\t"), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ParsesIdentifier()
        {
            const string input = "   hell1o ";

            var result = LexemeParser.Parse(input).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(LF.Identifier("hell1o")));
        }

        [Test]
        public void ParsesConstant()
        {
            const int input = 876_232_12;

            var result = LexemeParser.Parse(input.ToString()).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(LF.Constant(input)));
        }

        [Test]
        public void ParsesAssigment()
        {
            const string input = ":=";

            var result = LexemeParser.Parse(input).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(LF.Assigment()));
        }

        [TestCase("<", ExpectedResult = ComparisonType.Less)]
        [TestCase(">", ExpectedResult = ComparisonType.Greater)]
        [TestCase("=", ExpectedResult = ComparisonType.Equal)]

        [TestCase("  <  ", ExpectedResult = ComparisonType.Less)]
        [TestCase("  >\t", ExpectedResult = ComparisonType.Greater)]
        [TestCase("  =\n", ExpectedResult = ComparisonType.Equal)]

        [TestCase("<>", ExpectedResult = ComparisonType.NotEqual)]
        [TestCase("<=", ExpectedResult = ComparisonType.LessEqual)]
        [TestCase(">=", ExpectedResult = ComparisonType.GreaterEqual)]

        [TestCase("  <> \n", ExpectedResult = ComparisonType.NotEqual)]
        [TestCase("\n<=  ", ExpectedResult = ComparisonType.LessEqual)]
        [TestCase("   \n>= \t\t", ExpectedResult = ComparisonType.GreaterEqual)]
        public ComparisonType ParsesComparison(string input)
        {
            var actual = LexemeParser.Parse(input).ToList();
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual[0], Is.TypeOf<ComparisonLexeme>());
            return ((ComparisonLexeme)actual[0]).Type;
        }

        [Test]
        public void ParsesSemicolon()
        {
            const string input = ";";

            var result = LexemeParser.Parse(input).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(LF.Semicolon()));
        }

        [Test]
        public void ParsesLeftBracket()
        {
            const string input = "(";

            var result = LexemeParser.Parse(input).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(LF.LeftBracket()));
        }

        [Test]
        public void ParsesRightBracket()
        {
            const string input = ")";

            var result = LexemeParser.Parse(input).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(LF.RightBracket()));
        }

        [TestCase("+", ExpectedResult = OperationType.Plus)]
        [TestCase("-", ExpectedResult = OperationType.Minus)]
        [TestCase("*", ExpectedResult = OperationType.Multiply)]
        [TestCase("/", ExpectedResult = OperationType.Divide)]
        [TestCase("%", ExpectedResult = OperationType.Modulo)]

        [TestCase("  + \n\t", ExpectedResult = OperationType.Plus)]
        [TestCase("  -  \n\t", ExpectedResult = OperationType.Minus)]
        [TestCase("\n\t*", ExpectedResult = OperationType.Multiply)]
        [TestCase("\n\n/"   , ExpectedResult = OperationType.Divide)]
        [TestCase("\t\t%\n\n  ", ExpectedResult = OperationType.Modulo)]
        public OperationType ParseOperationType(string input)
        {
            var result = LexemeParser.Parse(input).ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.TypeOf<OperatorLexeme>());
            return ((OperatorLexeme)result[0]).Type;
        }
    }
}
