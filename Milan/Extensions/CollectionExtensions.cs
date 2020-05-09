using System.Collections.Generic;

namespace Milan.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            foreach (T item in source)
            {
                target.Add(item);
            }
        }
    }
}
