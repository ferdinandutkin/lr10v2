using System;
using System.Collections.Generic;
using System.Text;

namespace lr10v2
{
    static class IEExtension
    {
        public static bool TryFirst<T>(this IEnumerable<T> seq, Func<T, bool> pred, out T result)
        {
            result = default;
            foreach (var item in seq)
            {
                if (pred(item))
                {
                    result = item;
                    return true;
                }
            }
            return false;
        }

    }
  
}
