using System.Collections.Generic;
using System.Linq;

namespace Genomic.Genomes.Parsers
{
    //public interface IGenomeParser<T, G>  where G : class, IGenome
    //{
    //    IReadOnlyList<ISequenceBlock<T>> GetSequenceBlocks(G genomeOld);
    //    IReadOnlyList<uint> GetSequence(IReadOnlyList<ISequenceBlock<T>> sequenceBlocks);
    //    GenomeParserType GenomeParserType { get; }
    //}

    //public static class GenomeParser
    //{
    //    public static IGenomeParser<uint?, IGenome> MakeSimple()
    //    {
    //        return new SimpleGenomeParser();
    //    }
    //}

    //public class SimpleGenomeParser : IGenomeParser<uint?, IGenome>
    //{
    //    public IReadOnlyList<ISequenceBlock<uint?>> GetSequenceBlocks(IGenome genomeOld)
    //    {
    //        return genomeOld.Sequence.Select(v => new uint?(v).ToSequenceBlock()).ToList();
    //    }

    //    public IReadOnlyList<uint> GetSequence(IReadOnlyList<ISequenceBlock<uint?>> sequenceBlocks)
    //    {
    //        return sequenceBlocks.SelectMany(b => b.ToGenomeSubSequence()).ToList();
    //    }

    //    public GenomeParserType GenomeParserType
    //    {
    //        get { return GenomeParserType.Simple; }
    //    }
    //}

}
