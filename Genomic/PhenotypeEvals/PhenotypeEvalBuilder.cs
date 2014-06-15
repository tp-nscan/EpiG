using System;
using System.Threading.Tasks;
using Genomic.Phenotypes;
using MathUtils.Collections;

namespace Genomic.PhenotypeEvals
{

    public interface IPhenotypeEvalBuilder<TP, T> : IGuid 
        where TP : IPhenotypeEval<T>, IGuid
    {
        Task<TP> Make();
        IPhenotype<T> Phenotype { get; }
        string PhenotypeEvalBuilderType { get; }
    }

    public abstract class PhenotypeEvalBuilder<TP, T> 
        : IPhenotypeEvalBuilder<TP, T> where TP : IPhenotypeEval<T>, IGuid
    {
        protected PhenotypeEvalBuilder(
            Guid guid, 
            IPhenotype<T> phenotype, 
            string phenotypeEvalBuilderType
         )
        {
            _guid = guid;
            _phenotype = phenotype;
            _phenotypeEvalBuilderType = phenotypeEvalBuilderType;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract Task<TP> Make();

        private readonly IPhenotype<T> _phenotype;
        public IPhenotype<T> Phenotype
        {
            get { return _phenotype; }
        }

        private readonly string _phenotypeEvalBuilderType;
        public string PhenotypeEvalBuilderType
        {
            get { return _phenotypeEvalBuilderType; }
        }
    }


}
