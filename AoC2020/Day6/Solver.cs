using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace AoC2020.Day6
{
    internal class Solver
    {
        public void Task1(string allAnswers)
        {
            var groups = allAnswers.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            //Console.WriteLine(groups[1]);
            const string alpha = "abcdefghijklmnopqrstuvwxyz";
            
            //results.Populate(0);
            var results = new List<int[]>();

            foreach (var group in groups)
            {
                var groupStrip = group.Replace(System.Environment.NewLine, string.Empty);
                var result = new int[26];
                foreach (var letter in groupStrip)
                {
                    var index = alpha.IndexOf(letter);
                    result[index] = 1;
                }
                var sum = result.Sum();
                Console.WriteLine(sum);
                results.Add(result);
            }

            var total = results.Sum(item => item.Sum());
            Console.WriteLine(total);
        }
    }

    internal static class Extension
    {
        public static void Populate<T>(this T[] arr, T value)
        {
            for (var i = 0; i < arr.Length; ++i)
            {
                arr[i] = value;
            }
        }
    }
}