using System.Collections.Generic;

namespace DummyVirtualMachine.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T Item, int Index)> WithIndexes<T>(this IEnumerable<T> items)
        {
            int index = 0;
            foreach (T item in items)
            {
                yield return (item, index);
                index += 1;
            }
        }
    }
}
