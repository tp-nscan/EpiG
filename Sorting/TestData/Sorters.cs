using MathUtils.Rand;
using Sorting.Sorters;

namespace Sorting.TestData
{
    public static class Sorters
    {
        public static ISorter TestSorter(int keyCount, int seed, int keyPairCount)
        {
            return Rando.Fast(seed).ToSorter(keyCount, keyPairCount);
        }
    }
}
