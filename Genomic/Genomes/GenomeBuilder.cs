using System;
using MathUtils.Collections;

namespace Genomic.Genomes
{
    public interface IGenomeBuilder<G> : IGuid where G : class, IGenome
    {
        G Make();
        GenomeBuilderType GenomeBuilderType { get; }
    }

    public interface ISingleGenomeMutator<G> : IGenomeBuilder<G> where G : class, IGenome
    {
        G SourceGenome { get; }
    }

    public static class GenomeBuilder
    {
        public static ISingleGenomeMutator<IGenome> MakeMutator
            (
                IGenome sourceGenome,
                int seed,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                Guid guid
            )
        {
            {
                return new GenomeMutatorImpl
                    (
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

    public class GenomeMutatorImpl : ISingleGenomeMutator<IGenome>
    {
        public GenomeMutatorImpl
            (
                IGenome sourceGenome, 
                int seed, 
                double deletionRate, 
                double insertionRate, 
                double mutationRate, 
                Guid guid
            )
        {
            _sourceGenome = sourceGenome;
            _seed = seed;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;
            _mutationRate = mutationRate;
            _guid = guid;
        }

        private readonly IGenome _sourceGenome;
        public IGenome SourceGenome
        {
            get { return _sourceGenome; }
        }

        public IGenome Make()
        {
            return Genome.Make(
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

    public class GenomeGenerator : IGenomeBuilder<IGenome>
    {
        public GenomeGenerator
            (
                int symbolCount, 
                int sequenceLength, 
                int seed
            )
        {
            _symbolCount = symbolCount;
            _sequenceLength = sequenceLength;
            _seed = seed;
        }

        public IGenome Make()
        {
            throw new NotImplementedException();
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

        private readonly int _symbolCount;
        public int SymbolCount
        {
            get { return _symbolCount; }
        }

        public GenomeBuilderType GenomeBuilderType
        {
            get { return GenomeBuilderType.SimpleRandom; }
        }

        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }

}