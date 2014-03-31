using Sorting.CompetePools;

namespace Sorting.TestData
{
    public static class SorterEvals
    {
        public static ISortResult TestSorterEval(int keyCount, int seed, int keyPairCount)
        {
            return Sorters.TestSorter(keyCount, seed, keyPairCount).ToSorterEval();
        }
    }
}
