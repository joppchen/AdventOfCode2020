using System;
using System.IO;

namespace AoC2020.Day6
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Day 6:");

            var solver = new Solver();

            Console.WriteLine(Directory.GetCurrentDirectory());
            const string textFile = "../../../Day6/input.txt";
            //const string textFile = "../../../Day6/inputExample.txt";

            if (File.Exists(textFile))
            {
                var lines = File.ReadAllText(textFile);
                /*foreach (string line in lines){
                  Console.WriteLine(line);
                }*/

                solver.Task1(lines);
                //solver.task2(lines);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }
}