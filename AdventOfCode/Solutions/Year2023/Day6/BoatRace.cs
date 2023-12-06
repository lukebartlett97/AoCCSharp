using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace AdventOfCode.Solutions
{
    class BoatRace : SolutionMain
    {

        public BoatRace() : base("year2023/Day6/") { }

        protected override String Solve(List<String> data)
        {
            var time = BigInteger.Parse(data[0].Split(':')[1].Replace(" ", ""));
            var distance = BigInteger.Parse(data[1].Split(':')[1].Replace(" ", ""));

            PrintInfo($"Time: {time}. Distance: {distance}");
            for (var i = 1; i <= time / 2; i++)
            {
                if (i * (time - i) > distance)
                {
                    var possibilities = time - i - i + 1;
                    PrintInfo($"First win: {i}. Possibilities: {possibilities}"); 
                    return possibilities.ToString();
                }
            }

            return "";
        }
        private String SolvePart1(List<String> data)
        {
            var times = data[0].Split(':')[1].Trim().Split(' ').Where(val => val.Length > 0).Select(val => int.Parse(val)).ToList();
            var distances = data[1].Split(':')[1].Trim().Split(' ').Where(val => val.Length > 0).Select(val => int.Parse(val)).ToList();

            var acc = 1;

            for (var index = 0; index < times.Count; index++)
            {
                var time = times[index];
                var distance = distances[index];
                for (var i = 1; i <= time / 2; i++)
                {
                    if (i * (time - i) > distance)
                    {
                        var possibilities = time - i - i + 1;
                        PrintInfo($"First win: {i}. Possibilities: {possibilities}");
                        acc *= possibilities;
                        break;
                    }
                }
            }
            return acc.ToString();
        }
    }
}