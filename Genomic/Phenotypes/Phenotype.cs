using System;
using Genomic.Layers;
using MathUtils.Collections;

namespace Genomic.Phenotypes
{
    public interface IPhenotype : IGuid
    {
        IOrg Org { get; }
    }

    public interface IPhenotype<T> : IPhenotype
    {
        T Value { get; }
        PhenotypeBuilder<IPhenotype<T>, T> PhenotypeBuilder { get; }
    }

    public static class Phenotype
    {
        public static IPhenotype<T> Make<T>        (
                IOrg org, 
                Guid guid, 
                T value, 
                PhenotypeBuilder<IPhenotype<T>, T> phenotypeBuilder
            )        {
                return new PhenotypeImpl<T>(org, guid, value, phenotypeBuilder);
        }
    }

    public class PhenotypeImpl<T> : IPhenotype<T>
    {
        public PhenotypeImpl
            (
                IOrg org, 
                Guid guid, 
                T value, 
                PhenotypeBuilder<IPhenotype<T>, T> phenotypeBuilder
            )
        {
            _org = org;
            _value = value;
            _phenotypeBuilder = phenotypeBuilder;
            _guid = guid;
        }

        private readonly IOrg _org;
        public IOrg Org
        {
            get { return _org; }
        }

        private readonly T _value;
        public T Value
        {
            get { return _value; }
        }

        private readonly PhenotypeBuilder<IPhenotype<T>, T> _phenotypeBuilder;
        public PhenotypeBuilder<IPhenotype<T>, T> PhenotypeBuilder
        {
            get { return _phenotypeBuilder; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
