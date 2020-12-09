using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Common
{
    internal static class SharedMethods
    {
        internal static int[] ParseStringArrayToInt(string[] integerStrings)
        {
            var integers = new int[integerStrings.Length];
            for (var n = 0; n < integerStrings.Length; n++) integers[n] = int.Parse(integerStrings[n]);
            return integers;
        }

        internal static long[] ParseStringArrayToLong(string[] integerStrings)
        {
            var integers = new long[integerStrings.Length];
            for (var n = 0; n < integerStrings.Length; n++) integers[n] = long.Parse(integerStrings[n]);
            return integers;
        }

        /// <summary>
        /// Find corresponding indices of two unique integers in array that sums to the specified number
        /// </summary>
        /// <param name="arr">Integer array in which to sum two and two integers</param>
        /// <param name="goal">Specified number/sum to search for</param>
        /// <returns>Tuple with counters index 1 and index 2 (of the numbers that sum to goal)</returns>
        internal static (int, int) TwoUniqueNumbersInArrayThatSumTo(int[] arr, int goal)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                for (var j = 1; j < arr.Length; j++)
                {
                    if (i == j) continue;
                    if (arr[i] + arr[j] == goal) return (i, j);
                }
            }

            return (-1, -1);
        }

        /// <summary>
        /// Find corresponding indices of two unique longs in array that sums to the specified number
        /// </summary>
        /// <param name="arr">Long array in which to sum two and two longs</param>
        /// <param name="goal">Specified number/sum to search for </param>
        /// <returns>Tuple with counters index 1 and index 2 (of the numbers that sum to goal)</returns>
        internal static (int, int) TwoUniqueNumbersInArrayThatSumTo(long[] arr, long goal)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                for (var j = 1; j < arr.Length; j++)
                {
                    if (i == j) continue;
                    if (arr[i] + arr[j] == goal) return (i, j);
                }
            }

            return (-1, -1);
        }
    }

    internal static class ArrayExtensions
    {
        // pre-populate array with same value at all indices
        public static void Populate<T>(this T[] arr, T value)
        {
            for (var i = 0; i < arr.Length; ++i)
            {
                arr[i] = value;
            }
        }

        /// <summary>
        /// Create a subset from a range of indices
        /// </summary>
        /// <param name="array">Input array</param>
        /// <param name="index"></param>
        /// <param name="startIndex">Start index of subarray</param>
        /// <param name="length">Length of subarray</param>
        /// <typeparam name="T">Type of array, should work on most types of arrays</typeparam>
        /// <returns>Subset array of 'array'</returns>
        public static T[] RangeSubset<T>(this T[] array, long[] index, long startIndex, int length)
        {
            var subset = new T[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }

        // create a subset from a specific list of indices
        public static T[] Subset<T>(this T[] array, params int[] indices)
        {
            T[] subset = new T[indices.Length];
            for (int i = 0; i < indices.Length; i++)
            {
                subset[i] = array[indices[i]];
            }

            return subset;
        }
    }

    internal static class ListExtensions
    {
        public static void RemoveDuplicates<T>(this List<T> list)
        {
            var enumerable = (IEnumerable<T>) list;

            ICollection<T> withoutDuplicates = new HashSet<T>(enumerable);

            list.Clear();
            list.AddRange(withoutDuplicates.ToList());
        }
    }
}