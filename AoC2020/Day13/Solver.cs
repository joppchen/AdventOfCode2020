using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Transactions;
using System.Xml.XPath;
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

        private static (long[], long[]) FindBusesAndOffsetsLong(string[] instructions)
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
            // Regn ut m som funksjon av s og gcd --> begge resultater av GcdExtended
            // TODO: Sjekk om bussene bør sorteres i stignede rekkefølge - kan gjøres ved testing
            // TODO: Legg inn sjekk om at g divides with (phi_A - phi_B) - kanskje ikke nødvendig for oppgaven som lover at det finnes en løsning

            long red = 7; //30; //9; //4; //30; //15;
            long green = 6; //38; //15; // //38; //9;
            long phaseRed = 0; //6; //3;
            long phaseGreen = 3; //-6; //-3; //1; //0;
            long advantage = 3;

            var pCommon = Lcm(red, green);
            var (g, (s, t), (coeff1, coeff2)) = GcdExtended(red, green);
            var z = (-phaseRed + phaseGreen) / g;
            var m = z * s;
            var n = -z * t;
            var x1 = (m * red + phaseRed) % pCommon;
            var y1 = (n * green + phaseGreen) & pCommon;

            var x2 = Mod(m * red + phaseRed, pCommon);
            var y2 = Mod(n * green + phaseGreen, pCommon);

            var x3 = -(m * red + phaseRed) % pCommon;
            var y3 = -(n * green + phaseGreen) % pCommon;

            var x4 = Mod(-(m * red + phaseRed), pCommon);
            var y4 = Mod(-(n * green + phaseGreen), pCommon);

            var svada = 0;


            /*var (buses, offsets) = FindBusesAndOffsets(instructions);

            //var lcmCommon = LcmMulti(buses.Select(Convert.ToInt64).ToArray());
            var lcmCommon = Lcm(buses[0], buses[1]);
            Console.WriteLine(lcmCommon);

            long gcd;
            (long, long) bezout;
            (long, long) gcdQuotients;

            (gcd, bezout, gcdQuotients) = GcdExtended(buses[0], buses[1]);
            var (gcd2, (s, t), (svada1, svada2)) = GcdExtended(buses[0], buses[1]);

            var z = (offsets[0] - offsets[1]) / gcd;
            var m = bezout.Item1;
            var x = (m * buses[0] - offsets[0]) % lcmCommon;
            x = mod(m * buses[0] - offsets[0], lcmCommon);*/

            /*long a = 240;
            long b = 46;
            lcmCommon = LcmMulti(new[] {a, b});
    
            (gcd, bezout, gcdQ) = GcdExtended(a, b);*/

            //Console.WriteLine(gcd);
        }

        public static void Task2C(string[] instructions) //       672 754 131 923 874 is correct
            //       138 320 488 030 126 is too low :(
            // 9 223 372 036 854 775 807 is long.MaxValue
        {
            TestLcmWithOffset();
            // Regn ut lcm av alle bussene --> hvordan? Trenger jeg en (standard) lcm-algoritme også?
            //     --> look up lcm algorithms, e.g. combined_period = a_period // gcd * b_period (python)
            //    LCM(a,b) = (a×b)/GCF(a,b)
            //    LCM(4, 6, 7) = LCM(LCM(4, 6), 7) (https://www.calculatorsoup.com/calculators/math/lcm.php)
            // Regn ut m som funksjon av s og gcd --> begge resultater av GcdExtended
            // TODO: Sjekk om bussene bør sorteres i stignede rekkefølge - kan gjøres ved testing
            // TODO: Legg inn sjekk om at g divides with (phi_A - phi_B) - kanskje ikke nødvendig for oppgaven som lover at det finnes en løsning

            /*long pA = 7; //30; //9; //4; //30; //15;
            long pB = 6; //38; //15; // //38; //9;
            long phaseA = 0; //6; //3;
            long phaseB = 3; //-6; //-3; //1; //0;

            var answer = LcmWithOffset(pA, pB, phaseA, phaseB);
            //Console.WriteLine(answer);
            
            Console.WriteLine($"20 = {LcmWithOffset(4, 7, 0, 1)}"); // 20
            Console.WriteLine($"21 = {LcmWithOffset(7, 6, 0, 3)}"); // 21 Here treating relative differences (between the two numbers)
            Console.WriteLine($"20 = {LcmWithOffset(7, 6, 1, 4)}"); // 20 Here treating absolute differences (between the numbers and the origin)
            Console.WriteLine($"18 = {LcmWithOffset(9, 15, 0, -3)}"); // 18
            Console.WriteLine($"120 = {LcmWithOffset(30, 38, 0, -6)}"); // 120*/


            var (buses, offsets) = FindBusesAndOffsetsLong(instructions);

            //var lcmCommon = LcmMulti(buses.Where(x => x != 7).Select(Convert.ToInt64).ToArray());
            //var lcmCommon = LcmMulti(buses);
            //var lcmCommon = Lcm(buses[0], buses[1]);

            //(gcd, bezout, gcdQuotients) = GcdExtended(buses[0], buses[1]);
            //var (gcd_old, (s, t), (_, _)) = GcdExtended(buses[0], buses.Last());
            //var (gcd, s, t) = GcdExtendedMulti(buses);

            // Regn ut x for de to første bussene
            // Gjør disse om til én samle-buss med periode t + phaseB, og fase = 0;
            // Regn ut x for samle-bussen og den neste
            // Fortsett til siste buss er håndtert
            var busA = buses[0];
            var phaseA = offsets[0];
            long busB = 0;
            long phaseB = 0;
            long lcmCommon = busA;
            long xA = 0;
            for (var i = 1; i < buses.Length; i++)
            {
                //busA = buses[i - 1];
                busB = buses[i];
                if (busB == 353)
                {
                    var svada = 0;
                }

                //phaseA = offsets[i - 1];
                phaseB = offsets[i]; // - offsets[i - 1]; // Relative offset to preceding bus

                lcmCommon = Lcm(busA, busB);
                var (gcd, (s, t), _) = GcdExtended(busA, busB);
                var z = (phaseB - phaseA) /
                        gcd; // TODO: Add check that this integer division leaves no remains (see Python example code)
                //var z = Mod(phaseB - phaseA, gcd);
                var m = s * z;
                var n = -t * z;
                //xA = Mod(-(m * busA + phaseA), lcmCommon);
                BigInteger tempInput = (BigInteger) m * (BigInteger) busA + (BigInteger) phaseA;
                //xA = ModBig(-(m * busA + phaseA), lcmCommon);
                xA = ModBig(-(tempInput), lcmCommon);
                //var xB = Mod(-(n * busB + phaseB), lcmCommon);
                //tempInput = (BigInteger) n * (BigInteger) busB + (BigInteger) phaseB;
                //var xB = ModBig(-(n * busB + phaseB), lcmCommon);
                //var xB = ModBig(-(tempInput), lcmCommon);
                //if (xA != xB) Console.WriteLine($"xA ({xA}) is not equal to xB ({xB}). Something is wrong.");
                if (i == buses.Length - 1) break;
                busA = lcmCommon;
                phaseA = -xA;
            }

            Console.WriteLine($"Step for first sync: {xA}");
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

        private static long LcmWithOffset(long pA, long pB, long phaseA, long phaseB)
        {
            /*long pA = 7; //30; //9; //4; //30; //15;
            long pB = 6; //38; //15; // //38; //9;
            long phaseA = 0; //6; //3;
            long phaseB = 3; //-6; //-3; //1; //0;*/

            var pCommon = Lcm(pA, pB);
            var (g, (s, t), (coeff1, coeff2)) = GcdExtended(pA, pB);
            var z = (-phaseA + phaseB) / g;
            var m = z * s;
            var n = -z * t;

            var firstStepInSyncA = Mod(-(m * pA + phaseA), pCommon);
            var firstStepInSyncB = Mod(-(n * pB + phaseB), pCommon); // Shall be equal to firstStepInSyncA

            return firstStepInSyncA;
        }

        private static long Mod(long a, long n)
        {
            var result = a % n;
            if ((result < 0 && n > 0) || (result > 0 && n < 0))
            {
                result += n;
            }

            return result;
        }

        private static long ModBig(BigInteger a, BigInteger n)
        {
            var result = a % n;
            if ((result < 0 && n > 0) || (result > 0 && n < 0))
            {
                result += n;
            }

            return (long) result;
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
            return a * (b / GcdExtended(a, b).Item1);
        }

        private static (long, long, long) GcdExtendedMulti(long[] ints)
        {
            var gcdCommon = ints[0];
            long sCommon = 0;
            long tCommon = 0;

            //1
            var (gcd1, (s1, t1), _) = GcdExtended(ints[0], ints[1]);

            //2
            var (gcd2, (s2, t2), _) = GcdExtended(gcd1, ints[2]);

            //3
            //var (gcd3, s3, t3) = GcdExtended(gcd2, ints[3]);

            for (var i = 0; i < ints.Length; i++)
            {
                (gcdCommon, (sCommon, tCommon), _) = GcdExtended(gcdCommon, ints[i]);
            }

            return (gcd2, s2, t2);
            return (gcdCommon, sCommon, tCommon);
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
            var gcdQuotients = (Math.Abs(s), Math.Abs(t)); // TODO: Might not need these for Day 13 Task 2

            return (gcd, bezoutCoefficients, gcdQuotients);
        }
    }
}