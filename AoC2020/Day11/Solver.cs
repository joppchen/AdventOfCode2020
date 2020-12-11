using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day11
{
    internal static class Solver
    {
        public static void Task1(string[] layout) // Answer: 2281
        {
            var occupiedSeats = OccupiedSeats(layout);
            Console.WriteLine($"Task 1: Occupied seats: {occupiedSeats}");
        }

        private static int OccupiedSeats(string[] layout)
        {
            const string empty = "L";
            const string occupied = "#";
            const string floor = ".";

            var workLayout = new string[layout.Length];
            layout.CopyTo(workLayout, 0);

            var prevOccupiedSeats = 0;
            var occupiedSeats = 1;

            while (occupiedSeats != prevOccupiedSeats)
            {
                prevOccupiedSeats = occupiedSeats;

                for (var i = 0; i < layout.Length; i++)
                {
                    var newLine = layout[i].ToCharArray();
                    for (var j = 0; j < layout[0].Length; j++)
                    {
                        if (layout[i][j].ToString().Equals(floor)) continue;

                        var num = NoOfOccupiedAdjacentSeats(layout, (i, j));

                        if (num == 0 && layout[i][j].ToString() == empty) newLine[j] = occupied.ToCharArray()[0];
                        if (num >= 4 && layout[i][j].ToString() == occupied) newLine[j] = empty.ToCharArray()[0];
                    }

                    workLayout[i] = string.Concat(newLine);
                }

                occupiedSeats = workLayout.Sum(s => s.Count(ch => ch.ToString().Equals(occupied)));
                workLayout.CopyTo(layout, 0);
            }

            return occupiedSeats;
        }

        private static int NoOfOccupiedAdjacentSeats(IReadOnlyList<string> layout, (int, int) position)
        {
            var count = 0;
            const string occupied = "#";

            var iLow = 1;
            var iHigh = 1;
            var jLow = 1;
            var jHigh = 1;

            var (row, col) = position;
            if (row == 0) iLow = 0;
            if (row == layout.Count - 1) iHigh = 0;
            if (col == 0) jLow = 0;
            if (col == layout[0].Length - 1) jHigh = 0;

            for (var i = row - iLow; i < row + iHigh + 1; i++)
            {
                for (var j = col - jLow; j < col + jHigh + 1; j++)
                {
                    if (i == row && j == col) continue;
                    if (layout[i][j].ToString().Equals(occupied)) count += 1;
                }
            }

            return count;
        }
    }
}