using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Phenotypes;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome
{
    public static class Phenotypers
    {
        public static Func<IGenome, IRando, IEnumerable<IPhenotype>> MakeStandard
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

            return (g, r) => null;

                ///phenoFunc.ToPhenotype(g, r.NextGuid()).ToEnumerable();
        }

        public static Func<IGenome, IRando, IEnumerable<IPhenotype>> MakePermuterSlider
            (
                int keyCount
            )
        {

            Func<IGenome, ISorter> phenoFunc1 =
                g =>
                {
                    var prefix = g.Sequence.Take(keyCount*0);
                    return
                        prefix.Concat
                        (
                            g.Sequence.Skip(keyCount * 2)
                        )
                        .ToKeyPairs().ToSorter(keyCount);
                };

            Func<IGenome, ISorter> phenoFunc2 =
                g =>
                {
                    var prefix = g.Sequence.Take(keyCount * 1);
                    return
                        prefix.Concat
                        (
                            g.Sequence.Skip(keyCount * 2)
                        )
                        .ToKeyPairs().ToSorter(keyCount);
                };


            Func<IGenome, ISorter> phenoFunc3 =
                g =>
                {
                    var prefix = g.Sequence.Take(keyCount * 2);
                    return
                        prefix.Concat
                        (
                            g.Sequence.Skip(keyCount * 3)
                        )
                        .ToKeyPairs().ToSorter(keyCount);
                };


            Func<IGenome, ISorter> phenoFunc4 =
                g =>
                {
                    var prefix = g.Sequence.Take(keyCount * 3);
                    return 
                        prefix.Concat
                        (
                            g.Sequence.Skip(keyCount * 4)
                        )
                        .ToKeyPairs().ToSorter(keyCount);
                };

            Func<IGenome, ISorter> phenoFunc5 =
                g =>
                {
                    var prefix = g.Sequence.Take(keyCount*4);
                    return
                        prefix.Concat
                        (
                            g.Sequence.Skip(keyCount * 5)
                        )
                        .ToKeyPairs().ToSorter(keyCount);
                };

            Func<IGenome, ISorter> phenoFunc6 =
                g =>
                {
                    var prefix = g.Sequence.Take(keyCount * 5);
                    return
                        prefix.Concat
                        (
                            g.Sequence.Skip(keyCount * 6)
                        )
                        .ToKeyPairs().ToSorter(keyCount);
                };


            return (g, r) => Enumerable.Empty<IPhenotype>();




            //    new List<IPhenotype>
            //{
            //      phenoFunc1.ToPhenotype(g, r.NextGuid())
            //    , phenoFunc2.ToPhenotype(g, r.NextGuid())
            //    , phenoFunc3.ToPhenotype(g, r.NextGuid())
            //    , phenoFunc4.ToPhenotype(g, r.NextGuid())
            //    , phenoFunc5.ToPhenotype(g, r.NextGuid())
            //    , phenoFunc6.ToPhenotype(g, r.NextGuid())
            //};
        }


    }
}
