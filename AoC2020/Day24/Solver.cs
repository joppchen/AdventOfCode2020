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
        public static void Task1(IEnumerable<string> instructions) // Answer: 465
        {
            var registeredTiles = GetBlackTiles(instructions);

            var numberOfBlackTiles = registeredTiles.Sum(x => x.Value);
            Console.WriteLine($"Task 1: Number of black registeredTiles: {numberOfBlackTiles}");
        }

        public static void Task2(IEnumerable<string> instructions) // Answer: 
        {
            var registeredTiles = GetBlackTiles(instructions);

            for (var i = 1; i <= 100; i++)
            {
                AddWhiteTilesAdjacentToBlackTiles(registeredTiles);

                var tilesToFlip = new Dictionary<int[], int>(new IntArrayEqualityComparer());
                foreach (var (key, value) in registeredTiles)
                {
                    var numberOfAdjacentBlackTiles = NumberOfAdjacentBlackTiles(registeredTiles, key);
                    switch (value)
                    {
                        // Black tiles
                        case 1:
                        {
                            if (numberOfAdjacentBlackTiles == 0 || numberOfAdjacentBlackTiles > 2)
                            {
                                if (!tilesToFlip.ContainsKey(key)) tilesToFlip.Add(key, value);
                            }

                            break;
                        }
                        // White tiles
                        case 0:
                        {
                            if (numberOfAdjacentBlackTiles == 2)
                            {
                                if (!tilesToFlip.ContainsKey(key)) tilesToFlip.Add(key, value);
                            }

                            break;
                        }
                    }
                }

                var positionsToFlip = tilesToFlip.Keys.ToArray();
                foreach (var pos in positionsToFlip)
                {
                    FlipTile(registeredTiles, pos);
                }

                /*var numberOfBlackTiles = registeredTiles.Sum(x => x.Value);
                Console.WriteLine($"Task 2: Number of black tiles DAY {i}: {numberOfBlackTiles}");*/
            }

            var numberOfBlackTiles = registeredTiles.Sum(x => x.Value);
            Console.WriteLine($"Task 2: Number of black tiles day 100: {numberOfBlackTiles}");
        }

        private static Dictionary<int[], int> GenerateInterestingWhiteTiles(Dictionary<int[], int> registeredTiles)
        {
            var interestingWhiteTiles = new Dictionary<int[], int>(new IntArrayEqualityComparer());
            var adjacentDirections = new[] {"ne", "e", "se", "sw", "w", "nw"};

            foreach (var (key, _) in registeredTiles)
            {
                foreach (var adjacentDirection in adjacentDirections)
                {
                    var adjacentPosition = AddVectorElements(key, VectorizeDirectionString(adjacentDirection));

                    // If tile does not exist in registeredTiles, register it as white
                    if (!interestingWhiteTiles.ContainsKey(adjacentPosition))
                        interestingWhiteTiles.Add(adjacentPosition, 0);
                }
            }

            return interestingWhiteTiles;
        }

        private static void AddWhiteTilesAdjacentToBlackTiles(Dictionary<int[], int> registeredTiles)
        {
            var interestingWhiteTiles = new Dictionary<int[], int>(new IntArrayEqualityComparer());
            var adjacentDirections = new[] {"ne", "e", "se", "sw", "w", "nw"};

            var keys = registeredTiles.Keys.ToList();

            foreach (var key in keys)
            {
                foreach (var adjacentDirection in adjacentDirections)
                {
                    var adjacentPosition = AddVectorElements(key, VectorizeDirectionString(adjacentDirection));

                    // If tile does not exist in registeredTiles, register it as white
                    if (!registeredTiles.ContainsKey(adjacentPosition)) registeredTiles.Add(adjacentPosition, 0);
                }
            }
        }

        private static int NumberOfAdjacentBlackTiles(Dictionary<int[], int> registeredTiles, int[] position)
        {
            var adjacentDirections = new[] {"ne", "e", "se", "sw", "w", "nw"};
            var numberOfAdjacentBlackTiles = 0;

            foreach (var adjacentDirection in adjacentDirections)
            {
                var adjacentPosition = AddVectorElements(position, VectorizeDirectionString(adjacentDirection));
                if (!registeredTiles.ContainsKey(adjacentPosition)) continue;
                registeredTiles.TryGetValue(adjacentPosition, out var value);
                if (value == 1) numberOfAdjacentBlackTiles += 1; // Black registeredTiles
            }

            return numberOfAdjacentBlackTiles;
        }

        private static Dictionary<int[], int> GetBlackTiles(IEnumerable<string> instructions)
        {
            var blackTiles = new Dictionary<int[], int>(new IntArrayEqualityComparer());
            foreach (var instruction in instructions)
            {
                var tilePosition = TilePositionToFlip(instruction);

                FlipTile(blackTiles, tilePosition);
            }

            return blackTiles;
        }

        private static int[] TilePositionToFlip(string instruction)
        {
            var directions = Regex.Matches(instruction.ToLower(), "(ne)|(e)|(se)|(sw)|(w)|(nw)");
            var tilePosition = new[] {0, 0};

            foreach (Match direction in directions)
            {
                tilePosition = AddVectorElements(tilePosition, VectorizeDirectionString(direction.Value));
            }

            return tilePosition;
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
                    //Console.WriteLine("Flip back to black");
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