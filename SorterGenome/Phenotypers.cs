using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Phenotypes;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class Phenotypers
    {
        public static Func<IGenome, IRando, IEnumerable<IPhenotype<T>>> MakeStandard<T>
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

                phenoFunc.ToPhenotype(g, r.NextGuid()).ToEnumerable();
        }

        public static Func<IGenome, IRando, IEnumerable<IPhenotype<T>>> MakePermuter<T>
            (
                int keyCount
            )
            where T : ISorter
        {
            Func<IGenome, T> phenoFunc1 =
                g =>
                    (T) g.Sequence.ToKeyPairs().ToSorter(keyCount);

            Func<IGenome, T> phenoFunc2 =
                g =>
                    (T)g.Sequence.Skip(keyCount).ToKeyPairs().ToSorter(keyCount);

            return (g, r) => new List<IPhenotype<T>>
            {
                phenoFunc1.ToPhenotype(g, r.NextGuid())
               , phenoFunc2.ToPhenotype(g, r.NextGuid())
            };
        }


    }
}
