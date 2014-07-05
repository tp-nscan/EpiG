using System;
using Genomic.Genomes;
using MathUtils;
using Sorting.Sorters;

namespace SorterGenome.Phenotypes
{
    public interface ISorterPhenotype : IEntity
    {
        ISorterPhenotypeBuilder SorterPhenotypeBuilder { get; }
        ISorter Sorter { get; }
    }

    public static class SorterPhenotype
    {
        public static ISorterPhenotype ToSorterPhenotypeStandard
            (
                this IGenome genome, 
                Guid builderGuid,
                Guid phenotypeGuid,
                int keyCount
            )
        {
            var sorterPhenotypeBuilderStandard = new SorterPhenotypeBuilderStandard
                (
                    guid: builderGuid,
                    genome: genome,
                    keyCount: keyCount
                );

            return sorterPhenotypeBuilderStandard.Make(phenotypeGuid);
        }

        public static ISorterPhenotype ToSorterPhenotypePermuter
        (
            this IGenome genome,
            Guid builderGuid,
            Guid phenotypeGuid,
            int keyCount,
            int skips
        )
        {
            var sorterPhenotypeBuilderStandard = new SorterPhenotypeBuilderPermuter
                (
                    guid: builderGuid,
                    genome: genome,
                    keyCount: keyCount,
                    skips: skips
                );

            return sorterPhenotypeBuilderStandard.Make(phenotypeGuid);
        }


    }

    public class SorterPhenotypeStandard : ISorterPhenotype
    {
        public SorterPhenotypeStandard
            (
                Guid guid, 
                ISorter sorter,
                ISorterPhenotypeBuilder sorterPhenotypeBuilder
            )
        {
            _guid = guid;
            _sorter = sorter;
            _sorterPhenotypeBuilder = sorterPhenotypeBuilder;
        }

        public string EntityName
        {
            get { return "SorterPhenotypeStandard"; }
        }

        public IEntity GetPart(Guid key)
        {
            if (SorterPhenotypeBuilder.GetPart(key) != null)
            {
                return SorterPhenotypeBuilder.GetPart(key);
            }

            if (Guid == key)
            {
                return this;
            }

            return null;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly ISorterPhenotypeBuilder _sorterPhenotypeBuilder;
        public ISorterPhenotypeBuilder SorterPhenotypeBuilder
        {
            get { return _sorterPhenotypeBuilder; }
        }

        private readonly ISorter _sorter;
        public ISorter Sorter
        {
            get { return _sorter; }
        }
    }
}
