using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using AoC2020.Common;

namespace AoC2020.Day23
{
    internal static class Solver
    {
        public static void Task1(string inputStr) // Answer: 38756249
        {
            var input = inputStr.Select(x => int.Parse(x.ToString())).ToList();

            var counter = 0;
            var i = 0;

            while (counter < 100)
            {
                counter++;
                //Console.WriteLine($"-- move {counter} --");

                var currentCup = input[i];
                //Console.WriteLine("cups:  {0}", string.Join("\t", input));
                //Console.WriteLine($"current cup: {currentCup}");

                var pickUp = PickUpThreeCups(input, i + 1);
                //Console.WriteLine("pick up: {0}", string.Join("\t", pickUp));

                input.RemoveAll(x => pickUp.Any(y => y == x)); // Remove picked up cups

                var destinationCup = DestinationCup(input, currentCup);
                //Console.WriteLine($"destination: {destinationCup}");
                //Console.WriteLine();

                input.InsertRange(input.IndexOf(destinationCup) + 1, pickUp); // Insert after destination cup

                RotateToGetCurrentCupAtCorrectIndex(counter, input, currentCup);

                i = input.IndexOf(currentCup) + 1;
                if (i > input.Count - 1) i = 0;
            }

            var result = GetResultFromFinalCupOrder(input);
            Console.WriteLine("Task 1: {0}", string.Join("", result));
        }

        private static void RotateToGetCurrentCupAtCorrectIndex(int counter, List<int> input, int currentCup)
        {
            var targetIndex = counter - 1;
            var actualIndex = input.IndexOf(currentCup);
            var diff = actualIndex - targetIndex;
            if (diff < 0) return;
            var moveToEnd = input.GetRange(0, diff);
            input.RemoveRange(0, diff);
            input.AddRange(moveToEnd);
        }

        private static List<int> GetResultFromFinalCupOrder(List<int> input)
        {
            var result = new List<int>();
            var startIndex = input.IndexOf(1) + 1;
            for (var i = startIndex; i < input.Count + startIndex - 1; i++)
            {
                result.Add(input[RotationalIndex(input, i)]);
            }

            return result;
        }

        private static List<int> PickUpThreeCups(List<int> input, in int startIndex)
        {
            var pickUp = new List<int>();
            for (var i = 0; i < 3; i++)
            {
                pickUp.Add(input[RotationalIndex(input, startIndex + i)]);
            }

            return pickUp;
        }

        private static int RotationalIndex(List<int> input, in int indexIn)
        {
            if (indexIn > input.Count - 1) return indexIn - (input.Count);
            return indexIn;
        }

        private static int DestinationCup(List<int> input, in int currentCup)
        {
            var destinationCup = currentCup - 1;
            while (!input.Contains(destinationCup))
            {
                destinationCup -= 1;
                if (destinationCup < input.Min()) return input.Max();
            }

            return destinationCup;
        }
    }
}