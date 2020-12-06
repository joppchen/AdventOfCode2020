using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2020.Day5
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Day 5:");

            var solver = new solver();


            Console.WriteLine(Directory.GetCurrentDirectory());
            string textFile = "../../../a/input.txt";
            //string textFile ="a/inputExample.txt";

            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile);
                //foreach (string line in lines){
                //  Console.WriteLine(line);
                //}

                solver.task1(lines);
                //solver.task2(lines);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }

    class solver
    {
        public void task1(string[] BSPs)
        {
            var allRows = (low: 0, high: 128);
            var allCols = (low: 0, high: 7);
            var highestSeatID = 0;
            var seatIDs = new List<int>();

            foreach (var bsp in BSPs)
            {
                var rowRng = allRows;
                for (int i = 0; i < 7; i++)
                {
                    string key = bsp[i].ToString();
                    if (key.Equals("F"))
                    {
                        rowRng = lowerHalf(rowRng);
                    }

                    if (key.Equals("B"))
                    {
                        rowRng = upperHalf(rowRng);
                    }
                }

                var colRng = allCols;
                for (int i = 7; i < 10; i++)
                {
                    string key = bsp[i].ToString();
                    if (key.Equals("L"))
                    {
                        colRng = lowerHalf(colRng);
                    }

                    if (key.Equals("R"))
                    {
                        colRng = upperHalf(colRng);
                    }
                }

                var seatID = rowRng.low * 8 + colRng.low;

                seatIDs.Add(seatID);
                if (seatID > highestSeatID) highestSeatID = seatID;
            }

            Console.WriteLine($"Highest seat ID: {highestSeatID}");

            seatIDs.Sort();
            for (var i = 0; i < seatIDs.Count - 1; i++)
            {
                if (seatIDs[i + 1] - seatIDs[i] > 1)
                {
                    Console.WriteLine(seatIDs[i]);
                    Console.WriteLine(seatIDs[i + 1]);
                    var mySeat = (seatIDs[i] + seatIDs[i + 1]) / 2;
                    Console.WriteLine($"My seat: {mySeat}");
                    break;
                }
            }
        }

        private (int low, int high) lowerHalf((int low, int high) range)
        {
            var width = range.high - range.low;
            range.high = range.high - (width + 1) / 2;
            return range;
        }

        private (int low, int high) upperHalf((int low, int high) range)
        {
            var width = range.high - range.low;
            range.low = range.low + (width + 1) / 2;
            return range;
        }
    }
}