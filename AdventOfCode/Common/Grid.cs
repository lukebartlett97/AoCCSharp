using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class Grid<T>
    {
        public IEnumerable<IEnumerable<T>> Data;
        public Grid(IEnumerable<IEnumerable<T>> values) {
            Data = values;
        }
    }
}
