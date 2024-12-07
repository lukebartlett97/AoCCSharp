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

namespace AdventOfCode.Solutions
{
    class MirrorReflection : SolutionMain
    {

        public MirrorReflection() : base("year2023/Day13/") { }

        protected override String Solve(List<String> data)
        {
            var grids = new List<List<string>>();
            var currentGrid = new List<string>();
            foreach(var row in data)
            {
                if(row == "")
                {
                    grids.Add(currentGrid);
                    currentGrid = new List<string>();
                    continue;
                }
                currentGrid.Add(row);
            }
            grids.Add(currentGrid);

            return grids.Select(grid =>
            {
                var horizontalMirror = FindMirror(grid);
                if (horizontalMirror != null)
                {
                    PrintInfo($"Found horizontal mirror at line {horizontalMirror}");
                    PrintGrid(grid, horizontalMirror, null);
                    return horizontalMirror * 100;
                }
                var verticalGrid = new List<string>();
                for(int i = 0; i < grid.ElementAt(0).Count(); i++)
                {
                    verticalGrid.Add(String.Join("", grid.Select(row => row.ElementAt(i))));
                }
                var verticalmirror = FindMirror(verticalGrid);
                if(verticalmirror != null)
                {
                    PrintInfo($"Found vertical mirror at line {verticalmirror}");
                    PrintGrid(grid, null, verticalmirror);
                    return verticalmirror;
                }
                PrintInfo($"No mirror found");
                PrintGrid(grid, null, null);
                return 0;
            }).Sum().ToString();
        }

        protected int? FindMirror(List<string> grid)
        {
            for(int i = 1; i < grid.Count(); i++)
            {
                var errors = 0;
                for(int offset = 1; offset <= Math.Min(i, grid.Count() - i); offset++)
                {
                    errors += CountErrors(grid.ElementAt(i - offset), grid.ElementAt(i + offset - 1));

                    if(errors > 1)
                    {
                        break;
                    }
                }
                if(errors == 1)
                {
                    return i;
                }
            }
            return null;
        }

        protected int CountErrors(string first, string second)
        {
            var acc = 0;
            for(int i = 0; i < first.Length; i++)
            {
                if(first.ElementAt(i) != second.ElementAt(i))
                {
                    acc++;
                }
            }
            return acc;
        }

        protected void PrintGrid(List<string> grid, int? horizontalMirror, int? verticalMirror)
        {
            for(int i = 0; i < grid.Count(); i++)
            {
                var currentRow = grid.ElementAt(i);
                if(horizontalMirror == i)
                {
                    PrintInfo(new string('-', currentRow.Count()));
                }
                if(verticalMirror == null)
                {
                    PrintInfo(currentRow);
                }
                else
                {
                    var prefix = String.Join("", currentRow[..(int)verticalMirror]);
                    var suffix = String.Join("", currentRow.Skip((int)verticalMirror));
                    PrintInfo($"{prefix}|{suffix}");
                }
            }
        }
    }
}