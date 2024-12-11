using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode.Solutions
{
    abstract class SolutionMain
    {
        private readonly String ResourcePath;
        private readonly string RunningPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        public bool Verbose = false;

        public SolutionMain(String resourcePath)
        {
            ResourcePath = resourcePath;
        }

        protected abstract String Solve(List<String> data);

        public void PrintSolution(bool exampleVerbose = false, bool solutionVerbose = false) {
            Verbose = exampleVerbose;
            GetProblem().ForEach(s => Console.WriteLine(s));
            Console.WriteLine();
            List<String> exampleData = GetExample();
            if (exampleData != null)
            {
                PrintSolution(exampleData, "Example");
            }
            Verbose = solutionVerbose;
            PrintSolution(GetData(), "Real");
        }

            public void PrintExample(bool verbose = true)
        {
            Verbose = verbose;
            GetProblem().ForEach(Console.WriteLine);
            List<String> exampleData = GetExample();
            PrintSolution(exampleData, "Example");
        }

        private void PrintSolution(List<String> data, string label)
        {
            PrintInfo(label + " Data:");
            data.ForEach(PrintInfo);
            PrintInfo("---");
            var solution = Solve(data);
            Console.WriteLine("---");
            Console.WriteLine(label + " Solution:");
            Console.WriteLine(solution);
        }

        public void TimeSolution(int runs) {
            List<String> exampleData = GetExample();
            if (exampleData != null)
            {
                Console.WriteLine("Example Data:");
                TimeSolution(exampleData, runs, "Time taken for first run: ");
                TimeSolution(exampleData, runs, "Average time taken after " + runs + " runs: ");
            }
            Console.WriteLine("Real Data:");
            TimeSolution(GetData(), runs, "Time taken for first run: ");
            TimeSolution(GetData(), runs, "Average time taken after " + runs + " runs: ");
        }

        private void TimeSolution(List<String> data, int runs, String message) {
            List<double> times = new List<double>();
            for (int i = 0; i < runs; i++)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Solve(data);
                stopWatch.Stop();
                times.Add(stopWatch.Elapsed.TotalMilliseconds);
            }
            double total = times.Sum();
            Console.WriteLine(message + (total / runs) + " milliseconds");
        }

        private List<String> GetData()
        {
            var path = Path.GetFullPath(Path.Combine(RunningPath, "resources", ResourcePath, "data.txt"));
            return ReadFile(path);
        }

        private List<String> GetExample()
        {
            var path = Path.GetFullPath(Path.Combine(RunningPath, "resources", ResourcePath, "example.txt"));
            return ReadFile(path);
        }

        private List<String> GetProblem()
        {
            var path = Path.GetFullPath(Path.Combine(RunningPath, "resources", ResourcePath, "problem.txt"));
            return ReadFile(path);
        }

        private List<String> ReadFile(string file)
        {
            return File.ReadAllLines(file).ToList();
        }

        protected void PrintInfo(String line)
        {
            if (Verbose)
            {
                Console.WriteLine(line);
            }
        }

        protected void PrintInfo(int number)
        {
            if (Verbose)
            {
                Console.WriteLine(number);
            }
        }

        protected void PrintInfo(long number)
        {
            if (Verbose)
            {
                Console.WriteLine(number);
            }
        }
    }
}
