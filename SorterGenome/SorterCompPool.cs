using System;
using System.Collections.Generic;
using Genomic.Layers;
using MathUtils.Collections;
using MathUtils.Rand;
using Utils;

namespace SorterGenome
{
    public interface ISorterCompPool : IRandomWalk<ISorterCompPool>, IGuid, IGuidParts
    {
        int Generation { get; }
        SorterCompPoolStageType SorterCompPoolStageType { get; }   
    }

    public interface ISorterCompPool<TO,TP,TE>
        : IRandomWalk<ISorterCompPool<TO, TP, TE>>, IGuid, IGuidParts //, ISorterCompPool
        where TO : IOrg
        where TP : IPhenotype
        where TE : IPhenotypeEval
    {
        IReadOnlyDictionary<Guid, TO> Orgs { get; }

        IReadOnlyDictionary<Guid, TP> Phenotypes { get; }

        IReadOnlyDictionary<Guid, TE> PhenotypeEvals { get; }

        Func<TO, IRando, TP> Phenotyper { get; }

        Func<TO, IRando, TP> PhenotypeEvaluator { get; }

        Func<IReadOnlyDictionary<Guid, TE>, int, IReadOnlyDictionary<Guid, TO>> NextGenerator { get; }


        int Generation { get; }
        SorterCompPoolStageType SorterCompPoolStageType { get; }  

    }

    public static class SorterCompPool
    {

        public static ISorterCompPool<IOrg, IPhenotype, IPhenotypeEval> MakeStandard
            (
                int seed,
                int orgCount,
                int seqenceLength,
                int keyCount,
                double mutationRate,
                double multiplicationRate,
                double cubRate,
                Guid guid
            )
        {
            return new SorterCompPoolImpl<IOrg, IPhenotype, IPhenotypeEval>(
                    generation: 0, 
                    orgs: null,
                    phenotypes: null,
                    phenotypeEvals: null,
                    sorterCompPoolStageType: SorterCompPoolStageType.EvaluatePhenotypes,
                    phenotyper: null,
                    phenotypeEvaluator: null,
                    nextGenerator: (evs, i) => null,
                    guid: guid
                );
        }
    }

    //public static class SorterCompPool<TO,TP,TE> 
    //    where TO : IOrg
    //    where TP : IPhenotype 
    //    where TE : IPhenotypeEval
    //{
    //    public static ISorterCompPool<TO,TP,TE> Make()
    //    {
    //        return null;// new SorterCompPoolImpl();
    //    }
    //}

    public enum SorterCompPoolStageType
    {
        MakePhenotypes,
        EvaluatePhenotypes,
        MakeNextGeneration
    }


    public class SorterCompPoolImpl<TO, TP, TE> : ISorterCompPool<TO, TP, TE>
        where TO : IOrg
        where TP : IPhenotype
        where TE : IPhenotypeEval
    {
        public SorterCompPoolImpl(
                int generation, 
                IReadOnlyDictionary<Guid, TO> orgs, 
                IReadOnlyDictionary<Guid, TP> phenotypes, 
                IReadOnlyDictionary<Guid, TE> phenotypeEvals, 
                SorterCompPoolStageType sorterCompPoolStageType, 
                Func<TO, IRando, TP> phenotyper, 
                Func<TO, IRando, TP> phenotypeEvaluator, 
                Func<IReadOnlyDictionary<Guid, TE>, int, IReadOnlyDictionary<Guid, TO>> nextGenerator, 
                Guid guid
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
            _guid = guid;
        }

        public ISorterCompPool<TO, TP, TE> Step(int seed)
        {
            for (var i = 0; i < 80000000; i++)
            {
                seed = seed ^ i;
            }

            switch (SorterCompPoolStageType)
            {
                case SorterCompPoolStageType.MakePhenotypes:
                    return new SorterCompPoolImpl<TO, TP, TE>
                        (
                            generation: Generation, 
                            orgs: Orgs, 
                            phenotypes: Phenotypes, 
                            phenotypeEvals: PhenotypeEvals, 
                            sorterCompPoolStageType:SorterCompPoolStageType.EvaluatePhenotypes,
                            phenotyper: Phenotyper, 
                            phenotypeEvaluator: PhenotypeEvaluator, 
                            nextGenerator: NextGenerator,
                            guid: Guid
                        );
                case SorterCompPoolStageType.EvaluatePhenotypes:
                    return new SorterCompPoolImpl<TO, TP, TE>
                        (
                            generation: Generation,
                            orgs: Orgs,
                            phenotypes: Phenotypes,
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.MakeNextGeneration,
                            phenotyper: Phenotyper,
                            phenotypeEvaluator: PhenotypeEvaluator,
                            nextGenerator: NextGenerator,
                            guid: Guid
                        );
                case SorterCompPoolStageType.MakeNextGeneration:
                    return new SorterCompPoolImpl<TO, TP, TE>
                        (
                            generation: Generation + 1,
                            orgs: NextGenerator(PhenotypeEvals, seed),
                            phenotypes: Phenotypes,
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                            phenotyper: Phenotyper,
                            phenotypeEvaluator: PhenotypeEvaluator,
                            nextGenerator: NextGenerator,
                            guid: Guid
                        );
                default:
                    throw new Exception(SorterCompPoolStageType + " not handled");
            }

        }


        private readonly SorterCompPoolStageType _sorterCompPoolStageType;
        public SorterCompPoolStageType SorterCompPoolStageType
        {
            get { return _sorterCompPoolStageType; }
        }

        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }

        private readonly IReadOnlyDictionary<Guid, TO> _orgs;
        public IReadOnlyDictionary<Guid, TO> Orgs
        {
            get { return _orgs; }
        }
        
        private readonly IReadOnlyDictionary<Guid, TP> _phenotypes;
        public IReadOnlyDictionary<Guid, TP> Phenotypes
        {
            get { return _phenotypes; }
        }
        
        private readonly IReadOnlyDictionary<Guid, TE> _phenotypeEvals;
        public IReadOnlyDictionary<Guid, TE> PhenotypeEvals
        {
            get { return _phenotypeEvals; }
        }

        private readonly Func<TO, IRando, TP> _phenotyper;
        public Func<TO, IRando, TP> Phenotyper
        {
            get { return _phenotyper; }
        }

        private readonly Func<TO, IRando, TP> _phenotypeEvaluator;
        public Func<TO, IRando, TP> PhenotypeEvaluator
        {
            get { return _phenotypeEvaluator; }
        }

        private readonly Func<IReadOnlyDictionary<Guid, TE>, int, IReadOnlyDictionary<Guid, TO>> _nextGenerator;
        public Func<IReadOnlyDictionary<Guid, TE>, int, IReadOnlyDictionary<Guid, TO>> NextGenerator
        {
            get { return _nextGenerator; }
        }

        //ISorterCompPool IRandomWalk<ISorterCompPool>.Step(int seed)
        //{
        //    return Step(seed);
        //}

        private readonly Guid _guid;
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
