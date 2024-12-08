using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class GuardPatrol : SolutionMain
    {
        public GuardPatrol() : base("year2024/Day6/"){}

        protected override String Solve(List<String> data)
        {
            var guardX = 0;
            var guardY = 0;
            var grid = new List<List<Node>>();

            #region Setup
            for (int i = 0; i < data.Count; i++)
            {
                string item = data[i];
                var newLine = new List<Node>();

                char[] nodes = item.ToArray();
                for (int j = 0; j < nodes.Length; j++)
                {
                    var newNode = new Node();
                    if(nodes[j] == '^')
                    {
                        newNode.VisitedDirections.Add(CardinalDirections.NORTH);
                        guardX = j;
                        guardY = i;
                    }
                    if (nodes[j] == '#')
                    {
                        newNode.Blocker = true;
                    }
                    newLine.Add(newNode);
                }

                grid.Add(newLine);
            }

            foreach (var line in grid)
            {
                PrintInfo(line.Select(node => node.ToString()).Aggregate((a, b) => a + b));
            }
            #endregion


            var basicGrid = grid.Select(line => line.Select(node => node.Copy()).ToArray()).ToArray();
            CreatesLoop(basicGrid, guardX, guardY);

            var acc = 0;

            for (int y = 0; y < grid.Count; y++)
            {
                List<Node> line = grid[y];
                for (int x = 0; x < line.Count; x++)
                {
                    Node node = line[x];
                    if (basicGrid[y][x].VisitedDirections.Any() && !node.VisitedDirections.Any())
                    {
                        node.Blocker = true;
                        node.SpecialBlocker = true;
                        var testGrid = grid.Select(line => line.Select(node => node.Copy()).ToArray()).ToArray();
                        node.Blocker = false;
                        node.SpecialBlocker = false;

                        if(CreatesLoop(testGrid, guardX, guardY))
                        {
                            acc++;
                        }
                    }
                }
            }

            return acc.ToString();
        }

        public bool CreatesLoop(Node[][] grid, int guardX, int guardY)
        {

            var guardDir = CardinalDirections.NORTH;
            var looping = false;

            while (true)
            {
                int checkX;
                int checkY;
                switch (guardDir)
                {
                    case CardinalDirections.NORTH:
                        checkX = guardX;
                        checkY = guardY - 1;
                        break;
                    case CardinalDirections.EAST:
                        checkX = guardX + 1;
                        checkY = guardY;
                        break;
                    case CardinalDirections.SOUTH:
                        checkX = guardX;
                        checkY = guardY + 1;
                        break;
                    case CardinalDirections.WEST:
                        checkX = guardX - 1;
                        checkY = guardY;
                        break;
                    default:
                        throw new Exception("wtf");
                }

                if (checkX < 0 || checkY < 0 || checkY >= grid.Length || checkX >= grid[checkY].Length)
                {
                    break;
                }

                var checkNode = grid[checkY][checkX];

                if (checkNode.VisitedDirections.Contains(guardDir))
                {
                    looping = true;
                    break;
                }

                if (checkNode.Blocker)
                {
                    switch (guardDir)
                    {
                        case CardinalDirections.NORTH:
                            guardDir = CardinalDirections.EAST;
                            break;
                        case CardinalDirections.EAST:
                            guardDir = CardinalDirections.SOUTH;
                            break;
                        case CardinalDirections.SOUTH:
                            guardDir = CardinalDirections.WEST;
                            break;
                        case CardinalDirections.WEST:
                            guardDir = CardinalDirections.NORTH;
                            break;
                    }
                }
                else
                {
                    checkNode.VisitedDirections.Add(guardDir);
                    guardX = checkX;
                    guardY = checkY;
                }
            }

            PrintInfo(looping ? "Looping" : "Not Looping");
            foreach (var line in grid)
            {
                PrintInfo(line.Select(node => node.ToString()).Aggregate((a, b) => a + b));
            }

            return looping;
        }
    }

    record Node
    {
        public bool Blocker = false;
        public bool SpecialBlocker = false;
        public HashSet<CardinalDirections> VisitedDirections = new();

        public Node Copy()
        {
            return new Node()
            {
                Blocker = Blocker,
                SpecialBlocker = SpecialBlocker,
                VisitedDirections = VisitedDirections.ToArray().ToHashSet()
            };
        }

        public override string ToString()
        {
            if (SpecialBlocker)
            {
                return "@";
            }
            if (Blocker)
            {
                return "#";
            }
            if (!VisitedDirections.Any())
            {
                return ".";
            }
            if (VisitedDirections.Contains(CardinalDirections.NORTH) || VisitedDirections.Contains(CardinalDirections.SOUTH))
            {
                if (VisitedDirections.Contains(CardinalDirections.EAST) || VisitedDirections.Contains(CardinalDirections.WEST))
                {
                    return "+";
                }
                return "|";
            }
            return "-";
        }
    }
}
