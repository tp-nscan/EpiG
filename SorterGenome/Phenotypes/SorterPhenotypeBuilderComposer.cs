using System;
using System.Linq;
using Genomic.Genomes;
using MathUtils;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome.Phenotypes
{
    public class SorterPhenotypeBuilderComposer : ISorterPhenotypeBuilder
    {
        public SorterPhenotypeBuilderComposer(
            Guid guid, 
            IGenome genome, 
            int keyCount, 
            int skips
            )
        {
            _guid = guid;
            _genome = genome;
            _keyCount = keyCount;
            _skips = skips;
        }

        public ISorterPhenotype Make(Guid guid)
        {
            var prefix = Genome.Sequence.Take(KeyCount * Skips);
            var sorter = 
                prefix.Concat
                (
                    Genome.Sequence.Skip(KeyCount * (Skips + 1))
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

        private readonly int _skips;
        public int Skips
        {
            get { return _skips; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }


        public string EntityName
        {
            get { return "SorterPhenotypeBuilderComposer"; }
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

    public static class OpsEx
    {
        

    }
}
