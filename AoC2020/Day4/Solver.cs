using System;

namespace AoC2020.Day4
{
    internal static class Solver
    {
        public static void Task1(string passports)
        {
            var passPorts = passports.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"No. of passports: {passPorts.Length}");

            var reqFields = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
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
        }
    }
}