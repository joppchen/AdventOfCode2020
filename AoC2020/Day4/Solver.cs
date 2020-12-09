using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AoC2020.Day4
{
    internal static class Solver
    {
        public static void Task1(string passports) // Answer: 235
        {
            var passPorts = passports.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"No. of passports: {passPorts.Length}");

            var reqFields = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

            Task1Imperative(passPorts, reqFields);

            Task1OneLinerFunctional(passPorts, reqFields);
        }

        /// <summary>
        /// Does the same as 'Task1OneLinerFunctional', but is much faster: Consistently takes ~0 ms to run
        /// </summary>
        private static void Task1Imperative(IEnumerable<string> passPorts, string[] reqFields)
        {
            var watchOld = System.Diagnostics.Stopwatch.StartNew();

            var validPassports = 0;
            foreach (var passPort in passPorts)
            {
                var valid = false;

                foreach (var field in reqFields)
                {
                    valid = passPort.Contains(field);
                    if (!valid) break;
                }

                if (valid) validPassports += 1;
            }

            Console.WriteLine($"Valid passports: {validPassports}");

            watchOld.Stop();
            var elapsedMsOld = watchOld.ElapsedMilliseconds;
            Console.WriteLine($"Time consumption new method: {elapsedMsOld} [ms]");
        }

        /// <summary>
        /// Does the same as 'Task1Imperative', but is much slower: Takes anywhere between 5 and 25 ms
        /// </summary>
        private static void Task1OneLinerFunctional(IEnumerable<string> passPorts, string[] reqFields)
        {
            var watchNew = System.Diagnostics.Stopwatch.StartNew();

            var validNoOfPassports =
                passPorts.Select(passPort => reqFields.All(passPort.Contains)).Count(isValid => isValid);
            Console.WriteLine($"Valid passports: {validNoOfPassports}");

            watchNew.Stop();
            var elapsedMsNew = watchNew.ElapsedMilliseconds;
            Console.WriteLine($"Time consumption new method: {elapsedMsNew} [ms]");
        }
    }
}