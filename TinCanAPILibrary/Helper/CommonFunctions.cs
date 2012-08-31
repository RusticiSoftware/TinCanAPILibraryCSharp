using System;
using System.Collections.Generic;

namespace RusticiSoftware.TinCanAPILibrary.Helper
{
    /// <summary>
    /// A collection of commonly used static functions
    /// </summary>
    public static class CommonFunctions
    {
        /// <summary>
        /// Returns an array with all entries lower cased
        /// </summary>
        /// <param name="array">The array to edit</param>
        /// <returns>A new array with all lower cased entries</returns>
        public static String[] ArrayToLower(String[] array)
        {
            if (array == null)
                return null;
            String[] lower = new String[array.Length];
            for (int i = 0; i < array.Length; i++)
                lower[i] = array[i].ToLower();
            return lower;
        }

        public static bool AreDictionariesEqual<K, V>(IDictionary<K, V> dict1, IDictionary<K, V> dict2)
        {
            if (dict1 == null || dict2 == null)
            {
                return dict1 == null && dict2 == null;
            }
            if (dict1.Count != dict2.Count)
            {
                return false;
            }

            foreach (K key in dict1.Keys)
            {
                if (!(dict2.ContainsKey(key) && Object.Equals(dict1[key], dict2[key])))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AreListsEqual<T>(IList<T> col1, IList<T> col2)
        {
            if (col1 == null || col2 == null)
            {
                return col1 == null && col2 == null;
            }
            if (col1.Count != col2.Count)
            {
                return false;
            }

            foreach (T item in col1)
            {
                if (!col2.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
