using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using MathUtils.Collections;

namespace Genomic.Genomes.Parsers
{
    public interface IPermutationGenomeParser : IGenomeParser<IPermutation, IGenomeOld> 
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

        public IReadOnlyList<ISequenceBlock<IPermutation>> GetSequenceBlocks(IGenomeOld genomeOld)
        {
            return genomeOld.Sequence
                        .Chunk(Degree)
                        .Select(c => Permutation.Make(c).ToSequenceBlock())
                        .ToList();
        }

        public IGenomeOld GetGenome(IReadOnlyList<ISequenceBlock<IPermutation>> sequenceBlocks)
        {
            return GenomeOld.Make(sequenceBlocks.SelectMany(b => b.Data.Values())
                .Select(v=>(uint)v)
                .ToList());
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
