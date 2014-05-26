using System;

namespace Genomic.Layers
{
    public interface IPhenotypeEval : IComparable
    {
        IPhenotype Phenotype { get; }
    }

    public static class PhenotypeEval
    {
        public static IPhenotypeEval Make(IPhenotype phenotype, double result)
        {
            return new PhenotypeEvalDbl(phenotype, result);
        }
    }

    public class PhenotypeEvalDbl : IPhenotypeEval
    {
        public PhenotypeEvalDbl(IPhenotype phenotype, double result)
        {
            _phenotype = phenotype;
            _result = result;
        }

        private readonly IPhenotype _phenotype;
        public IPhenotype Phenotype
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
            var c1 = (PhenotypeEvalDbl)obj;

            if (c1.Result > Result)
                return -1;
            if (Result < c1.Result)
                return 1;
            else
                return 0;
        }
    }
}
