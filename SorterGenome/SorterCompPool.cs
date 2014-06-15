using System;
using System.Collections.Generic;
using Genomic.Layers;
using Genomic.PhenotypeEvals;
using Genomic.Phenotypes;
using MathUtils.Collections;
using MathUtils.Rand;
using Utils;

namespace SorterGenome
{
    public interface ISorterCompPool<T> : IRandomWalk<ISorterCompPool<T>>, IGuid, IGuidParts
    {
        int Generation { get; }

        SorterCompPoolStageType SorterCompPoolStageType { get; }

        IReadOnlyDictionary<Guid, IOrg> Orgs { get; }

        IReadOnlyDictionary<Guid, IPhenotype<T>> Phenotypes { get; }

        IReadOnlyDictionary<Guid, IPhenotypeEval<T>> PhenotypeEvals { get; }

        Func<IOrg, IRando, IPhenotype<T>> Phenotyper { get; }

        Func<IPhenotype<T>, IRando, IPhenotype<T>> PhenotypeEvaluator { get; }

        Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IOrg>> NextGenerator { get; }

    }

    public static class SorterCompPool
    {
        public static ISorterCompPool<T> MakeStandard<T, TO, TP, TE>

            (
                int seed,
                int orgCount,
                int seqenceLength,
                int keyCount,
                double mutationRate,
                double multiplicationRate,
                double cubRate
            )
            where TO : IOrg
            where TP : IPhenotype<T>
            where TE : IPhenotypeEval<T>
        {
            return new SorterCompPoolImpl<T>(
                    generation: 0,
                    orgs: null,
                    phenotypes: null,
                    phenotypeEvals: null,
                    sorterCompPoolStageType: SorterCompPoolStageType.EvaluatePhenotypes,
                    phenotyper: null,
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
                IReadOnlyDictionary<Guid, IOrg> orgs,
                IReadOnlyDictionary<Guid, IPhenotype<T>> phenotypes,
                IReadOnlyDictionary<Guid, IPhenotypeEval<T>> phenotypeEvals,
                SorterCompPoolStageType sorterCompPoolStageType,
                Func<IOrg, IRando, IPhenotype<T>> phenotyper,
                Func<IPhenotype<T>, IRando, IPhenotype<T>> phenotypeEvaluator,
                Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IOrg>> nextGenerator
            )
        {
            _generation = generation;
            _orgs = orgs;
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
                            orgs: Orgs,
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
                            orgs: Orgs,
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
                            orgs: NextGenerator(PhenotypeEvals, seed),
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

        private readonly IReadOnlyDictionary<Guid, IOrg> _orgs;
        public IReadOnlyDictionary<Guid, IOrg> Orgs
        {
            get { return _orgs; }
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

        private readonly Func<IOrg, IRando, IPhenotype<T>> _phenotyper;
        public Func<IOrg, IRando, IPhenotype<T>> Phenotyper
        {
            get { return _phenotyper; }
        }

        private readonly Func<IPhenotype<T>, IRando, IPhenotype<T>> _phenotypeEvaluator;
        public Func<IPhenotype<T>, IRando, IPhenotype<T>> PhenotypeEvaluator
        {
            get { return _phenotypeEvaluator; }
        }

        private readonly Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IOrg>> _nextGenerator;
        public Func<IReadOnlyDictionary<Guid, IPhenotypeEval<T>>, int, IReadOnlyDictionary<Guid, IOrg>> NextGenerator
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
            if (Orgs.ContainsKey(key))
            {
                return Orgs[key];
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







