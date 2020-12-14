using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day13
{
    internal static class Solver
    {
        public static void Task1(string[] instructions) // Answer: 3269
        {
            var timeEstimate = int.Parse(instructions[0]);
            var buses = instructions[1].Split(new[] {',', 'x'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var minutesToNextDeparture = new int[buses.Length];
            var firstBus = 0;
            var minimumWaitTime = 1000;

            for (var i = 0; i < buses.Length; i++)
            {
                var minutes =
                    Convert.ToInt32((Math.Ceiling((double) timeEstimate / buses[i]) * buses[i]) - timeEstimate);
                minutesToNextDeparture[i] = minutes;
                if (minutesToNextDeparture[i] >= minimumWaitTime) continue;
                minimumWaitTime = minutesToNextDeparture[i];
                firstBus = buses[i];
            }

            Console.WriteLine(minimumWaitTime);
            Console.WriteLine(firstBus);
            Console.WriteLine($"Task 1: Answer: {minimumWaitTime * firstBus}");
        }

        public static void Task2(string[] instructions) // Answer: 3269
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var busesString = instructions[1].Split(',');
            for (var i = 0; i < busesString.Length; i++)
            {
                if (busesString[i].Equals("x"))
                {
                    busesString[i] = "0";
                }
            }

            var busesCluttered = busesString.Select(int.Parse).ToArray();

            var diffsCluttered = Enumerable.Range(0, busesString.Length).ToArray();

            var busesList = new List<int>();
            var diffsList = new List<int>();

            // Remove all x's
            for (var i = 0; i < busesString.Length; i++)
            {
                if (busesCluttered[i] <= 0) continue;
                busesList.Add(busesCluttered[i]);
                diffsList.Add(diffsCluttered[i]);
            }

            var buses = busesList.ToArray();
            var diffs = diffsList.ToArray();
            var diffsCalculated = new int[diffs.Length];
            var diffOfDiffs = new int[diffs.Length];

            var multipliers = Enumerable.Repeat(1, buses.Length).ToArray();
            var multipliersDouble = Enumerable.Repeat(1.0, buses.Length).ToArray();

            var foundT = false;
            var allTsMatch = new int[buses.Length];
            allTsMatch[0] = 1;

            var timeEstimate = 9;
            var timeSolution = 0;
            var timeEstimates = new[] {20, 104};
            var counter = 0;
            while (!foundT)
            {
                timeEstimate += 1;
                //timeEstimate = timeEstimates[counter];
                counter += 1;
                for (var i = 0; i < buses.Length; i++)
                {
                    multipliers[i] = FindMultiplier(timeEstimate + diffs[i], buses[i]);
                    multipliersDouble[i] = FindMultiplierDouble(timeEstimate + diffs[i], buses[i]);
                }

                for (var i = 1; i < buses.Length; i++)
                {
                    diffsCalculated[i] = multipliers[i] * buses[i] - multipliers[0] * buses[0];
                }

                for (var i = 0; i < buses.Length; i++)
                {
                    diffOfDiffs[i] = diffsCalculated[i] - diffs[i];
                }

                if (diffOfDiffs.Sum() != 0) continue;
                foundT = multipliersDouble.All(x => Math.Abs((x % 1)) < 1e-13);
                timeSolution = timeEstimate;
                /*bool testBool = multipliersDouble.All(x => Math.Abs((x % 1)) < 1e-13);
                timeEstimate += 84;*/
            }

            Console.WriteLine($"Task2: Earliest matching time stamp: {timeSolution}");

            watch.Stop();
            var elapsedMsOld = watch.ElapsedMilliseconds;
            Console.WriteLine($"Time consumption new method: {elapsedMsOld} [ms]");
        }

        private static int FindMultiplier(int t_est, int Id)
        {
            return Convert.ToInt32(Math.Ceiling((double) t_est / Id));
        }

        private static double FindMultiplierDouble(double t_est, double Id)
        {
            return t_est / Id;
        }
    }
}