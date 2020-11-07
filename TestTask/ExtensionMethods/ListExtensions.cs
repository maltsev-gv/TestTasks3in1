using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestTask.ExtensionMethods
{
    public static class ListExtensions
    {
        /// <summary>
        /// ForEach для любого IEnumerable
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (var item in source)
                    action(item);
            }
        }

        public static void RemoveAll<T>(this HashSet<T> source, Predicate<T> match)
        {
            List<T> list = new List<T>();
            foreach (T item in source)
            {
                if (match(item))
                {
                    list.Add(item);
                }
            }
            list.ForEach(id => source.Remove(id));
        }

        public static List<T> ToList<T>(this ICollection source)
        {
            return new List<T>(source.OfType<T>());
        }

        public static T[] ToArray<T>(this ICollection source)
        {
            return source?.OfType<T>().ToArray();
        }
    }
}
