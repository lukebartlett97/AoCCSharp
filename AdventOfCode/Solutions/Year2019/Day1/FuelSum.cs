using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class FuelSum : SolutionMain
    {

        public FuelSum() : base("year2019/Day1/"){}

        protected override String Solve(List<String> data)
        {
            return data.Select(s => GetFuelForModule(int.Parse(s))).Sum().ToString();
        }

        private int GetFuelForModule(int mass)
        {
            int fuel = (mass / 3) - 2;
            int total = 0;
            while (fuel > 0)
            {
                total += fuel;
                fuel = (fuel / 3) - 2;
            }
            return total;

        }
    }
}
