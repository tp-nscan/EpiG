using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genomic.Layers
{
    public interface IPhenotype
    {
        IOrg Org { get; }
    }

    public interface IPhenotype<T>
    {
        IOrg Org { get; }
        T Value { get; }
    }

    public static class Phenotype
    {
        public static IPhenotype<T> Make<T>(IOrg org, T value)
        {
            return new PhenotypeImpl<T>(org, value);
        }
    }

    public class PhenotypeImpl<T> : IPhenotype<T>
    {
        public PhenotypeImpl(IOrg org, T value)
        {
            _org = org;
            _value = value;
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
    }
}
