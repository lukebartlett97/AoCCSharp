using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AdventOfCode.Solutions
{
    class CurvyDjikstras : SolutionMain
    {

        private readonly int MAX_MOVES = 10;
        private readonly int FORCED_MOVES = 4;

        public CurvyDjikstras() : base("year2023/Day17/") { }

        protected override String Solve(List<String> data)
        {
            var nodes = new List<List<Node>>();
            for (int y = 0; y < data.Count; y++)
            {
                var row = new List<Node>();
                for (int x = 0; x < data.ElementAt(0).Length; x++)
                {
                    row.Add(new Node(data.ElementAt(y).ElementAt(x), x, y));
                }
                nodes.Add(row);
            }
            nodes.First().First().Potentials.Add(new Potential(Direction.East, 10, 0));
            var allNodes = nodes.SelectMany(row => row);
            var count = 0;
            while (true)
            {
                if(count++ % 1000 == 0)
                {
                    PrintInfo("");
                    foreach (var row in nodes)
                    {
                        PrintInfo(String.Join("", row.Select(node => node.GetStringValue())));
                    }
                }
                var currentNode = allNodes.OrderBy(node => {
                    var best = node.BestPotential();
                    if(best == null)
                    {
                        return int.MaxValue;
                    }
                    return best.TotalCost;
                }).First();
                var currentPotential = currentNode.BestPotential();
                if (currentNode.X == data.ElementAt(0).Length - 1 && currentNode.Y == data.Count() - 1)
                {
                    PrintInfo("");
                    foreach (var row in nodes)
                    {
                        PrintInfo(String.Join("", row.Select(node => node.GetStringValue())));
                    }
                    return currentPotential.TotalCost.ToString();
                }
                currentPotential.Completed = true;

                if (currentPotential.Direction != Direction.South && 
                    !(currentPotential.Direction == Direction.North && currentPotential.Remaining == 0) &&
                    !(currentPotential.Direction == Direction.East && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    !(currentPotential.Direction == Direction.West && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    currentNode.Y != 0)
                {
                    var northNode = allNodes.First(node => node.X == currentNode.X && node.Y == currentNode.Y - 1);
                    northNode.OfferPotential(
                        new Potential(Direction.North, 
                        currentPotential.Direction == Direction.North ? currentPotential.Remaining - 1 : MAX_MOVES - 1, 
                        currentPotential.TotalCost + northNode.Cost));
                }

                if (currentPotential.Direction != Direction.North &&
                    !(currentPotential.Direction == Direction.South && currentPotential.Remaining == 0) &&
                    !(currentPotential.Direction == Direction.East && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    !(currentPotential.Direction == Direction.West && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    currentNode.Y != data.Count() - 1)
                {
                    var southNode = allNodes.First(node => node.X == currentNode.X && node.Y == currentNode.Y + 1);
                    southNode.OfferPotential(
                        new Potential(Direction.South,
                        currentPotential.Direction == Direction.South ? currentPotential.Remaining - 1 : MAX_MOVES - 1,
                        currentPotential.TotalCost + southNode.Cost));
                }

                if (currentPotential.Direction != Direction.East &&
                    !(currentPotential.Direction == Direction.West && currentPotential.Remaining == 0) &&
                    !(currentPotential.Direction == Direction.North && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    !(currentPotential.Direction == Direction.South && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    currentNode.X != 0)
                {
                    var westNode = allNodes.First(node => node.X == currentNode.X - 1 && node.Y == currentNode.Y);
                    westNode.OfferPotential(
                        new Potential(Direction.West,
                        currentPotential.Direction == Direction.West ? currentPotential.Remaining - 1 : MAX_MOVES - 1,
                        currentPotential.TotalCost + westNode.Cost));
                }

                if (currentPotential.Direction != Direction.West &&
                    !(currentPotential.Direction == Direction.East && currentPotential.Remaining == 0) &&
                    !(currentPotential.Direction == Direction.North && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    !(currentPotential.Direction == Direction.South && currentPotential.Remaining > MAX_MOVES - FORCED_MOVES) &&
                    currentNode.X != data.Count() - 1)
                {
                    var eastNode = allNodes.First(node => node.X == currentNode.X + 1 && node.Y == currentNode.Y);
                    eastNode.OfferPotential(
                        new Potential(Direction.East,
                        currentPotential.Direction == Direction.East ? currentPotential.Remaining - 1 : MAX_MOVES - 1,
                        currentPotential.TotalCost + eastNode.Cost));
                }
            }
        }


        private class Node
        {
            public int Cost;
            public int X;
            public int Y;
            public List<Potential> Potentials = new List<Potential>();

            public Node(char cost, int x, int y)
            {
                Cost = (int) char.GetNumericValue(cost);
                X = x;
                Y = y;
            }

            public void OfferPotential(Potential potential)
            {
                var betterPotential = Potentials.Any(existingPotential =>
                    existingPotential.Direction == potential.Direction &&
                    existingPotential.Remaining == potential.Remaining &&
                    existingPotential.TotalCost <= potential.TotalCost);
                if (!betterPotential)
                {
                    Potentials.RemoveAll(existingPotential =>
                    existingPotential.Direction == potential.Direction &&
                    existingPotential.Remaining == potential.Remaining &&
                    existingPotential.TotalCost > potential.TotalCost);
                    Potentials.Add(potential);
                }
            }

            public Potential? BestPotential()
            {
                var options = Potentials.Where(potential => !potential.Completed);
                return options.Count() == 0 ? null : options.OrderBy(p => p.TotalCost).First();
            }

            public string GetStringValue()
            {
                var best = Potentials.Count() == 0 ? null : Potentials.OrderBy(p => p.TotalCost).First();
                if (best == null)
                {
                    return "-   ";
                }
                return best.TotalCost.ToString() + new string(' ', 4 - best.TotalCost.ToString().Length);
            }
        }

        private class Potential
        {
            public Direction Direction;
            public int Remaining;
            public int TotalCost;
            public bool Completed;

            public Potential(Direction direction, int remaining, int totalCost)
            {
                Direction = direction;
                Remaining = remaining;
                TotalCost = totalCost;
            }
        }

        private enum Direction
        {
            North,
            East,
            South,
            West,
        }
    }
}