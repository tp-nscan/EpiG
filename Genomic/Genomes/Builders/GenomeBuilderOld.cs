using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes.Builders
{
    public interface IGenomeBuilderOld<G> : IGuid where G : class, IGenomeOld
    {
        G Make();
        GenomeBuilderType GenomeBuilderType { get; }
        int Seed { get; }
    }

    public interface ISingleGenomeMutator<G> : IGenomeBuilderOld<G> where G : class, IGenomeOld
    {
        G SourceGenomeOld { get; }
        uint SymbolCount { get; }
    }

    public static class GenomeBuilderOld
    {
        public static IEnumerable<IGenomeBuilderOld<IGenomeOld>> MakeGenerators
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
                                        symbolCount: symbolCount,
                                        sequenceLength: sequenceLength,
                                        seed: randy.NextInt(), 
                                        guid: randy.NextGuid()
                                    )
                             );
        }


        public static IGenomeBuilderOld<IGenomeOld> MakeGenerator
            (
                uint symbolCount,
                int sequenceLength,
                int seed, 
                Guid guid
            )
        {
            return new GenomeBuilderOldRandom
                (
                    symbolCount: symbolCount,
                    sequenceLength: sequenceLength,
                    seed: seed,
                    guid: guid
                );
        }

        public static ISingleGenomeMutator<IGenomeOld> MakeMutator
            (
                Guid guid,
                IGenomeOld sourceGenomeOld,
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
                        sourceGenomeOld: sourceGenomeOld,
                        seed: seed,
                        deletionRate: deletionRate,
                        insertionRate: insertionRate,
                        mutationRate: mutationRate,
                        guid: guid
                    );
            }
        }
    }

    public class GenomeBuilderMutator : ISingleGenomeMutator<IGenomeOld>
    {
        public GenomeBuilderMutator
            (
                Guid guid,
                IGenomeOld sourceGenomeOld,
                uint symbolCount, 
                int seed, 
                double deletionRate, 
                double insertionRate, 
                double mutationRate
            )
        {
            _sourceGenomeOld = sourceGenomeOld;
            _seed = seed;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;
            _mutationRate = mutationRate;
            _guid = guid;
            _symbolCount = symbolCount;
        }

        private readonly IGenomeOld _sourceGenomeOld;
        public IGenomeOld SourceGenomeOld
        {
            get { return _sourceGenomeOld; }
        }

        private readonly uint _symbolCount;

        public uint SymbolCount
        {
            get { return _symbolCount; }
        }

        public IGenomeOld Make()
        {
            return GenomeOld.Make
                (
                    sequence: SourceGenomeOld.Sequence
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
        private IGenomeOld _sourceGenome;

        public Guid Guid
        {
            get { return _guid; }
        }
    }

    public class GenomeBuilderOldRandom : IGenomeBuilderOld<IGenomeOld>
    {
        public GenomeBuilderOldRandom
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

        public IGenomeOld Make()
        {
            var randy = Rando.Fast(Seed);
            return GenomeOld.Make(
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