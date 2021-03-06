using System;
using System.IO;
using AoC2020.Common;

namespace AoC2020.Day1
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine($"Day 1:");
            const string folder = "Day1";

            Console.WriteLine(Directory.GetCurrentDirectory());

            var textFile = $"../../../{folder}/input.txt";
            //var textFile = $"../../../{folder}/inputExample.txt";

            if (File.Exists(textFile))
            {
                var lines = File.ReadAllLines(textFile);
                var integers = SharedMethods.ParseStringArrayToInt(lines);

                /*foreach (var integer in integers)
                {
                    Console.WriteLine(integer);
                }*/

                Console.WriteLine("Task 1:");
                Solver.Task1(integers);

                Console.WriteLine("Task 2:");
                Solver.Task2(integers);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }
}