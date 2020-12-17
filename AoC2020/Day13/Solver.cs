using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AoC2020.Common;

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

        private static (long[], long[]) FindBusesAndOffsetsLong(IReadOnlyList<string> instructions)
        {
            var busesStringArray = instructions[1].Split(',');

            var busList = new List<long>();
            var offsetList = new List<long>();

            for (var i = 0; i < busesStringArray.Length; i++)
            {
                if (busesStringArray[i].Equals("x")) continue;
                busList.Add(long.Parse(busesStringArray[i]));
                offsetList.Add(i);
            }

            var buses = busList.ToArray();
            var offsets = offsetList.ToArray();
            return (buses, offsets);
        }

        public static void Task2(string[] instructions) // Answer: 672 754 131 923 874
        {
            TestLcmWithOffset();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var (buses, offsets) = FindBusesAndOffsetsLong(instructions);

            Console.WriteLine($"Task 2: Step for first sync: {FirstStepInSyncMultiSignal(buses, offsets)}");

            watch.Stop();
            Console.WriteLine($"Time consumption Task 2: {watch.ElapsedMilliseconds} [ms]");
        }

        /// <summary>
        /// Finds first step multiple signals with start offsets are in sync using lcm with offset
        /// </summary>
        /// <param name="periods">Array of periods of the input signals</param>
        /// <param name="offsets">Array of offsets compared to the first signal</param>
        /// <returns>First step (counting from zero) when all signals are in sync, accounting for offsets/phase shifts</returns>
        private static long FirstStepInSyncMultiSignal(IReadOnlyList<long> periods, IReadOnlyList<long> offsets)
        {
            var periodA = periods[0];
            var phaseA = offsets[0];
            long firstStepInSync = 0;
            for (var i = 1; i < periods.Count; i++)
            {
                var periodB = periods[i];
                var phaseB = offsets[i];

                var (x, combinedPeriod) = LcmWithOffset(periodA, periodB, phaseA, phaseB);
                firstStepInSync = x;

                periodA = combinedPeriod;
                phaseA = -firstStepInSync;
            }

            return firstStepInSync;
        }

        private static void TestLcmWithOffset()
        {
            Console.WriteLine($"20 = {LcmWithOffset(4, 7, 0, 1)}"); // 20
            Console.WriteLine(
                $"21 = {LcmWithOffset(7, 6, 0, 3)}"); // 21 Here treating relative differences (between the two numbers)
            Console.WriteLine(
                $"20 = {LcmWithOffset(7, 6, 1, 4)}"); // 20 Here treating absolute differences (between the numbers and the origin)
            Console.WriteLine($"18 = {LcmWithOffset(9, 15, 0, -3)}"); // 18
            Console.WriteLine($"120 = {LcmWithOffset(30, 38, 0, -6)}"); // 120
        }

        /// <summary>
        /// Least Common Multiplier with offset
        /// A slightly modified version of what is described here: https://math.stackexchange.com/questions/2218763/how-to-find-lcm-of-two-numbers-when-one-starts-with-an-offset
        /// Considering the phase as number of steps left to reach its references point, thus positive offsets/phases
        /// </summary>
        /// <param name="pA">Period of first entry</param>
        /// <param name="pB">Period of second entry</param>
        /// <param name="phaseA">Phase/offset of first entry</param>
        /// <param name="phaseB">Phase/offset of second entry</param>
        /// <returns>Tuple: (First step when the two signals are in sync, Least Common Multiple of the two periods)</returns>
        /// <exception cref="Exception">Thrown if the two signals will never sync</exception>
        private static (long, long) LcmWithOffset(long pA, long pB, long phaseA, long phaseB)
        {
            var (gcd, (s, _), (_, _)) = GcdExtended(pA, pB);
            var phaseDiff = phaseB - phaseA;
            if (phaseDiff % gcd != 0)
                throw new Exception(
                    "(phaseB - phaseA) does not integer divide gcd --> the two signals will never sync");
            var z = phaseDiff / gcd;
            var m = z * s;

            var pCommon = Lcm(pA, pB, gcd);
            // This intermediate calculation exceeds long.MaxValue and returns garbage (but does not fail) if using long instead of BigInteger:
            var firstStepInSync = SharedMethods.ModBig(-(m * (BigInteger) pA + phaseA), pCommon);

            return (firstStepInSync, pCommon);
        }

        /// <summary>
        /// Least Common Multiplier of two numbers
        /// </summary>
        /// <param name="a">First integer</param>
        /// <param name="b">Second integer</param>
        /// <param name="gcd">Optional: Greatest Common Denominator (gcd) if available (to save computational time)</param>
        /// <returns>lcm - Least Common Multiplier</returns>
        private static long Lcm(long a, long b, long gcd = 0)
        {
            return a * (b / (gcd == 0 ? GcdExtended(a, b).Item1 : gcd));
        }

        /// <summary>
        /// Calculates Greatest Common Denominator using the extended Euclidean Algorithm: https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
        /// This can be used to calculate the Least Common Multiple (LCM) with offset.
        /// The extended Euclidean algorithm is an extension to the Euclidean algorithm, and computes,
        /// in addition to the greatest common divisor (gcd) of ints a and b,
        /// also the coefficients of Bézout's identity, which are ints x and y such that
        /// a*s + b*t = gcd(a,b).
        /// Both a > b and b > a is handled. Note that b > a yields one extra step in the Euclidean algorithm
        /// (and in the extended one) where the values of a and b as effectively flipped
        /// </summary>
        /// <param name="a">First integer. Note that argument as int is automatically parsed to long.</param>
        /// <param name="b">Second integer. Note that argument as int is automatically parsed to long.</param>
        /// <returns>A triple consisting of one long and two tuples, all elements of type long:
        /// (The greatest common denominator, Bézout coefficient:('s', 't'), Quotients by the gcd:(a/gcd, b/gcd))</returns>
        private static (long, (long, long), (long, long)) GcdExtended(long a, long b)
        {
            long oldR, oldS, oldT, r, s, t;

            (oldR, r) = (a, b);
            (oldS, s) = (1, 0);
            (oldT, t) = (0, 1);


            while (r != 0)
            {
                var quotient = oldR / r;
                (oldR, r) = (r, oldR - quotient * r);
                (oldS, s) = (s, oldS - quotient * s);
                (oldT, t) = (t, oldT - quotient * t);
            }

            var gcd = oldR;
            var bezoutCoefficients = (old_s: oldS, old_t: oldT);
            var gcdQuotients = (Math.Abs(s), Math.Abs(t)); // Do not need these for Day 13 Task 2

            return (gcd, bezoutCoefficients, gcdQuotients);
        }
    }
}