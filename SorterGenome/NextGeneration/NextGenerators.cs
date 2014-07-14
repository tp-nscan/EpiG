using System;
using System.Collections.Generic;
using Genomic.Genomes;
using SorterGenome.PhenotypeEvals;

namespace SorterGenome.NextGeneration
{
    public static class NextGenerators
    {

        public static Func<
            IReadOnlyDictionary<Guid, ISorterPhenotypeEval>,
            int,
            IReadOnlyDictionary<Guid, IGenome>> LookupPhenotypeEvaluator
            (
                string name,
                int keyCount,
                int orgCount,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                double legacyRate,
                double cubRate
            )
        {
            switch (name)
            {
                case "Standard":
                    return new NextGeneratorForStandardSorter
                               (
                                   keyCount : keyCount, 
                                   orgCount : orgCount,
                                   deletionRate: deletionRate,
                                   insertionRate: insertionRate,
                                   mutationRate: mutationRate,
                                   legacyRate: legacyRate,
                                   cubRate: cubRate
                               ).NextGenerator;
                case "Permutation":
                    return new NextGeneratorForPermutationSorter
                                (
                                    keyCount: keyCount,
                                    orgCount: orgCount,
                                    deletionRate: deletionRate,
                                    insertionRate: insertionRate,
                                    mutationRate: mutationRate,
                                    legacyRate: legacyRate,
                                    cubRate: cubRate
                                ).NextGenerator;
                case "PermutationAggregator":
                    return new NextGeneratorForPermutationSorterAggregator
                                (
                                    keyCount: keyCount,
                                    orgCount: orgCount,
                                    deletionRate: deletionRate,
                                    insertionRate: insertionRate,
                                    mutationRate: mutationRate,
                                    legacyRate: legacyRate,
                                    cubRate: cubRate
                                ).NextGenerator;
                default:
                    throw new Exception(String.Format("LookupPhenotyper: {0} not found", name));
            }
        }
    }
}
