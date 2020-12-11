using System;
using System.IO;
using AoC2020.Common;

namespace AoC2020.Day11
{
    internal static class Main
    {
        public static void Solve()
        {
            const string folder = "Day11";
            Console.WriteLine(folder);

            var textFile = $"../../../{folder}/input.txt";
            //var textFile = $"../../../{folder}/example.txt";

            if (File.Exists(textFile))
            {
                var lines = File.ReadAllLines(textFile);
                //var integers = SharedMethods.ParseStringArrayToInt(lines);

                /*foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }*/

                Solver.Task1(lines);
                //Solver.Task2(longs);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }
}