using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Solutions
{
    class Seedmapper : SolutionMain
    {

        public Seedmapper() : base("year2023/Day5/") { }

        protected override String Solve(List<String> data)
        {
            var seedRanges = GetSeedRangesPart2(data.ElementAt(0));
            List<List<Mapper>> mapperSets = GetMapperSets(data);

            foreach (var mapperSet in mapperSets)
            {
                PrintInfo(String.Join(", ", seedRanges));
                PrintInfo("");
                seedRanges = seedRanges.SelectMany(seedRange =>
                {
                    var startVal = seedRange.StartVal;
                    var endVal = seedRange.EndVal;
                    var newRanges = new List<SeedRange>();
                    foreach (var mapper in mapperSet)
                    {

                        if (mapper.SourceStart > startVal)
                        {
                            var topLimit = BigInteger.Min(mapper.SourceStart - 1, endVal);
                            newRanges.Add(new SeedRange(startVal, topLimit));
                            startVal = topLimit + 1;
                            if (startVal > endVal)
                            {
                                return newRanges;
                            }
                        }

                        if (mapper.SourceEnd >= startVal)
                        {
                            var topLimit = BigInteger.Min(mapper.SourceEnd, endVal);
                            newRanges.Add(new SeedRange(startVal + mapper.Shift, topLimit + mapper.Shift));
                            startVal = topLimit + 1;
                            if (startVal > endVal)
                            {
                                return newRanges;
                            }
                        }
                    }
                    return new List<SeedRange>() { new SeedRange(startVal, endVal) };
                });
            }
            PrintInfo(String.Join(", ", seedRanges));

            return seedRanges.Select(seedRange => seedRange.StartVal).Min().ToString();
        }

        private static List<List<Mapper>> GetMapperSets(List<string> data)
        {
            var mapperSets = new List<List<Mapper>>();
            var currentMapperSet = new List<Mapper>();
            for (int i = 3; i < data.Count(); i++)
            {
                var row = data[i];
                if (row.Equals(""))
                {
                    i++;
                    currentMapperSet.Sort((a, b) => a.SourceStart - b.SourceStart < 0 ? -1 : 1);
                    mapperSets.Add(currentMapperSet);
                    currentMapperSet = new List<Mapper>();
                    continue;
                }
                var split = row.Split(" ");
                currentMapperSet.Add(new Mapper(split[0], split[1], split[2]));
            }
            currentMapperSet.Sort((a, b) => a.SourceStart - b.SourceStart < 0 ? -1 : 1);
            mapperSets.Add(currentMapperSet);
            return mapperSets;
        }

        private IEnumerable<SeedRange> GetSeedRangesPart1(string row)
        {
            return row[6..].Trim().Split(" ").Select(seed => new SeedRange(BigInteger.Parse(seed), BigInteger.Parse(seed)));
        }

        private IEnumerable<SeedRange> GetSeedRangesPart2(string row)
        {
            var values = row[6..].Trim().Split(" ");
            var seedRanges = new List<SeedRange>();
            for (int i = 0; i < values.Length; i += 2)
            {
                seedRanges.Add(new SeedRange(BigInteger.Parse(values[i]), BigInteger.Parse(values[i]) + BigInteger.Parse(values[i + 1]) - 1));
            }
            return seedRanges;
        }

        private class SeedRange
        {
            public BigInteger StartVal;
            public BigInteger EndVal;

            public SeedRange(BigInteger startVal, BigInteger endVal)
            {
                StartVal = startVal;
                EndVal = endVal;
            }

            public override string ToString()
            {
                return $"({StartVal}, {EndVal})";
            }
        }

        private class Mapper
        {
            public BigInteger Shift;
            public BigInteger SourceStart;
            public BigInteger SourceEnd;

            public Mapper(string destinationStart, string sourceStart, string range)
            {
                this.Shift = BigInteger.Parse(destinationStart) - BigInteger.Parse(sourceStart);
                this.SourceStart = BigInteger.Parse(sourceStart);
                this.SourceEnd = BigInteger.Parse(sourceStart) + BigInteger.Parse(range) - 1;
            }

            public override string ToString()
            {
                return $"<Shift:{Shift} Range:{SourceStart}-{SourceEnd}>";
            }
        }
    }
}