using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2020.Day3
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Day 3:");

            var solver = new solver();

            string textFile = "../../../Day3/input.txt";
            //string textFile ="Day3/inputExample.txt";

            if (File.Exists(textFile))
            {
                string[] lines = File.ReadAllLines(textFile);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }

                //var route = (1, 3);
                //var count = solver.task1(lines, route);
                solver.task2(lines);
            }
            else
            {
                Console.WriteLine("Fant IKKE filen");
            }
        }
    }

    class solver
    {
        public int task1(string[] map, (int down, int right) route)
        {
            int down = route.down;
            int right = route.right;

            int height = map.Length;
            int width = map[0].Length;

            int movesToBottom = (height - 1) / down;
            int minCanvasWidth = movesToBottom * right;
            int mapsToAdd = (int) (minCanvasWidth / width);
            Console.WriteLine(mapsToAdd);

            // Move in map:
            int row = 0;
            int col = 0;
            int move = 0;
            string tree = "#";
            int treeCount = 0;
            while (move < movesToBottom)
            {
                row += down;
                col += right;

                // Check if inside map
                if (col >= width)
                {
                    col = col - width;
                }

                if (row > height - 1)
                {
                    break;
                }

                // Sjekk for tre
                if (map[row][col].ToString().Equals(tree))
                {
                    treeCount += 1;
                }
            }

            Console.WriteLine($"Tree count: {treeCount}");
            return treeCount;
        }

        public void task2(string[] map)
        {
            var routes = new List<(int down, int right)>()
            {
                (1, 1),
                (1, 3),
                (1, 5),
                (1, 7),
                (2, 1)
            };

            var count = 0;
            var treeCount = 0;
            long product = 1;
            foreach (var route in routes)
            {
                count = task1(map, route);
                //Console.WriteLine(count);
                treeCount += count;
                //Console.WriteLine(treeCount);
                product = product * (long) count;
                //Console.WriteLine(product);
            }

            Console.WriteLine($"Tree count: {treeCount}");
            Console.WriteLine($"Product: {product}");
        }
    }
}