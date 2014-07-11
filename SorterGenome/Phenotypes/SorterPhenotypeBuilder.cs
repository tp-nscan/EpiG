using System;
using Genomic.Genomes;
using MathUtils;

namespace SorterGenome.Phenotypes
{
    public interface ISorterPhenotypeBuilder : IEntity
    {
        ISorterPhenotype Make(Guid guid);
        IGenome Genome { get; }
        int KeyCount { get; }
    }
}
