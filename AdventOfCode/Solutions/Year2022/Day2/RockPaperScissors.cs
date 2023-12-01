using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class RockPaperScissors : SolutionMain
    {

        public RockPaperScissors() : base("year2022/Day2/"){}

        protected override String Solve(List<String> data)
        {

            var plays = new Play[]
            {
                new Play("X", 1, new string[] { "B", "A", "C" }, new Dictionary<string, int> { { "A", 3 }, { "B", 1 }, { "C", 2 } }),
                new Play("Y", 2, new string[] { "C", "B", "A" }, new Dictionary<string, int> { { "A", 4 }, { "B", 5 }, { "C", 6 } }),
                new Play("Z", 3, new string[] { "A", "C", "B" }, new Dictionary<string, int> { { "A", 8 }, { "B", 9 }, { "C", 7 } }),
            };

            
            var scores = data.Select(line => {
                var game = line.Split(' ');
                var play = plays.Where(play => play.Key == game[1]).First();
                //return play.Value + (3 * Array.IndexOf(play.Opps, game[0]));
                return play.Results[game[0]];
            });

            return scores.Sum().ToString();
        }
    }

    class Play
    {
        public string Key;
        public int Value;
        public string[] Opps;
        public Dictionary<string, int> Results;

        public Play(string key, int value, string[] opps, Dictionary<string, int> results)
        { 
            this.Key = key;
            this.Value = value;
            this.Opps = opps;
            this.Results = results;
        }
    }
}
