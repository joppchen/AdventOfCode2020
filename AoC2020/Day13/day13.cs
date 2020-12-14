using System;
using System.IO;
using AoC2020.Common;

namespace AoC2020.Day13
{
    internal static class Main
    {
        public static void Solve()
        {
            const string folder = "Day13";
            Console.WriteLine(folder);

            //var textFile = $"../../../{folder}/input.txt";
            //var textFile = $"../../../{folder}/example.txt";
            var textFile = $"../../../{folder}/exampleJP.txt";
            //var textFile = $"../../../{folder}/example1.txt";
            //var textFile = $"../../../{folder}/example5.txt";

            if (File.Exists(textFile))//
            {
                var lines = File.ReadAllLines(textFile);
                //var integers = SharedMethods.ParseStringArrayToInt(lines);

                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }

                /*var linesRaw = new string[lines.Length];
                lines.CopyTo(linesRaw, 0);*/

                //Solver.Task1(lines);
                //linesRaw.CopyTo(lines, 0);
                Solver.Task2(lines);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }
}