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
                position = Translate(direction, magnitude, position);

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
                        position = Translate(cHeading, magnitude, position);
                        break;
                }

                //Console.WriteLine(instruction);
                //Console.WriteLine(position);
            }

            Console.WriteLine(
                $"Task 1: Abs. sum of positions: {Math.Abs(position.eastWest) + Math.Abs(position.northSouth)}");
        }

        public static void Task2(string[] instructions) // Answer: 62045
        {
            // Initial positions
            var vesselPosition = (eastWest: 0, northSouth: 0);
            var wayPointPosition = (eastWest: 10, northSouth: 1);

            foreach (var instruction in instructions)
            {
                var command = instruction[0];
                var magnitude = int.Parse(instruction.Substring(1));

                wayPointPosition = Translate(command, magnitude, wayPointPosition);

                wayPointPosition = RotateWayPoint(command, magnitude, wayPointPosition);

                switch (command)
                {
                    case 'F':
                        vesselPosition.eastWest += magnitude * wayPointPosition.eastWest;
                        vesselPosition.northSouth += magnitude * wayPointPosition.northSouth;
                        break;
                }
            }

            Console.WriteLine(
                $"Task 2: Abs. sum of positions: {Math.Abs(vesselPosition.eastWest) + Math.Abs(vesselPosition.northSouth)}");
        }

        private static (int eastWest, int northSouth) Translate(char command, int magnitude,
            (int eastWest, int northSouth) position)
        {
            switch (command)
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

        private static (int eastWest, int northSouth) RotateWayPoint(char command, int magnitude,
            (int eastWest, int northSouth) position)
        {
            return command switch
            {
                'L' => RotatePosition(command, magnitude, position),
                'R' => RotatePosition(command, magnitude, position),
                _ => position
            };
        }

        private static (int eastWest, int northSouth) RotatePosition(char direction, int magnitude,
            (int eastWest, int northSouth) position)
        {
            var orientation = NormalizeAngle(magnitude);
            var multiplier = 0;
            switch (orientation)
            {
                case 0:
                    break;
                case 90:
                    multiplier = 1;
                    break;
                case 180:
                    multiplier = 2;
                    break;
                case 270:
                    multiplier = 3;
                    break;
                case 360:
                    break;
                default:
                    throw new NotImplementedException($"Heading of {orientation} degrees is not implemented.");
            }

            for (var i = 0; i < multiplier; i++)
            {
                position = Rotate90deg(position, direction);
            }

            return position;
        }

        private static (int, int) Rotate90deg((int eastWest, int northSouth) position, char direction)
        {
            var multiplier = direction switch
            {
                'L' => -1,
                'R' => 1,
                _ => throw new NotImplementedException($"Only 'L' and 'R' are valid directions (tried {direction}).")
            };

            var temp = position.eastWest;
            position.eastWest = multiplier * position.northSouth;
            position.northSouth = multiplier * -1 * temp;
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