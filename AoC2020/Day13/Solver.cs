using System;
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
                var minutes = Convert.ToInt32((Math.Ceiling((double) timeEstimate / buses[i]) * buses[i]) - timeEstimate);
                minutesToNextDeparture[i] = minutes;
                if (minutesToNextDeparture[i] >= minimumWaitTime) continue;
                minimumWaitTime = minutesToNextDeparture[i];
                firstBus = buses[i];
            }
            
            Console.WriteLine(minimumWaitTime);
            Console.WriteLine(firstBus);
            Console.WriteLine($"Task 1: Answer: {minimumWaitTime * firstBus}");
        }
    }
}