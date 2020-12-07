using System;
using System.IO;

namespace AoC2020.Day7
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Day 7:");

            var solver = new Solver();

            Console.WriteLine(Directory.GetCurrentDirectory());
            const string textFile = "../../../Day7/input.txt";
            //const string textFile = "../../../Day7/inputExample.txt";
            //const string textFile = "../../../Day7/inputExample2.txt";

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