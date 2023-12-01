using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class SeaDepth : SolutionMain
    {

        public SeaDepth() : base("year2021/Day1/"){}

        private readonly int PART_1_WINDOW_SIZE = 1;
        private readonly int PART_2_WINDOW_SIZE = 3;

        protected override String Solve(List<String> data)
        {
            return GetNumberIncreased(ConvertToIntegerList(data), PART_2_WINDOW_SIZE).ToString();
        }

        private int GetNumberIncreased(List<int> data, int windowSize)
        {
            var acc = 0;
            for(var i = 0; i < data.Count()- windowSize; i++)
            {
                acc += data.GetRange(i+1, windowSize).Sum() - data.GetRange(i, windowSize).Sum() > 0 ? 1 : 0;
            }
            return acc;
        }
    }
}
