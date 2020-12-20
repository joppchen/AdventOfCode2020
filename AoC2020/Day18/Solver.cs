using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace AoC2020.Day18
{
    internal static class Solver
    {
        public static void Task1(string[] expressions) // Answer: 16332191652452
        {
            long grandTotal = 0;
            foreach (var expression in expressions)
            {
                // TODO: Explain regex
                const string pattern = "^[^()]*" +
                                       "(" +
                                       "((?'Open'\\()[^()]*)+" +
                                       "((?'Close-Open'\\))[^()]*)+" +
                                       ")*" +
                                       "(?(Open)(?!))$";
                var input = string.Concat(expression.Where(x => !char.IsWhiteSpace(x)));

                var m = Regex.Match(input, pattern);

                var inputArray = new string[1];
                inputArray[0] = input;

                if (m.Success == true)
                {
                    while (m.Groups[5].Length > 0) // TODO: Explain group 5
                    {
                        foreach (Capture cap in m.Groups[5].Captures)
                        {
                            var groupString = cap.Value;
                            if (groupString.Contains("(")) continue;
                            var groupResult = CalculateGroup(groupString);
                            inputArray[0] = inputArray[0].Replace("(" + groupString + ")", groupResult.ToString());
                        }

                        m = Regex.Match(inputArray[0], pattern);
                        if (!m.Success) throw new Exception($"No regex matches in string '{inputArray[0]}'");

                        // Print content of whole match tree
                        /*Console.WriteLine("Input: \"{0}\" \nMatch: \"{1}\"", input, m2);
                        int grpCtr = 0;
                        foreach (Group grp in m2.Groups)
                        {
                            Console.WriteLine("   Group {0}: {1}", grpCtr, grp.Value);
                            grpCtr++;
                            int capCtr = 0;
                            foreach (Capture cap in grp.Captures)
                            {
                                Console.WriteLine("      Capture {0}: {1}", capCtr, cap.Value);
                                capCtr++;
                            }
                        }*/
                    }
                }
                else
                {
                    Console.WriteLine("Match failed.");
                    Console.WriteLine($"Expression: {input}");
                    break;
                }

                var finalResult = CalculateGroup(inputArray[0]);

                checked // Throws exception if integer overflow
                {
                    grandTotal += finalResult;
                }
            }

            Console.WriteLine($"Task 1: Grand total: {grandTotal}");
        }

        private static long CalculateGroup(string example1)
        {
            var allNumbers = Regex.Matches(example1, "\\d+");
            var allOperators = Regex.Matches(example1, "\\D");

            var result = long.Parse(allNumbers[0].Value);

            for (var i = 0; i < allOperators.Count; i++)
            {
                var number = int.Parse(allNumbers[i + 1].Value);
                var theOperator = allOperators[i].Value;
                switch (theOperator)
                {
                    case "+":
                        checked
                        {
                            result += number;
                        }

                        break;
                    case "*":
                        checked
                        {
                            result *= number;
                        }

                        break;
                    default:
                        throw new NotImplementedException($"Operator '{allOperators[i].Value}' is not implemented");
                }
            }

            return result;
        }
    }
}