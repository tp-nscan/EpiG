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
                case "MakePermuterCubeCombo":
                    return MakePermuterCubeCombo(keyCount);
                case "MakePermuterComposer":
                    return MakePermuterComposer(keyCount);
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


        public static Func<
                IGenome,
                IRando,
                IEnumerable<ISorterPhenotype>> MakePermuterSlider
            (
                int keyCount
            )
        {
            return (g, r) => new List<ISorterPhenotype>
            {
                //g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 0
                //),
                // g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 1
                //),
                //g.ToSorterPhenotypePermuter
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
                //)
                //,
                // g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 4
                //),
                // g.ToSorterPhenotypePermuter
                //(
                //    builderGuid: r.NextGuid(),
                //    phenotypeGuid: r.NextGuid(),
                //    keyCount: keyCount,
                //    skips: 5
                //)
                //,

                g.ToSorterPhenotypePermuter
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    skipStart: 5,
                    skipBlocks: 3
                )
                ,

                 g.ToSorterPhenotypePermuter
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    skipStart: 8,
                    skipBlocks: 3
                )
            };

        }



        public static Func<
        IGenome,
        IRando,
        IEnumerable<ISorterPhenotype>> MakePermuterCubeCombo
        (
            int keyCount
        )
        {
            return (g, r) => new List<ISorterPhenotype>
            {
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: true,
                    marginC: true
                )
                ,
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: true,
                    marginC: false
                ),
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: false,
                    marginC: true
                )
                ,
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: false,
                    marginC: false
                ),
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: true,
                    marginC: true
                )
                ,
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: true,
                    marginC: false
                )
                ,
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: false,
                    marginC: true
                ),
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: false,
                    marginC: false
                )
            };

        }


        public static Func<
            IGenome,
            IRando,
            IEnumerable<ISorterPhenotype>> MakePermuterComposer
            (
                int keyCount
            )
        {
            return (g, r) => new List<ISorterPhenotype>
            {
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: true,
                    marginC: true
                )
                ,
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: true,
                    marginC: false
                ),
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: false,
                    marginC: true
                )
                ,
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: true,
                    marginB: false,
                    marginC: false
                ),
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: true,
                    marginC: true
                )
                ,
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: true,
                    marginC: false
                )
                ,
                g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: false,
                    marginC: true
                ),
                 g.ToSorterPhenotypePermuterCubeCombo
                (
                    builderGuid: r.NextGuid(),
                    phenotypeGuid: r.NextGuid(),
                    keyCount: keyCount,
                    marginA: false,
                    marginB: false,
                    marginC: false
                )
            };

        }




    }
}
