using System;
using Genomic.Phenotypes;
using MathUtils.Collections;

namespace Genomic.PhenotypeEvals
{
    public interface IPhenotypeEvalBuilder<T> : IGuid
    {
        IPhenotypeEval<T> Make();
        IPhenotype<T> Phenotype { get; }
        string PhenotypeEvalBuilderType { get; }
    }

    public class PhenotypeEvalBuilderStandard<T> : PhenotypeEvalBuilder<T>
    {
        public PhenotypeEvalBuilderStandard
            (
                Guid guid, 
                IPhenotype<T> phenotype, 
                Func<T, double> evaluatorFunc
            )
            : base(guid, phenotype, "PhenotypeEvalBuilderStandard")
        {
            _evaluatorFunc = evaluatorFunc;
        }

        private readonly Func<T, double> _evaluatorFunc;
        public Func<T, double> EvaluatorFunc
        {
            get { return _evaluatorFunc; }
        }

        public override IPhenotypeEval<T> Make()
        {
            return PhenotypeEval.Make(
                    guid: Guid,
                    phenotype:Phenotype,
                    result: _evaluatorFunc(Phenotype.Value),
                    phenotypeEvalBuilder: this
                );
        }
    }

    public abstract class PhenotypeEvalBuilder<T> : IPhenotypeEvalBuilder<T>
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

        public abstract  IPhenotypeEval<T> Make();

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
