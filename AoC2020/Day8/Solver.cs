using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day8
{
    internal static class Solver
    {
        public static void Task1(string[] program) // Answer: 1766
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

        public static void Task2(string[] program) // Answer: 1639
        {
            var (_, instrToAlter) = RunProgram(program);

            instrToAlter.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            var testProgram = new string[program.Length];
            program.CopyTo(testProgram, 0);

            foreach (var (line, counter) in instrToAlter)
            {
                var instruction = program[line].Substring(0, 3);
                testProgram[line] = instruction switch
                {
                    "jmp" => testProgram[line].Replace("jmp", "nop"),
                    "nop" => testProgram[line].Replace("nop", "jmp"),
                    _ => testProgram[line]
                };

                var (infiniteLoop, _) = RunProgram(testProgram);
                if (infiniteLoop)
                {
                    program.CopyTo(testProgram, 0);
                    continue;
                }

                break;
            }
        }

        private static (bool, List<(int, int)>) RunProgram(string[] program)
        {
            var accounting = new int[program.Length];

            var accumulator = 0;
            var counter = 0;
            var line = 0;

            bool infiniteLoop;

            var instrToAlter = new List<(int, int)>();

            while (true)
            {
                if (line >= program.Length)
                {
                    infiniteLoop = false;
                    Console.WriteLine($"Accumulator at end of program: {accumulator}");
                    break;
                }

                if (accounting[line] > 0)
                {
                    infiniteLoop = true;
                    Console.WriteLine($"Accumulator after one run of infinite loop: {accumulator}");
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

            (bool, List<(int, int)>) result = (infiniteLoop, instrToAlter);

            return result;
        }
    }
}