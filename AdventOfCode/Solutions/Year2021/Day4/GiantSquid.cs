using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions
{
    class GiantSquid : SolutionMain
    {

        public GiantSquid() : base("year2021/Day4/") { }

        protected override String Solve(List<String> data)
        {
            var orderedNums = ConvertToIntegerList(data[0].Split(',').ToList());
            var boards = GetBoards(data, orderedNums);
            boards.Sort((a, b) => a.WinningTurn - b.WinningTurn);
            return boards[^1].GetScore(orderedNums).ToString();
        }

        private List<BingoBoard> GetBoards(List<string> data, List<int> orderedNums)
        {
            var boards = new List<BingoBoard>();
            for(var i = 2; i<data.Count; i+=6)
            {
                List<List<int>> boardData = new List<List<int>>();
                for(var j = 0; j<5; j++)
                {
                    boardData.Add(ConvertToIntegerList(data[i + j].Split(' ').Where(val => val != "").ToList()));
                }
                boards.Add(new BingoBoard(boardData, orderedNums, Verbose));
            }
            return boards;
        }

        private class BingoBoard
        {
            List<List<int>> Rows;
            List<List<int>> Columns;
            public int WinningTurn { get; private set;  }
            private bool Verbose;

            public BingoBoard(List<List<int>> board, List<int> orderedNumbers, bool verbose)
            {
                Verbose = verbose;
                Rows = board;
                Columns = new List<List<int>>();
                for (int i = 0; i < 5; i++)
                {
                    var column = new List<int>();
                    for (int j = 0; j < 5; j++)
                    {
                        column.Add(board[j][i]);
                    }
                    Columns.Add(column);
                }
                PrintInfo(ToString());
                FindWinningTurn(orderedNumbers);
                PrintInfo("");
            }

            private void PrintInfo(string value)
            {
                if (Verbose)
                {
                    Console.WriteLine(value);
                }

            }

            private void FindWinningTurn(List<int> orderedNumbers)
            {

                WinningTurn = Math.Min(ColumnTurn(orderedNumbers), RowTurn(orderedNumbers));
            }

            private int ColumnTurn(List<int> orderedNumbers)
            {
                var columnScores = Columns.ConvertAll(column => column.ConvertAll(value =>
                {
                    var index = orderedNumbers.IndexOf(value);
                    if (index == -1)
                    {
                        return 999;
                    }
                    return index;
                }).Max());
                PrintInfo("Column Scores: " + String.Join(", ", columnScores));
                return columnScores.Min();
            }

            private int RowTurn(List<int> orderedNumbers)
            {
                var rowScores = Rows.ConvertAll(row => row.ConvertAll(value =>
                {
                    var index = orderedNumbers.IndexOf(value);
                    if (index == -1)
                    {
                        return 999;
                    }
                    return index;
                }).Max());
                PrintInfo("Row Scores: " + String.Join(", ", rowScores));
                return rowScores.Min();
            }

            public override string ToString() 
            {
                return String.Join("\n", Rows.ConvertAll(row => String.Join(", ", row)));
            }

            internal int GetScore(List<int> orderedNums)
            {
                var calledNums = orderedNums.GetRange(0, WinningTurn + 1);
                return calledNums[^1] * Rows.ConvertAll(row => row.Where(value => !calledNums.Contains(value)).Sum()).Sum();
            }
        }
    }
}
