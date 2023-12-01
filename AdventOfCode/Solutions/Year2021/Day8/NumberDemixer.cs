using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions
{
    class NumberDemixer : SolutionMain
    {

        public NumberDemixer() : base("year2021/Day8/") { }

        protected override String Solve(List<String> data)
        {
            var patterns = GeneratePatterns();
            var displays = data.ConvertAll(l => CreateDisplay(l, patterns));
            displays.ForEach(display => display.TrimLetterMap());
            displays.ForEach(display => display.TrimMatches());
            displays.ForEach(display => display.TrimLetterMap());
            displays.ForEach(display => PrettyPrintDisplay(display));
            return displays.ConvertAll(d => d.GetUniqueOutputs()).Sum().ToString();
        }

        private Display CreateDisplay(string line, List<Pattern> patterns)
        {
            var inputs = line.Split('|')[0].Split(' ').ToList();
            var outputs = line.Split('|')[1].Split(' ').ToList();
            return new Display(inputs, outputs, patterns, GenerateLetterMap());
        }

        private class Matcher
        {
            public string Value;
            public List<Pattern> Matches;
        }

        private class Display
        {
            List<Pattern> Patterns;
            public List<Matcher> Inputs = new List<Matcher>();
            public List<Matcher> Outputs = new List<Matcher>();
            public Dictionary<char, string> LetterMap;

            public Display(List<string> inputs, List<string> outputs, List<Pattern> patterns, Dictionary<char, string> letterMap)
            {
                Patterns = patterns;
                LetterMap = letterMap;
                inputs.ConvertAll(i => String.Concat(i.OrderBy(c => c))).ForEach(i => Inputs.Add(new Matcher() { Value = i, Matches = GetMatchingPatterns(i) }));
                outputs.ConvertAll(i => String.Concat(i.OrderBy(c => c))).ForEach(o => Outputs.Add(new Matcher() { Value = o, Matches = GetMatchingPatterns(o) }));
                inputs.AddRange(outputs);
            }

            protected List<Pattern> GetMatchingPatterns(string value)
            {
                return Patterns.Where(p => p.Letters.Length == value.Length).ToList();
            }

            public void TrimLetterMap()
            {
                var NewLetterMap = new Dictionary<char, string>();
                foreach (var letter in LetterMap)
                {
                    var remaining = letter.Value;
                    foreach (var input in Inputs)
                    {
                        if(input.Value.Contains(letter.Key))
                        {
                            var possibles = input.Matches.ConvertAll(m => m.Letters).Aggregate("", (a, b) => a + b);
                            remaining = String.Concat(remaining.Where(c => possibles.Contains(c)));
                        } else if(input.Matches.Count == 1)
                        {
                            var confirmed = input.Matches[0].Letters;
                            remaining = String.Concat(remaining.Where(c => !confirmed.Contains(c)));
                        }
                    }
                    NewLetterMap.Add(letter.Key, remaining);
                }
                LetterMap = NewLetterMap;
            }

            public void TrimMatches()
            {
                foreach (var input in Inputs)
                {
                    var possibles = input.Value.ToCharArray().ToList().ConvertAll(letter => LetterMap[letter]).Aggregate("", (a, b) => a + b);
                    input.Matches.RemoveAll(pattern => !pattern.Letters.All(letter => possibles.Contains(letter)));
                }
            }

            public int GetUniqueOutputs()
            {
                return Outputs.Count(v => v.Matches.Count == 1);
            }
        }

        private void PrettyPrintDisplay(Display display)
        {
            PrintInfo("Inputs:");
            display.Inputs.ForEach(input =>
            {
                PrintInfo(input.Value + " -> " + String.Join(", ", input.Matches.ConvertAll(p => p.Letters)));
            });
            PrintInfo("LetterMap:");
            foreach(var letterMap in display.LetterMap)
            {
                PrintInfo(letterMap.Key + " -> " + letterMap.Value);
            }
            PrintInfo("");
        }

        protected new Dictionary<char, string> GenerateLetterMap()
        {
            return new Dictionary<char, string>()
            {
                ['a'] = "abcdefg",
                ['b'] = "abcdefg",
                ['c'] = "abcdefg",
                ['d'] = "abcdefg",
                ['e'] = "abcdefg",
                ['f'] = "abcdefg",
                ['g'] = "abcdefg",
            };
        }

        private List<Pattern> GeneratePatterns()
        {
            return new List<Pattern>() {
                new Pattern() { Value = 0, Letters = "abcefg" },
                new Pattern() { Value = 1, Letters = "cf" },
                new Pattern() { Value = 2, Letters = "acdeg" },
                new Pattern() { Value = 3, Letters = "acdfg" },
                new Pattern() { Value = 4, Letters = "bcdf" },
                new Pattern() { Value = 5, Letters = "abdfg" },
                new Pattern() { Value = 6, Letters = "abdefg" },
                new Pattern() { Value = 7, Letters = "acf" },
                new Pattern() { Value = 8, Letters = "abcdefg" },
                new Pattern() { Value = 9, Letters = "abcdfg" },
            };
        }

        private class Pattern
        {
            public int Value;
            public string Letters;
        }
    }
}