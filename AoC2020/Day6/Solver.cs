using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace AoC2020.Day6
{
    internal class Solver
    {
        public static void Task1(string allAnswers)
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
                foreach (var index in groupStrip.Select(letter => alpha.IndexOf(letter)))
                {
                    result[index] = 1;
                }

                var sum = result.Sum();
                //Console.WriteLine(sum);
                results.Add(result);
            }

            var total = results.Sum(item => item.Sum());
            Console.WriteLine(total);
        }

        public static void Task2(string allAnswers)
        {
            var groups = allAnswers.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

            const string alpha = "abcdefghijklmnopqrstuvwxyz";

            var results = new List<int[]>();

            foreach (var group in groups)
            {
                var persons = group.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                var result = new int[26];
                var resultPerPerson = new List<int[]>();

                foreach (var person in persons)
                {
                    var sub = new int[26];
                    foreach (var index in person.Select(letter => alpha.IndexOf(letter)))
                    {
                        if (index > -1) sub[index] = 1;
                    }
                    resultPerPerson.Add(sub);
                }
                // sum per letter
                for (var i = 0; i < result.Length; i++)
                {
                    if (resultPerPerson.Sum(item => item[i]) == resultPerPerson.Count) result[i] = 1;
                }

                results.Add(result);
            }

            var total = results.Sum(item => item.Sum());
            Console.WriteLine($"Number of questions to which everyone answered 'yes': {total}");
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