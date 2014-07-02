using System;
using System.Collections.Generic;
using Genomic.Genomes.Builders;
using MathUtils;

namespace Genomic.Genomes
{
    public interface IGenome : IEntity
    {
        IReadOnlyList<uint> Sequence { get; }
        IGenomeBuilder GenomeBuilder { get; }
        int SequenceLength { get; }
    }

    public static class Genome
    {
        public static IGenome Make
            (
                Guid guid,
                IReadOnlyList<uint> sequence,
                IGenomeBuilder genomeBuilder
            )
        {
            return new SimpleGenome(
                    guid: guid,
                    sequence: sequence,
                    genomeBuilder: genomeBuilder
                );
        }
    }

    class SimpleGenome : IGenome 
    {
        public SimpleGenome
        (
            Guid guid,
            IReadOnlyList<uint> sequence,
            IGenomeBuilder genomeBuilder)
        {
            _sequence = sequence;
            _genomeBuilder = genomeBuilder;
            _guid = guid;
        }

        private readonly IReadOnlyList<uint> _sequence;
        public IReadOnlyList<uint> Sequence
        {
            get { return _sequence; }
        }

        private readonly IGenomeBuilder _genomeBuilder;
        public IGenomeBuilder GenomeBuilder
        {
            get { return _genomeBuilder; }
        }

        public int SequenceLength
        {
            get { return _sequence.Count; }
        }

        public string EntityName
        {
            get { return "SimpleGenome"; }
        }

        public IEntity GetPart(Guid key)
        {
            if (key == Guid)
            {
                return this;
            }
            if (key == GenomeBuilder.Guid)
            {
                return GenomeBuilder;
            }
            return null;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
