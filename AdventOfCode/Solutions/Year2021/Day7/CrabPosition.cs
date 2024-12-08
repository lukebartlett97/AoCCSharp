using System;
using System.Collections.Generic;
using System.Linq;
namespace AdventOfCode.Solutions
{
    class CrabPosition : SolutionMain
    {

        public CrabPosition() : base("year2021/Day7/") { }

        protected override String Solve(List<String> data)
        {
            var nums = data[0].Split(',').ToList().ConvertToIntegerList();
            return FindMostEfficientFuel(nums).ToString();
        }

        private int FindMedian(List<int> nums)
        {
            nums.Sort();
            PrintInfo("Sorted: " + String.Join(", ", nums));
            return nums[nums.Count / 2];
        }

        private int FindMostEfficientFuel(List<int> nums)
        {
            var point = FindMedian(nums);
            PrintInfo("Median: " + point);
            var fuelCost = FindFuelCost(nums, point);
            var above = FindFuelCost(nums, point + 1);
            var below = FindFuelCost(nums, point - 1);
            while(above < fuelCost)
            {
                point++;
                fuelCost = above;
                above = FindFuelCost(nums, point + 1);
            }
            while (below < fuelCost)
            {
                point--;
                fuelCost = below;
                below = FindFuelCost(nums, point + 1);
            }
            PrintInfo("Settling Point: " + point);
            return fuelCost;
        }

        private int FindFuelCost(List<int> nums, int point)
        {
            return nums.ConvertAll(num =>
            {
                var diff = Math.Abs(num - point);
                return diff * (diff + 1) / 2;
            }).Sum();
        }
    }
}
