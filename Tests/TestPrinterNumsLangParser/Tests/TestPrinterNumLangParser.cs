using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PrinterNumsLangParser;
using PrinterNumsLangParser.Exceptions;

namespace TestPrinterNumsLangParser.Tests
{
    [TestFixture]
    public class TestPrinterNumLangParser
    {
        private readonly IReadOnlyCollection<(string Input, IEnumerable<NumberRange> Expected)> _validInputs;

        public TestPrinterNumLangParser()
        {
            _validInputs = new (string Input, IEnumerable<NumberRange> Expected)[]
            {
                ("1", new [] { new NumberRange(1, 1) }),
                ("2", new [] { new NumberRange(2, 2) }),
                ("1233", new [] { new NumberRange(1233, 1233) }),
                ("1,2,3", new [] { new NumberRange(1, 1), new NumberRange(2, 2), new NumberRange(3, 3) }),
                ("1, 2 ,  \n3", new [] { new NumberRange(1, 1), new NumberRange(2, 2), new NumberRange(3, 3) }),
                ("3-7", new [] { new NumberRange(3, 7) }),
                ("3,7-4 ,1, 1, 1, 99-110", new []
                {
                    new NumberRange(3, 3), new NumberRange(7, 4), new NumberRange(1, 1), new NumberRange(1, 1), new NumberRange(1, 1),
                    new NumberRange(99, 110),
                }),
                ("1,2,3,10-10,10-10,123-123,9,9-88", new []
                {
                    new NumberRange(1, 1), new NumberRange(2, 2), new NumberRange(3, 3), new NumberRange(10, 10),
                    new NumberRange(10, 10), new NumberRange(123, 123), new NumberRange(9, 9), new NumberRange(9, 88)
                })
            };
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("\n  \n\t")]
        public void ThrowsOnInvalidInput(string input)
        {
            Assert.That(() => PrinterNumsLangParser.PrinterNumsLangParser.Parse(input), Throws.TypeOf<ArgumentException>());
        }

        [TestCase("1,")]
        [TestCase(",1")]
        [TestCase("123,")]
        [TestCase(",123")]
        [TestCase(",,123")]
        [TestCase(",123,")]
        [TestCase("1,,2")]
        [TestCase("12,35,,123")]

        [TestCase("1-")]
        [TestCase("-1")]
        [TestCase("-  1")]
        [TestCase("123--")]
        [TestCase("2--3")]
        [TestCase("2-4-5")]

        [TestCase("5,1,4--5")]
        [TestCase("   23-123-,33")]
        [TestCase("23,-23")]
        [TestCase("13,123,-")]
        [TestCase("-,12")]
        [TestCase("33,-")]
        [TestCase("3-7,")]

        [TestCase("123asd")]
        [TestCase("1 asd  dasd,")]
        [TestCase("123, 123, asd")]
        public void ThrowsParserExceptionOnInvalidInput(string input)
        {
            Assert.That(() => PrinterNumsLangParser.PrinterNumsLangParser.Parse(input).ToList(), Throws.TypeOf<ParserException>());
        }

        [Test]
        public void TestValidInputs()
        {
            foreach (var input in _validInputs)
            {
                var actual = PrinterNumsLangParser.PrinterNumsLangParser.Parse(input.Input);
                CollectionAssert.AreEqual(input.Expected, actual, $"Input string was: \"{input.Input}\".");
            }
        }
    }
}
