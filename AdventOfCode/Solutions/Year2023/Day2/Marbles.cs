using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class Marbles : SolutionMain
    {

        public Marbles() : base("year2023/Day2/"){}

        protected override String Solve(List<String> data)
        {
            return data.Select(row => new Game(row)).Select(game => game.Power).Sum().ToString();
        }

        private class Game
        {
            public int Id;

            public bool Possible;

            public int Power;

            public Game(string row)
            {
                var colonSplit = row.Split(":");

                Id = int.Parse(colonSplit[0].Replace("Game ", ""));

                var reveals = colonSplit[1].Split(";").Select(reveal => new Reveal(reveal));

                Possible = !reveals.Any(reveal => reveal.Red > 12 || reveal.Green > 13 || reveal.Blue > 14);

                Power = reveals.Max(reveal => reveal.Red) * reveals.Max(reveal => reveal.Green) * reveals.Max(reveal => reveal.Blue);
            }
        }

        private class Reveal
        {
            public int Blue = 0;
            public int Red = 0;
            public int Green = 0;

            public Reveal(string info)
            {
                var lines = info.Split(",");
                foreach (var line in lines)
                {
                    var pair = line.Split(' ');
                    switch(pair[2])
                    {
                        case "blue":
                            Blue = int.Parse(pair[1]);
                            break;
                        case "green":
                            Green = int.Parse(pair[1]);
                            break;
                        case "red":
                            Red = int.Parse(pair[1]);
                            break;
                    }
                }
            }
        }
    }
}
