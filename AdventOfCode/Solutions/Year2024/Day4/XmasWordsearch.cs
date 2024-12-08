using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class XmasWordsearch : SolutionMain
    {
        public XmasWordsearch() : base("year2024/Day4/"){}

        protected override String Solve(List<String> data)
        {
            foreach(var line in data)
            {
                PrintInfo(line);
            }

            var grid = data.Select(line => line.ToArray()).ToArray();

            var acc = 0;
            for (int i = 1; i < grid.Length - 1; i++)
            {
                for (int j = 1; j < grid[i].Length - 1; j++)
                {
                    var currentChar = grid[i][j];

                    if(currentChar == 'A')
                    {
                        acc += CheckDirections(grid, i, j);
                    }
                }
            }
            return acc.ToString();
        }

        private int CheckDirections(char[][] grid, int i, int j)
        {
            var acc = 0;
            acc += CheckDirection(grid, i, j, 1, 0) ? 1 : 0;
            acc += CheckDirection(grid, i, j, 0, 1) ? 1 : 0;
            acc += CheckDirection(grid, i, j, -1, 0) ? 1 : 0;
            acc += CheckDirection(grid, i, j, 0, -1) ? 1 : 0;
            return acc;
        }

        private bool CheckDirection(char[][] grid, int i, int j, int iDir, int jDir)
        {
            bool valid;
            if(jDir == 0)
            {
                valid = CheckChar(grid, i + iDir, j + 1, 'M') &&
                    CheckChar(grid, i + iDir, j - 1, 'M') &&
                    CheckChar(grid, i - iDir, j + 1, 'S') &&
                    CheckChar(grid, i - iDir, j - 1, 'S');
            }
            else
            {
                valid = CheckChar(grid, i + 1, j + jDir, 'M') &&
                    CheckChar(grid, i - 1, j + jDir, 'M') &&
                    CheckChar(grid, i + 1, j - jDir, 'S') &&
                    CheckChar(grid, i - 1, j - jDir, 'S');
            }
            if(valid)
            {
                PrintInfo($"X-MAS found at {i},{j}");
            }
            return valid;
        }

        private bool CheckChar(char[][] grid, int i, int j, char check)
        {
            return i > -1 && j > -1 && grid.Length > i && grid[i].Length > j && grid[i][j] == check;
        }
    }
}
