using System.Collections.Generic;
using Genomic.Genomes.Builders;

namespace Genomic.Genomes
{
    public interface IGenome
    {
        IReadOnlyList<uint> Sequence { get; }
        IGenomeBuilder GenomeBuilder { get; }
    }

    public static class Genome
    {
        public static IGenome Make
            (
                IReadOnlyList<uint> sequence,
                IGenomeBuilder genomeBuilder
            )
        {
            return new SimpleGenome
                (
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
    }
}
