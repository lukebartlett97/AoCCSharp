using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class FloorLines : SolutionMain
    {

        public FloorLines() : base("year2021/Day5/") { }
        protected override String Solve(List<String> data)
        {
            Map map = CreateMap(data);
            var pointCount = map.Dict.ToList().ConvertAll(pairing => pairing.Value.ToList().Count(val => val.Value > 1)).Sum();
            return pointCount.ToString();
        }

        private Map CreateMap(List<String> data)
        {
            var map = new Map();
            data.ConvertAll(line => Regex.Split(line, @" -> |,").ToList().ConvertToIntegerList()).ForEach(line => map.AddLine(line[0], line[1], line[2], line[3], Verbose));
            return map;
        }

        private class Map
        {
            public Dictionary<int, Dictionary<int, int>> Dict = new Dictionary<int, Dictionary<int, int>>();
            public void AddLine(int startX, int startY, int endX, int endY, bool verbose)
            {
                var currentX = startX;
                var currentY = startY;
                while(true)
                {
                    if (Dict.ContainsKey(currentX))
                    {
                        if (Dict[currentX].ContainsKey(currentY))
                        {
                            Dict[currentX][currentY] += 1;
                            PrintInfo("Adding point: (" + currentX + "," + currentY + ")", verbose);
                        }
                        else
                        {
                            Dict[currentX].Add(currentY, 1);
                            PrintInfo("Adding point: (" + currentX + "," + currentY + ")", verbose);
                        }
                    }
                    else
                    {
                        Dict.Add(currentX, new Dictionary<int, int>() { [currentY] = 1 });
                        PrintInfo("Adding point: (" + currentX + "," + currentY + ")", verbose);
                    }
                    if (currentX == endX && currentY == endY)
                    {
                        break;
                    }
                    if (currentX < endX)
                    {
                        currentX++;
                    } else if(currentX > endX)
                    {
                        currentX--;
                    }
                    if (currentY < endY)
                    {
                        currentY++;
                    } else if (currentY > endY)
                    {
                        currentY--;
                    }
                }
            }

            private void PrintInfo(string line, bool verbose)
            {
                if(verbose)
                {
                    Console.WriteLine(line);
                }
            }
        }

        private class Point
        {
            public int X;
            public int Y;
        }
    }
}
