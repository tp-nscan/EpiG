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

    public abstract class PhenotypeEvalBuilder<P, E> : IPhenotypeEvalBuilder
        where P : IPhenotype
        where E : IPhenotypeEval
    {
        protected PhenotypeEvalBuilder
            (
                Guid guid, 
                P phenotype
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

        private readonly P _phenotype;
        public IPhenotype Phenotype
        {
            get { return _phenotype; }
        }

        protected abstract Func<P, E> EvaluatorFunc { get; } 

        public abstract string EntityName { get; }

        public abstract IEntity GetPart(Guid key);
    }


}
