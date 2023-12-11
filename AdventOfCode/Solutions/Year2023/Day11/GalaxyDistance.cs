using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace AdventOfCode.Solutions
{
    class GalaxyDistance : SolutionMain
    {

        public GalaxyDistance() : base("year2023/Day11/") { }

        protected override String Solve(List<String> data)
        {
            var map = data.Select(row => row.ToCharArray());
            var largeRows = new List<int>();
            var largeColumns = new List<int>();
            for (int i = 0; i < map.Count(); i++)
            {
                if (map.ElementAt(i).Where(val => val == '#').Count() == 0)
                {
                    largeRows.Add(i);
                }
            }

            for (int i = 0; i < map.First().Count(); i++)
            {
                if (map.All(row => row.ElementAt(i) == '.'))
                {
                    largeColumns.Add(i);
                }
            }

            var coords = new List<CoOrd>();

            for (int i = 0; i < map.Count(); i++)
            {
                var currentRow = map.ElementAt(i);
                var printOut = "";
                for (int j = 0; j < currentRow.Count(); j++)
                {
                    if(currentRow.ElementAt(j) == '#')
                    {
                        coords.Add(new CoOrd(i, j));
                        printOut += "#";
                    }
                    else if (largeRows.Contains(i) || largeColumns.Contains(j))
                    {
                        printOut += "x";
                    } else
                    {
                        printOut += ".";
                    }
                }
                PrintInfo(printOut);
            }

            BigInteger acc = 0;

            for (int i = 0; i < coords.Count(); i++)
            {
                var currentGalaxy = coords.ElementAt(i);
                for (int j = i + 1; j < coords.Count(); j++)
                {
                    var distance = currentGalaxy.DistanceTo(coords.ElementAt(j), largeRows, largeColumns);
                    //PrintInfo($"Distance between {i} and {j} is {distance}");
                    acc += distance;
                }
            }

            return acc.ToString();
        }
    }

    class CoOrd
    {
        public int X;
        public int Y;

        public CoOrd(int x, int y)
        {
            X = x;
            Y = y;
        }

        public BigInteger DistanceTo(CoOrd other, List<int> largeRows, List<int> largeColumns)
        {
            BigInteger startingVal = Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
            BigInteger rowCount = largeRows.Where(val => val > Math.Min(other.X, X) && val < Math.Max(other.X, X)).Count() * 999999;
            BigInteger columnCount = largeColumns.Where(val => val > Math.Min(other.Y, Y) && val < Math.Max(other.Y, Y)).Count() * 999999;
            return startingVal + rowCount + columnCount;
        }
    }
}