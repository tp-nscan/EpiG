using System;
using MathUtils.Rand;
using SorterGenome.Phenotypes;

namespace SorterGenome.PhenotypeEvals
{
    public static class PhenotypeEvaluators
    {

        public static Func<
            ISorterPhenotype,
            IRando,
            ISorterPhenotypeEval> LookupPhenotypeEvaluator
        (
            string name
        )
        {
            switch (name)
            {
                case "Standard":
                    return MakeStandard();
                case "MakePermuterSlider":
                    return MakeStandard();
                default:
                    throw new Exception(String.Format("LookupPhenotyper: {0} not found", name));
            }
        }



        public static Func<
            ISorterPhenotype, 
            IRando,
            ISorterPhenotypeEval> MakeStandard()
        {

            return (p, r) => p.ToStandard(r.NextGuid(), r.NextGuid());
        }

    }
}
