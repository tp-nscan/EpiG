using System;
using System.Collections.Generic;
using System.Linq;

namespace Genomic.Genomes
{
    public interface IGenomeParser<T, G>  where G : class, IGenome
    {
        IReadOnlyList<ISequenceBlock<T>> GetSequenceBlocks(G genome);
        G GetGenome(IReadOnlyList<ISequenceBlock<T>> sequenceBlocks);
        GenomeParserType GenomeParserType { get; }
    }

    public static class GenomeParser
    {
        public static IGenomeParser<uint?, IGenome> MakeSimple()
        {
            return new SimpleGenomeParser();
        }
    }

    public class SimpleGenomeParser : IGenomeParser<uint?, IGenome>
    {
        public IReadOnlyList<ISequenceBlock<uint?>> GetSequenceBlocks(IGenome genome)
        {
            return genome.Sequence.Select(v => new uint?(v).ToSequenceBlock()).ToList();
        }

        public IGenome GetGenome(IReadOnlyList<ISequenceBlock<uint?>> sequenceBlocks)
        {
            return Genome.Make(sequenceBlocks.SelectMany(b => b.ToGenomeSubSequence()).ToList());
        }

        public GenomeParserType GenomeParserType
        {
            get { return GenomeParserType.Simple; }
        }
    }

}
