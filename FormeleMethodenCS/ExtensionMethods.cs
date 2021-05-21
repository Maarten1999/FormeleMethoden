using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    public static class ExtensionMethods
    {
        public static void AddAll<T>(this SortedSet<T> thisSet, SortedSet<T> set)
        {
            foreach (T t in set)
                thisSet.Add(t);
        }
    }
}
