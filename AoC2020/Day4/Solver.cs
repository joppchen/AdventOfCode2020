using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace AoC2020.Day4
{
    internal static class Solver
    {
        public static void Task1(string passportBatch) // Answer: 235
        {
            var passports = passportBatch.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"No. of passports: {passports.Length}");

            var reqFields = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

            Task1Imperative(passports, reqFields);

            Task1OneLinerFunctional(passports, reqFields);
        }

        public static void Task2(string passportBatch)
        {
            var passports = passportBatch.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

            var reqFields = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

            var validPassports = 0;

            foreach (var passport in passports)
            {
                var valid = false;
                var matchRequirements = new[] {false, false, false, false, false, false, false};

                foreach (var field in reqFields)
                {
                    int index;
                    int intValue = 0;
                    string txtValue;
                    Match match;

                    switch (field)
                    {
                        case "byr":
                            index = passport.IndexOf("byr", StringComparison.Ordinal);
                            if (index < 0) break;
                            intValue = int.Parse(passport.Substring(index + 4, 4));
                            if (intValue >= 1920 && intValue <= 2002) matchRequirements[0] = true;
                            break;
                        case "iyr":
                            index = passport.IndexOf("iyr", StringComparison.Ordinal);
                            if (index < 0) break;
                            intValue = int.Parse(passport.Substring(index + 4, 4));
                            if (intValue >= 2010 && intValue <= 2020) matchRequirements[1] = true;
                            break;
                        case "eyr":
                            index = passport.IndexOf("eyr", StringComparison.Ordinal);
                            if (index < 0) break;
                            intValue = int.Parse(passport.Substring(index + 4, 4));
                            if (intValue >= 2020 && intValue <= 2030) matchRequirements[2] = true;
                            break;
                        case "hgt":
                            //Console.WriteLine(passport);
                            match = Regex.Match(passport, "hgt:[0-9]{2,3}(cm|in)");
                            if (!match.Success) break;
                            var foundString = match.Value;
                            if (passport.Contains("cid:214 iyr:2017 hcl:#866857 ecl:brn byr:1988 hgt:161cm"))
                            {
                                var svada = 0;
                                
                            }
                            intValue = int.Parse(Regex.Match(foundString, "[0-9]{2,3}").Value);

                            index = passport.IndexOf("hgt", StringComparison.Ordinal);
                            if (index < 0) break;
                            int index2 = 0;
                            var unit = "";

                            if (match.Value.Contains("cm"))
                            {
                                unit = "cm";
                                //index2 = passport.IndexOf("cm", StringComparison.Ordinal);
                            }

                            if (match.Value.Contains("in"))
                            {
                                unit = "in";
                                //index2 = passport.IndexOf("in", StringComparison.Ordinal);
                            }

                            //txtValue = passport.Substring(index + 4, 5);
                            switch (unit)
                            {
                                case "cm":
                                    //intValue = int.Parse(passport.Substring(index + 4, index2 - (index + 4)));
                                    if (intValue >= 150 && intValue <= 193) matchRequirements[3] = true;
                                    break;
                                case "in":
                                    //index2 = passport.IndexOf("in", StringComparison.Ordinal);
                                    //intValue = int.Parse(passport.Substring(index + 4, index2 - (index + 4)));
                                    if (intValue >= 59 && intValue <= 76) matchRequirements[3] = true;
                                    break;
                            }

                            break;
                        case "hcl":
                            match = Regex.Match(passport, "(#)([a-f]|[0-9]){6}");
                            if (!match.Success) break;
                            index = passport.IndexOf("hcl", StringComparison.Ordinal);
                            if (index < 0) break;
                            //Console.WriteLine(passport);
                            txtValue = passport.Substring(index + 4, 7);
                            match = Regex.Match(txtValue, "(#)([a-f]|[0-9]){6}");
                            if (match.Success) matchRequirements[4] = true;
                            break;
                        case "ecl":
                            index = passport.IndexOf("ecl", StringComparison.Ordinal);
                            if (index < 0) break;
                            txtValue = passport.Substring(index + 4, 3);
                            string[] eyeColors = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
                            if (eyeColors.Any(eyeColor => txtValue.Contains(eyeColor))) matchRequirements[5] = true;
                            break;
                        case "pid":
                            match = Regex.Match(passport, "pid:[0-9]{9}");
                            if (!match.Success) break;
                            index = passport.IndexOf("pid", StringComparison.Ordinal);
                            if (index < 0) break;
                            //Console.WriteLine(passport);
                            var subString = passport.Substring(index + 4, 9);
                            if (int.TryParse(subString, out intValue)) matchRequirements[6] = true;
                            break;
                    }
                }

                if (matchRequirements.All(x => x)) validPassports += 1;
            }

            Console.WriteLine($"Valid passports: {validPassports}");
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

            Console.WriteLine($"Valid passportBatch: {validPassports}");

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
            Console.WriteLine($"Valid passportBatch: {validNoOfPassports}");

            watchNew.Stop();
            var elapsedMsNew = watchNew.ElapsedMilliseconds;
            Console.WriteLine($"Time consumption new method: {elapsedMsNew} [ms]");
        }
    }
}