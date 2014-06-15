using MathUtils.Collections;

namespace Genomic.Genomes.Builders
{
    public interface IGenomeBuilder : IGuid
    {
        IGenome Make();
        string GenomeBuilderType { get; }
    }

    public static class GenomeBuilder
    {

    }


    public class GenomeBuilderRandom
    {

    }
}
