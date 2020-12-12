using System;

namespace AoC2020.Day12
{
    internal static class Solver
    {
        public static void Task1(string[] instructions) // Answer: 1710
        {
            var position = (eastWest: 0, northSouth: 0);
            var heading = 90;

            foreach (var instruction in instructions)
            {
                var direction = instruction[0];
                var magnitude = int.Parse(instruction.Substring(1));
                position = TranslateVessel(direction, magnitude, position);

                switch (direction)
                {
                    case 'L':
                        heading -= magnitude;
                        break;
                    case 'R':
                        heading += magnitude;
                        break;
                    case 'F':
                        var cHeading = Heading2Char(heading);
                        position = TranslateVessel(cHeading, magnitude, position);
                        break;
                }

                //Console.WriteLine(instruction);
                //Console.WriteLine(position);
            }

            Console.WriteLine($"Task 1: Abs. sum of positions: {Math.Abs(position.eastWest) + Math.Abs(position.northSouth)}");
        }

        private static (int northSouth, int eastWest) TranslateVessel(char direction, int magnitude,
            (int eastWest, int northSouth) position)
        {
            switch (direction)
            {
                case 'N':
                    position.northSouth += magnitude;
                    break;
                case 'S':
                    position.northSouth -= magnitude;
                    break;
                case 'E':
                    position.eastWest += magnitude;
                    break;
                case 'W':
                    position.eastWest -= magnitude;
                    break;
            }

            return position;
        }

        private static char Heading2Char(in int heading)
        {
            var orientation = NormalizeAngle(heading);
            return orientation switch
            {
                0 => 'N',
                90 => 'E',
                180 => 'S',
                270 => 'W',
                360 => 'N',
                _ => throw new NotImplementedException($"Heading of {orientation} degrees is not implemented.")
            };
        }

        private static int NormalizeAngle(int degrees)
        {
            var orientation = degrees;
            orientation %= 360;
            if (orientation < 0) orientation += 360;
            return orientation;
        }
    }
}