using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

class MathOperators : SolutionMain
{
    public MathOperators() : base("year2024/Day7/") { }

    protected override String Solve(List<String> data)
    {
        return data
            .Select(line => new Equation(line))
            .Where(equation => equation.Solvable())
            .OrderBy(equation => equation.Result)
            .Select(equation =>
            {
                PrintInfo($"{equation.Result}: {equation.Solution}");
                return equation.Result;
            })
            .Aggregate((a, b) => a + b)
            .ToString();
    }

    class Equation
    {
        public double Result;

        public double[] Parts;

        public string Solution;

        public Equation(string line)
        {
            var split = line.Split(' ');
            Result = double.Parse(split[0][0..^1]);
            Parts = split[1..].Select(double.Parse).ToArray();
        }

        public bool Solvable()
        {
            return IsSolvable(Parts[0], Parts[1..], Array.Empty<string>());
        }

        private bool IsSolvable(double current, double[] parts, string[] route)
        {
            if(current > Result)
            {
                return false;
            }

            var added = current + parts[0];
            if (parts.Length == 1)
            {
                if (added == Result)
                {
                    StoreSolution(route.Concat(new[] { "+" }).ToArray());
                    return true;
                }
            }
            else
            {
                var isSolvableWithAdded = IsSolvable(added, parts[1..], route.Concat(new[] { "+" }).ToArray());
                if (isSolvableWithAdded)
                {
                    return true;
                }
            }

            var multiplied = current * parts[0];
            if (parts.Length == 1)
            {
                if (multiplied == Result)
                {
                    StoreSolution(route.Concat(new[] { "*" }).ToArray());
                    return true;
                }
            }
            else
            {
                var isSolvableWithMultiplied = IsSolvable(multiplied, parts[1..], route.Concat(new[] { "*" }).ToArray());
                if (isSolvableWithMultiplied)
                {
                    return true;
                }
            }

            var concatenated = double.Parse(current.ToString() + parts[0].ToString());
            if (parts.Length == 1)
            {
                if (concatenated == Result)
                {
                    StoreSolution(route.Concat(new[] { "||" }).ToArray());
                    return true;
                }
            }
            else
            {
                var isSolvableWithConcatenated = IsSolvable(concatenated, parts[1..], route.Concat(new[] { "||" }).ToArray());
                if (isSolvableWithConcatenated)
                {
                    return true;
                }
            }

            return false;
        }

        private void StoreSolution(string[] route)
        {
            var solution = "";

            for (int i = 0; i < Parts.Length; i++)
            {
                solution += Parts[i].ToString();
                if(i != route.Length)
                {
                    solution += route[i];
                }
            }

            Solution = solution;
        }
    }
}
