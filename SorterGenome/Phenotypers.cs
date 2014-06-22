using System;
using Genomic.Genomes;
using Genomic.Phenotypes;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class Phenotypers
    {
        public static Func<IGenome, IRando, IPhenotype<ISorter>> MakeStandard
        (
            int keyCount
        )
        {
            Func<IGenome, ISorter> phenoFunc = 
                g =>
                   KeyPairRepository.KeyPairSet(keyCount)
                                    .KeyPairs.ToSorter
                                    (
                                        keyPairChoices: g.Sequence,
                                        keyCount: keyCount
                                    );

            return (g, r) =>

                phenoFunc.ToPhenotype(g, r.NextGuid());
        }

    }
}
