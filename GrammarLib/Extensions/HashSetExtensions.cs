using System.Collections.Generic;

namespace GrammarLib.Extensions
{
    public static class HashSetExtensions
    {
        public static void AddRange<T>(this HashSet<T> set,  IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                set.Add(item);
            }
        }
    }
}
