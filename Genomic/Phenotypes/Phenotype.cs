using System;
using MathUtils.Collections;

namespace Genomic.Phenotypes
{
    public interface IPhenotype<T> : IGuid
    {
        T Value { get; }
        IPhenotypeBuilder<IPhenotype<T>, T> PhenotypeBuilder { get; }
    }

    public static class Phenotype
    {
        public static IPhenotype<T> Make<T>        
            (
                Guid guid, 
                T value, 
                IPhenotypeBuilder<IPhenotype<T>, T> phenotypeBuilder
            )        
        {
            return new PhenotypeImpl<T>(value, phenotypeBuilder);
        }
    }

    public class PhenotypeImpl<T> : IPhenotype<T>
    {
        public PhenotypeImpl
        (
            T value, 
            IPhenotypeBuilder<IPhenotype<T>, T> phenotypeBuilder
        )
        {
            _value = value;
            _phenotypeBuilder = phenotypeBuilder;
        }

        private readonly T _value;
        public T Value
        {
            get { return _value; }
        }

        private readonly IPhenotypeBuilder<IPhenotype<T>, T> _phenotypeBuilder;
        public IPhenotypeBuilder<IPhenotype<T>, T> PhenotypeBuilder
        {
            get { return _phenotypeBuilder; }
        }

        public Guid Guid
        {
            get { return PhenotypeBuilder.Guid; }
        }

    }
}
