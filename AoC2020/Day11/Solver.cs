using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day11
{
    internal static class Solver
    {
        private const string Empty = "L";
        private const string Occupied = "#";
        private const string Floor = ".";

        public static void Task1(string[] layout) // Answer: 2281
        {
            var occupiedSeats = TotalNoOfOccupiedSeats(layout, NoOfOccupiedSeatsTask1, RulesTask1);
            Console.WriteLine($"Task 1: Occupied seats: {occupiedSeats}");
        }

        public static void Task2(string[] layout)
        {
            var occupiedSeats = TotalNoOfOccupiedSeats(layout, NoOfOccupiedSeatsTask2, RulesTask2);
            Console.WriteLine($"Task 2: Occupied seats: {occupiedSeats}");
        }

        private static int TotalNoOfOccupiedSeats(string[] layout,
            Func<string[], (int, int), int> surroundingOccupiedSeats, Func<char, int, char> Rules)
        {
            var workLayout = new string[layout.Length];
            layout.CopyTo(workLayout, 0);

            var prevOccupiedSeats = 0;
            var occupiedSeats = 1;

            while (occupiedSeats != prevOccupiedSeats)
            {
                prevOccupiedSeats = occupiedSeats;

                workLayout = UpdateLayout(layout, surroundingOccupiedSeats, Rules);

                occupiedSeats = workLayout.Sum(s => s.Count(ch => ch.ToString().Equals(Occupied)));
                workLayout.CopyTo(layout, 0);
            }

            return occupiedSeats;
        }

        private static string[] UpdateLayout(string[] layout, Func<string[], (int, int), int> surroundingOccupiedSeats,
            Func<char, int, char> rules)
        {
            var workLayout = new string[layout.Length];

            for (var i = 0; i < layout.Length; i++)
            {
                var newLine = layout[i].ToCharArray();
                for (var j = 0; j < layout[0].Length; j++)
                {
                    if (layout[i][j].ToString().Equals(Floor)) continue;

                    newLine[j] = rules(layout[i][j], surroundingOccupiedSeats(layout, (i, j)));
                }

                workLayout[i] = string.Concat(newLine);
            }

            return workLayout;
        }

        private static char RulesTask1(char key, int occupiedSeats)
        {
            var newKey = key;

            if (occupiedSeats == 0 && key.ToString() == Empty) newKey = Occupied.ToCharArray()[0];
            if (occupiedSeats >= 4 && key.ToString() == Occupied) newKey = Empty.ToCharArray()[0];
            return newKey;
        }

        private static char RulesTask2(char key, int occupiedSeats)
        {
            var newKey = key;

            if (occupiedSeats == 0 && key.ToString() == Empty) newKey = Occupied.ToCharArray()[0];
            if (occupiedSeats >= 5 && key.ToString() == Occupied) newKey = Empty.ToCharArray()[0];
            return newKey;
        }

        private static int NoOfOccupiedSeatsTask1(string[] layout, (int, int) position)
        {
            var count = 0;

            var iLow = 1;
            var iHigh = 1;
            var jLow = 1;
            var jHigh = 1;

            var (row, col) = position;
            if (row == 0) iLow = 0;
            if (row == layout.Length - 1) iHigh = 0;
            if (col == 0) jLow = 0;
            if (col == layout[0].Length - 1) jHigh = 0;

            for (var i = row - iLow; i < row + iHigh + 1; i++)
            {
                for (var j = col - jLow; j < col + jHigh + 1; j++)
                {
                    if (i == row && j == col) continue;
                    if (layout[i][j].ToString().Equals(Occupied)) count += 1;
                }
            }

            return count;
        }

        private static int NoOfOccupiedSeatsTask2(string[] layout, (int, int) position)
        {
            return 1337; // TODO: implement procedure
        }
    }
}