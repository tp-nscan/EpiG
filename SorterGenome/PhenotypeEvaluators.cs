using System;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Rand;
using Sorting.Evals;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class PhenotypeEvaluators
    {
        public static Func<IPhenotype<T>, IRando, IPhenotypeEval<T>> MakeStandard<T>()
            where T : ISorter
    {

        Func<T, ISorterEval> phenoEvalFunc =
            s =>
                s.ToSorterResult().ToSorterEval();


        return (p, r) => phenoEvalFunc.ToPhenotypeEval(p, r.NextGuid());
    }

    }
}
