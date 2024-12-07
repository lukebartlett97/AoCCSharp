using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode.Solutions
{
    class RockShifting : SolutionMain
    {

        public RockShifting() : base("year2023/Day14/") { }

        protected override String Solve(List<String> data)
        {
            PrintInfo("Before:");
            for (int i = 0; i < data.Count(); i++)
            {
                PrintInfo(data.ElementAt(i));
            }
            var grid = data.Select(row => {
                return row.ToCharArray();
            }).ToArray();
            char[][]? previousGrid = null;
            for (BigInteger iterations = 0; iterations < 1_000_000_000; iterations++)
            {
                if(iterations % 4 == 0)
                {
                    if(previousGrid == null)
                    {
                        previousGrid = grid.Select(row => row.Select(val => val).ToArray()).ToArray();
                    }
                    else
                    {
                        var match = true;
                        for (int i = 0; i < previousGrid.Length; i++)
                        {
                            for (int j = 0; j < previousGrid[i].Length; j++)
                            {
                                if (previousGrid[i][j] != grid[i][j])
                                {
                                    match = false;
                                    break;
                                }
                            }
                            if (!match) break;
                        }
                        if (match)
                        {
                            break;
                        }
                        else
                        {
                            previousGrid = grid.Select(row => row.Select(val => val).ToArray()).ToArray();
                            if(iterations % 1000 == 0)
                            {
                                PrintInfo("");
                                PrintInfo($"Adding to memories after {iterations} iterations:");
                                PrintGrid(grid);
                            }
                        }
                    }
                }
                for (int i = 0; i < grid.ElementAt(0).Length; i++)
                {
                    var previousSolid = -1;
                    for (int j = 0; j < grid.Count(); j++)
                    {
                        var current = grid.ElementAt(j).ElementAt(i);
                        if (current == 'O')
                        {
                            if (previousSolid != j - 1)
                            {
                                grid.ElementAt(j)[i] = '.';
                                grid.ElementAt(previousSolid + 1)[i] = 'O';
                                previousSolid++;
                            }
                            else
                            {
                                previousSolid = j;
                            }
                        }
                        else if (current == '#')
                        {
                            previousSolid = j;
                        }
                    }
                }
                grid = RotateGrid(grid);
            }
            var acc = 0;

            for (int i = 0; i < grid.Count(); i++)
            {
                acc += grid.ElementAt(i).Count(val => val == 'O') * (grid.Count() - i);
            }
            return acc.ToString();
        }

        private void PrintGrid(char[][] grid)
        {
            for (int i = 0; i < grid.Count(); i++)
            {
                PrintInfo(String.Join("", grid.ElementAt(i)));
            }
        }

        private char[][] RotateGrid(char[][] grid)
        {

            var rotated = new List<char[]>();
            for (int i = grid.ElementAt(0).Count() - 1; i >= 0 ; i--)
            {
                rotated.Add(grid.Select(row => row.ElementAt(i)).ToArray());
            }
            return rotated.ToArray();
        }
    }
}