﻿using System;
using System.Collections.Generic;
using Genomic.Layers;
using Genomic.Workflows;
using MathUtils.Rand;

namespace SorterGenome
{
    public interface ISorterCompPool<TO,TP,TE>
        : IRandomWalk<ISorterCompPool<TO, TP, TE>>
        where TO : IOrg
        where TP : IPhenotype
        where TE : IPhenotypeEval
    {
        int Generation { get; }

        SorterCompPoolStageType SorterCompPoolStageType { get; }

        IReadOnlyDictionary<Guid, TO> Orgs { get; }

        IReadOnlyDictionary<Guid, TP> Phenotypes { get; }

        IReadOnlyDictionary<Guid, TE> PhenotypeEvals { get; }

        Func<TO, IRando, TP> Phenotyper { get; }

        Func<TO, IRando, TP> PhenotypeEvaluator { get; }

        Func<IReadOnlyDictionary<Guid, TE>, int, IReadOnlyDictionary<Guid, TO>> NextGenerator { get; }

    }

    public static class SorterCompPool<TO,TP,TE> 
        where TO : IOrg
        where TP : IPhenotype 
        where TE : IPhenotypeEval
    {
        public static ISorterCompPool<TO,TP,TE> Make()
        {
            return null;// new SorterCompPoolImpl();
        }
    }

    public enum SorterCompPoolStageType
    {
        MakePhenotypes,
        EvaluatePhenotypes,
        MakeNextGeneration
    }


    public class SorterCompPoolImpl<TO,TP,TE> : ISorterCompPool<TO,TP,TE>
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
                Func<IReadOnlyDictionary<Guid, TE>, int, IReadOnlyDictionary<Guid, TO>> nextGenerator
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

        public ISorterCompPool<TO, TP, TE> Step(int seed)
        {

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
                            nextGenerator: NextGenerator
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
                            nextGenerator: NextGenerator
                        );
                case SorterCompPoolStageType.MakeNextGeneration:
                    return new SorterCompPoolImpl<TO, TP, TE>
                        (
                            generation: Generation + 1,
                            orgs: Orgs,
                            phenotypes: Phenotypes,
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                            phenotyper: Phenotyper,
                            phenotypeEvaluator: PhenotypeEvaluator,
                            nextGenerator: NextGenerator
                        );
                default:
                    break;
            }

            //for (var i = 0; i < 80000000; i++)
            //{
            //    seed = seed ^ i;
            //}

            //if (Generation > 2)
            //{
            //    string ss = "s";
            //}

            //return new SorterCompPoolImpl(
                
            //    Generation + 1
                
            //    );

            throw new NotImplementedException();
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
    }
}
