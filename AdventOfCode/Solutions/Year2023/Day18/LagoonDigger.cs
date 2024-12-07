using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace AdventOfCode.Solutions
{
    class LagoonDigger : SolutionMain
    {

        public LagoonDigger() : base("year2023/Day18/") { }

        protected override String Solve(List<String> data)
        {
            BigInteger currentX = 0;
            BigInteger currentY = 0;

            var nodes = new List<CornerNode>();
            var lastnodeGoingRight = false;
            foreach (var row in data)
            {
                var split = row.Split(' ');
                var hexCode = Int64.Parse(split[2][2..^2], System.Globalization.NumberStyles.HexNumber);
                switch (split[2][7])
                {
                    case '3': //Up
                        PrintInfo("Up " + hexCode);
                        currentY -= hexCode;
                        break;
                    case '1': //Down
                        PrintInfo("Down " + hexCode);
                        currentY += hexCode;
                        break;
                    case '2': //Left
                        PrintInfo("Left " + hexCode);
                        currentX -= hexCode;
                        break;
                    case '0': //Right
                        PrintInfo("Right " + hexCode);
                        currentX += hexCode;
                        break;
                }
                nodes.Add(new CornerNode(currentX, currentY));
                if (split[2][7] == '1')
                {
                    nodes.ElementAt(nodes.Count() - 2).GoingDown = true;
                }
                if (split[2][7] == '3')
                {
                    nodes.ElementAt(nodes.Count() - 1).GoingDown = true;
                }
                if (split[2][7] == '0')
                {
                    if(nodes.Count() < 2)
                    {
                        lastnodeGoingRight = true;
                    }
                    else
                    {
                        nodes.ElementAt(nodes.Count() - 2).GoingRight = true;
                    }
                }
                if (split[2][7] == '2')
                {
                    nodes.ElementAt(nodes.Count() - 1).GoingRight = true;
                }
            }

            if(lastnodeGoingRight)
            {
                nodes.ElementAt(nodes.Count() - 1).GoingRight = true;
            }

            var cornerRows = nodes.GroupBy(node => node.Y).OrderBy(group => group.Key).ToList();

            BigInteger sizeAcc = 0;
            var previousCorners = new List<CornerNode>();
            var previousY = nodes.Min(node => node.Y);

            foreach(var cornerRow in cornerRows)
            {
                for(int i = 0; i < previousCorners.Count; i += 2)
                {
                    sizeAcc += (previousCorners.ElementAt(i + 1).X - previousCorners.ElementAt(i).X + 1) * (cornerRow.Key - previousY - 1);
                }
                PrintInfo($"With Area: {sizeAcc}");
                previousY = cornerRow.Key;
                var rowCorners = cornerRow.OrderBy(node => node.X);

                bool inside = false;
                bool inWall = false;
                BigInteger prevX = 0;
                var newCorners = rowCorners.Where(node => node.GoingDown).ToList();
                previousCorners.RemoveAll(corner => cornerRow.Any(oldCorner => oldCorner.X == corner.X));

                foreach (var corner in rowCorners.Concat(previousCorners).OrderBy(node => node.X))
                {
                    if (inside || inWall)
                    {
                        sizeAcc += corner.X - prevX;
                    }
                    prevX = corner.X;
                    if (corner.Y != cornerRow.Key)
                    {
                        inside = !inside;
                    }
                    else
                    {
                        inWall = !inWall;
                        if(corner.GoingDown)
                        {
                            inside = !inside;
                        }
                    }
                }

                previousCorners = previousCorners.Concat(newCorners).OrderBy(node => node.X).ToList();
                sizeAcc += previousCorners.Count() / 2;
                PrintInfo($"With Line: {sizeAcc}");
                PrintInfo($"Corners: {String.Join(", ", previousCorners)}");
            }
            sizeAcc++;
            return sizeAcc.ToString();
        }

        private class CornerNode
        {
            public BigInteger X;
            public BigInteger Y;
            public bool GoingDown = false;
            public bool GoingRight = false;

            public CornerNode(BigInteger x, BigInteger y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                return $"({X},{Y})";
            }
        }
    }
}