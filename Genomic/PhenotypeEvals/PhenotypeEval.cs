using System;
using Genomic.Phenotypes;
using MathUtils;

namespace Genomic.PhenotypeEvals
{
    public interface IPhenotypeEval : IEntity
    {
        IPhenotype Phenotype { get; }
        IPhenotypeEvalBuilder PhenotypeEvalBuilder { get; }
    }

    public abstract class PhenotypeEvalDbl : IPhenotypeEval
    {
        protected PhenotypeEvalDbl
        (
            Guid guid, 
            IPhenotype phenotype, 
            double result,
            IPhenotypeEvalBuilder phenotypeEvalBuilder
        )

        {
            _phenotype = phenotype;
            _result = result;
            _phenotypeEvalBuilder = phenotypeEvalBuilder;
            _guid = guid;
        }

        private readonly IPhenotype _phenotype;
        public IPhenotype Phenotype
        {
            get { return _phenotype; }
        }

        private readonly IPhenotypeEvalBuilder _phenotypeEvalBuilder;
        public IPhenotypeEvalBuilder PhenotypeEvalBuilder
        {
            get { return _phenotypeEvalBuilder; }
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

        public abstract string EntityName { get; }

        public abstract IEntity GetPart(Guid key);
    }


    public abstract class PhenotypeEvalComparable<T, C> : IPhenotypeEval
        where C : IComparable
        where T : IEntity
    {
        protected PhenotypeEvalComparable
        (
            Guid guid,
            IPhenotype phenotype,
            C result,
            IPhenotypeEvalBuilder phenotypeEvalBuilder
        )
        {
            _phenotype = phenotype;
            _result = result;
            _phenotypeEvalBuilder = phenotypeEvalBuilder;
            _guid = guid;
        }

        private readonly IPhenotype _phenotype;
        public IPhenotype Phenotype
        {
            get { return _phenotype; }
        }

        private readonly IPhenotypeEvalBuilder _phenotypeEvalBuilder;
        public IPhenotypeEvalBuilder PhenotypeEvalBuilder
        {
            get { return _phenotypeEvalBuilder; }
        }

        private readonly C _result;
        public C Result
        {
            get { return _result; }
        }

        public int CompareTo(object obj)
        {
            var c1 = (PhenotypeEvalComparable<T, C>)obj;
            return Result.CompareTo(c1.Result);
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract string EntityName { get; }

        public abstract IEntity GetPart(Guid key);
    }
}
