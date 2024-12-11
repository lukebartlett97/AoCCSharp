using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode.Solutions;

class HikingTrails : SolutionMain
{
    public HikingTrails() : base("year2024/Day10/") { }

    protected override String Solve(List<String> data)
    {
        var grid = data.Select(line => line.Select(val => new Node((int)char.GetNumericValue(val))).ToArray()).ToArray();
        for(var i = 8; i >= 0; i--)
        {
            for (var y = 0; y < grid.Length; y++)
            {
                for (var x = 0; x < grid[y].Length; x++)
                {
                    var currentNode = grid[y][x];
                    if (currentNode.Value != i)
                    {
                        continue;
                    }
                    if(x > 0)
                    { 
                        var rightNode = grid[y][x - 1];
                        if(rightNode.Value == i + 1)
                        {
                            currentNode.SeenPeaks.UnionWith(rightNode.SeenPeaks);
                            currentNode.Score += rightNode.Score;
                        }
                    }
                    if (x < grid[y].Length - 1)
                    {
                        var leftNode = grid[y][x + 1];
                        if (leftNode.Value == i + 1)
                        {
                            currentNode.SeenPeaks.UnionWith(leftNode.SeenPeaks);
                            currentNode.Score += leftNode.Score;
                        }
                    }
                    if (y > 0)
                    {
                        var belowNode = grid[y - 1][x];
                        if (belowNode.Value == i + 1)
                        {
                            currentNode.SeenPeaks.UnionWith(belowNode.SeenPeaks);
                            currentNode.Score += belowNode.Score;
                        }
                    }
                    if (y < grid.Length - 1)
                    {
                        var aboveNode = grid[y + 1][x];
                        if (aboveNode.Value == i + 1)
                        {
                            currentNode.SeenPeaks.UnionWith(aboveNode.SeenPeaks);
                            currentNode.Score += aboveNode.Score;
                        }
                    }
                }
            }
        }

        foreach (var line in grid)
        {
            PrintInfo(line.Select(node => node.Score.ToString().PadRight(2)).Aggregate((a, b) => a + b));
        }

        return grid
            .SelectMany(line => line.Where(node => node.Value == 0).Select(node => node.Score))
            .Sum()
            .ToString();
    }

    class Node
    {
        public int Value;
        public HashSet<Node> SeenPeaks = new HashSet<Node>();
        public int Score = 0;

        public Node(int value)
        {
            Value = value;
            if(value == 9)
            {
                SeenPeaks.Add(this);
                Score = 1;
            }
        }
    }
}
