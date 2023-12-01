using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class RouteTracker : SolutionMain
    {

        public RouteTracker() : base("year2021/Day2/"){}

        protected override String Solve(List<String> data)
        {
            return GetPosition(data).ToString();
        }

        private int GetPosition(List<string> data)
        {
            var pos = new Position();
            data.Select(data => data.Split(' ')).ToList().ForEach(command =>
            {
                var val = int.Parse(command[1]);
                switch (command[0])
                {
                    case "down":
                        pos.Aim += val;
                        break;
                    case "up":
                        pos.Aim -= val;
                        break;
                    case "forward":
                        pos.X += val;
                        pos.Y += pos.Aim * val;
                        break;
                    default:
                        PrintInfo("Something bad has happened.");
                        break;
                }
            });
            return pos.X * pos.Y;
        }

        private class Position
        {
            public int X = 0;
            public int Y = 0;
            public int Aim = 0;
        }
    }
}
