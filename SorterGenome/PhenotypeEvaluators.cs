using System;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Rand;
using Sorting.CompetePools;
using Sorting.Evals;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class PhenotypeEvaluators
    {
        public static Func<IPhenotype<ISorter>, IRando, IPhenotypeEval<ISorter>> MakeStandard()
        {

            Func<ISorter, ISorterEval> phenoEvalFunc =
                s =>
                    s.ToSorterResult().ToSorterEval();


            return (p, r) => phenoEvalFunc.ToPhenotypeEavl(p, r.NextGuid());
        }

    }
}
