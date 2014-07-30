using System;
using System.Collections.Generic;
using Genomic.Genomes;
using MathUtils.Rand;

namespace SorterGenome.Phenotypes.PhenotyperSpecs
{
    public interface IPhenotyperSpec
    {
        PhenotyperSpecType PhenotyperSpecType { get; }
    }

    public enum PhenotyperSpecType
    {
        Standard,
        MakePermuterSlider,
        MakePermuterCubeCombo,
        MakePermuterComposer
    }

    public static class PhenotyperSpec
    {
        public static Func<
            IGenome,
            IRando,
            IEnumerable<ISorterPhenotype>> ToPenotyper
            (
                this IPhenotyperSpec phenotyperSpec,
                int keyCount
            )
        {
            switch (phenotyperSpec.PhenotyperSpecType)
            {
                case PhenotyperSpecType.Standard:
                    return Phenotypers.MakeStandard(keyCount);
                case PhenotyperSpecType.MakePermuterSlider:
                    return Phenotypers.MakePermuterSlider(keyCount);
                case PhenotyperSpecType.MakePermuterCubeCombo:
                    return Phenotypers.MakePermuterCubeCombo(keyCount);
                case PhenotyperSpecType.MakePermuterComposer:
                    return Phenotypers.MakePermuterComposer(keyCount);
                default:
                    throw new Exception(String.Format("PhenotyperSpecType: {0} not handled", phenotyperSpec.PhenotyperSpecType));
            }
        }
    }

    public class PhenotyperSpecStandard : IPhenotyperSpec
    {
        public PhenotyperSpecType PhenotyperSpecType
        {
            get { return PhenotyperSpecType.Standard; }
        }
    }

    public class PhenotyperSpecPermuterSlider : IPhenotyperSpec
    {
        public PhenotyperSpecType PhenotyperSpecType
        {
            get { return PhenotyperSpecType.MakePermuterSlider; }
        }
    }

    public class PhenotyperSpecPermuterCubeCombo : IPhenotyperSpec
    {
        public PhenotyperSpecType PhenotyperSpecType
        {
            get { return PhenotyperSpecType.MakePermuterCubeCombo; }
        }
    }

    public class PhenotyperSpecPermuterComposer : IPhenotyperSpec
    {
        public PhenotyperSpecType PhenotyperSpecType
        {
            get { return PhenotyperSpecType.MakePermuterComposer; }
        }

    }
}
