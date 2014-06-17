using System;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genomes.Builders
{
    public interface IGenomeBuilder : IGuid
    {
        IGenome Make();
        string GenomeBuilderType { get; }
    }

    public interface ISimpleGenomeEncoding
    {
        uint SymbolCount { get; }
    }

    public interface IGenomeBuilderMutator : IGuid
    {
        int Seed { get; }
        IGenome SourceGenome { get; }
        string GenomeBuilderType { get; }
    }

    public interface IGenomeBuilderRandom : IGenomeBuilder
    {
        int Seed { get; }
    }

    public interface ISimpleGenomeBuilderRandom : IGenomeBuilderRandom, ISimpleGenomeEncoding
    { }

    public static class GenomeBuilder
    {

    }


    public class GenomeBuilderRandom : ISimpleGenomeBuilderRandom
    {
        public GenomeBuilderRandom
        (
            Guid guid, 
            string genomeBuilderType, 
            int seed, 
            uint symbolCount, 
            int sequenceLength
        )
        {
            _guid = guid;
            _genomeBuilderType = genomeBuilderType;
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
                    sequence: Enumerable.Range(0, SequenceLength).Select(i => randy.NextUint(SymbolCount)).ToList(),
                    genomeBuilder: this
                );
        }

        private readonly string _genomeBuilderType;
        public string GenomeBuilderType
        {
            get { return _genomeBuilderType; }
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


    }
}
