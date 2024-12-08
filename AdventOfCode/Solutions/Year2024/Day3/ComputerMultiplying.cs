using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class ComputerMultiplying : SolutionMain
    {
        public ComputerMultiplying() : base("year2024/Day3/"){}

        protected override String Solve(List<String> data)
        {
            var stuff = data.Aggregate((a, b) => a + b);
            var matches = Regex.Matches(stuff, "(mul\\(\\d+,\\d+\\))|do\\(\\)|don't\\(\\)")
                .Select(match => match.Value);
            var enabled = true;
            var acc = 0;
            foreach ( var match in matches )
            {
                if(match.Equals("do()"))
                {
                    enabled = true;
                    continue;
                }
                if (match.Equals("don't()"))
                {
                    enabled = false;
                    continue;
                }
                if (enabled)
                {
                    var nums = match[4..^1].Split(',');
                    acc += int.Parse(nums[0]) * int.Parse(nums[1]);
                }
            }
            return acc.ToString();
        }
    }
}
