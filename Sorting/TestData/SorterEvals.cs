using Sorting.CompetePools;
using Sorting.Evals;

namespace Sorting.TestData
{
    public static class SorterEvals
    {
        public static ISorterEval TestSorterEval(int keyCount, int seed, int keyPairCount)
        {
            return Sorters.TestSorter(keyCount, seed, keyPairCount).ToSorterResult() 
                .ToSorterEval();
        }
    }
}
