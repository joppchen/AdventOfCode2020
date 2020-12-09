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

        internal static (int, int) TwoUniqueNumbersInArrayThatSumTo(int[] arr, int goal)
        {
            foreach (var integer in arr)
            {
                for (var j = 1; j < arr.Length; j++)
                {
                    if (integer + arr[j] == goal) return (integer, arr[j]);
                }
            }

            throw new InvalidOperationException($"No two numbers in {nameof(arr)} sums to {goal}.");
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

        // create a subset from a range of indices
        public static T[] RangeSubset<T>(this T[] array, int startIndex, int length)
        {
            T[] subset = new T[length];
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