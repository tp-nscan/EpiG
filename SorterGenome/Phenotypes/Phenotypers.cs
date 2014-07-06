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
    }
}
