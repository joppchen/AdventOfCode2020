using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AoC2020.Day10
{
    internal static class Solver
    {
        public static void Task1(IEnumerable<int> adapters) // Answer: 2664
        {
            var sortedAdapters = adapters.ToImmutableSortedSet();

            var usedAdapters = new List<(int, int)>();

            var currentRating = 0;

            foreach (var adapterRating in sortedAdapters)
            {
                var diff = adapterRating - currentRating;
                if (diff <= 3 && diff >= 0)
                {
                    usedAdapters.Add((adapterRating, diff));
                }

                currentRating = adapterRating;
            }

            usedAdapters.Add((sortedAdapters.Max + 3, 3));

            var sum1 = usedAdapters.Count(x => x.Item2 == 1);
            var sum3 = usedAdapters.Count(x => x.Item2 == 3);

            Console.WriteLine($"Task 1: {sum1 * sum3}");
        }
    }
}