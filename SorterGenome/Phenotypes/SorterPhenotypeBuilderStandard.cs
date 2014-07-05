using System;
using Genomic.Genomes;
using MathUtils;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome.Phenotypes
{
    public class SorterPhenotypeBuilderStandard : ISorterPhenotypeBuilder
    {
        public SorterPhenotypeBuilderStandard(Guid guid, IGenome genome, int keyCount)
        {
            _guid = guid;
            _genome = genome;
            _keyCount = keyCount;
        }

        public ISorterPhenotype Make(Guid guid)
        {
            var sorter = KeyPairRepository.KeyPairSet(KeyCount)
                .KeyPairs.ToSorter
                (
                    keyPairChoices: Genome.Sequence,
                    keyCount: KeyCount
                );

            return  new SorterPhenotypeStandard(
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


        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }


        public string EntityName
        {
            get { return "SorterPhenotypeBuilderStandard"; }
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