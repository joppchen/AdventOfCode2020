using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AoC2020.Day10
{
    internal static class Solver
    {
        public static void Task1(IEnumerable<long> adapters) // Answer: 2664
        {
            var usedAdapters = JoltageDiffDistribution(adapters);

            var sum1 = usedAdapters.Count(x => x.Item2 == 1);
            var sum3 = usedAdapters.Count(x => x.Item2 == 3);

            Console.WriteLine($"Task 1: {sum1 * sum3}");
        }

        public static void Task2(long[] adapters) // Answer: 148 098 383 347 712
        {
            var allAdapters = adapters.Append(adapters.Max() + 3).Append(0);
            var sortedAdapters = allAdapters.ToImmutableSortedSet(); //Assuming all adapters are unique
            var adaptersCount = new long[sortedAdapters.Count];
            adaptersCount[0] = 1; // Initialize count, assuming first adapter only have one possible arrangement :)

            for (var index = 0; index < sortedAdapters.Count; index++)
            {
                var possibleAdapters = FindPossibleAdapters(index, sortedAdapters);

                for (var i = 0; i < possibleAdapters.Count; i++)
                {
                    adaptersCount[possibleAdapters[i].Item1] += adaptersCount[index];
                }
            }

            var possibleArrangements = adaptersCount.Last();
            Console.WriteLine($"Day 2: Total number of possible arrangements: {possibleArrangements}");
        }

        private static List<(long, long)> FindPossibleAdapters(long currentAdapterIndex, IReadOnlyList<long> sortedAdapters)
        {
            var possibleAdapters = new List<(long, long)>();

            for (var i = currentAdapterIndex + 1; i < sortedAdapters.Count(); i++)
            {
                var diff = sortedAdapters[(Index) i] - sortedAdapters[(Index) currentAdapterIndex];
                if (diff <= 3)
                {
                    possibleAdapters.Add((i, sortedAdapters[(Index) i]));
                }

                if (diff > 3) break;
            }

            return possibleAdapters;
        }

        private static List<(long, long)> JoltageDiffDistribution(IEnumerable<long> adapters)
        {
            var sortedAdapters = adapters.ToImmutableSortedSet();
            var usedAdapters = new List<(long, long)>();
            long currentRating = 0;

            foreach (var adapterRating in sortedAdapters)
            {
                var diff = adapterRating - currentRating;
                if (diff <= 3 && diff >= 0)
                {
                    usedAdapters.Add((adapterRating, diff));
                }

                currentRating = adapterRating;
            }

            usedAdapters.Add((sortedAdapters.Max + 3, 3)); // My device's built-in joltage difference = 3

            return usedAdapters;
        }
    }
}