using System;
using MathUtils;

namespace Genomic.Phenotypes
{
    public interface IPhenotype<T> : IEntity 
        where T : IEntity
    {
        T Value { get; }
        IPhenotypeBuilder<T> PhenotypeBuilder { get; }
    }

    public abstract class PhenotypeImpl<T> : IPhenotype<T>
        where T : IEntity
    {
        protected PhenotypeImpl
        (
            T value,
            IPhenotypeBuilder<T> phenotypeBuilder
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

        private readonly IPhenotypeBuilder<T> _phenotypeBuilder;
        public IPhenotypeBuilder<T> PhenotypeBuilder
        {
            get { return _phenotypeBuilder; }
        }

        public Guid Guid
        {
            get { return PhenotypeBuilder.Guid; }
        }

        public abstract string EntityName { get; }

        public IEntity GetPart(Guid key)
        {
            return (PhenotypeBuilder.Guid == key) ? PhenotypeBuilder : null;
        }
    }
}
