using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class LaserReflection : SolutionMain
    {

        public LaserReflection() : base("year2023/Day16/") { }

        protected override String Solve(List<String> data)
        {
            var grid = data.Select(row => row.Select(val => new Cell(val)).ToList()).ToList();
            var max = 0;
            for(int i = 0; i < grid.Count(); i ++)
            {
                for(int j = 0; j < grid[i].Count(); j ++)
                {
                    if(i == 0)
                    {
                        max = Math.Max(max, GetEnergised(grid, i, j, 'S'));
                    }
                    if (j == 0)
                    {
                        max = Math.Max(max, GetEnergised(grid, i, j, 'E'));
                    }
                    if (i == grid.Count() - 1)
                    {
                        max = Math.Max(max, GetEnergised(grid, i, j, 'N'));
                    }
                    if (j == grid[i].Count() - 1)
                    {
                        max = Math.Max(max, GetEnergised(grid, i, j, 'W'));
                    }
                }
            }
            return max.ToString();
        }

        private int GetEnergised(List<List<Cell>> grid, int startingRow, int startingColumn, char direction)
        {
            grid.ForEach(row => row.ForEach(cell => cell.ResetEnergised()));
            BeamMeUpScotty(grid, startingRow, startingColumn, direction);
            return grid.Select(row => row.Where(cell => cell.Energised.Any(energise => energise)).Count()).Sum();
        }

        private void BeamMeUpScotty(List<List<Cell>> grid, int currentRow, int currentColumn, char direction)
        {
            if(currentRow >= grid.Count() || currentRow < 0 || currentColumn >= grid.ElementAt(currentRow).Count() || currentColumn < 0)
            {
                return;
            }

            var currentCell = grid.ElementAt(currentRow).ElementAt(currentColumn);
            switch (direction)
            {
                case 'N':
                    if (currentCell.Energised[0]) return;
                    currentCell.Energised[0] = true;
                    break;
                case 'E':
                    if (currentCell.Energised[1]) return;
                    currentCell.Energised[1] = true;
                    break;
                case 'S':
                    if (currentCell.Energised[2]) return;
                    currentCell.Energised[2] = true;
                    break;
                case 'W':
                    if (currentCell.Energised[3]) return;
                    currentCell.Energised[3] = true;
                    break;
            }
            if(currentCell.Val == '.' || 
                (currentCell.Val == '-' && direction == 'E') ||
                (currentCell.Val == '-' && direction == 'W') ||
                (currentCell.Val == '|' && direction == 'N') ||
                (currentCell.Val == '|' && direction == 'S')
                ) 
            {
                switch(direction)
                {
                    case 'N':
                        BeamMeUpScotty(grid, currentRow - 1, currentColumn, 'N');
                        break;
                    case 'E':
                        BeamMeUpScotty(grid, currentRow, currentColumn + 1, 'E');
                        break;
                    case 'S':
                        BeamMeUpScotty(grid, currentRow + 1, currentColumn, 'S');
                        break;
                    case 'W':
                        BeamMeUpScotty(grid, currentRow, currentColumn - 1, 'W');
                        break;
                }
            }
            else if((currentCell.Val == '|' && direction == 'E') ||
                (currentCell.Val == '|' && direction == 'W'))
            {
                BeamMeUpScotty(grid, currentRow - 1, currentColumn, 'N');
                BeamMeUpScotty(grid, currentRow + 1, currentColumn, 'S');
            }
            else if ((currentCell.Val == '-' && direction == 'N') ||
                (currentCell.Val == '-' && direction == 'S'))
            {
                BeamMeUpScotty(grid, currentRow, currentColumn + 1, 'E');
                BeamMeUpScotty(grid, currentRow, currentColumn - 1, 'W');
            }
            else if ((currentCell.Val == '/' && direction == 'E') ||
                (currentCell.Val == '\\' && direction == 'W'))
            {
                BeamMeUpScotty(grid, currentRow - 1, currentColumn, 'N');
            }
            else if ((currentCell.Val == '/' && direction == 'W') ||
                (currentCell.Val == '\\' && direction == 'E'))
            {
                BeamMeUpScotty(grid, currentRow + 1, currentColumn, 'S');
            }
            else if ((currentCell.Val == '/' && direction == 'N') ||
                (currentCell.Val == '\\' && direction == 'S'))
            {
                BeamMeUpScotty(grid, currentRow, currentColumn + 1, 'E');
            }
            else if ((currentCell.Val == '/' && direction == 'S') ||
                (currentCell.Val == '\\' && direction == 'N'))
            {
                BeamMeUpScotty(grid, currentRow, currentColumn - 1, 'W');
            }
            else
            {
                throw new Exception("wtf");
            }
        }

        private class Cell
        {
            public char Val;
            public bool[] Energised = new bool[4];
            public Cell(char val)
            {
                Val = val;
            }

            public void ResetEnergised()
            {
                Energised[0] = false;
                Energised[1] = false;
                Energised[2] = false;
                Energised[3] = false;
            }
        }
    }
}