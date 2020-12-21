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
            var memDict = new Dictionary<long, long>();

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

        public static void Task2(string[] initProgram) // Answer: 4197941339968
        {
            const string
                patternMemoryAddress =
                    "\\d+(?=\\])"; // Match one or more digits that are immediately followed by ']' (positive lookahead)
            const string
                patternMemoryValue =
                    "(?<=\\s)(\\d+)"; // Match one or more digits that are immediately precedented by whitespace (positive lookbehind) -  Works if used for a memory insertion line
            var mask = string.Empty;
            var memory = new Dictionary<long, long>();

            foreach (var line in initProgram)
            {
                if (line.Substring(0, 3).Equals("mas")) mask = line.Substring(7);

                if (!line.Substring(0, 3).Equals("mem")) continue;
                var address = int.Parse(Regex.Match(line, patternMemoryAddress).Value);

                var bitAddress = DecimalTo36BitStr(address);
                var floatings = Regex.Matches(mask, "X");
                var maskElements = Regex.Matches(mask, "X|\\d");

                // Add mask to bit value of memory address
                var bitAddressMasked = AddMaskToBitValueTask2(maskElements, bitAddress);

                // # Calculate addresses from mask
                // if X create two addresses with X = 0 and X = 1, create all combinations of addresses, which is 2^num(X)

                var combinations = GetCombinations(new List<int> {0, 1}, floatings.Count).ToArray().ToArray();

                // Write value to each address
                var value = int.Parse(Regex.Match(line, patternMemoryValue).Value);

                foreach (var combination in combinations)
                {
                    var combinationArr = combination.ToArray();
                    var counter = 0;

                    var tempAddress = bitAddressMasked.ToCharArray();

                    foreach (Match floating in floatings)
                    {
                        tempAddress[floating.Index] = Convert.ToChar(combinationArr[counter].ToString());

                        var addressDecimalTemp = BinaryToDec(new string(tempAddress));
                        WriteToMemory(memory, addressDecimalTemp, value);
                        counter++;
                    }
                }
            }

            long sum;
            checked
            {
                sum = memory.Sum(x => x.Value);
            }

            Console.WriteLine(sum);
        }

        private static void WriteToMemory(IDictionary<long, long> memDict, long address, long memoryValue)
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

            AddMaskToBitValueTask1(maskValues, bitValue);

            return BinaryToDec(new string(bitValue));
        }

        private static void AddMaskToBitValueTask1(MatchCollection maskValues, char[] bitValue)
        {
            foreach (Match val in maskValues) bitValue[val.Index] = char.Parse(val.Value);
        }

        private static string AddMaskToBitValueTask2(MatchCollection maskValues, string bitValue)
        {
            var temp = bitValue.ToCharArray();
            foreach (Match val in maskValues)
            {
                if (val.Value.Equals("0")) continue;
                temp[val.Index] = char.Parse(val.Value);
            }

            return new string(temp);
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

        private static IEnumerable<IEnumerable<T>> GetCombinations<T>(IList<T> list, int length)
        {
            var numberOfCombinations = (long) Math.Pow(list.Count, length);
            for (long i = 0; i < numberOfCombinations; i++)
            {
                yield return BuildCombination(list, length, i);
            }
        }

        private static IEnumerable<T> BuildCombination<T>(IList<T> list, int length, long combinationNumber)
        {
            var temp = combinationNumber;
            for (var j = 0; j < length; j++)
            {
                yield return list[(int) (temp % list.Count)];
                temp /= list.Count;
            }
        }
    }
}