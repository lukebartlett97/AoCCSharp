using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class ReactorData : SolutionMain
    {
        public ReactorData() : base("year2024/Day2/"){}

        protected override String Solve(List<String> data)
        {
            return data
                .Count(line =>
                {
                    var report = line.Split(' ').Select(item => int.Parse(item)).ToArray();
                    var asc = report[0] < report[1];
                    var failed = false;
                    for (int i = 0; i < report.Length - 1; i++)
                    {
                        var diff = report[i] - report[i+1];
                        if (diff == 0 || diff < 0 != asc || Math.Abs(diff) > 3)
                        {
                            failed = true;
                            break;
                        }
                    }

                    if (!failed)
                    {
                        PrintInfo($"Report {line} is safe.");
                        return true;
                    }

                    for (int i = 0; i < report.Length; i++)
                    {
                        var dampenedReport = report.ToList();
                        dampenedReport.RemoveAt(i);
                        var ascDampened = dampenedReport[0] < dampenedReport[1];
                        var failedDampen = false;
                        for (int j = 0; j < dampenedReport.Count - 1; j++)
                        {
                            var diff = dampenedReport[j] - dampenedReport[j + 1];
                            if (diff == 0 || diff < 0 != ascDampened || Math.Abs(diff) > 3)
                            {
                                failedDampen = true;
                                break;
                            }
                        }

                        if(!failedDampen)
                        {
                            PrintInfo($"Report {line} is safe when dampening {report[i]}.");
                            return true;
                        }
                    }

                    PrintInfo($"Report {line} is unsafe.");
                    return false;
                })
                .ToString();
        }
    }
}
