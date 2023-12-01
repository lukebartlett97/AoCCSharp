using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class Calibration : SolutionMain
    {

        private static readonly Dictionary<string, int> REPLACEMENTS = new Dictionary<string, int>()
        {
            { "zero", 0 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

        public Calibration() : base("year2023/Day1/"){}

        protected override String Solve(List<String> data)
        {
            return data.Select(item =>
            {
                var value = FindFirst(item) * 10 + FindLast(item);
                Console.WriteLine($"{item} => {value}");
                return value;
            }).Sum().ToString();
        }

        private int FindFirst(string item)
        {
            
            var firstChar = item.ElementAt(0);
            if(Char.IsDigit(firstChar))
            {
                return int.Parse(firstChar.ToString());
            }
            foreach(var replacement in REPLACEMENTS)
            {
                if(item.StartsWith(replacement.Key))
                {
                    return replacement.Value;
                }
            }
            return FindFirst(item[1..]);
        }

        private int FindLast(string item)
        {
            var lastChar = item.ElementAt(item.Count() - 1);
            if (Char.IsDigit(lastChar))
            {
                return int.Parse(lastChar.ToString());
            }
            foreach (var replacement in REPLACEMENTS)
            {
                if (item.EndsWith(replacement.Key))
                {
                    return replacement.Value;
                }
            }
            return FindLast(item.Substring(0, item.Count() - 1));
        }
    }
}
