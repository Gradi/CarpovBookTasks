using System.Linq;
using Milan;
using Milan.Expressions;
using Milan.Expressions.Enums;
using Milan.Parsers.Syntax;
using Milan.Statements;
using NUnit.Framework;

namespace TestMilan.Tests.Parsers.Syntax
{
    [TestFixture]
    public class TestSyntaxParser
    {
        [Test]
        public void Sample000()
        {
            var source = @"
            begin
                x := 123;
                write(x);
            end
            ";

            var builder = new ProgramBuilder();
            var x = builder.GetOrAddIdentifier("x");
            var c = builder.GetOrAddConstant(123);
            builder.Statements.Add(new AssigmentStatement(x, ExpressionFactory.Constant(c)));
            builder.Statements.Add(new WriteStatement(ExpressionFactory.Identifier(x)));
            var expected = builder.Build();

            var actual = SyntaxParser.Parse(source);

            Assert.That(actual.Constants, Is.EqualTo(expected.Constants).AsCollection);
            Assert.That(actual.Identifiers, Is.EqualTo(expected.Identifiers).AsCollection);
            Assert.That(actual.Statements.Count, Is.EqualTo(expected.Statements.Count));

            Assert.That(actual.Statements.ElementAt(0), Is.TypeOf(actual.Statements.ElementAt(0).GetType()));
            Assert.That(actual.Statements.ElementAt(1), Is.TypeOf(actual.Statements.ElementAt(1).GetType()));
        }

        [Test]
        public void Sample001()
        {
            var source = @"
            begin
                x := read;
                if x = 0 then
                    write(x + 1);
                    write(x + 2);
                    write(x + read);
                else
                    if x = 1 then
                        write(x * 1);
                        write(x * 2);
                        write(read * read + 123);
                    fi;
                fi;
                write(read);
            end
            ";

            var builder = new ProgramBuilder();
            var x = builder.GetOrAddIdentifier("x");
            var c0 = builder.GetOrAddConstant(0);
            var c1 = builder.GetOrAddConstant(1);
            var c2 = builder.GetOrAddConstant(2);
            var c123 = builder.GetOrAddConstant(123);
            builder.Statements.Add(new AssigmentStatement(x, ExpressionFactory.Read()));
            builder.Statements.Add(new IfStatement(new ComparisonExpression(ComparisonType.Equal, ExpressionFactory.Identifier(x), ExpressionFactory.Constant(c0)), new StatementCollection(new []
            {
                new WriteStatement(ExpressionFactory.Plus(ExpressionFactory.Identifier(x), ExpressionFactory.Constant(c1))),
                new WriteStatement(ExpressionFactory.Plus(ExpressionFactory.Identifier(x), ExpressionFactory.Constant(c2))),
                new WriteStatement(ExpressionFactory.Plus(ExpressionFactory.Identifier(x), ExpressionFactory.Read())),
            }), new StatementCollection(new []
            {
                new IfStatement(new ComparisonExpression(ComparisonType.Equal, ExpressionFactory.Identifier(x), ExpressionFactory.Constant(c1)), new StatementCollection(new []
                {
                    new WriteStatement(ExpressionFactory.Multiply(ExpressionFactory.Identifier(x), ExpressionFactory.Constant(c1))),
                    new WriteStatement(ExpressionFactory.Multiply(ExpressionFactory.Identifier(x), ExpressionFactory.Constant(c2))),
                    new WriteStatement(ExpressionFactory.Plus(ExpressionFactory.Multiply(ExpressionFactory.Read(), ExpressionFactory.Read()), ExpressionFactory.Constant(c123))),
                }) ),
            })));
            builder.Statements.Add(new WriteStatement(ExpressionFactory.Read()));

            var expected = builder.Build();

            var actual = SyntaxParser.Parse(source);

            Assert.That(actual.Constants, Is.EqualTo(expected.Constants).AsCollection);
            Assert.That(actual.Identifiers, Is.EqualTo(expected.Identifiers).AsCollection);

            Assert.That(actual.Statements.Count, Is.EqualTo(expected.Statements.Count));
            Assert.That(actual.Statements.Last(), Is.TypeOf<WriteStatement>());
        }

        [Test]
        public void Gcd()
        {
            Assert.That(() => SyntaxParser.Parse(Sources.GreatestCommonDivisor), Throws.Nothing);
        }

    }
}
