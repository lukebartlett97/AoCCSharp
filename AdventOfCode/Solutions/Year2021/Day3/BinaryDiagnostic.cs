using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions
{
    class BinaryDiagnostic : SolutionMain
    {

        public BinaryDiagnostic() : base("year2021/Day3/") { }

        protected override String Solve(List<String> data)
        {
            return SolvePart2(data).ToString();
        }

        private int GetResultPart1(List<String> data)
        {
            var result = new List<int>( new int[data[0].Length]);
            data.ForEach(value =>
            {
                for(var i = 0; i<value.Length; i++)
                {
                    if(value[i] == '1')
                    {
                        result[i]++;
                    }
                }
            });
            var gamma = result.ConvertAll(value => value > (data.Count / 2));
            var epsilon = gamma.ConvertAll(value => !value);
            PrintInfo("Gamma: " + String.Join(", ", gamma));
            PrintInfo("Epsilon: " + String.Join(", ", epsilon));
            return ConvertToVal(gamma) * ConvertToVal(epsilon);
        }

        private int ConvertToVal(List<bool> bools)
        {
            var acc = 0;
            for(var i = 0; i<bools.Count; i++)
            {
                if (bools[i])
                {
                    acc += (int) Math.Pow(2, bools.Count - i - 1);
                }
            }
            return acc;
        }

        private int SolvePart2(List<String> data)
        {
            return FindOxygenRating(data) * FindCo2Rating(data);
        }

        private int FindOxygenRating(List<String> data)
        {
            var filtered = new List<String>(data);
            for(int i = 0; i < filtered[0].Length; i++)
            {
                var ones = filtered.Count(val => val[i] == '1');
                if(ones < (double)filtered.Count / 2)
                {
                    filtered.RemoveAll(val => val[i] == '1');
                } else
                {
                    filtered.RemoveAll(val => val[i] == '0');
                }
                if(filtered.Count < 2)
                {
                    break;
                }
            }
            var output = ConvertToVal(filtered[0]);
            PrintInfo("Oxygen rating: " + filtered[0] + " (" + output + ")");
            return output;
        }

        private int FindCo2Rating(List<String> data)
        {
            var filtered = new List<String>(data);
            for (int i = 0; i < filtered[0].Length; i++)
            {
                var ones = filtered.Count(val => val[i] == '1');
                if (ones < (double)filtered.Count / 2)
                {
                    filtered.RemoveAll(val => val[i] == '0');
                }
                else
                {
                    filtered.RemoveAll(val => val[i] == '1');
                }
                if (filtered.Count < 2)
                {
                    break;
                }
            }
            var output = ConvertToVal(filtered[0]);
            PrintInfo("CO2 rating: " + filtered[0] + " (" + output + ")");
            return output;
        }

        private int ConvertToVal(String binary)
        {
            var acc = 0;
            for (var i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1')
                {
                    acc += (int)Math.Pow(2, binary.Length - i - 1);
                }
            }
            return acc;
        }

    }
}
