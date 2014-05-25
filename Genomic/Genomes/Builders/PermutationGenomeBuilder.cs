using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes.Parsers;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes.Builders
{
    public interface IPermutationGenomeBuilder : IGenomeBuilder<IGenome>
    {
        int Degree { get; }
        int TargetPermutationCount { get; }
    }

    public static class PermutationGenomeBuilder
    {
        public static IPermutationGenomeBuilder MakeRandom(
                Guid guid,
                int seed,
                int degree,
                int targetPermutationCount
            )
        {

            return new PermutationGenomeBuilderRandom
                (
                    guid: guid,
                    seed: seed,
                    degree: degree,
                    targetPermutationCount: targetPermutationCount
                );
        }


        public static IPermutationGenomeBuilder MakeMutator(
                Guid guid,
                int seed,
                int degree,
                IGenome sourceGenome,
                double mutationRate
            )
        {
            return new PermutationGenomeBuilderMutator
                (
                    guid: guid,
                    seed: seed,
                    permutationGenomeParser: PermutationGenomeParser.Make(degree),
                    sourceGenome: sourceGenome,
                    mutationRate: mutationRate
                );
        }

    }


    public class PermutationGenomeBuilderRandom : IPermutationGenomeBuilder
    {
        public PermutationGenomeBuilderRandom
            (
                Guid guid, 
                int seed,
                int degree,
                int targetPermutationCount
            )
        {
            _guid = guid;
            _seed = seed;
            _degree = degree;
            _targetPermutationCount = targetPermutationCount;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IGenome Make()
        {
            IReadOnlyList<uint> initialPermutation = 
                      Enumerable.Range(0, Degree)
                                .Select(v => (uint)v)
                                .ToList();

            var randy = Rando.Fast(Seed);

            return Genome.Make
                (
                    sequence: Enumerable.Range(0, TargetPermutationCount)
                    .Select(i => initialPermutation.FisherYatesShuffle(randy))
                    .SelectMany(b => b)
                    .ToList()
                );
        }

        public GenomeBuilderType GenomeBuilderType
        {
            get { return GenomeBuilderType.PermutationRandom; }
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private readonly int _degree;
        public int Degree
        {
            get { return _degree; }
        }

        private readonly int _targetPermutationCount;
        public int TargetPermutationCount
        {
            get { return _targetPermutationCount; }
        }

    }

    public class PermutationGenomeBuilderMutator: IPermutationGenomeBuilder
    {
        public PermutationGenomeBuilderMutator
         (
                Guid guid,
                int seed,
                IGenome sourceGenome, 
                IPermutationGenomeParser permutationGenomeParser, 
                double mutationRate
         )
        {
            _guid = guid;
            _seed = seed;
            _sourceGenome = sourceGenome;
            _permutationGenomeParser = permutationGenomeParser;
            _mutationRate = mutationRate;
            _targetPermutationCount = SourceGenome.Sequence.Count/PermutationGenomeParser.Degree;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IGenome Make()
        {
            var randy = Rando.Fast(Seed);

            IReadOnlyList<ISequenceBlock<IPermutation>> sbs =
                PermutationGenomeParser.GetSequenceBlocks(SourceGenome)
                                       .Select(b => b.Data.Mutate(randy, MutationRate))
                                       .Select(mb => mb.ToSequenceBlock())
                                       .ToList();

            return PermutationGenomeParser.GetGenome(sbs);
        }

        public GenomeBuilderType GenomeBuilderType
        {
            get { return GenomeBuilderType.PermutationRandom; }
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private readonly IPermutationGenomeParser _permutationGenomeParser;
        public IPermutationGenomeParser PermutationGenomeParser
        {
            get { return _permutationGenomeParser; }
        }

        private readonly IGenome _sourceGenome;
        public IGenome SourceGenome
        {
            get { return _sourceGenome; }
        }

        public int Degree
        {
            get { return PermutationGenomeParser.Degree; }
        }

        private readonly double _mutationRate;
        public double MutationRate
        {
            get { return _mutationRate; }
        }

        private readonly int _targetPermutationCount;
        public int TargetPermutationCount
        {
            get { return _targetPermutationCount; }
        }
    }

}
