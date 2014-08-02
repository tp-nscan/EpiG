using System;
using System.Collections.Generic;
using Genomic.Genomes;
using SorterGenome.PhenotypeEvals;

namespace SorterGenome.NextGeneration.NextGenSpecs
{
    public enum NextGenSpecType
    {
        Standard,
        Permutation,
        PermutationAggregator
    }

    public interface INextGenSpec
    {
        NextGenSpecType NextGenSpecType { get; }
    }

    public static class NextGenSpec
    {
        public static Func<
                IReadOnlyDictionary<Guid, ISorterPhenotypeEval>,
                int,
                IReadOnlyDictionary<Guid, IGenome>
            >
            ToNextGenerator
            (
                this INextGenSpec nextGenSpec,
                int keyCount,
                int orgCount,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                int legacyCount,
                int cubCount
            )
        {
            switch (nextGenSpec.NextGenSpecType)
            {
                case NextGenSpecType.Standard:
                    return new NextGeneratorForStandardSorter
                       (
                           keyCount: keyCount,
                           orgCount: orgCount,
                           deletionRate: deletionRate,
                           insertionRate: insertionRate,
                           mutationRate: mutationRate,
                           legacyCount: legacyCount,
                           cubCount: cubCount
                       ).NextGenerator;

                case NextGenSpecType.Permutation:
                    return new NextGeneratorForPermutationSorter
                                (
                                    keyCount: keyCount,
                                    orgCount: orgCount,
                                    deletionRate: deletionRate,
                                    insertionRate: insertionRate,
                                    mutationRate: mutationRate,
                                    legacyCount: legacyCount,
                                    cubCount: cubCount
                                ).NextGenerator;

                case NextGenSpecType.PermutationAggregator:
                    return new NextGeneratorForPermutationSorterAggregator
                                (
                                    keyCount: keyCount,
                                    orgCount: orgCount,
                                    deletionRate: deletionRate,
                                    insertionRate: insertionRate,
                                    mutationRate: mutationRate,
                                    legacyCount: legacyCount,
                                    cubCount: cubCount
                                ).NextGenerator;
            }
            return null;
        }
    }


    public class StandardNextGenSpec : INextGenSpec
    {
        public NextGenSpecType NextGenSpecType
        {
            get { return NextGenSpecType.Standard; }
        }
    }


    public class PermutationNextGenSpec : INextGenSpec
    {
        public NextGenSpecType NextGenSpecType
        {
            get { return NextGenSpecType.Permutation; }
        }
    }


    public class PermutationAggregatorNextGenSpec : INextGenSpec
    {
        public NextGenSpecType NextGenSpecType
        {
            get { return NextGenSpecType.PermutationAggregator; }
        }
    }
}
