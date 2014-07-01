using System;
using MathUtils;

namespace Genomic.Phenotypes
{
    public interface IPhenotype : IEntity
    {
        IPhenotypeBuilder PhenotypeBuilder { get; }
    }

    public abstract class PhenotypeImpl<T> : IPhenotype
    {
        protected PhenotypeImpl
        (
            T value,
            IPhenotypeBuilder phenotypeBuilder
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

        private readonly IPhenotypeBuilder _phenotypeBuilder;
        public IPhenotypeBuilder PhenotypeBuilder
        {
            get { return _phenotypeBuilder; }
        }

        public Guid Guid
        {
            get { return PhenotypeBuilder.Guid; }
        }

        public abstract string EntityName { get; }

        public abstract IEntity GetPart(Guid key);
    }
}
