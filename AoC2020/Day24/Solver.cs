using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC2020.Common;

namespace AoC2020.Day24
{
    internal static class Solver
    {
        public static void Task1(IEnumerable<string> instructions) // Answer: 465
        {
            var registeredTiles = GetBlackTiles(instructions);

            var numberOfBlackTiles = registeredTiles.Sum(x => x.Value);
            Console.WriteLine($"Task 1: Number of black registeredTiles: {numberOfBlackTiles}");
        }

        public static void Task2(IEnumerable<string> instructions) // Answer: 4078
        {
            var registeredTiles = GetBlackTiles(instructions);

            for (var i = 1; i <= 100; i++)
            {
                AddWhiteTilesAdjacentToBlackTiles(registeredTiles);

                var tilesToFlip = new Dictionary<int[], int>(new IntArrayEqualityComparer());
                foreach (var (tilePosition, value) in registeredTiles)
                {
                    if (tilesToFlip.ContainsKey(tilePosition)) continue;
                    var adjacentBlackTiles = NumberOfAdjacentBlackTiles(registeredTiles, tilePosition);
                    switch (value)
                    {
                        case 1:
                            if (adjacentBlackTiles == 0 || adjacentBlackTiles > 2) tilesToFlip.Add(tilePosition, value);
                            break;
                        case 0:
                            if (adjacentBlackTiles == 2) tilesToFlip.Add(tilePosition, value);
                            break;
                    }
                }

                var positionsToFlip = tilesToFlip.Keys.ToArray();
                foreach (var pos in positionsToFlip) FlipTile(registeredTiles, pos);
            }

            var numberOfBlackTiles = registeredTiles.Sum(x => x.Value);
            Console.WriteLine($"Task 2: Number of black tiles day 100: {numberOfBlackTiles}");
        }

        private static void AddWhiteTilesAdjacentToBlackTiles(Dictionary<int[], int> registeredTiles)
        {
            var adjacentDirections = new[] {"ne", "e", "se", "sw", "w", "nw"};
            var tilePositions = registeredTiles.Keys.ToList();

            foreach (var tilePosition in tilePositions)
            {
                foreach (var adjacentDirection in adjacentDirections)
                {
                    var adjacentPosition = AddVectorElements(tilePosition, VectorizeDirectionString(adjacentDirection));
                    if (!registeredTiles.ContainsKey(adjacentPosition)) registeredTiles.Add(adjacentPosition, 0);
                }
            }
        }

        private static int NumberOfAdjacentBlackTiles(IReadOnlyDictionary<int[], int> registeredTiles, int[] position)
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

        private static IEnumerable<int> VectorizeDirectionString(string directionValue)
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

        private static int[] AddVectorElements(IEnumerable<int> a, IEnumerable<int> b)
        {
            return a.Zip(b, (x0, x1) => x0 + x1).ToArray();
        }

        private static void FlipTile(IDictionary<int[], int> tiles, int[] position)
        {
            // Used for Task 1, a bit smelly to keep when using this method also for Task 2, but it works:
            if (!tiles.ContainsKey(position)) tiles.Add(position, 1);

            else
            {
                if (tiles[position] == 1) tiles[position] = 0;

                else tiles[position] = 1;
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