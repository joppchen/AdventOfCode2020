using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AoC2020.Common;

namespace AoC2020.Day9
{
    internal static class Solver
    {
        public static void Task1(long[] xmas) // Answer: 25918798
        {
            const int preAmbleSize = 25;
            //const int preAmbleSize = 5;

            var invalidNumber = xmas[InvalidXmasIndex(xmas, preAmbleSize)];
            Console.WriteLine($"Task 1: {invalidNumber}");
        }

        public static void Task2(long[] xmas)
        {
            const long targetInvalidNumber = 25918798;
            //const long targetInvalidNumber = 127;

            var subArray = ContiguousSetInArrayThatSumTo(xmas, targetInvalidNumber);
            var answer = subArray.Min() + subArray.Max();
            Console.WriteLine($"Task 2: {answer}");
        }

        private static long[] ContiguousSetInArrayThatSumTo(long[] arr, long goal) // Answer: 3340942
        {
            for (var i = 0; i < arr.Length; i++)
            {
                for (var j = i + 1; j < arr.Length; j++)
                {
                    var subArray = arr.RangeSubset(i, j - i + 1);
                    if (subArray.Sum() == goal) return subArray;
                }
            }

            return new long[0];
        }

        private static int InvalidXmasIndex(long[] xmas, int preAmbleSize)
        {
            for (var i = preAmbleSize; i < xmas.Length; i++)
            {
                var subarray = xmas.RangeSubset(i - preAmbleSize, preAmbleSize);
                var (index1, index2) = SharedMethods.TwoUniqueNumbersInArrayThatSumTo(subarray, xmas[i]);
                if (index1 < 0) return i;
            }

            return -1;
        }
    }
}