using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day8
{
    internal static class Solver
    {
        public static void Task1(string[] program)
        {
            var accounting = new int[program.Length];

            var accumulator = 0;

            var counter = 0;
            var line = 0;
            while (true)
            {
                if (accounting[line] > 0) break;

                counter += 1;
                var instruction = program[line].Substring(0, 3);
                var argument = int.Parse(program[line].Substring(4));

                switch (instruction)
                {
                    case "acc":
                        accounting[line] = counter;
                        accumulator += argument;
                        line += 1;
                        break;
                    case "jmp":
                        accounting[line] = counter;
                        line += argument;
                        break;
                    case "nop":
                        accounting[line] = counter;
                        line += 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(instruction), instruction);
                }
            }

            Console.WriteLine(accumulator);
        }

        public static void Task2(string[] program)
        {
            var (failedLine, infiniteLoop, accounting, instrToAlter, accumulator) = RunProgram(program);

            instrToAlter.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            var lastLine = accounting.Max();
            var tempProg = new string[program.Length];
            program.CopyTo(tempProg, 0);

            foreach (var (line, counter) in instrToAlter)
            {
                var instruction = program[line].Substring(0, 3);
                switch (instruction)
                {
                    case "jmp":
                        tempProg[line] = tempProg[line].Replace("jmp", "nop");
                        break;
                    case "nop":
                        tempProg[line] = tempProg[line].Replace("nop", "jmp");
                        break;
                }

                var quadruple = RunProgram(tempProg);
                if (quadruple.Item2)
                {
                    program.CopyTo(tempProg, 0);
                    continue;
                }

                Console.WriteLine($"Accumulator: {quadruple.Item5}");
                break;
            }
        }

        private static (int, bool, int[], List<(int, int)>, int) RunProgram(string[] program)
        {
            var accounting = new int[program.Length];

            var accumulator = 0;

            var counter = 0;
            var line = 0;

            bool inifiniteLoop;

            var instrToAlter = new List<(int, int)>();

            while (true)
            {
                if (line >= program.Length)
                {
                    inifiniteLoop = false;
                    break;
                }

                if (accounting[line] > 0)
                {
                    inifiniteLoop = true;
                    break;
                }

                counter += 1;
                var instruction = program[line].Substring(0, 3);
                var argument = int.Parse(program[line].Substring(4));

                switch (instruction)
                {
                    case "acc":
                        accounting[line] = counter;
                        accumulator += argument;
                        line += 1;
                        break;
                    case "jmp":
                        accounting[line] = counter;
                        instrToAlter.Add((line, counter));
                        line += argument;
                        break;
                    case "nop":
                        accounting[line] = counter;
                        instrToAlter.Add((line, counter));
                        line += 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(instruction), instruction);
                }
            }

            Console.WriteLine(accumulator);

            (int, bool, int[], List<(int, int)>, int) result = (line, inifiniteLoop, accounting, instrToAlter,
                accumulator);

            return result;
        }
    }
}