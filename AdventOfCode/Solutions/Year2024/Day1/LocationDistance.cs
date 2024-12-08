using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class LocationDistance : SolutionMain
    {
        public LocationDistance() : base("year2024/Day1/"){}

        protected override String Solve(List<String> data)
        {
            var split = data.Select(line => line.Split("   "));
            var first = split.Select(line => int.Parse(line[0]));
            var second = split.Select(line => int.Parse(line[1]));
            var acc = 0;
            foreach (var value in first)
            {
                var count = second.Count(line => line == value);
                acc += count * value;
            }
            return acc.ToString();
        }
    }
}
