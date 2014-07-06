﻿using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using MathUtils;
using MathUtils.Rand;
using SorterGenome.PhenotypeEvals;
using SorterGenome.Phenotypes;
using Sorting.KeyPairs;
using Utils;

namespace SorterGenome.CompPool
{
    public interface ISorterCompPool : IRandomWalk<ISorterCompPool>, IEntity
    {
        int Generation { get; }

        SorterCompPoolStageType SorterCompPoolStageType { get; }

        IReadOnlyDictionary<Guid, IGenome> Genomes { get; }

        IReadOnlyDictionary<Guid, ISorterPhenotype> Phenotypes { get; }

        IReadOnlyDictionary<Guid, ISorterPhenotypeEval> PhenotypeEvals { get; }

        string NextGeneratorName { get; }

        string PhenotyperName { get; }

        string PhenotyperEvaluatorName { get; }
    }

    public static class SorterCompPool
    {
        public static ISorterCompPool InitStandardFromSeed
        (
            int seed,
            int orgCount,
            int seqenceLength,
            int keyCount,
            double deletionRate, 
            double insertionRate,
            double mutationRate,
            double legacyRate,
            double cubRate
        )
        {
            return new SorterCompPoolStandard
                (
                    guid: Guid.NewGuid(),
                    generation: 0,
                    genomes: Rando.Fast(seed).ToSimpleGenomeBuilders
                            (
                               builderCount: orgCount,
                               symbolCount: (uint)KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount),
                               sequenceLength: seqenceLength
                            ).Select(b => b.Make()).ToDictionary(v => v.GenomeBuilder.Guid),
                    phenotypes: null,
                    phenotypeEvals: null,
                    sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                    keyCount: keyCount,
                    orgCount: orgCount,
                    deletionRate: deletionRate,
                    insertionRate: insertionRate,
                    mutationRate: mutationRate,
                    legacyRate: legacyRate,
                    cubRate: cubRate,
                    phenotyperName: "Standard",
                    phenotyperEvaluatorName: "Standard",
                    nextGeneratorName: "Standard"
                );
        }



        public static ISorterCompPool InitPermuterFromSeed
        (
            int seed,
            int orgCount,
            int degree,
            int permutationCount,
            double deletionRate,
            double insertionRate,
            double mutationRate,
            double legacyRate,
            double cubRate
        )
        {
            return new SorterCompPoolStandard
                (
                    guid: Guid.NewGuid(),
                    generation: 0,
                    genomes: Rando.Fast(seed).ToPermutationGenomeBuilders
                            (
                               builderCount: orgCount,
                               degree: degree,
                               permutationCount: permutationCount
                            ).Select(b => b.Make()).ToDictionary(v => v.GenomeBuilder.Guid),
                    phenotypes: null,
                    phenotypeEvals: null,
                    sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                    keyCount: degree,
                    orgCount: orgCount,
                    deletionRate: deletionRate,
                    insertionRate: insertionRate,
                    mutationRate: mutationRate,
                    legacyRate: legacyRate,
                    cubRate: cubRate,
                    phenotyperName: "MakePermuterSlider",
                    phenotyperEvaluatorName: "Standard",
                    nextGeneratorName: "Permutation"
                );
        }

    }
}







