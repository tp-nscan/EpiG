using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes.Builders
{
    public interface IGenomeBuilder : IEntity
    {
        IGenome Make();
    }

    public interface ISimpleGenomeEncoding
    {
        uint SymbolCount { get; }
    }

    public interface IGenomeBuilderMutator : IGenomeBuilder
    {
        IGenome SourceGenome { get; }
    }

    public interface IGenomeBuilderRandom : IGenomeBuilder
    {
        int Seed { get; }
    }

    public interface ISimpleGenomeBuilderRandom : IGenomeBuilderRandom, ISimpleGenomeEncoding
    { }

    public interface ISimpleGenomeBuilderMutator : IGenomeBuilderMutator, ISimpleGenomeEncoding
    { }

    public static class GenomeBuilder
    {
        public static IEnumerable<ISimpleGenomeBuilderRandom> ToSimpleGenomeBuilders
            (
                this IRando randy,
                int builderCount,
                uint symbolCount,
                int sequenceLength
            )
        {
            return Enumerable.Range(0, builderCount)
                             .Select
                             (
                                 i => MakeSimpleGenomeBuilderRandom
                                     (
                                        symbolCount: symbolCount,
                                        sequenceLength: sequenceLength,
                                        seed: randy.NextInt(),
                                        guid: randy.NextGuid()
                                    )
                             );
        }

        public static ISimpleGenomeBuilderRandom MakeSimpleGenomeBuilderRandom
            (
                Guid guid,
                int seed,
                uint symbolCount,
                int sequenceLength
            )
        {
            return new SimpleGenomeBuilderRandom
                (
                    guid: guid,
                    seed: seed,
                    symbolCount: symbolCount,
                    sequenceLength: sequenceLength
                );
        }

        public static ISimpleGenomeBuilderMutator ToSimpleGenomeBuilderMutator
            (
                this IGenome sourceGenome,
                Guid guid,
                uint symbolCount,
                int seed,
                double deletionRate,
                double insertionRate,
                double mutationRate
            )
        {
            {
                return new SimpleGenomeBuilderMutator
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


    public class SimpleGenomeBuilderRandom : ISimpleGenomeBuilderRandom
    {
        public SimpleGenomeBuilderRandom
        (
            Guid guid,
            int seed, 
            uint symbolCount, 
            int sequenceLength
        )
        {
            _guid = guid;
            _seed = seed;
            _symbolCount = symbolCount;
            _sequenceLength = sequenceLength;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IGenome Make()
        {
            var randy = Rando.Fast(Seed);
            return Genome.Make(
                    sequence: Enumerable.Range(0, SequenceLength)
                                        .Select(i => randy.NextUint(SymbolCount))
                                        .ToList(),
                    genomeBuilder: this
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


        public string EntityName
        {
            get { return "SimpleGenomeBuilderRandom"; }
        }

        public IEntity GetPart(Guid key)
        {
            throw new NotImplementedException();
        }
    }

    public class SimpleGenomeBuilderMutator : ISimpleGenomeBuilderMutator
    {
        public SimpleGenomeBuilderMutator
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
            var randy = Rando.Fast(Seed);
            var randoMutate = randy.Spawn();
            var randoInsert = randy.Spawn();
            var randoDelete = randy.Spawn();

            return Genome.Make
                (
                    sequence: SourceGenome
                                .Sequence
                                .MutateInsertDelete
                                (
                                    doMutation: randoMutate.ToBoolEnumerator(MutationRate),
                                    doInsertion: randoInsert.ToBoolEnumerator(InsertionRate),
                                    doDeletion: randoDelete.ToBoolEnumerator(DeletionRate),
                                    mutator: x => randy.NextUint(SymbolCount),
                                    inserter: x => randy.NextUint(SymbolCount),
                                    paddingFunc: x => randy.NextUint(SymbolCount)
                                ).ToList(),
                    genomeBuilder: this
                );
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

        public string EntityName
        {
            get { return "SimpleGenomeBuilderRandom"; }
        }

        public IEntity GetPart(Guid key)
        {
            throw new NotImplementedException();
        }
    }
}
