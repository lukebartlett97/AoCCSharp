using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace AdventOfCode.Solutions
{
    class PokerHands : SolutionMain
    {

        public PokerHands() : base("year2023/Day7/") { }

        protected override String Solve(List<String> data)
        {
            var hands = data.Select(row => new PokerHand(row)).ToArray();
            Array.Sort(hands);
            var acc = 0;
            for(var i = 0; i < hands.Length; i++)
            {
                PrintInfo($"{i + 1}: {hands[i].Hand} ({hands[i].Rank})");
                acc += hands[i].Bet * (i + 1);
            }
            return acc.ToString();
        }

        private class PokerHand : IComparable<PokerHand>
        {
            public string Hand;
            public int Bet;
            public HandRank Rank;

            public PokerHand(string row) {
                var split = row.Split(' ');
                Hand = split[0];
                Bet = int.Parse(split[1]);
                Rank = CalculateRank();
            }

            public int CompareTo([AllowNull] PokerHand other)
            {
                if(Rank != other.Rank) return Rank - other.Rank;

                var thisValues = CalculateValues();
                var otherValues = other.CalculateValues();
                for (var i  = 0; i < Hand.Length; i++)
                {
                    if (thisValues[i] != otherValues[i]) return thisValues[i] - otherValues[i];
                }
                return 0;
            }

            private int[] CalculateValues()
            {
                return Hand.ToCharArray().Select(val =>
                {
                    switch (val)
                    {
                        case 'A':
                            return 14;
                        case 'K':
                            return 13;
                        case 'Q':
                            return 12;
                        case 'J':
                            return 1;
                        case 'T':
                            return 10;
                        default:
                            return int.Parse(val.ToString());
                    }
                }).ToArray();
            }

            private HandRank CalculateRank()
            {
                var jackless = Hand.ToCharArray().Where(val => val != 'J');
                var jackCount = Hand.Count() - jackless.Count();
                var groups = jackless.GroupBy(val => val).OrderByDescending(group => group.Count());

                if (groups.Count() == 0)
                {
                    return HandRank.FiveOfAKind;
                }

                var largestGroupCount = groups.ElementAt(0).Count() + jackCount;

                if (groups.Count() == 1)
                {
                    return HandRank.FiveOfAKind;
                }
                if (largestGroupCount == 4)
                {
                    return HandRank.FourOfAKind;
                }
                if(largestGroupCount == 3)
                {
                    if(groups.Count() == 2)
                    {
                        return HandRank.FullHouse;
                    } else
                    {
                        return HandRank.ThreeOfAKind;
                    }
                }
                if(largestGroupCount == 2)
                {
                    if (groups.Count() == 3)
                    {
                        return HandRank.TwoPair;
                    }
                    else
                    {
                        return HandRank.Pair;
                    }
                }
                return HandRank.HighCard;
            }
        }

        enum HandRank
        {
            HighCard = 0,
            Pair = 1,
            TwoPair = 2,
            ThreeOfAKind = 3,
            FullHouse = 4,
            FourOfAKind = 5,
            FiveOfAKind = 6
        }
    }
}