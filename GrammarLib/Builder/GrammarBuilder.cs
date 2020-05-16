using System;
using System.Collections.Generic;

namespace GrammarLib.Builder
{
    public class GrammarBuilder : IProductionBuilder
    {
        private NonTerminal? _startSymbol;
        private readonly List<Production> _productions;

        private ICollection<Symbol>? _fromSymbols;
        private ICollection<Symbol>? _toSymbols;
        private ICollection<Symbol>? _currentSymbolsCollection;

        public GrammarBuilder()
        {
            _productions = new List<Production>();
        }

        public GrammarBuilder StartIs(string startNonTerminal)
        {
            if (startNonTerminal == null)
                throw new ArgumentNullException(nameof(startNonTerminal));
            if (_startSymbol != null)
                throw new InvalidOperationException("Can't set start non terminal twice.");
            _startSymbol = new NonTerminal(startNonTerminal);
            return this;
        }

        public IProductionBuilder AddProduction()
        {
            _fromSymbols = new List<Symbol>();
            _toSymbols = new List<Symbol>();
            _currentSymbolsCollection = _fromSymbols;
            return this;
        }

        IProductionBuilder IProductionBuilder.Term(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var symbol = new Terminal(value);
            _currentSymbolsCollection!.Add(symbol);
            return this;
        }

        IProductionBuilder IProductionBuilder.NonTerm(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var symbol = new NonTerminal(value);
            _currentSymbolsCollection!.Add(symbol);
            return this;
        }

        IProductionBuilder IProductionBuilder.To()
        {
            _currentSymbolsCollection = _toSymbols;
            return this;
        }

        GrammarBuilder IProductionBuilder.Done()
        {
            _productions.Add(new Production(new SymbolCollection(_fromSymbols!), new SymbolCollection(_toSymbols!)));
            return this;
        }

        public Grammar Build()
        {
            ValidateAndThrow();
            return new Grammar(_startSymbol!, _productions);
        }

        private void ValidateAndThrow()
        {
            if (_startSymbol == null)
                throw new InvalidOperationException($"You need to call {nameof(StartIs)} to specify starting non terminal.");
        }
    }
}
