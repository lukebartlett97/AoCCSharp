using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common;

public class Grid<T> where T : Node
{
    public IEnumerable<IEnumerable<T>> Data;

    public int currentX = 0, currentY = 0;

    public Grid(IEnumerable<IEnumerable<T>> values) {
        Data = values;
    }
}

public abstract class Node
{
    public List<CardinalDirections> MarkedCardinalDirections = new();
    public List<DiagonalDirections> MarkedDiagonalDirections = new();
}
