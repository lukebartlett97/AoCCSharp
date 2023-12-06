using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class Scratchcards : SolutionMain
    {

        public Scratchcards() : base("year2023/Day4/"){}

        protected override String Solve(List<String> data)
        {
            var copies = new int[data.Count()];
            Array.Fill(copies, 1);
            for(int i = 0; i < data.Count(); i++)
            {
                var line = data[i].Split(":")[1];

                var split = line.Split('|');
                var winners = split[0].Split(" ").Where(val => val != "").Select(val => int.Parse(val)).ToArray();
                var havers = split[1].Split(" ").Where(val => val != "").Select(val => int.Parse(val)).ToArray();
                var gotchas = havers.Intersect(winners);
                PrintInfo(String.Join(", ", gotchas));

                for(int j = i + 1; j < i + gotchas.Count() + 1; j++)
                {
                    copies[j] += copies[i];
                }
            }

            return copies.Sum().ToString();
        }

        private String SolvePart1(List<String> data)
        {
            return data.Select(line =>
            {
                line = line.Split(":")[1];
                var split = line.Split('|');
                var winners = split[0].Split(" ").Where(val => val != "").Select(val => int.Parse(val)).ToArray();
                var havers = split[1].Split(" ").Where(val => val != "").Select(val => int.Parse(val)).ToArray();
                var gotchas = havers.Intersect(winners);
                PrintInfo(String.Join(", ", gotchas));
                if (gotchas.Count() == 0) return 0;
                return Math.Pow(2, (gotchas.Count() - 1));
            })
            .Sum()
            .ToString();
        }
    }
}
