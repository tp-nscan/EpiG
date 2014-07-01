using System;
using Genomic.Phenotypes;
using MathUtils;

namespace Genomic.PhenotypeEvals
{
    public interface IPhenotypeEvalBuilder : IEntity 
    {
        IPhenotypeEval Make();
        IPhenotype Phenotype { get; }
    }

    public abstract class PhenotypeEvalBuilder<T, E> : IPhenotypeEvalBuilder
        where T : IPhenotype
        where E : IPhenotypeEval
    {
        protected PhenotypeEvalBuilder
            (
                Guid guid, 
                T phenotype
         )
        {
            _guid = guid;
            _phenotype = phenotype;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IPhenotypeEval Make()
        {
            return EvaluatorFunc(_phenotype);
        }

        private readonly T _phenotype;
        public IPhenotype Phenotype
        {
            get { return _phenotype; }
        }

        protected abstract Func<T, E> EvaluatorFunc { get; } 

        public abstract string EntityName { get; }

        public abstract IEntity GetPart(Guid key);
    }


}
