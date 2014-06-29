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
        public static Func<IGenome, IRando, IPhenotype<T>> MakeStandard<T>
        (
            int keyCount
        )
        where T : ISorter
        {
            Func<IGenome, T> phenoFunc = 
                g =>
                   (T) KeyPairRepository.KeyPairSet(keyCount)
                       .KeyPairs.ToSorter
                       (
                           keyPairChoices: g.Sequence,
                           keyCount: keyCount
                       );

            return (g, r) =>

                phenoFunc.ToPhenotype(g, r.NextGuid());
        }

        public static Func<IGenome, IRando, IPhenotype<T>> MakePermuter<T>
            (
                int keyCount
            )
            where T : ISorter
        {
            Func<IGenome, T> phenoFunc =
                g =>
                    (T) g.Sequence.ToKeyPairs().ToSorter(keyCount);

            return (g, r) =>

                phenoFunc.ToPhenotype(g, r.NextGuid());
        }
    }
}
