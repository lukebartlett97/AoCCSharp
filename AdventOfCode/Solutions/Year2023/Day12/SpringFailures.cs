using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class SpringFailures : SolutionMain
    {

        public SpringFailures() : base("year2023/Day12/") { }

        protected override String Solve(List<String> data)
        {
            var counter = 1;
            BigInteger acc = 0;
            var values = data.Select(row =>
            {
                var split = row.Split(" ");
                var springs = split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0];
                var nums = split[1].Split(',').Select(num => int.Parse(num));
                var numsExpanded = nums.Concat(nums).Concat(nums).Concat(nums).Concat(nums);
                var info = new SpringsInfo(springs, numsExpanded.ToArray());
                PrintInfo($"Line {counter++} - Starting At: {DateTime.Now.ToLongTimeString()}");
                PrintInfo($"{springs} {String.Join(", ", numsExpanded)}");
                var knownCounts = new List<CountInfo>();
                var found = CountMatches(info.Springs, info.Numbers, "", knownCounts);
                PrintInfo($"Total: {found}");
                acc += found;
                return found;
            }).OrderByDescending(a => a);

            PrintInfo(String.Join(", ", values));

            return acc.ToString();
        }

        private class CountInfo
        {
            public string springs;
            public int[] groups;
            public BigInteger count;

            public CountInfo(string springs, int[] groups, BigInteger count)
            {
                this.springs = springs;
                this.groups = groups;
                this.count = count;
            }
        }

        private BigInteger CountMatches(string springs, int[] groups, string soFar, List<CountInfo> knownCounts)
        {
            var knownCount = knownCounts.FirstOrDefault(countInfo => countInfo.springs == springs && Enumerable.SequenceEqual(countInfo.groups, groups));
            if(knownCount != null)
            {
                //PrintInfo($"{knownCount.count} known for {springs} with groups {String.Join(", ", groups)}");
                return knownCount.count;
            }

            var requiredRemaining = groups.Sum() + groups.Length - 1;

            if (springs.Length < requiredRemaining)
            {
                return 0;
            }

            var currentGroupSize = groups[0];
            BigInteger acc = 0;
            var finished = false;

            var newGroups = groups.Skip(1).ToArray();
            for (int i = 0; i <= springs.Length - requiredRemaining; i++)
            {

                if (finished)
                {
                    break;
                }
                if (springs.ElementAt(i) == '#')
                {
                    finished = true;
                }

                var currentSprings = soFar + new string('.', i) + new string('#', currentGroupSize);
                var isValid = !springs.Substring(i, currentGroupSize).Contains(".") &&
                    springs.ElementAtOrDefault(i + currentGroupSize) != '#';

                if (!isValid)
                {
                    continue;
                }

                if(newGroups.Length == 0)
                {
                    var remainingSprings = springs.Skip(currentGroupSize + i).Count(val => val == '#');
                    if (isValid && remainingSprings == 0)
                    {
                        //PrintInfo(currentSprings);
                        acc++;
                    }
                    continue;
                }

                currentSprings += '.';
                var nextSprings = springs.Substring(i + currentGroupSize + 1);
                var found = CountMatches(nextSprings, newGroups, currentSprings, knownCounts);
                acc += found;
            }
            knownCounts.Add(new CountInfo(springs, groups, acc));
            return acc;
        }

        private class SpringsInfo
        {
            public readonly string Springs;
            public readonly string Pattern;
            public readonly int[] Numbers;

            public SpringsInfo(string springs, int[] info)
            {
                Springs = springs;
                Numbers = info;
                Pattern = "^[\\.?]*" + String.Join("[\\.?]+", info.Select(info =>
                {
                    var output = "";
                    for (var i = 0; i < info; i++)
                    {
                        output += "[#?]";
                    }
                    return output;
                })) + "[\\.?]*$";
            }

            public BigInteger CalculatePermutations()
            {
                return CalculatePermutationsInner(Springs);
            }

            private BigInteger CalculatePermutationsInner(string springs)
            {
                if(springs.Contains("?"))
                {
                    if(!Regex.IsMatch(springs, Pattern))
                    {
                        return 0;
                    }
                    var replaceIndex = springs.IndexOf("?");
                    var brokenSpring = springs.Substring(0, replaceIndex) + "#" + springs.Substring(replaceIndex + 1);
                    var workingSpring = springs.Substring(0, replaceIndex) + "." + springs.Substring(replaceIndex + 1);
                    return CalculatePermutationsInner(brokenSpring) + CalculatePermutationsInner(workingSpring);
                } else
                {
                    return Regex.IsMatch(springs, Pattern) ? 1 : 0;
                }
            }
        }
    }
}