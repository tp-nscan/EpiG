using System;
using MathUtils.Collections;

namespace Genomic.Genomes.Builders
{
    public interface IGenomeBuilder : IGuid
    {
        IGenome Make();
        string GenomeBuilderType { get; }
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

    public static class GenomeBuilder
    {

    }


    public class GenomeBuilderRandom : IGenomeBuilderRandom
    {
        public GenomeBuilderRandom(Guid guid, string genomeBuilderType, int seed)
        {
            _guid = guid;
            _genomeBuilderType = genomeBuilderType;
            _seed = seed;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IGenome Make()
        {
            throw new NotImplementedException();
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
    }
}
