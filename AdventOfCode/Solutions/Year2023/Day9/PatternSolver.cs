using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace AdventOfCode.Solutions
{
    class PatternSolver: SolutionMain
    {

        public PatternSolver() : base("year2023/Day9/") { }

        protected override String Solve(List<String> data)
        {

            return data.Select(row => {
                var pattern = row.Split(' ').Select(num => int.Parse(num)).ToList();
                PrintInfo("");
                PrintInfo(String.Join(", ", pattern));
                return GetPreviousNumber(pattern);
            }).Sum().ToString();
        }

        private int GetNextNumber(List<int> Numbers)
        {
            var diffs = new List<int>();
            for (var i = 0; i < Numbers.Count - 1; i++)
            {
                diffs.Add(Numbers[i + 1] - Numbers[i]);
            }

            if (diffs.Distinct().Count() == 1)
            {
                return Numbers.ElementAt(Numbers.Count() - 1) + diffs.First();
            }

            return Numbers.ElementAt(Numbers.Count() - 1) + GetNextNumber(diffs);
        }

        private int GetPreviousNumber(List<int> Numbers)
        {
            var diffs = new List<int>();
            for (var i = 0; i < Numbers.Count - 1; i++)
            {
                diffs.Add(Numbers[i + 1] - Numbers[i]);
            }

            if (diffs.Distinct().Count() == 1)
            {
                return Numbers.ElementAt(0) - diffs.First();
            }

            return Numbers.ElementAt(0) - GetPreviousNumber(diffs);
        }
    }
}