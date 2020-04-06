using System;
using NUnit.Framework;
using RomanNumbersParser;

namespace TestRomanNumbersParser.Tests
{
    [TestFixture]
    public class TestRomanNumbersParser
    {
        [TestCase(null, ExpectedResult = (int)0)]
        [TestCase("", ExpectedResult = (int)0)]
        [TestCase("    ", ExpectedResult = (int)0)]
        [TestCase("    \n\n\t", ExpectedResult = (int)0)]
        public int ReturnsZeroOnEmptyInput(string input) => RomanNumbersParser.RomanNumbersParser.Parse(input);

        [TestCase("A")]
        [TestCase("1")]
        [TestCase("1123")]
        [TestCase("XF")]
        [TestCase("IXDDD")]
        [TestCase("XI:")]
        [TestCase("XXAXAXAXAXXAXAXAXAXAXAXAXXAXAXAX")]
        public void ThrowsInInputContainingInvalidChars(string input)
        {
            Assert.That(() => RomanNumbersParser.RomanNumbersParser.Parse(input), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void SimpleOneDigitCase()
        {
            foreach (var digit in RomanDigits.AllDigits)
            {
                Assert.That(RomanNumbersParser.RomanNumbersParser.Parse(digit.ToString()), Is.EqualTo(digit.NumericValue),
                    $"Input was {digit}.");
            }
        }

        [TestCase("IIX")]
        [TestCase("XXXX")]
        [TestCase("XIMMM")]
        [TestCase("MIMMXX")]
        [TestCase("IXXXX")]
        public void TestInvalidInputs(string input)
        {
            Assert.That(() => RomanNumbersParser.RomanNumbersParser.Parse(input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("XXXIX", ExpectedResult = (int)39)]
        [TestCase("XXIV", ExpectedResult = (int)24)]
        [TestCase("CMLIX", ExpectedResult = (int)959)]
        [TestCase("MMMXIX", ExpectedResult = (int)3019)]
        public int TestValidInputs(string input) => RomanNumbersParser.RomanNumbersParser.Parse(input);
    }
}
