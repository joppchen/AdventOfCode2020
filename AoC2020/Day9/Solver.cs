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

            var invalidNumber = xmas[InvalidXmasIndex(xmas, preAmbleSize)];
            Console.WriteLine(invalidNumber);
        }

        private static int InvalidXmasIndex(long[] xmas, int preAmbleSize)
        {
            for (var i = preAmbleSize; i < xmas.Length; i++)
            {
                var subarray = xmas.RangeSubset(xmas, i - preAmbleSize, preAmbleSize);
                var (index1, index2) = SharedMethods.TwoUniqueNumbersInArrayThatSumTo(subarray, xmas[i]);
                if (index1 < 0) return i;
            }

            return -1;
        }
    }
}