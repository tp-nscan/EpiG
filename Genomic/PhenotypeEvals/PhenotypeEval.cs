using System;
using Genomic.Phenotypes;
using MathUtils.Collections;

namespace Genomic.PhenotypeEvals
{
    public interface IPhenotypeEval<T> : IGuid, IComparable
    {
        IPhenotype<T> Phenotype { get; }
    }

    public static class PhenotypeEval
    {
        public static IPhenotypeEval<T> Make<T>(Guid guid, IPhenotype<T> phenotype, double result)
        {
            return new PhenotypeEvalDbl<T>(guid, phenotype, result);
        }
    }

    public class PhenotypeEvalDbl<T> : IPhenotypeEval<T> 
    {
        public PhenotypeEvalDbl(Guid guid, IPhenotype<T> phenotype, double result)
        {
            _phenotype = phenotype;
            _result = result;
            _guid = guid;
        }

        private readonly IPhenotype<T> _phenotype;
        public IPhenotype<T> Phenotype
        {
            get { return _phenotype; }
        }

        private readonly double _result;
        public double Result
        {
            get { return _result; }
        }

        public int CompareTo(object obj)
        {
            var c1 = (PhenotypeEvalDbl<T>)obj;

            if (c1.Result > Result)
            {
                return -1;
            }
            if (Result < c1.Result)
            {
                return 1;
            }
                
            return 0;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
