using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class Gears : SolutionMain
    {

        public Gears() : base("year2023/Day3/"){}

        protected override String Solve(List<String> data)
        {
            var matrix = data.Select(row => row.ToCharArray()).ToArray();

            var totalVal = 0;

            for(var i = 0; i < matrix.Length; i++)
            {
                var row = matrix[i];

                var currentVal = "";
                var isAdjacent = false;
                for (var j = 0; j < row.Length; j++)
                {
                    var symbol = row[j];

                    if (char.IsDigit(symbol))
                    {
                        currentVal += symbol;
                        if (!isAdjacent && IsAdjacent(matrix, i, j))
                        {
                            isAdjacent = true;
                        }
                    }
                    else
                    {
                        if (currentVal.Length > 0)
                        {
                            if (isAdjacent)
                            {
                                totalVal += int.Parse(currentVal);
                                PrintInfo("Adding " + currentVal + " to total " + totalVal);
                            }
                            currentVal = "";
                            isAdjacent = false;
                        }
                    }
                }
                if (isAdjacent)
                {
                    totalVal += int.Parse(currentVal);
                    PrintInfo("Adding " + currentVal + " to total " + totalVal);
                }
            }

            return totalVal.ToString();
        }

        private bool IsAdjacent(char[][] matrix, int currentRow, int currentColumn)
        {
            return
                IsSymbol(matrix, currentRow-1, currentColumn-1) |
                IsSymbol(matrix, currentRow-1, currentColumn) |
                IsSymbol(matrix, currentRow-1, currentColumn+1) |
                IsSymbol(matrix, currentRow, currentColumn-1) |
                IsSymbol(matrix, currentRow, currentColumn+1) |
                IsSymbol(matrix, currentRow+1, currentColumn-1) |
                IsSymbol(matrix, currentRow+1, currentColumn) |
                IsSymbol(matrix, currentRow+1, currentColumn+1);
        }

        private bool IsSymbol(char[][] matrix, int currentRow, int currentColumn)
        {
            if(currentRow < 0 || currentColumn < 0) return false;
            if(currentRow >= matrix.Length) return false;
            if(currentColumn >= matrix[currentRow].Length) return false;
            var val = matrix[currentRow][currentColumn];
            return !char.IsDigit(val) && val != '.';
        }
    }
}
