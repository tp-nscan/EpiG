using System;
using System.Collections.Generic;
using Genomic.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace SorterGenome.Phenotypes
{
    public static class Phenotypers
    {
        public static Func<
            IGenome,
            IRando,
            IEnumerable<ISorterPhenotype>> LookupPhenotyper
            (
            string name,
            int keyCount
            )
        {
            switch (name)
            {
                case "Standard":
                    return MakeStandard(keyCount);
                case "MakePermuterSlider":
                    return MakePermuterSlider(keyCount);
                default:
                    throw new Exception(String.Format("LookupPhenotyper: {0} not found", name));
            }
        }

        public static Func<
            IGenome,
            IRando,
            IEnumerable<ISorterPhenotype>> MakeStandard
            (
            int keyCount
            )
        {
            return (g, r) => g.ToSorterPhenotypeStandard
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount
                ).ToEnumerable();
        }


        public static Func
            <
                IGenome,
                IRando,
                IEnumerable<ISorterPhenotype>> MakePermuterSlider
            (
                int keyCount
            )
        {
            //return (g, r) => g.ToSorterPhenotypePermuter
            //(
            //    builderGuid: r.NextGuid(),
            //    phenotypeGuid: r.NextGuid(),
            //    keyCount: keyCount
            //).ToEnumerable();


            return (g, r) => new List<ISorterPhenotype>
            {
                g.ToSorterPhenotypePermuter
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    skips: 0
                )
                //,
                // g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 1
                //),
                // g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 2
                //),
                //g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 3
                //),
                // g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 4
                //)

            };


        }

    //public static Func
        //    <
        //        IGenome,
        //        IRando,
        //        IEnumerable<ISorterPhenotype>> MakePermuterSlider
        //    (
        //        int keyCount
        //    )
        //{

        //    Func<IGenome, ISorter> phenoFunc1 =
        //        g =>
        //        {
        //            var prefix = g.Sequence.Take(keyCount * 0);
        //            return
        //                prefix.Concat
        //                (
        //                    g.Sequence.Skip(keyCount * 2)
        //                )
        //                .ToKeyPairs().ToSorter(keyCount);
        //        };

        //    Func<IGenome, ISorter> phenoFunc2 =
        //        g =>
        //        {
        //            var prefix = g.Sequence.Take(keyCount * 1);
        //            return
        //                prefix.Concat
        //                (
        //                    g.Sequence.Skip(keyCount * 2)
        //                )
        //                .ToKeyPairs().ToSorter(keyCount);
        //        };


        //    Func<IGenome, ISorter> phenoFunc3 =
        //        g =>
        //        {
        //            var prefix = g.Sequence.Take(keyCount * 2);
        //            return
        //                prefix.Concat
        //                (
        //                    g.Sequence.Skip(keyCount * 3)
        //                )
        //                .ToKeyPairs().ToSorter(keyCount);
        //        };


        //    Func<IGenome, ISorter> phenoFunc4 =
        //        g =>
        //        {
        //            var prefix = g.Sequence.Take(keyCount * 3);
        //            return
        //                prefix.Concat
        //                (
        //                    g.Sequence.Skip(keyCount * 4)
        //                )
        //                .ToKeyPairs().ToSorter(keyCount);
        //        };

        //    Func<IGenome, ISorter> phenoFunc5 =
        //        g =>
        //        {
        //            var prefix = g.Sequence.Take(keyCount * 4);
        //            return
        //                prefix.Concat
        //                (
        //                    g.Sequence.Skip(keyCount * 5)
        //                )
        //                .ToKeyPairs().ToSorter(keyCount);
        //        };

        //    Func<IGenome, ISorter> phenoFunc6 =
        //        g =>
        //        {
        //            var prefix = g.Sequence.Take(keyCount * 5);
        //            return
        //                prefix.Concat
        //                (
        //                    g.Sequence.Skip(keyCount * 6)
        //                )
        //                .ToKeyPairs().ToSorter(keyCount);
        //        };


        //    return (g, r) => Enumerable.Empty<ISorterPhenotype>();




        //    //    new List<ISorterPhenotype>
        //    //{
        //    //      phenoFunc1(g, r.NextGuid())
        //    //    , phenoFunc2.ToPhenotype(g, r.NextGuid())
        //    //    , phenoFunc3.ToPhenotype(g, r.NextGuid())
        //    //    , phenoFunc4.ToPhenotype(g, r.NextGuid())
        //    //    , phenoFunc5.ToPhenotype(g, r.NextGuid())
        //    //    , phenoFunc6.ToPhenotype(g, r.NextGuid())
        //    //};
        //}

    }
}
