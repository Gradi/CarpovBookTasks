using System.Collections.Generic;
using System.Linq;

namespace GrammarLib.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool AllIsTerminals(this IEnumerable<Symbol> symbols) => symbols.All(s => s is Terminal);

        public static bool AllIsNonTerminals(this IEnumerable<Symbol> symbols) => symbols.All(s => s is NonTerminal);

        public static bool IsSingleTerminal(this IEnumerable<Symbol> symbols) => symbols.SingleOrDefault() is Terminal;

        public static bool IsSingleNonTerminal(this IEnumerable<Symbol> symbols) => symbols.SingleOrDefault() is NonTerminal;

        public static bool IsSingle(this IEnumerable<Symbol> symbols, Symbol target)
        {
            var left = symbols.SingleOrDefault();
            return left != null && left == target;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items) => new HashSet<T>(items);

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            foreach (IEnumerable<T> items in enumerable)
            {
                foreach (T item in items)
                {
                    yield return item;
                }
            }
        }
    }
}
