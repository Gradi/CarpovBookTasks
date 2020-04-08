using System.Linq;
using GrammarLib;
using GrammarLib.Builder;
using GrammarLib.Extensions;
using NUnit.Framework;

namespace TestGrammarLib.Tests.Extensions
{
    [TestFixture]
    public class TestGrammarExtensions
    {
        [Test]
        public void GetDerivableSymbols_EmptyGrammar()
        {
            var grammar = new GrammarBuilder().StartIs("S").Build();
            var symbols = grammar.GetDerivableSymbols();

            Assert.That(symbols.Count, Is.EqualTo(1));
            Assert.That(symbols.Single(), Is.EqualTo(new NonTerminal("S")));
        }

        [Test]
        public void GetDerivavbleSymbols()
        {
            var grammar = new GrammarBuilder()
                .StartIs("S")
                .AddProduction().NonTerm("S").To().Term("a").NonTerm("S").Term("b").NonTerm("D").Term("c").NonTerm("C").Done()
                .AddProduction().NonTerm("S").To().NonTerm("B").Term("c").NonTerm("A").Done()
                .AddProduction().NonTerm("A").To().NonTerm("B").Term("c").NonTerm("S").Done()
                .AddProduction().NonTerm("A").To().Term("a").Term("c").Term("b").Done()
                .AddProduction().NonTerm("B").To().Term("b").NonTerm("B").Done()
                .AddProduction().NonTerm("B").To().Term("c").Done()
                .AddProduction().NonTerm("C").To().Term("b").NonTerm("C").Term("c").Done()
                .AddProduction().NonTerm("C").To().Term("c").NonTerm("D").NonTerm("A").Done()
                .AddProduction().NonTerm("D").To().Term("c").NonTerm("C").NonTerm("A").NonTerm("D").Done()
                .AddProduction().NonTerm("E").To().Term("d").Term("e").Done()
                .Build();

            var derivableSymbols = new Symbol[]
            {
                new NonTerminal("S"), new NonTerminal("A"), new NonTerminal("B"), new NonTerminal("C"),
                new NonTerminal("D"), new Terminal("a"), new Terminal("b"), new Terminal("c"),
            };

            var actual = grammar.GetDerivableSymbols();
            Assert.That(actual.Count, Is.EqualTo(derivableSymbols.Length));
            Assert.That(actual, Is.EquivalentTo(derivableSymbols));
        }
    }
}
