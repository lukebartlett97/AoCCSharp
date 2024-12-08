using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

class AntennaFrequency : SolutionMain
{
    public AntennaFrequency() : base("year2024/Day8/") { }

    protected override String Solve(List<String> data)
    {
        var grid = data.Select(line => line.ToCharArray()).ToArray();
        var height = grid.Length;
        var width = grid[0].Length;
        var antennas = new Dictionary<char, List<Position>>();

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                var node = grid[y][x];
                if(node != '.')
                {
                    if (antennas.ContainsKey(node))
                    {
                        antennas[node].Add(new Position(x, y));
                    }
                    else
                    {
                        antennas.Add(node, new List<Position>() { new Position(x, y) });
                    }
                }
            }
        }

        var antinodes = antennas
             .SelectMany(antennaSet =>
             {
                 var antinodes = new List<Position>();
                 var values = antennaSet.Value;
                 for (int i = 0; i < values.Count; i++)
                 {
                     var first = values[i];
                     for (int j = 0; j < values.Count; j++)
                     {
                         if (i != j)
                         {
                             var second = values[j];
                             var xDiff = first.x - second.x;
                             var yDiff = first.y - second.y;
                             var mult = 0;
                             while(true)
                             {
                                 var targetX = first.x + (xDiff * mult);
                                 var targetY = first.y + (yDiff * mult);
                                 if (targetX < 0 || targetY < 0 || targetX >= width || targetY >= height)
                                 {
                                     break;
                                 }
                                 antinodes.Add(new Position(targetX, targetY));
                                 mult++;
                             }

                         }
                     }
                 }
                 return antinodes;
             });


        for (int y = 0; y < height; y++)
        {
            var plainGrid = "";
            var antennasAdded = "";
            for (int x = 0; x < width; x++)
            {
                var node = grid[y][x];
                plainGrid += node;
                if (antinodes.Any(position => position.x == x && position.y == y))
                {
                    antennasAdded += '#';
                } else
                {
                    antennasAdded += node;
                }
            }
            PrintInfo($"{plainGrid}   {antennasAdded}");
        }

        return antinodes
            .Distinct(new PositionComparer())
            .Count()
            .ToString();
    }

    class Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class PositionComparer : EqualityComparer<Position>
    {
        public override bool Equals(Position a, Position b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public override int GetHashCode(Position obj)
        {
            return base.GetHashCode();
        }
    }
}
