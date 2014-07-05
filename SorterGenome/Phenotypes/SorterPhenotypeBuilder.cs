using System;
using Genomic.Genomes;
using MathUtils;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterGenome.Phenotypes
{
    public interface ISorterPhenotypeBuilder : IEntity
    {
        ISorterPhenotype Make(Guid guid);
        IGenome Genome { get; }
        int KeyCount { get; }
    }
}
