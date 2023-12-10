using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace AdventOfCode.Solutions
{
    class PipeMaze : SolutionMain
    {

        public PipeMaze() : base("year2023/Day10/") { }

        protected override String Solve(List<String> data)
        {
            var maze = data.Select(row => (row.Select(val => new Pipe(val)).ToArray())).ToArray();
            var currentRow = 0;
            var currentColumn = 0;
            for(int i = 0; i < maze.Length; i++)
            {
                for(int j = 0; j < maze[i].Length; j++)
                {
                    if (maze[i][j].Starting)
                    {
                        currentRow = i;
                        currentColumn = j;
                    }
                }
            }

            if(maze[currentRow - 1][currentColumn].Down)
            {
                maze[currentRow][currentColumn].Up = true;
            }
            if (maze[currentRow + 1][currentColumn].Up)
            {
                maze[currentRow][currentColumn].Down = true;
            }
            if (maze[currentRow][currentColumn-1].Right)
            {
                maze[currentRow][currentColumn].Left = true;
            }
            if (maze[currentRow][currentColumn+1].Left)
            {
                maze[currentRow][currentColumn].Right = true;
            }

            var steps = 0;
            var lastDirection = '.';

            while (true)
            {
                maze[currentRow][currentColumn].Visited = true;
                steps++;
                if (maze[currentRow][currentColumn].Up && maze[currentRow - 1][currentColumn].Down && lastDirection != 'S')
                {
                    lastDirection = 'N';
                    currentRow -= 1;
                }
                else if (maze[currentRow][currentColumn].Down && maze[currentRow + 1][currentColumn].Up && lastDirection != 'N')
                {
                    lastDirection = 'S';
                    currentRow += 1;
                }
                else if (maze[currentRow][currentColumn].Left && maze[currentRow][currentColumn - 1].Right && lastDirection != 'E')
                {
                    lastDirection = 'W';
                    currentColumn -= 1;
                }
                else if (maze[currentRow][currentColumn].Right && maze[currentRow][currentColumn + 1].Left && lastDirection != 'W')
                {
                    lastDirection = 'E';
                    currentColumn += 1;
                }

                if(maze[currentRow][currentColumn].Starting)
                {
                    break;
                }
            }
            var acc = 0;

            foreach (var row in maze)
            {
                for(int i = 0; i < row.Length; i++)
                {
                    if (!row[i].Visited && row.Skip(i).Where(pipe => pipe.Visited && pipe.Up).Count() % 2 == 1)
                    {
                        row[i].Inside = true;
                        acc++;
                    }
                }
            }

            foreach (var row in maze)
            {
                PrintInfo(String.Join("", row.Select(pipe => pipe.Starting ? 'S' : pipe.Inside ? '*' : pipe.Visited ? pipe.Val : '.')));
            }

            return acc.ToString();
        }

        private class Pipe
        {
            public bool Up = false;
            public bool Down = false;
            public bool Left = false;
            public bool Right = false;
            public bool Starting = false;
            public bool Visited = false;
            public bool Inside = false;
            public char Val;

            public Pipe(char val) {
                Val = val;
                if(val == '|')
                {
                    Up = true;
                    Down = true;

                }
                else if (val == '-')
                {
                    Left = true;
                    Right = true;
                }
                else if (val == 'L')
                {
                    Up = true;
                    Right = true;
                    Val = '└';
                }
                else if (val == 'J')
                {
                    Up = true;
                    Left = true;
                    Val = '┘';
                }
                else if (val == '7')
                {
                    Left = true;
                    Down = true;
                    Val = '┐';
                }
                else if (val == 'F')
                {
                    Right = true;
                    Down = true;
                    Val = '┌';
                }
                else if (val == 'S')
                {
                    Starting = true;
                }
            }
        }
    }
}