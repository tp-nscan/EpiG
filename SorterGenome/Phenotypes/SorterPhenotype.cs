﻿using System;
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
            int skipStart,
            int skipBlocks
        )
        {
            var sorterPhenotypeBuilderStandard = new SorterPhenotypeBuilderPermuterSkip
                (
                    guid: builderGuid,
                    genome: genome,
                    keyCount: keyCount,
                    skipStart: skipStart,
                    skipBlocks: skipBlocks
                );

            return sorterPhenotypeBuilderStandard.Make(phenotypeGuid);
        }


        public static ISorterPhenotype ToSorterPhenotypeComposer
        (
            this IGenome genome,
            Guid builderGuid,
            Guid phenotypeGuid,
            int keyCount,
            int skips
        )
        {
            var phenotypeBuilderComposer = new SorterPhenotypeBuilderComposer
                (
                    guid: builderGuid,
                    genome: genome,
                    keyCount: keyCount,
                    skips: skips
                );

            return phenotypeBuilderComposer.Make(phenotypeGuid);
        }

        public static ISorterPhenotype ToSorterPhenotypePermuterCubeCombo
        (
            this IGenome genome,
            Guid builderGuid,
            Guid phenotypeGuid,
            int keyCount,
            bool marginA,
            bool marginB,
            bool marginC
        )
        {
            var sorterPhenotypeBuilderPermuterCubeCombo = new SorterPhenotypeBuilderPermuterCubeCombo
                (
                    guid: builderGuid,
                    genome: genome,
                    keyCount: keyCount,
                    positionA: marginA,
                    positionB: marginB,
                    positionC: marginC
                );

            return sorterPhenotypeBuilderPermuterCubeCombo.Make(phenotypeGuid);
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
            if (Guid == key)
            {
                return this;
            }

            if (SorterPhenotypeBuilder.GetPart(key) != null)
            {
                return SorterPhenotypeBuilder.GetPart(key);
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
