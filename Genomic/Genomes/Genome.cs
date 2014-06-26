using System;
using System.Collections.Generic;
using Genomic.Genomes.Builders;
using MathUtils.Collections;

namespace Genomic.Genomes
{
    public interface IGenome : IGuid
    {
        IReadOnlyList<uint> Sequence { get; }
        IGenomeBuilder GenomeBuilder { get; }
        int SequenceLength { get; }
    }

    public static class Genome
    {
        public static IGenome Make
            (
                IReadOnlyList<uint> sequence,
                IGenomeBuilder genomeBuilder
            )
        {
            return new SimpleGenome(
                    sequence: sequence,
                    genomeBuilder: genomeBuilder
                );
        }
    }

    class SimpleGenome : IGenome 
    {
        public SimpleGenome
        (
            IReadOnlyList<uint> sequence,
            IGenomeBuilder genomeBuilder
        )
        {
            _sequence = sequence;
            _genomeBuilder = genomeBuilder;
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

        public Guid Guid
        {
            get { return _genomeBuilder.Guid; }
        }
    }
}
