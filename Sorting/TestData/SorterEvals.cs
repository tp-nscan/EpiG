using MathUtils.Rand;
using Sorting.Evals;
using Sorting.Sorters;

namespace Sorting.TestData
{
    public static class SorterEvals
    {
        public static ISorterEval TestSorterEval(int keyCount, int seed, int keyPairCount)
        {
            return Rando.Fast(seed).ToSorter(keyCount, keyPairCount)
                        .ToSorterResult().ToSorterEval();
        }
    }
}
