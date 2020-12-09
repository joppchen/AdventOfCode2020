using System;
using System.IO;

namespace AoC2020.Day4
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Day 4:");
            const string folder = "Day4";

            var textFile = $"../../../{folder}/input.txt";
            //var textFile = $"../../../{folder}/inputExample.txt";

            if (File.Exists(textFile))
            {
                var text = File.ReadAllText(textFile);
                Console.WriteLine(text.Length);
                Solver.Task1(text);
                //solver.task2(text);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }
}