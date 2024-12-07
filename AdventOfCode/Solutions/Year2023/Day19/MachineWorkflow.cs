using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace AdventOfCode.Solutions
{
    class MachineWorkflow : SolutionMain
    {

        public MachineWorkflow() : base("year2023/Day19/") { }

        protected override String Solve(List<String> data)
        {
            var workflows = new List<Workflow>();
            var processingWorkflows = true;
            foreach(var row in data)
            {
                if(processingWorkflows)
                {
                    if(row == "")
                    {
                        processingWorkflows = false;
                    }
                    else
                    {
                        var split = row.Split('{');
                        workflows.Add(new Workflow(split[0], split[1][..^1]));
                    }
                }
            }

            var machineParts = new List<MachinePart>()
            { 
                new MachinePart(new Dictionary<string, (int, int)>()
                {
                    {"x", (1, 4000) },
                    {"m", (1, 4000) },
                    {"a", (1, 4000) },
                    {"s", (1, 4000) },
                }
                , "in")
            };

            while(!machineParts.All(part => part.CurrentWorkflow == "A" ||  part.CurrentWorkflow == "R")) {
                var currentPart = machineParts.First(part => part.CurrentWorkflow != "A" && part.CurrentWorkflow != "R");
                PrintInfo($"Processing part: {currentPart}");
                machineParts.Remove(currentPart);
                var currentWorkflow = workflows.First(workflow => workflow.Name == currentPart.CurrentWorkflow);
                var newParts = currentWorkflow.ProcessPart(currentPart);
                newParts.ForEach(part =>
                {
                    PrintInfo($"New Part: {part}");
                });
                machineParts.AddRange(newParts);
            }

            return machineParts
                .Where(part => part.CurrentWorkflow == "A")
                .Select(part => {
                    var value = part.GetValue();
                    PrintInfo($"Accepting part {part} with value {value}");
                    return value;
                }).Aggregate(new BigInteger(0), (a, b) => a + b).ToString();
        }

        private class MachinePart
        {
            public Dictionary<string, (int, int)> ValueRanges;
            public string CurrentWorkflow;

            public MachinePart(Dictionary<string, (int, int)> valueRanges, string currentWorkflow)
            {
                ValueRanges = valueRanges;
                CurrentWorkflow = currentWorkflow;
            }

            public BigInteger GetValue()
            {
                return ValueRanges.Values.Select(range => range.Item2 - range.Item1 + 1).Aggregate(new BigInteger(1), (a, b) => a * b);
            }

            public override string ToString()
            {
                return $"<{String.Join(", ", ValueRanges.Keys.Select(key => $"'{key}': {ValueRanges.GetValueOrDefault(key).Item1}-{ValueRanges.GetValueOrDefault(key).Item2}"))}> @ {CurrentWorkflow}";
            }
        }

        private class Workflow
        {
            public string Name;
            public List<WorkflowStep> Steps;

            public Workflow(string name, string steps)
            {
                Name = name;
                Steps = steps.Split(",").Select(step => new WorkflowStep(step)).ToList();
            }

            public List<MachinePart> ProcessPart(MachinePart part)
            {
                var outcomes = new List<MachinePart>();
                var processingPart = part;
                foreach (var step in Steps)
                {
                    if(processingPart != null)
                    {
                        var stepOutput = step.SplitPart(processingPart);
                        if (stepOutput.Item1 != null)
                        {
                            outcomes.Add(stepOutput.Item1);
                        }
                        processingPart = stepOutput.Item2;
                    }
                }
                return outcomes;
            }
        }

        private class WorkflowStep
        {
            public string Catagory;
            public StepCompatorType ComparatorType;
            public int Value;
            public string Output;

            public WorkflowStep(string info)
            {
                if(info.Contains(">"))
                {
                    ComparatorType = StepCompatorType.GreaterThan;
                    var colonSplit = info.Split(':');
                    Output = colonSplit[1];
                    var operandSplit = colonSplit[0].Split('>');
                    Catagory = operandSplit[0];
                    Value = int.Parse(operandSplit[1]);
                }
                else if(info.Contains("<"))
                {
                    ComparatorType = StepCompatorType.LessThan;
                    var colonSplit = info.Split(':');
                    Output = colonSplit[1];
                    var operandSplit = colonSplit[0].Split('<');
                    Catagory = operandSplit[0];
                    Value = int.Parse(operandSplit[1]);
                }
                else
                {
                    ComparatorType = StepCompatorType.Absolute;
                    Output = info;
                }
            }

            //Returns (accept, fail)
            public (MachinePart?, MachinePart?) SplitPart(MachinePart part)
            {
                if(ComparatorType == StepCompatorType.Absolute)
                {
                    var fullRange = part.ValueRanges.ToDictionary(entry => entry.Key, entry => entry.Value);
                    return (new MachinePart(fullRange, Output), null);
                }

                MachinePart? lower = null;
                MachinePart? higher = null;
                var currentMin = part.ValueRanges.GetValueOrDefault(Catagory).Item1;
                var currentMax = part.ValueRanges.GetValueOrDefault(Catagory).Item2;

                if(ComparatorType == StepCompatorType.LessThan)
                {
                    if (Value > currentMin)
                    {
                        var lowerRanges = part.ValueRanges.ToDictionary(entry => entry.Key, entry => entry.Value);
                        lowerRanges[Catagory] = (currentMin, Math.Min(Value - 1, currentMax));
                        lower = new MachinePart(lowerRanges, Output);
                    }
                    if (Value < currentMax)
                    {
                        var higherRanges = part.ValueRanges.ToDictionary(entry => entry.Key, entry => entry.Value);
                        higherRanges[Catagory] = (Math.Max(Value, currentMin), currentMax);
                        higher = new MachinePart(higherRanges, part.CurrentWorkflow);
                    }
                    return (lower, higher);
                }
                if (Value > currentMin)
                {
                    var lowerRanges = part.ValueRanges.ToDictionary(entry => entry.Key, entry => entry.Value);
                    lowerRanges[Catagory] = (currentMin, Math.Min(Value, currentMax));
                    lower = new MachinePart(lowerRanges, part.CurrentWorkflow);
                }
                if (Value < currentMax)
                {
                    var higherRanges = part.ValueRanges.ToDictionary(entry => entry.Key, entry => entry.Value);
                    higherRanges[Catagory] = (Math.Max(Value + 1, currentMin), currentMax);
                    higher = new MachinePart(higherRanges, Output);
                }
                return (higher, lower);
            }
        }

        private enum StepCompatorType
        {
            GreaterThan,
            LessThan,
            Absolute
        }
    }
}