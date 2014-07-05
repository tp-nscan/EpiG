using System;
using MathUtils;
using SorterGenome.Phenotypes;
using Sorting.Evals;

namespace SorterGenome.PhenotypeEvals
{
    public interface ISorterPhenotypeEvalBuilder : IEntity
    {
        ISorterPhenotypeEval Make(Guid guid);
        ISorterPhenotype SorterPhenotype { get; }
    }

    public static class SorterPhenotypeEvalBuilder
    {
    }

    public class SorterPhenotypeEvalBuilderStandard : ISorterPhenotypeEvalBuilder
    {
        public SorterPhenotypeEvalBuilderStandard(Guid guid, ISorterPhenotype sorterPhenotype)
        {
            _guid = guid;
            _sorterPhenotype = sorterPhenotype;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public string EntityName
        {
            get { return "SorterPhenotypeEvalBuilderStandard"; }
        }

        public IEntity GetPart(Guid key)
        {
            if (SorterPhenotype.GetPart(key) != null)
            {
                return SorterPhenotype.GetPart(key);
            }

            if (Guid == key)
            {
                return this;
            }

            return null;
        }

        public ISorterPhenotypeEval Make(Guid guid)
        {
            return new SorterPhenotypeEvalStandard
                (
                    guid: guid,
                    sorterPhenotypeEvalBuilder: this,
                    sorterEval: SorterPhenotype.Sorter.ToSorterResult().ToSorterEval()
                );
        }

        private readonly ISorterPhenotype _sorterPhenotype;
        public ISorterPhenotype SorterPhenotype
        {
            get { return _sorterPhenotype; }
        }
    }
}
