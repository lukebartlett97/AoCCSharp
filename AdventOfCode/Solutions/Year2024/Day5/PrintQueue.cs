using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class PrintQueue : SolutionMain
    {
        public PrintQueue() : base("year2024/Day5/"){}

        protected override String Solve(List<String> data)
        {
            var rules = new List<Rule>();
            var updates = new List<List<string>>();
            var addingRules = true;

            foreach(var line in data)
            {
                if(line == "")
                {
                    addingRules = false;
                    continue;
                }

                if(addingRules)
                {
                    var split = line.Split('|');
                    rules.Add(new Rule() { First = split[0], Second = split[1] });
                }
                else
                {
                    updates.Add(line.Split(',').ToList());
                }
            }

            return updates
                .Where(update => rules.Any(rule => !rule.IsValid(update)))
                .Select(update =>
                {
                    PrintInfo($"Is not valid: {update.Aggregate((a, b) => a + "," + b)}");

                    var sortedList = new List<string>();
                    foreach(var value in update)
                    {
                        var added = false;
                        for(int i = 0; i < sortedList.Count; i++)
                        {
                            var existing = sortedList[i];
                            var rule = rules.Single(rule => (rule.First == value || rule.First == existing) && (rule.Second == value || rule.Second == existing));
                            if(rule.First == value)
                            {
                                sortedList.Insert(i, value);
                                added = true;
                                break;
                            }
                        }
                        if(!added)
                        {
                            sortedList.Add(value);
                        }
                    }
                    PrintInfo($"Fixed: {sortedList.Aggregate((a, b) => a + "," + b)}");
                    return int.Parse(sortedList.ElementAt((update.Count - 1) / 2));
                })
                .Aggregate((a, b) => a + b)
                .ToString();
        }
    }

    class Rule
    {
        public string First;
        public string Second;

        public bool IsValid(List<string> update)
        {
            if(!update.Contains(First) || !update.Contains(Second))
            {
                return true;
            }
            var indexFirst = update.IndexOf(First);
            var indexSecond = update.IndexOf(Second);
            return indexFirst < indexSecond;
        }
    }
}
