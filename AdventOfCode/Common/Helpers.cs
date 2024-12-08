using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Common;

public static class Helpers
{
    public static List<int> ConvertToIntegerList(this List<String> strings)
    {
        return strings
            .Select(x => int.Parse(x))
            .ToList();
    }

    public static List<long> ConvertToLongList(this List<String> strings)
    {
        return strings
            .Select(x => long.Parse(x))
            .ToList();
    }
}
