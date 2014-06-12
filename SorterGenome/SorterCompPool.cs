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
    public interface ISorterCompPool : IRandomWalk<ISorterCompPool>, IGuid, IGuidParts
    {
        int Generation { get; }

        SorterCompPoolStageType SorterCompPoolStageType { get; }

        IReadOnlyDictionary<Guid, IOrg> Orgs { get; }

        IReadOnlyDictionary<Guid, IPhenotype> Phenotypes { get; }

        IReadOnlyDictionary<Guid, IPhenotypeEval> PhenotypeEvals { get; }

        Func<IOrg, IRando, IPhenotype> Phenotyper { get; }

        Func<IOrg, IRando, IPhenotype> PhenotypeEvaluator { get; }

        Func<IReadOnlyDictionary<Guid, IPhenotypeEval>, int, IReadOnlyDictionary<Guid, IOrg>> NextGenerator { get; }

    }

    public static class SorterCompPool
    {
        public static ISorterCompPool MakeStandard<TO, TP, TE>

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
            where TP : IPhenotype
            where TE : IPhenotypeEval
        {
            return new SorterCompPoolImpl(
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


    public class SorterCompPoolImpl : ISorterCompPool
    {
        public SorterCompPoolImpl(
                int generation,
                IReadOnlyDictionary<Guid, IOrg> orgs,
                IReadOnlyDictionary<Guid, IPhenotype> phenotypes,
                IReadOnlyDictionary<Guid, IPhenotypeEval> phenotypeEvals,
                SorterCompPoolStageType sorterCompPoolStageType,
                Func<IOrg, IRando, IPhenotype> phenotyper,
                Func<IOrg, IRando, IPhenotype> phenotypeEvaluator,
                Func<IReadOnlyDictionary<Guid, IPhenotypeEval>, int, IReadOnlyDictionary<Guid, IOrg>> nextGenerator
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

        public ISorterCompPool Step(int seed)
        {
            for (var i = 0; i < 80000000; i++)
            {
                seed = seed ^ i;
            }

            switch (SorterCompPoolStageType)
            {
                case SorterCompPoolStageType.MakePhenotypes:
                    return new SorterCompPoolImpl
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
                    return new SorterCompPoolImpl
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
                    return new SorterCompPoolImpl
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

        private readonly IReadOnlyDictionary<Guid, IPhenotype> _phenotypes;
        public IReadOnlyDictionary<Guid, IPhenotype> Phenotypes
        {
            get { return _phenotypes; }
        }

        private readonly IReadOnlyDictionary<Guid, IPhenotypeEval> _phenotypeEvals;
        public IReadOnlyDictionary<Guid, IPhenotypeEval> PhenotypeEvals
        {
            get { return _phenotypeEvals; }
        }

        private readonly Func<IOrg, IRando, IPhenotype> _phenotyper;
        public Func<IOrg, IRando, IPhenotype> Phenotyper
        {
            get { return _phenotyper; }
        }

        private readonly Func<IOrg, IRando, IPhenotype> _phenotypeEvaluator;
        public Func<IOrg, IRando, IPhenotype> PhenotypeEvaluator
        {
            get { return _phenotypeEvaluator; }
        }

        private readonly Func<IReadOnlyDictionary<Guid, IPhenotypeEval>, int, IReadOnlyDictionary<Guid, IOrg>> _nextGenerator;
        private Guid _guid;

        public Func<IReadOnlyDictionary<Guid, IPhenotypeEval>, int, IReadOnlyDictionary<Guid, IOrg>> NextGenerator
        {
            get { return _nextGenerator; }
        }

        ISorterCompPool IRandomWalk<ISorterCompPool>.Step(int seed)
        {
            return Step(seed);
        }

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







