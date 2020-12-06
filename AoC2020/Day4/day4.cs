using System;
using System.IO;

namespace AoC2020.Day4
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Day 4:");

            var solver = new solver();

            string textFile = "../../../Day4/input.txt";
            //string textFile ="Day4/inputExample.txt";
            if (File.Exists(textFile))
            {
                string text = File.ReadAllText(textFile);
                Console.WriteLine(text.Length);
                solver.task1(text);
                //solver.task2(text);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }

    class solver
    {
        public void task1(string passports)
        {
            string[] pports;

            pports = passports.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"No. of passports: {pports.Length}");

            string[] reqFields = new string[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
            var validPassports = 0;

            foreach (var p in pports)
            {
                bool valid = false;

                foreach (string field in reqFields)
                {
                    valid = p.Contains(field);
                    if (!valid) break;
                }

                if (valid) validPassports += 1;
            }

            Console.WriteLine($"Valid passports: {validPassports}");
        }
    }
}