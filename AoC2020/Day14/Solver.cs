using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Day14
{
    internal static class Solver
    {
        public static void Task1(IEnumerable<string> initProgram) // Answer: 15172047086292
        {
            const string
                patternMemoryAddress =
                    "\\d+(?=\\])"; // Match one or more digits that are immediately followed by ']' (positive lookahead)
            var mask = string.Empty;
            var memDict = new Dictionary<int, long>();

            foreach (var line in initProgram)
            {
                if (line.Substring(0, 3).Equals("mas")) mask = line.Substring(7);

                if (!line.Substring(0, 3).Equals("mem")) continue;
                var address = int.Parse(Regex.Match(line, patternMemoryAddress).Value);
                WriteToMemory(memDict, address, CalculateMemoryValue(line, mask));
            }

            long sum;
            checked
            {
                sum = memDict.Sum(x => x.Value);
            }

            Console.WriteLine(sum);
        }

        private static void WriteToMemory(IDictionary<int, long> memDict, int address, long memoryValue)
        {
            if (!memDict.ContainsKey(address)) memDict.Add(address, memoryValue);
            else memDict[address] = memoryValue;
        }

        private static long CalculateMemoryValue(string line, string mask)
        {
            const string
                patternMemoryValue =
                    "(?<=\\s)(\\d+)"; // Match one or more digits that are immediately precedented by whitespace (positive lookbehind) -  Works if used for a memory insertion line

            var value = int.Parse(Regex.Match(line, patternMemoryValue).Value);
            var bitValue = DecimalTo36BitStr(value).ToCharArray();
            var maskValues = Regex.Matches(mask, "\\d");

            foreach (Match val in maskValues) bitValue[val.Index] = char.Parse(val.Value);

            return BinaryToDec(new string(bitValue));
        }

        private static string DecimalTo36BitStr(int number)
        {
            var result = string.Empty;

            while (number > 0)
            {
                result = number % 2 + result;
                number /= 2;
            }

            var zeroString = new string('0', 36 - result.Length);

            return zeroString + result;
        }

        private static long BinaryToDec(string input)
        {
            var array = input.ToCharArray();
            Array.Reverse(array);
            long sum = 0;

            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] != '1') continue;
                if (i == 0)
                {
                    checked
                    {
                        sum += 1;
                    }
                }
                else
                {
                    checked
                    {
                        sum += (long) Math.Pow(2, i);
                    }
                }
            }

            return sum;
        }
    }
}