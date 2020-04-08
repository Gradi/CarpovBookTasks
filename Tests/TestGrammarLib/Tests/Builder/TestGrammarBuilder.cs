using System;
using System.Linq;
using GrammarLib;
using GrammarLib.Builder;
using NUnit.Framework;

namespace TestGrammarLib.Tests.Builder
{
    [TestFixture]
    public class TestGrammarBuilder
    {
        [Test]
        public void ThrowsOnEmptyInput()
        {
            Assert.That(() => new GrammarBuilder().Build(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ThrowsOnDoubleStartSymbol()
        {
            Assert.That(() => new GrammarBuilder().StartIs("S").StartIs("S"), Throws.TypeOf<InvalidOperationException>());
        }


        [Test]
        public void EmptyGrammar()
        {
            var grammar = new GrammarBuilder().StartIs("S").Build();
            Assert.That(grammar.AllSymbols.Count, Is.EqualTo(1));
            Assert.That(grammar.AllSymbols.Single(), Is.EqualTo(new NonTerminal("S")));
        }

        [Test]
        public void GrammarCreation()
        {
            var grammar = new GrammarBuilder()
                .StartIs("S")
                .AddProduction().NonTerm("S").To().Term("a").NonTerm("S").Term("b").NonTerm("B").Term("c").Done()
                .AddProduction().NonTerm("B").To().Term("c").NonTerm("D").Done()
                .AddProduction().NonTerm("S").To().NonTerm("B").Term("d").NonTerm("D").Done()
                .AddProduction().NonTerm("C").To().Term("b").NonTerm("C").Term("c").Done()
                .AddProduction().NonTerm("A").To().NonTerm("B").Term("c").NonTerm("S").Done()
                .Build();

            var terminals = new Symbol[]
            {
                new Terminal("a"), new Terminal("b"), new Terminal("c"),
                new Terminal("d")
            };
            var nonTerminals = new Symbol[]
            {
                new NonTerminal("S"), new NonTerminal("B"), new NonTerminal("D"),
                new NonTerminal("A"), new NonTerminal("C"),
            };

            Assert.That(grammar.Productions.Count, Is.EqualTo(5));
            Assert.That(grammar.AllSymbols.Count, Is.EqualTo(terminals.Length + nonTerminals.Length));
            CollectionAssert.AreEquivalent(terminals, grammar.Terminals);
            CollectionAssert.AreEquivalent(nonTerminals, grammar.NonTerminals);
            CollectionAssert.AreEquivalent(terminals.Concat(nonTerminals), grammar.AllSymbols);
        }
    }
}
