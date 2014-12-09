using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieService.Core.Helpers
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null)
            {
                foreach (var item in enumerable)
                {
                    action(item);
                }
            }
        }

        public static List<T> Many<T>(this IEnumerable<T> enumerable, Func<T, bool> where)
        {
            return enumerable.Where(where).ToList();
        }
    }
}
