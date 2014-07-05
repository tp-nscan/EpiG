using System;
using MathUtils;
using SorterGenome.Phenotypes;
using Sorting.Evals;

namespace SorterGenome.PhenotypeEvals
{
    public interface ISorterPhenotypeEval : IEntity
    {
        ISorterEval SorterEval { get; }

        ISorterPhenotypeEvalBuilder SorterPhenotypeEvalBuilder { get; }
    }

    public static class SorterPhenotypeEval
    {
        public static ISorterPhenotypeEval ToStandard
            (
                this ISorterPhenotype sorterPhenotype,
                Guid builderGuid,
                Guid phenoTypeGuid
            )
        {
            var sorterPhenotypeEvalBuilderStandard 
                    = new SorterPhenotypeEvalBuilderStandard
                        (
                            guid: builderGuid,
                            sorterPhenotype: sorterPhenotype
                        );

            return sorterPhenotypeEvalBuilderStandard.Make(phenoTypeGuid);
        }

    }

    public class SorterPhenotypeEvalStandard : ISorterPhenotypeEval
    {
        public SorterPhenotypeEvalStandard(
                Guid guid,
                ISorterPhenotypeEvalBuilder sorterPhenotypeEvalBuilder,
                ISorterEval sorterEval
            )
        {
            _guid = guid;
            _sorterPhenotypeEvalBuilder = sorterPhenotypeEvalBuilder;
            _sorterEval = sorterEval;
        }

        public string EntityName
        {
            get { return "SorterPhenotypeEvalStandard"; }
        }

        private readonly ISorterEval _sorterEval;
        public ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        private readonly ISorterPhenotypeEvalBuilder _sorterPhenotypeEvalBuilder;
        public ISorterPhenotypeEvalBuilder SorterPhenotypeEvalBuilder
        {
            get { return _sorterPhenotypeEvalBuilder; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IEntity GetPart(Guid key)
        {
            if (SorterPhenotypeEvalBuilder.GetPart(key) != null)
            {
                return SorterPhenotypeEvalBuilder.GetPart(key);
            }

            if (Guid == key)
            {
                return this;
            }

            return null;
        }
    }
}
