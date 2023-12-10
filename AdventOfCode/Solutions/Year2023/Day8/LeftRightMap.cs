using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace AdventOfCode.Solutions
{
    class LeftRightMap: SolutionMain
    {

        public LeftRightMap() : base("year2023/Day8/") { }

        protected override String Solve(List<String> data)
        {
            var instructions = data.ElementAt(0);
            data.RemoveRange(0, 2);
            var nodes = data.Select(row => new Node(row));
            var currentNodes = nodes.Where(node => node.Self.EndsWith("A")).ToArray();
            var instructionIndex = 0;
            var count = 0;
            var ends = new ulong[currentNodes.Length];
            while (ends.Any(val => val == 0))
            {
                count++;
                var currentInstruction = instructions[instructionIndex];
                for(var i = 0; i < currentNodes.Count(); i++)
                {
                    var currentTarget = currentInstruction == 'L' ? currentNodes[i].Left : currentNodes[i].Right;
                    currentNodes[i] = nodes.First(node => node.Self.Equals(currentTarget));
                    if(currentNodes[i].Self.EndsWith("Z"))
                    {
                        if (ends[i] == 0)
                        {
                            ends[i] = (ulong) count;
                            PrintInfo($"End found for {i} at count {count}");
                        }
                    }
                }
                instructionIndex++;
                if(instructionIndex == instructions.Length)
                {
                    instructionIndex = 0;
                }
            }

            PrintInfo($"praying to the old gods and the new...");

            var lcm = ends.Aggregate((a, b) => (a * b) / gcd(a, b));

            return lcm.ToString();
        }

        static ulong gcd(ulong n1, ulong n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return gcd(n2, n1 % n2);
            }
        }

        private class Node
        {
            public string Self;
            public string Left;
            public string Right;

            public Node(string row)
            {
                Self = row.Substring(0, 3);
                Left = row.Substring(7, 3);
                Right = row.Substring(12, 3);
            }
        }
    }
}