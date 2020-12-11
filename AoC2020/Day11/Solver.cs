using System;
using System.Diagnostics.Tracing;
using System.Linq;

namespace AoC2020.Day11
{
    internal static class Solver
    {
        public static void Task1(string[] layout) // Answer: 2281
        {
            var empty = "L";
            var occupied = "#";
            var floor = ".";

            var rounds = 7;
            var counter = 0;
            
            var workLayout = new string[layout.Length];
            layout.CopyTo(workLayout, 0);
            //workLayout = layout;

            var prevOccupiedSeats = 0;
            var occupiedSeats = 1;

            while (occupiedSeats != prevOccupiedSeats)
            {
                prevOccupiedSeats = occupiedSeats;
                
                counter += 1;
                Console.WriteLine("");
                Console.WriteLine($"{counter}: ");

                //Console.WriteLine(layout[2][3]);

                for (var i = 0; i < layout.Length; i++)
                {
                    char[] newLine = layout[i].ToCharArray();
                    for (var j = 0; j < layout[0].Length; j++)
                    {
                        //corners: 3 naboer
                        //edges: 5 naboer
                        //else: 8 naboer
                        if (layout[i][j].ToString().Equals(floor)) continue;

                        var num = NoOfOccupiedAdjacentSeats(layout, (i, j));
                        //Console.WriteLine(num);
                        if (num == 0 && layout[i][j].ToString() == empty) newLine[j] = occupied.ToCharArray()[0];
                        if (num >= 4 && layout[i][j].ToString() == occupied) newLine[j] = empty.ToCharArray()[0];
                    }

                    workLayout[i] = string.Concat(newLine);
                    //layout[i] = string.Concat(newLine);
                }

                /*foreach (var line in workLayout)
                {
                    Console.WriteLine(line);
                }*/

                occupiedSeats = workLayout.Sum(s => s.Count(ch => ch.ToString().Equals(occupied)));
                Console.WriteLine(occupiedSeats);
                //if (occupiedSeats == prevOccupiedSeats) break;
                
                
                workLayout.CopyTo(layout, 0);
            }

            //Console.WriteLine($"Task 1: {sum1 * sum3}");
        }

        private static int NoOfOccupiedAdjacentSeats(string[] layout, (int, int) position)
        {
            int count = 0;
            var empty = "L";
            var occupied = "#";
            var floor = ".";
            var symbols = new[] {empty, occupied, floor};

            int iLow = 1;
            int iHigh = 1;
            int jLow = 1;
            int jHigh = 1;

            // if corner
            if (position.Item1 == 0) iLow = 0;
            if (position.Item1 == layout.Length - 1) iHigh = 0;
            if (position.Item2 == 0) jLow = 0;
            if (position.Item2 == layout[0].Length - 1) jHigh = 0;
            // if edge
            for (var i = position.Item1 - iLow; i < position.Item1 + iHigh + 1; i++)
            {
                //if (i == position.Item1) continue;
                for (var j = position.Item2 - jLow; j < position.Item2 + jHigh + 1; j++)
                {
                    if (i == position.Item1 && j == position.Item2) continue;
                    if (layout[i][j].ToString().Equals(occupied)) count += 1;
                }
            }

            return count;
        }
    }
}