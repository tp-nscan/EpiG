using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using MathUtils;
using MathUtils.Rand;
using SorterGenome.NextGeneration.NextGenSpecs;
using SorterGenome.PhenotypeEvals;
using SorterGenome.PhenotypeEvals.PhenotypeEvalSpecs;
using SorterGenome.Phenotypes;
using SorterGenome.Phenotypes.PhenotyperSpecs;
using Sorting.KeyPairs;
using Utils;

namespace SorterGenome.CompPool
{
    public interface ISorterCompPool : IRandomWalk<ISorterCompPool>, IEntity
    {
        int Generation { get; }

        string Name { get; }
        
        SorterCompPoolStageType SorterCompPoolStageType { get; }

        IReadOnlyDictionary<Guid, IGenome> Genomes { get; }

        IReadOnlyDictionary<Guid, ISorterPhenotype> Phenotypes { get; }

        IReadOnlyDictionary<Guid, ISorterPhenotypeEval> PhenotypeEvals { get; }

        INextGenSpec NextGenSpec { get; }

        IPhenotyperSpec PhenotyperSpec { get; }

        IPhenotypeEvalSpec PhenotyperEvalSpec { get; }
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
            double cubRate,
            string name
        )
        {
            var randy = Rando.Fast(seed);
            return new SorterCompPoolStandard
                (
                    guid: randy.NextGuid(),
                    generation: 0,
                    genomes: randy.ToSimpleGenomeBuilders
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
                    phenotyperSpec: new PhenotyperSpecStandard(), 
                    phenotyperEvalSpec: new PhenotypeEvalSpecStandard(), 
                    nextGenSpec: new StandardNextGenSpec(), 
                    name: name
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
            double cubRate,
            string name
        )
        {
            var randy = Rando.Fast(seed);
            return new SorterCompPoolStandard
                (
                    guid: randy.NextGuid(),
                    generation: 0,
                    genomes: randy.ToPermutationGenomeBuilders
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
                    phenotyperSpec: new PhenotyperSpecPermuterSlider(), 
                    phenotyperEvalSpec: new PhenotypeEvalSpecStandard(),
                    nextGenSpec: new PermutationAggregatorNextGenSpec(), 
                    name: name
                );
        }

    }
}







