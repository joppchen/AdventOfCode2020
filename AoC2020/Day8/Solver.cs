using System;

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
    }
}