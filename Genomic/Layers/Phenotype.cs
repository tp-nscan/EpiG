using System;
using MathUtils.Collections;

namespace Genomic.Layers
{
    public interface IPhenotype : IGuid
    {
        IOrg Org { get; }
    }

    public interface IPhenotype<T> : IPhenotype
    {
        T Value { get; }
    }

    public static class Phenotype
    {
        public static IPhenotype<T> Make<T>(IOrg org, Guid guid, T value)
        {
            return new PhenotypeImpl<T>(org, guid, value);
        }
    }

    public class PhenotypeImpl<T> : IPhenotype<T>
    {
        public PhenotypeImpl(IOrg org, Guid guid, T value)
        {
            _org = org;
            _value = value;
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

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
