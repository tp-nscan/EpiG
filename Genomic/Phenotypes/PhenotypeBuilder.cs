using MathUtils.Collections;

namespace Genomic.Phenotypes
{
    public class PhenotypeBuilder<TP, T> where TP : IPhenotype<T>, IGuid
    {
        public TP Make { get; set; }
    }
}
