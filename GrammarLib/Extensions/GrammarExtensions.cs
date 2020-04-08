using System;
using System.Collections.Generic;
using System.Linq;

namespace GrammarLib.Extensions
{
    public static class GrammarExtensions
    {
        /// <summary>
        /// Returns a new reduced version of <paramref name="grammar"/>
        /// </summary>
        public static Grammar Reduce(this Grammar grammar)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns collection of derivable symbols
        /// </summary>
        public static HashSet<Symbol> GetDerivableSymbols(this Grammar grammar)
        {
            var derivableSymbols = new HashSet<Symbol>();
            var currentDerivableSymbols = new HashSet<Symbol>();
            var nextDerivableSymbols = new HashSet<Symbol>();

            currentDerivableSymbols.Add(grammar.StartSymbol);
            bool isTrue = true;
            while (isTrue)
            {
                foreach (var symbol in currentDerivableSymbols)
                {
                    nextDerivableSymbols.AddRange(grammar.Productions
                        .Where(p => p.Left.IsSingle(symbol))
                        .Select(p => p.Right as IEnumerable<Symbol>)
                        .Flatten());
                }
                derivableSymbols.AddRange(currentDerivableSymbols);
                currentDerivableSymbols.Clear();

                isTrue = false;
                foreach (var newSymbol in nextDerivableSymbols)
                {
                    if (derivableSymbols.Add(newSymbol))
                    {
                        isTrue = true;
                        currentDerivableSymbols.Add(newSymbol);
                    }
                }
            }

            return derivableSymbols;
        }
    }
}
