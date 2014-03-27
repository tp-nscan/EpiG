using Sorting.CompetePools;

namespace Sorting.TestData
{
    public static class SorterEvals
    {
        public static ISorterEval TestSorterEval(int keyCount, int keyPairCount)
        {
            return Sorters.TestSorter(keyCount, keyPairCount).ToSorterEval();
        }
    }
}
