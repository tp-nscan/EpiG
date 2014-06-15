using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;

namespace Genomic.Genomes.Parsers
{
    public interface IPermutationGenomeParser : IGenomeParser<IPermutation, IGenome> 
    {
        int Degree { get; }
    }

    public static class PermutationGenomeParser
    {
        public static IPermutationGenomeParser Make(int degree)
        {
            return new PermutationGenomeParserImpl(degree);
        }
    }

    public class PermutationGenomeParserImpl : IPermutationGenomeParser
    {
        public PermutationGenomeParserImpl(int degree)
        {
            _degree = degree;
        }

        public IReadOnlyList<ISequenceBlock<IPermutation>> GetSequenceBlocks(IGenome genome)
        {
            return genome.Sequence
                        .Chunk(Degree)
                        .Select(c => Permutation.Make(c).ToSequenceBlock())
                        .ToList();
        }

        public IReadOnlyList<uint> GetSequence(IReadOnlyList<ISequenceBlock<IPermutation>> sequenceBlocks)
        {
                return 
                    sequenceBlocks.SelectMany(b => b.Data.Values())
                                  .Select(v => (uint)v)
                                  .ToList();
        }

        public GenomeParserType GenomeParserType
        {
            get { return GenomeParserType.Permutation; }
        }

        private readonly int _degree;
        public int Degree
        {
            get { return _degree; }
        }
    }
}
