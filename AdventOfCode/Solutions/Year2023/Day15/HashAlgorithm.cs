using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions
{
    class HashAlgorithm : SolutionMain
    {

        public HashAlgorithm() : base("year2023/Day15/") { }

        protected override String Solve(List<String> data)
        {
            var instructions = data.ElementAt(0).Split(',');
            var boxes = new Dictionary<int, List<Lens>>();
            foreach ( var instruction in instructions )
            {
                if(instruction.EndsWith('-'))
                {
                    var label = instruction.Split('-')[0];
                    var box = Hash(label);
                    if(boxes.ContainsKey(box) == true)
                    {
                        boxes.GetValueOrDefault(box).RemoveAll(lens => lens.Label == label);
                    }
                } else
                {
                    var label = instruction.Split('=')[0];
                    var focus = int.Parse(instruction.Split('=')[1]);
                    var box = Hash(label);
                    if (boxes.ContainsKey(box) == true)
                    {
                        var lenses = boxes.GetValueOrDefault(box);
                        var foundLens = lenses.Find(lens => lens.Label == label);
                        if (foundLens != null)
                        {
                            foundLens.Strength = focus;
                        } else
                        {
                            lenses.Add(new Lens(focus, label));
                        }
                    }
                    else
                    {
                        var initialLens = new List<Lens>()
                        { new Lens(focus, label) };
                        boxes.Add(box, initialLens);
                    }
                }
            }

            var acc = 0;
            foreach (var box in boxes)
            {
                for (int i = 0; i < box.Value.Count(); i++)
                {
                    acc += box.Value[i].Strength * (i + 1) * (box.Key + 1);
                }
            }
            return acc.ToString();
        }

        private int Hash(string val)
        {
            return val.Aggregate(0, (a, b) => ((a + b) * 17) % 256);
        }

        private class Lens
        {
            public int Strength;
            public readonly string Label;

            public Lens(int strength, string label)
            {
                this.Strength = strength;
                this.Label = label;
            }
        }

        protected String SolvePart1(List<String> data)
        {
            return data.ElementAt(0).Split(',').Select(instruction => Hash(instruction)).Sum().ToString();
        }
    }
}