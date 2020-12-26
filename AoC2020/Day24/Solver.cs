using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC2020.Common;

namespace AoC2020.Day24
{
    internal enum Players
    {
        Player1 = 1,
        Player2 = 2
    }

    internal static class Solver
    {
        public static void Task1(string[] instructions) // Answer: 34005
        {
            var blackTiles = new Dictionary<int[], int>(new IntArrayEqualityComparer());
            var counter = 0;
            foreach (var instruction in instructions)
            {
                counter++;
                if (counter == 17) counter = 17;
                //Console.WriteLine($"Instruction no. {counter}: '{instruction}'");
                //Console.WriteLine(instruction);
                var directions = Regex.Matches(instruction.ToLower(), "(ne)|(e)|(se)|(sw)|(w)|(nw)");
                var tilePosition = new[] {0, 0};

                foreach (Match direction in directions)
                {
                    tilePosition = AddVectorElements(tilePosition, VectorizeDirectionString(direction.Value));
                }

                //Console.WriteLine(tilePosition[0] + ", " + tilePosition[1]);
                FlipTile(blackTiles, tilePosition);
            }

            var numberOfBlackTiles = blackTiles.Sum(x => x.Value);
            Console.WriteLine($"Task 1: Number of black tiles: {numberOfBlackTiles}");
        }

        private static int[] VectorizeDirectionString(string directionValue)
        {
            return directionValue switch
            {
                "ne" => new[] {1, 1},
                "e" => new[] {2, 0},
                "se" => new[] {1, -1},
                "sw" => new[] {-1, -1},
                "w" => new[] {-2, 0},
                "nw" => new[] {-1, 1},
                _ => throw new ArgumentException($"Direction '{directionValue}' is not supported.")
            };
        }

        private static int[] AddVectorElements(int[] a, int[] b)
        {
            return a.Zip(b, (x0, x1) => x0 + x1).ToArray();
        }

        private static void FlipTile(IDictionary<int[], int> blackTiles, int[] position)
        {
            if (!blackTiles.ContainsKey(position))
            {
                blackTiles.Add(position, 1);
                //Console.WriteLine("Flip first time");
            }
            else
            {
                if (blackTiles[position].Equals(1))
                {
                    //Console.WriteLine("Flip back to white");
                    blackTiles[position] = 0;
                }
                else
                {
                    Console.WriteLine("Flip back to black");
                    blackTiles[position] = 1;
                }
            }
        }

        private class IntArrayEqualityComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                if (x.Length != y.Length)
                {
                    return false;
                }

                for (var i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(int[] obj)
            {
                int result = 17;
                for (int i = 0; i < obj.Length; i++)
                {
                    unchecked
                    {
                        result = result * 23 + obj[i];
                    }
                }

                return result;
            }
        }
    }
}