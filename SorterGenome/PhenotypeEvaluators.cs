using System;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Rand;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class PhenotypeEvaluators
    {
        public static Func<IPhenotype<ISorter>, IRando, IPhenotypeEval<ISorter>> MakeStandard
        (
            int keyCount
        )
        {
            return null;
        }

    }
}
