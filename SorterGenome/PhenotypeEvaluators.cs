using System;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils;
using MathUtils.Rand;
using Sorting.Evals;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class PhenotypeEvaluators
    {
        public static Func<IPhenotype, IRando, IPhenotypeEval> MakeStandard<T>()
            where T : ISorter, IEntity
        {

        Func<T, ISorterEval> phenoEvalFunc =
            s =>
                s.ToSorterResult().ToSorterEval();


            return null;// (p, r) => phenoEvalFunc.ToPhenotypeEval(p, r.NextGuid());
    }

    }
}
