using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class CalorieCounting : SolutionMain
    {

        public CalorieCounting() : base("year2022/Day1/"){}

        readonly private int NUM_EXAMPLE = 1;

        readonly private int NUM_ACTUAL = 3;

        protected override String Solve(List<String> data)
        {
            var elves = new HashSet<int>();
            var current = 0;
            foreach(var line in data)
            {
                if(line.Length == 0)
                {
                    elves.Add(current);
                    current = 0;
                } else
                {
                    current += int.Parse(line);
                }
            }
            return elves.OrderByDescending(x => x).Take(NUM_ACTUAL).Sum().ToString();
        }
    }
}
