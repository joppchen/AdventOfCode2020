using System;
using System.IO;
using AoC2020.Common;

namespace AoC2020.Day9
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine($"Day 9:");
            const string folder = "Day9";

            Console.WriteLine(Directory.GetCurrentDirectory());

            var textFile = $"../../../{folder}/input.txt";
            //var textFile = $"../../../{folder}/inputExample.txt";

            if (File.Exists(textFile))
            {
                var lines = File.ReadAllLines(textFile);
                var longs = SharedMethods.ParseStringArrayToLong(lines);

                /*foreach (var integer in integers)
                {
                    Console.WriteLine(integer);
                }*/

                Solver.Task1(longs);
                Solver.Task2(longs);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }
}