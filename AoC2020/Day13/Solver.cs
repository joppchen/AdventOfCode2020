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

        public static void Task2(string[] instructions) // Answer:
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var (buses, offsets) = FindBusesAndOffsets(instructions);

            var diffsCalculated = new int[offsets.Length];
            var diffOfDiffs = new int[offsets.Length];

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
                    multipliers[i] = FindMultiplier(timeEstimate + offsets[i], buses[i]);
                    multipliersDouble[i] = FindMultiplierDouble(timeEstimate + offsets[i], buses[i]);
                }

                for (var i = 1; i < buses.Length; i++)
                {
                    diffsCalculated[i] = multipliers[i] * buses[i] - multipliers[0] * buses[0];
                }

                for (var i = 0; i < buses.Length; i++)
                {
                    diffOfDiffs[i] = diffsCalculated[i] - offsets[i];
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

        private static (int[], int[]) FindBusesAndOffsets(string[] instructions)
        {
            var busesStringArray = instructions[1].Split(',');

            var busList = new List<int>();
            var offsetList = new List<int>();

            for (var i = 0; i < busesStringArray.Length; i++)
            {
                if (busesStringArray[i].Equals("x")) continue;
                busList.Add(int.Parse(busesStringArray[i]));
                offsetList.Add(i);
            }

            var buses = busList.ToArray();
            var offsets = offsetList.ToArray();
            return (buses, offsets);
        }

        private static int FindMultiplier(int tEst, int id)
        {
            return Convert.ToInt32(Math.Ceiling((double) tEst / id));
        }

        private static double FindMultiplierDouble(double tEst, double id)
        {
            return tEst / id;
        }

        public static void Task2B(string[] instructions)
        {
            // Regn ut lcm av alle bussene --> hvordan? Trenger jeg en (standard) lcm-algoritme også?
            //     --> look up lcm algorithms, e.g. combined_period = a_period // gcd * b_period (python)
            //    LCM(a,b) = (a×b)/GCF(a,b)
            //    LCM(4, 6, 7) = LCM(LCM(4, 6), 7) (https://www.calculatorsoup.com/calculators/math/lcm.php)
            // Regn ut m som funksjon av s og gcd --> begge resultater av extended_gcd
            // TODO: Sjekk om bussene bør sorteres i stignede rekkefølge - kan gjøres ved testing
            // TODO: Legg inn sjekk om at g divides with (phi_A - phi_B) - kanskje ikke nødvendig for oppgaven som lover at det finnes en løsning

            var (buses, offsets) = FindBusesAndOffsets(instructions);

            long a = 240;
            long b = 46;
            long lcmCommon = LcmMulti(new[] {a, b});
            Console.WriteLine(lcmCommon);
            //(a, b) = (b, a);

            long gcd;
            (long, long) bezout;
            (long, long) gcdQ;

            (gcd, bezout, gcdQ) = extended_gcd(a, b);

            Console.WriteLine(gcd);
        }

        private static long LcmMulti(long[] ints)
        {
            //long lcmCommon;
            //     --> look up lcm algorithms, e.g. combined_period = a_period // gcd * b_period (python)
            //    LCM(a,b) = (a×b)/GCF(a,b)
            //    LCM(4, 6, 7) = LCM(LCM(4, 6), 7) (https://www.calculatorsoup.com/calculators/math/lcm.php)
            /*var a = ints[0];
            var b = ints[1];
            var c = ints[2];
            var d = ints[3];*/
            /*var ab = Lcm(ints[0], ints[1]);
            var abc = Lcm(ab, ints[2]);
            var abcd = Lcm(abc, ints[3]);*/
            var lcmCommon = ints[0];

            for (var i = 1; i < ints.Length; i++)
            {
                lcmCommon = Lcm(lcmCommon, ints[i]);
            }

            return lcmCommon;
        }

        private static long Lcm(long a, long b)
        {
            return a * (b / extended_gcd(a, b).Item1);
        }

        /// <summary>
        /// Calculates Greatest Common Denominator using the extended Euclidean Algorithm: https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
        /// This can be used to calculate the Least Common Multiple (LCM) with offset: https://math.stackexchange.com/questions/2218763/how-to-find-lcm-of-two-numbers-when-one-starts-with-an-offset
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
        private static (long, (long, long), (long, long)) extended_gcd(long a, long b)
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
            var gcdQuotients = (Math.Abs(s), Math.Abs(t)); // TODO: Might not need these for Day 13 Task 2

            return (gcd, bezoutCoefficients, gcdQuotients);
        }
    }
}