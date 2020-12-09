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
}