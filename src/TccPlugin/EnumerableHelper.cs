using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TccPlugin
{
    public static class EnumerableHelper
    {
        /// <summary>
        /// Create a sequence from a variable list of entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static IEnumerable<T> Enumerate<T>(params T[] objects)
        {
            foreach (var obj in objects)
            {
                yield return obj;
            }
        }
    }
}
