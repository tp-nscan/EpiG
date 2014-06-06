using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes.Builders
{
    public interface IGenomeBuilder<G> : IGuid where G : class, IGenome
    {
        G Make();
        GenomeBuilderType GenomeBuilderType { get; }
        int Seed { get; }
    }

    public interface ISingleGenomeMutator<G> : IGenomeBuilder<G> where G : class, IGenome
    {
        G SourceGenome { get; }
        uint SymbolCount { get; }
    }

    public static class GenomeBuilder
    {
        public static IEnumerable<IGenomeBuilder<IGenome>> MakeGenerators
            (
                int seed,
                int builderCount,
                uint symbolCount,
                int sequenceLength
            )
        {

            var randy = Rando.Fast(seed);

            return Enumerable.Range(0, builderCount)
                             .Select
                             (
                                 i=> MakeGenerator
                                     (
                                        symbolCount:symbolCount,
                                        sequenceLength: sequenceLength,
                                        seed: randy.NextInt(), 
                                        guid: randy.NextGuid()
                                    )
                             );
        }


        public static IGenomeBuilder<IGenome> MakeGenerator
            (
                uint symbolCount,
                int sequenceLength,
                int seed, 
                Guid guid
            )
        {
            return new GenomeBuilderRandom
                (
                    symbolCount: symbolCount,
                    sequenceLength: sequenceLength,
                    seed: seed,
                    guid: guid
                );
        }

        public static ISingleGenomeMutator<IGenome> MakeMutator
            (
                Guid guid,
                IGenome sourceGenome,
                uint symbolCount,
                int seed,
                double deletionRate,
                double insertionRate,
                double mutationRate
            )
        {
            {
                return new GenomeBuilderMutator
                    (
                        symbolCount: symbolCount,
                        sourceGenome: sourceGenome,
                        seed: seed,
                        deletionRate: deletionRate,
                        insertionRate: insertionRate,
                        mutationRate: mutationRate,
                        guid: guid
                    );
            }
        }
    }

    public class GenomeBuilderMutator : ISingleGenomeMutator<IGenome>
    {
        public GenomeBuilderMutator
            (
                Guid guid,
                IGenome sourceGenome,
                uint symbolCount, 
                int seed, 
                double deletionRate, 
                double insertionRate, 
                double mutationRate
            )
        {
            _sourceGenome = sourceGenome;
            _seed = seed;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;
            _mutationRate = mutationRate;
            _guid = guid;
            _symbolCount = symbolCount;
        }

        private readonly IGenome _sourceGenome;
        public IGenome SourceGenome
        {
            get { return _sourceGenome; }
        }

        private readonly uint _symbolCount;
        public uint SymbolCount
        {
            get { return _symbolCount; }
        }

        public IGenome Make()
        {
            return Genome.Make
                (
                    sequence: SourceGenome.Sequence
                );
        }

        public GenomeBuilderType GenomeBuilderType
        {
            get { return GenomeBuilderType.SimpleSingleParent; }
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private readonly double _deletionRate;
        public double DeletionRate
        {
            get { return _deletionRate; }
        }

        private readonly double _insertionRate;
        public double InsertionRate
        {
            get { return _insertionRate; }
        }

        private readonly double _mutationRate;
        public double MutationRate
        {
            get { return _mutationRate; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }

    public class GenomeBuilderRandom : IGenomeBuilder<IGenome>
    {
        public GenomeBuilderRandom
        (
            uint symbolCount,
            int sequenceLength, 
            int seed, 
            Guid guid
        )
        {
            _symbolCount = symbolCount;
            _sequenceLength = sequenceLength;
            _seed = seed;
            _guid = guid;
        }

        public IGenome Make()
        {
            var randy = Rando.Fast(Seed);
            return Genome.Make(
                sequence: Enumerable.Range(0, SequenceLength).Select(i=> randy.NextUint(SymbolCount)).ToList()
                );
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private readonly int _sequenceLength;
        public int SequenceLength
        {
            get { return _sequenceLength; }
        }

        private readonly uint _symbolCount;
        public uint SymbolCount
        {
            get { return _symbolCount; }
        }

        public GenomeBuilderType GenomeBuilderType
        {
            get { return GenomeBuilderType.SimpleRandom; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }

}