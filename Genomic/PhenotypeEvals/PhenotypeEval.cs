using System;
using Genomic.Phenotypes;
using MathUtils;

namespace Genomic.PhenotypeEvals
{
    public interface IPhenotypeEval<T> : IEntity, IComparable
        where T : IEntity
    {
        IPhenotype<T> Phenotype { get; }
        IPhenotypeEvalBuilder<T> PhenotypeEvalBuilder { get; }
    }


    public abstract class PhenotypeEvalDbl<T> : IPhenotypeEval<T>
        where T : IEntity
    {
        protected PhenotypeEvalDbl
        (
            Guid guid, 
            IPhenotype<T> phenotype, 
            double result,
            IPhenotypeEvalBuilder<T> phenotypeEvalBuilder
        )

        {
            _phenotype = phenotype;
            _result = result;
            _phenotypeEvalBuilder = phenotypeEvalBuilder;
            _guid = guid;
        }

        private readonly IPhenotype<T> _phenotype;
        public IPhenotype<T> Phenotype
        {
            get { return _phenotype; }
        }

        private readonly IPhenotypeEvalBuilder<T> _phenotypeEvalBuilder;
        public IPhenotypeEvalBuilder<T> PhenotypeEvalBuilder
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

        public abstract string EntityName { get; }

        public abstract IEntity GetPart(Guid key);
    }


    public abstract class PhenotypeEvalComparable<T, C> : IPhenotypeEval<T>
        where C : IComparable
        where T : IEntity
    {
        protected PhenotypeEvalComparable
        (
            Guid guid,
            IPhenotype<T> phenotype,
            C result,
            IPhenotypeEvalBuilder<T> phenotypeEvalBuilder
        )
        {
            _phenotype = phenotype;
            _result = result;
            _phenotypeEvalBuilder = phenotypeEvalBuilder;
            _guid = guid;
        }

        private readonly IPhenotype<T> _phenotype;
        public IPhenotype<T> Phenotype
        {
            get { return _phenotype; }
        }

        private readonly IPhenotypeEvalBuilder<T> _phenotypeEvalBuilder;
        public IPhenotypeEvalBuilder<T> PhenotypeEvalBuilder
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
