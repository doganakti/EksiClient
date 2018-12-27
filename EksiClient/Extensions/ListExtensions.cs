using System;
using System.Collections.Generic;

namespace EksiClient
{
    /// <summary>
    /// List extensions.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns a list with the object inside
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> AsList<T>(this T model)
        {
            var list = new List<T>();
#pragma warning disable RECS0017 // Possible compare of value type with 'null'
            if (model != null)
#pragma warning restore RECS0017 // Possible compare of value type with 'null'
            {
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// Hases the item.
        /// </summary>
        /// <returns><c>true</c>, if item was hased, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="TValue">The 1st type parameter.</typeparam>
        public static bool HasItem<TValue>(this List<TValue> list)
        {
            bool exists = list != null;
            if (exists)
            {
                if (list.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
