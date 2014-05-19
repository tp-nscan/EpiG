using System.Collections.Generic;

namespace Genomic.Genomes
{
    public interface IGenome
    {
        IReadOnlyList<uint> Sequence { get; }
    }

    public static class Genome
    {
        public static IGenome Make(IReadOnlyList<uint> sequence)
        {
            return new SimpleGenomeImpl
                (
                    sequence: sequence
                );
        }
    }

    public class SimpleGenomeImpl : IGenome
    {

        public SimpleGenomeImpl(IReadOnlyList<uint> sequence)
        {
            _sequence = sequence;
        }

        private readonly IReadOnlyList<uint> _sequence;
        public IReadOnlyList<uint> Sequence
        {
            get { return _sequence; }
        }
    }
}