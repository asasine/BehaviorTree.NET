using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree.NET
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
            => source.Where(element => element != null);

        public static IEnumerable<TOther> SelectWhereIs<T, TOther>(this IEnumerable<T> source)
            where TOther : class, T
            => source.Select(element => element as TOther).WhereNotNull();
    }
}
