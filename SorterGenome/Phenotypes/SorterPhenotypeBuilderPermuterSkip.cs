using System;
using System.Linq;
using Genomic.Genomes;
using MathUtils;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome.Phenotypes
{
    public class SorterPhenotypeBuilderPermuterSkip : ISorterPhenotypeBuilder
    {
        public SorterPhenotypeBuilderPermuterSkip(
                Guid guid, 
                IGenome genome, 
                int keyCount, 
                int skipStart,
                int skipBlocks
            )
        {
            _guid = guid;
            _genome = genome;
            _keyCount = keyCount;
            _skipStart = skipStart;
            _skipBlocks = skipBlocks;
        }

        public ISorterPhenotype Make(Guid guid)
        {

            var prefix = Genome.Sequence.Take(KeyCount * SkipStart);

            var sorter = 
                prefix.Concat
                (
                    Genome.Sequence.Skip(KeyCount  * (SkipStart  + SkipBlocks + 1))
                )
                .ToKeyPairs().ToSorter(KeyCount);

   
            return new SorterPhenotypeStandard(
                guid: guid,
                sorter: sorter,
                sorterPhenotypeBuilder: this
                );
        }

        private readonly IGenome _genome;
        public IGenome Genome
        {
            get { return _genome; }
        }


        private readonly int _skipBlocks;
        public int SkipBlocks
        {
            get { return _skipBlocks; }
        }

        private readonly int _skipStart;
        public int SkipStart
        {
            get { return _skipStart; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }


        public string EntityName
        {
            get { return "SorterPhenotypeBuilderPermuterSkip"; }
        }

        public IEntity GetPart(Guid key)
        {
            if (Guid == key)
            {
                return this;
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