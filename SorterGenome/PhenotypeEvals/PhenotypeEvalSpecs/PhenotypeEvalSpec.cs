using System;
using MathUtils.Rand;
using SorterGenome.Phenotypes;

namespace SorterGenome.PhenotypeEvals.PhenotypeEvalSpecs
{
    public enum PhenotypeEvalSpecType
    {
        Standard
    }

    public interface IPhenotypeEvalSpec
    {
        PhenotypeEvalSpecType PhenotypeEvalSpecType { get; }
    }

    public static class PhenotypeEvalSpec
    {
        public static Func<
            ISorterPhenotype,
            IRando,
            ISorterPhenotypeEval> ToPhenotypeEval
            (
                this IPhenotypeEvalSpec phenotypeEvalSpec
            )
        {
            switch (phenotypeEvalSpec.PhenotypeEvalSpecType)
            {
                case PhenotypeEvalSpecType.Standard:
                    return (p, r) => p.ToStandard(r.NextGuid(), r.NextGuid());
                default:
                    throw new Exception(String.Format("PhenotypeEvalSpecType: {0} not handled", phenotypeEvalSpec.PhenotypeEvalSpecType));
            }
        }
    }

    public class PhenotypeEvalSpecStandard : IPhenotypeEvalSpec
    {
        public PhenotypeEvalSpecType PhenotypeEvalSpecType
        {
            get { return PhenotypeEvalSpecType.Standard; }
        }
    }

}
