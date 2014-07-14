using System;
using Genomic.Genomes;
using MathUtils;
using MathUtils.Collections;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome.Phenotypes
{
    public class SorterPhenotypeBuilderPermuterCubeCombo : ISorterPhenotypeBuilder
    {
        public SorterPhenotypeBuilderPermuterCubeCombo
            (
                Guid guid, 
                IGenome genome, 
                int keyCount, 
                bool positionA, 
                bool positionB, 
                bool positionC
            )
        {
            _guid = guid;
            _genome = genome;
            _keyCount = keyCount;
            _positionA = positionA;
            _positionB = positionB;
            _positionC = positionC;
        }

        public ISorterPhenotype Make(Guid guid)
        {
            var sorter =
                Genome.Sequence
                    .CubeCorner
                    (
                        blockSize: KeyCount,
                        marginA: 4,
                        marginB: 0,
                        marginC: 0,
                        positionA: PositionA,
                        positionB: PositionB,
                        positionC: PositionC
                      )
                      .ToKeyPairs().ToSorter(KeyCount);

   
            return new SorterPhenotypeStandard
                (
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

        private readonly bool _positionA;
        public bool PositionA
        {
            get { return _positionA; }
        }

        private readonly bool _positionB;
        public bool PositionB
        {
            get { return _positionB; }
        }

        private readonly bool _positionC;
        public bool PositionC
        {
            get { return _positionC; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public string EntityName
        {
            get { return "SorterPhenotypeBuilderPermuterCubeCombo"; }
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
