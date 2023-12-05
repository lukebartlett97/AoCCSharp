using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    class Gears2 : SolutionMain
    {

        public Gears2() : base("year2023/Day3/"){}

        protected override String Solve(List<String> data)
        {
            var matrix = data.Select(row => row.ToCharArray()).ToArray();

            var matrixNumbers = new List<MatrixNumber>();

            for (var i = 0; i < matrix.Length; i++)
            {
                var row = matrix[i];

                MatrixNumber? currentNumber = null;
                for (var j = 0; j < row.Length; j++)
                {
                    var symbol = row[j];

                    if (char.IsDigit(symbol))
                    {
                        if(currentNumber == null)
                        {
                            currentNumber = new MatrixNumber(i, j);
                        }
                        currentNumber.val += symbol;
                    }
                    else
                    {
                        if (currentNumber != null)
                        {
                            matrixNumbers.Add(currentNumber);
                            currentNumber = null;
                        }
                    }
                }
                if (currentNumber != null)
                {
                    matrixNumbers.Add(currentNumber);
                }
            }

            var gears = matrixNumbers.Select(matrixNumber => matrixNumber.MapToAdjacentGear(matrix)).Where(gear => gear != null);

            //Don't ask...
            var total = gears.GroupBy(gear => gear.row).SelectMany(gearRow => gearRow.GroupBy(gear => gear.column).Where(group => group.Count() == 2).Select(group => group.ElementAt(0).val * group.ElementAt(1).val)).Sum();

            return total.ToString();
        }

        private class MatrixNumber
        {
            public string val = "";
            public int row;
            public int startColumn;

            public MatrixNumber(int row, int startColumn)
            {
                this.row = row;
                this.startColumn = startColumn;
            }

            public GearPos? MapToAdjacentGear(char[][] matrix)
            {
                for(var row = this.row - 1; row <= this.row + 1; row++)
                {
                    var endColumn = startColumn + val.Length;
                    for(var column = this.startColumn - 1; column <= endColumn; column++)
                    {
                        if(IsGear(matrix, row, column))
                        {
                            return new GearPos(row, column, int.Parse(val));
                        }
                    }
                }
                return null;
            }


            private bool IsGear(char[][] matrix, int currentRow, int currentColumn)
            {
                if (currentRow < 0 || currentColumn < 0) return false;
                if (currentRow >= matrix.Length) return false;
                if (currentColumn >= matrix[currentRow].Length) return false;
                var val = matrix[currentRow][currentColumn];
                return val == '*';
            }
        }

        private class GearPos
        {
            public int row;
            public int column;
            public int val;

            public GearPos(int row, int column, int val)
            {
                this.row = row;
                this.column = column;
                this.val = val;
            }
        }
    }
}
