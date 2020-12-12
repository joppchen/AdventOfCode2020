﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day11
{
    internal static class Solver
    {
        private const char Empty = 'L';
        private const char Occupied = '#';
        private const char Floor = '.';

        public static void Task1(string[] layout) // Answer: 2281
        {
            var occupiedSeats = TotalNoOfOccupiedSeats(layout, NoOfOccupiedSeatsTask1, RulesTask1);
            Console.WriteLine($"Task 1: Occupied seats: {occupiedSeats}");
        }

        public static void Task2(string[] layout) // Answer: 2085
        {
            var occupiedSeats = TotalNoOfOccupiedSeats(layout, NoOfOccupiedSeatsTask2, RulesTask2);
            Console.WriteLine($"Task 2: Occupied seats: {occupiedSeats}");
        }

        private static int TotalNoOfOccupiedSeats(string[] layout,
            Func<string[], (int, int), int> surroundingOccupiedSeats, Func<char, int, char> rules)
        {
            var workLayout = new string[layout.Length];
            layout.CopyTo(workLayout, 0);

            var prevOccupiedSeats = 0;
            var occupiedSeats = 1;

            while (occupiedSeats != prevOccupiedSeats)
            {
                prevOccupiedSeats = occupiedSeats;

                workLayout = UpdateLayout(layout, surroundingOccupiedSeats, rules);

                occupiedSeats = workLayout.Sum(s => s.Count(ch => ch == Occupied));
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
                    if (layout[i][j] == Floor) continue;

                    newLine[j] = rules(layout[i][j], surroundingOccupiedSeats(layout, (i, j)));
                }

                workLayout[i] = string.Concat(newLine);
            }

            return workLayout;
        }

        private static char RulesTask1(char key, int occupiedSeats)
        {
            var newKey = key;

            if (occupiedSeats == 0 && key == Empty) newKey = Occupied;
            if (occupiedSeats >= 4 && key == Occupied) newKey = Empty;
            return newKey;
        }

        private static char RulesTask2(char key, int occupiedSeats)
        {
            var newKey = key;

            if (occupiedSeats == 0 && key == Empty) newKey = Occupied;
            if (occupiedSeats >= 5 && key == Occupied) newKey = Empty;
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
                    if (layout[i][j] == Occupied) count += 1;
                }
            }

            return count;
        }

        private static int NoOfOccupiedSeatsTask2(string[] layout, (int, int) position)
        {
            var count = 0;
            var (row, col) = position;

            var rowMax = layout.Length - 1;
            var colMax = layout[0].Length - 1;

            // When looking a direction: look all the way till the end until an L or # is hit

            // Check the row look to the right
            for (var j = col + 1; j <= colMax; j++)
            {
                if (CountOccupiedSeat(layout, row, j, ref count)) break;
            }

            // Check the row look to the left (count down)
            for (var j = col - 1; j >= 0; j--)
            {
                if (CountOccupiedSeat(layout, row, j, ref count)) break;
            }

            // Check the col look down
            for (var i = row + 1; i <= rowMax; i++)
            {
                if (CountOccupiedSeat(layout, i, col, ref count)) break;
            }

            // Check the col look up (count down)
            for (var i = row - 1; i >= 0; i--)
            {
                if (CountOccupiedSeat(layout, i, col, ref count)) break;
            }

            // Check the diagonals
            // Diag 1
            for (var i = row + 1; i <= rowMax; i++)
            {
                var j = col + (i - row);
                if (j > colMax) break;
                if (CountOccupiedSeat(layout, i, j, ref count)) break;
            }

            // Diag 2
            for (var i = row + 1; i <= rowMax; i++)
            {
                var j = col - (i - row);
                if (j < 0) break;
                if (CountOccupiedSeat(layout, i, j, ref count)) break;
            }

            // Diag 3
            for (var i = row - 1; i >= 0; i--)
            {
                var j = col - (row - i);
                if (j < 0) break;
                if (CountOccupiedSeat(layout, i, j, ref count)) break;
            }

            // Diag 4
            for (var i = row - 1; i >= 0; i--)
            {
                var j = col + (row - i);
                if (j > colMax) break;
                if (CountOccupiedSeat(layout, i, j, ref count)) break;
            }

            return count;
        }

        private static bool CountOccupiedSeat(IReadOnlyList<string> layout, int row, int j, ref int count)
        {
            if (!EmptyOrOccupied(layout[row][j])) return false;
            if (layout[row][j] == Occupied) count += 1;
            return true;
        }

        private static bool EmptyOrOccupied(char key)
        {
            return key == Occupied || key == Empty;
        }
    }
}