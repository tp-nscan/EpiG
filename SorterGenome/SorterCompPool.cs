﻿using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Genomes.Builders;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;
using Utils;

namespace SorterGenome
{
    public interface ISorterCompPool<T> : IRandomWalk<ISorterCompPool<T>>, IGuid, IGuidParts
    {
        int Generation { get; }

        SorterCompPoolStageType SorterCompPoolStageType { get; }

        IReadOnlyDictionary<Guid, IGenome> Genomes { get; }

        IReadOnlyDictionary<Guid, IPhenotype<T>> Phenotypes { get; }

        IReadOnlyDictionary<Guid, IPhenotypeEval<T>> PhenotypeEvals { get; }

        Func<IGenome, IRando, IPhenotype<T>> Phenotyper { get; }

        Func<IPhenotype<T>, IRando, IPhenotype<T>> PhenotypeEvaluator { get; }

        Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IGenome>> NextGenerator { get; }

    }

    public static class SorterCompPool
    {
        public static ISorterCompPool<ISorter> MakeStandard<TG, TP, TE>

            (
                int seed,
                int orgCount,
                int seqenceLength,
                int keyCount,
                double mutationRate,
                double multiplicationRate,
                double cubRate
            )
            where TG : IGenome
            where TP : IPhenotype<ISorter>
            where TE : IPhenotypeEval<ISorter>
        {
            return new SorterCompPoolImpl<ISorter>(
                    generation: 0,
                    genomes: GenomeBuilder.MakeSimpleGenomeBuilderRandoms
                            (
                               seed: seed,
                               builderCount: orgCount,
                               symbolCount: (uint) KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount),
                               sequenceLength: seqenceLength
                            ).Select(b=>b.Make()).ToDictionary(v=>v.GenomeBuilder.Guid),
                    phenotypes: null,
                    phenotypeEvals: null,
                    sorterCompPoolStageType: SorterCompPoolStageType.EvaluatePhenotypes,
                    phenotyper: Phenotypers.MakeStandard(keyCount),
                    phenotypeEvaluator: null,
                    nextGenerator: (evs, i) => null
                );
        }
    }

    public enum SorterCompPoolStageType
    {
        MakePhenotypes,
        EvaluatePhenotypes,
        MakeNextGeneration
    }


    public class SorterCompPoolImpl<T> : ISorterCompPool<T>
    {
        public SorterCompPoolImpl(
                int generation,
                IReadOnlyDictionary<Guid, IGenome> genomes,
                IReadOnlyDictionary<Guid, IPhenotype<T>> phenotypes,
                IReadOnlyDictionary<Guid, IPhenotypeEval<T>> phenotypeEvals,
                SorterCompPoolStageType sorterCompPoolStageType,
                Func<IGenome, IRando, IPhenotype<T>> phenotyper,
                Func<IPhenotype<T>, IRando, IPhenotype<T>> phenotypeEvaluator,
                Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IGenome>> nextGenerator
            )
        {
            _generation = generation;
            _genomes = genomes;
            _phenotypes = phenotypes;
            _phenotypeEvals = phenotypeEvals;
            _sorterCompPoolStageType = sorterCompPoolStageType;
            _phenotyper = phenotyper;
            _phenotypeEvaluator = phenotypeEvaluator;
            _nextGenerator = nextGenerator;
        }

        public ISorterCompPool<T> Step(int seed)
        {
            for (var i = 0; i < 80000000; i++)
            {
                seed = seed ^ i;
            }

            switch (SorterCompPoolStageType)
            {
                case SorterCompPoolStageType.MakePhenotypes:
                    return new SorterCompPoolImpl<T>
                        (
                            generation: Generation,
                            genomes: Genomes,
                            phenotypes: Phenotypes,
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.EvaluatePhenotypes,
                            phenotyper: Phenotyper,
                            phenotypeEvaluator: PhenotypeEvaluator,
                            nextGenerator: NextGenerator
                        );
                case SorterCompPoolStageType.EvaluatePhenotypes:
                    return new SorterCompPoolImpl<T>
                        (
                            generation: Generation,
                            genomes: Genomes,
                            phenotypes: Phenotypes,
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.MakeNextGeneration,
                            phenotyper: Phenotyper,
                            phenotypeEvaluator: PhenotypeEvaluator,
                            nextGenerator: NextGenerator
                        );
                case SorterCompPoolStageType.MakeNextGeneration:
                    return new SorterCompPoolImpl<T>
                        (
                            generation: Generation + 1,
                            genomes: NextGenerator(PhenotypeEvals, seed),
                            phenotypes: Phenotypes,
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                            phenotyper: Phenotyper,
                            phenotypeEvaluator: PhenotypeEvaluator,
                            nextGenerator: NextGenerator
                        );
                default:
                    throw new Exception(SorterCompPoolStageType + " not handled");
            }

            return null;

        }


        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }

        private readonly SorterCompPoolStageType _sorterCompPoolStageType;
        public SorterCompPoolStageType SorterCompPoolStageType
        {
            get { return _sorterCompPoolStageType; }
        }

        private readonly IReadOnlyDictionary<Guid, IGenome> _genomes;
        public IReadOnlyDictionary<Guid, IGenome> Genomes
        {
            get { return _genomes; }
        }

        private readonly IReadOnlyDictionary<Guid, IPhenotype<T>> _phenotypes;
        public IReadOnlyDictionary<Guid, IPhenotype<T>> Phenotypes
        {
            get { return _phenotypes; }
        }

        private readonly IReadOnlyDictionary<Guid, IPhenotypeEval<T>> _phenotypeEvals;
        public IReadOnlyDictionary<Guid, IPhenotypeEval<T>> PhenotypeEvals
        {
            get { return _phenotypeEvals; }
        }

        private readonly Func<IGenome, IRando, IPhenotype<T>> _phenotyper;
        public Func<IGenome, IRando, IPhenotype<T>> Phenotyper
        {
            get { return _phenotyper; }
        }

        private readonly Func<IPhenotype<T>, IRando, IPhenotype<T>> _phenotypeEvaluator;
        public Func<IPhenotype<T>, IRando, IPhenotype<T>> PhenotypeEvaluator
        {
            get { return _phenotypeEvaluator; }
        }

        private readonly Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IGenome>> _nextGenerator;
        public Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IGenome>> NextGenerator
        {
            get { return _nextGenerator; }
        }

        ISorterCompPool<T> IRandomWalk<ISorterCompPool<T>>.Step(int seed)
        {
            return Step(seed);
        }

        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public object GetPart(Guid key)
        {
            if (Genomes.ContainsKey(key))
            {
                return Genomes[key];
            }
            if (Phenotypes.ContainsKey(key))
            {
                return Phenotypes[key];
            }
            if (PhenotypeEvals.ContainsKey(key))
            {
                return PhenotypeEvals[key];
            }
            return null;
        }
    }
}







