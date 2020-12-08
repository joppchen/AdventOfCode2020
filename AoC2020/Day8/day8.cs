using System;
using System.IO;

namespace AoC2020.Day8
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine($"Day 8:");
            const string folder = "Day8";

            //var solver = new Solver();

            Console.WriteLine(Directory.GetCurrentDirectory());

            var textFile = $"../../../{folder}/input.txt";
            //var textFile = $"../../../{folder}/inputExample.txt";

            if (File.Exists(textFile))
            {
                var lines = File.ReadAllLines(textFile);
                /*foreach (string line in lines){
                  Console.WriteLine(line);
                }*/

                //Solver.Task1(lines);
                Solver.Task2(lines);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }
}