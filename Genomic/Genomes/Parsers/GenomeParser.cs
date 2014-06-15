using System.Collections.Generic;
using System.Linq;

namespace Genomic.Genomes.Parsers
{
    public interface IGenomeParser<T, G>  where G : class, IGenomeOld
    {
        IReadOnlyList<ISequenceBlock<T>> GetSequenceBlocks(G genomeOld);
        G GetGenome(IReadOnlyList<ISequenceBlock<T>> sequenceBlocks);
        GenomeParserType GenomeParserType { get; }
    }

    public static class GenomeParser
    {
        public static IGenomeParser<uint?, IGenomeOld> MakeSimple()
        {
            return new SimpleGenomeParser();
        }
    }

    public class SimpleGenomeParser : IGenomeParser<uint?, IGenomeOld>
    {
        public IReadOnlyList<ISequenceBlock<uint?>> GetSequenceBlocks(IGenomeOld genomeOld)
        {
            return genomeOld.Sequence.Select(v => new uint?(v).ToSequenceBlock()).ToList();
        }

        public IGenomeOld GetGenome(IReadOnlyList<ISequenceBlock<uint?>> sequenceBlocks)
        {
            return GenomeOld.Make(sequenceBlocks.SelectMany(b => b.ToGenomeSubSequence()).ToList());
        }

        public GenomeParserType GenomeParserType
        {
            get { return GenomeParserType.Simple; }
        }
    }

}
