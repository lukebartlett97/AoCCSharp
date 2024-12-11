using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode.Solutions;

class DiskFragmenter : SolutionMain
{
    public DiskFragmenter() : base("year2024/Day9/") { }

    protected override String Solve(List<String> data)
    {
        var split = data[0].ToCharArray();

        var blocks = new List<Block>();
        var currentIndex = 0;
        var currentId = 0;
        var empty = false;
        foreach (var item in split)
        {
            var value = (int) char.GetNumericValue(item); 
            if (empty)
            {
                blocks.Add(new Block(currentIndex, value, null));
            }
            else
            {
                blocks.Add(new Block(currentIndex, value, currentId));
                currentId++;
            }
            currentIndex += value;
            empty = !empty;
        }

        PrintInfo(blocks.Select(block => block.ToString()).Aggregate((a, b) => a + b));

        var newEmpty = new List<Block>();

        var maxId = blocks.Max(block => block.Id);

        for(var i = maxId; i >= 0; i--)
        {
            var movingBlock = blocks.Single(block => block.Id == i);

            var firstOpenSpace = blocks.Where(block => block.Id == null && block.Size >= movingBlock.Size).FirstOrDefault();
            
            if (firstOpenSpace == null || firstOpenSpace.StartIndex > movingBlock.StartIndex)
            {
                continue;
            }

            newEmpty.Add(new Block(movingBlock.StartIndex, movingBlock.Size, null));
            movingBlock.StartIndex = firstOpenSpace.StartIndex;
            firstOpenSpace.StartIndex += movingBlock.Size;
            firstOpenSpace.Size -= movingBlock.Size;
        }

        PrintInfo(blocks.Concat(newEmpty).OrderBy(block => block.StartIndex).Select(block => block.ToString()).Aggregate((a, b) => a + b));

        return blocks.Select(block => block.Value()).Aggregate((a, b) => a + b).ToString();
    }

    class Block
    {
        public double StartIndex;
        public int Size;
        public double? Id;

        public Block(double startIndex, int size, double? id)
        {
            StartIndex = startIndex;
            Size = size;
            Id = id;
        }

        public double Value()
        {
            return Id == null ? 0 : Id.Value * (Size * (2 * StartIndex + (Size - 1))) / 2;
        }

        public override string ToString()
        {
            return String.Concat(Enumerable.Repeat(Id == null ? "." : Id.Value.ToString(), Size));
        }
    }
}
