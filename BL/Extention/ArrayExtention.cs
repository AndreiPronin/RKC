using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class ArrayExtention
    {
        public static string TryGetValue(this string[] Value, int index)
        {
            try
            {
                return Value[index];
            }
            catch
            {
                return "";
            }
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var known = new HashSet<TKey>();
            return source.Where(element => known.Add(keySelector(element)));
        }
    }
}
