using System;
using MathUtils.Rand;
using Sorting.Sorters;

namespace Sorting.TestData
{
    public static class Sorters
    {
        public static ISorter TestSorter(int keyCount, int keyPairCount)
        {
            return Rando.Fast(212).ToSorter(keyCount, keyPairCount, Guid.NewGuid());
        }
    }
}
