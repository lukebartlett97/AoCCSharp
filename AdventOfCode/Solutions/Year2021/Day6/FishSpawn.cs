using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class FishSpawn : SolutionMain
    {

        public FishSpawn() : base("year2021/Day6/") { }
        protected override String Solve(List<String> data)
        {
            var fish = PopulateInitialFish(data[0].Split(','));
            SimulateDays(fish, 256);
            return fish.Values.ToList().Aggregate(0L, (a, b) => a + b).ToString();
        }

        private void SimulateDays(Dictionary<long, long> fish, int days)
        {
            for (int i = 0; i < days; i++)
            {
                var newFish = fish[0];
                for (int fishDay = 0; fishDay < 8; fishDay++)
                {
                    fish[fishDay] = fish[fishDay + 1];
                }
                fish[8] = newFish;
                fish[6] += newFish;
            }
        }

        private Dictionary<long, long> PopulateInitialFish(string[] data)
        {
            var output = GetStartingDict();
            foreach(string value in data)
            {
                output[long.Parse(value)] += 1;
            }
            return output;
        }

        private Dictionary<long, long> GetStartingDict()
        {
            return new Dictionary<long, long>()
            {
                [0] = 0,
                [1] = 0,
                [2] = 0,
                [3] = 0,
                [4] = 0,
                [5] = 0,
                [6] = 0,
                [7] = 0,
                [8] = 0,
            };
        }
    }
}
