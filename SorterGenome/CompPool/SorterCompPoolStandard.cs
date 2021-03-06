using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using MathUtils;
using MathUtils.Rand;
using SorterGenome.NextGeneration.NextGenSpecs;
using SorterGenome.PhenotypeEvals;
using SorterGenome.PhenotypeEvals.PhenotypeEvalSpecs;
using SorterGenome.Phenotypes;
using SorterGenome.Phenotypes.PhenotyperSpecs;
using Sorting.SwitchFunctionSets;
using Utils;

namespace SorterGenome.CompPool
{
    public class SorterCompPoolStandard : ISorterCompPool
    {
        public SorterCompPoolStandard
            (
                Guid guid,
                int generation,
                IReadOnlyDictionary<Guid, IGenome> genomes,
                IReadOnlyDictionary<Guid, ISorterPhenotype> phenotypes,
                IReadOnlyDictionary<Guid, ISorterPhenotypeEval> phenotypeEvals,
                SorterCompPoolStageType sorterCompPoolStageType,
                int keyCount,
                int orgCount,
                int legacyCount,
                double deletionRate, 
                double insertionRate,
                double mutationRate, 
                int cubCount,
                IPhenotyperSpec phenotyperSpec,
                IPhenotypeEvalSpec phenotyperEvalSpec,
                INextGenSpec nextGenSpec, 
                string name
            )
        {
            _generation = generation;
            _genomes = genomes;
            _phenotypes = phenotypes;
            _phenotypeEvals = phenotypeEvals;
            _sorterCompPoolStageType = sorterCompPoolStageType;
            _keyCount = keyCount;
            _orgCount = orgCount;
            _mutationRate = mutationRate;
            _cubCount = cubCount;
            _phenotyperSpec = phenotyperSpec;
            _phenotyperEvalSpec = phenotyperEvalSpec;
            _nextGenSpec = nextGenSpec;
            _name = name;
            _guid = guid;
            _legacyCount = legacyCount;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;

            KeyPairSwitchSet.Make<uint>(KeyCount);
        }

        public ISorterCompPool Step(int seed)
        {
            var randy = Rando.Fast(seed);
            switch (SorterCompPoolStageType)
            {
                case SorterCompPoolStageType.MakePhenotypes:

                    return new SorterCompPoolStandard
                        (
                            guid: randy.NextGuid(),
                            generation: Generation,
                            genomes: Genomes,
                            phenotypes: Genomes.Values
                                .SelectMany(g => Phenotyper(g, randy))
                                .ToDictionary(p => p.Guid),
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.EvaluatePhenotypes,
                            keyCount: KeyCount,
                            orgCount: OrgCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            legacyCount: LegacyCount,
                            cubCount: CubCount,
                            phenotyperSpec: PhenotyperSpec,
                            phenotyperEvalSpec: PhenotyperEvalSpec,
                            nextGenSpec: NextGenSpec,
                            name: Name
                        );

                case SorterCompPoolStageType.EvaluatePhenotypes:

                    return new SorterCompPoolStandard
                        (
                            guid: randy.NextGuid(),
                            generation: Generation,
                            genomes: Genomes,
                            phenotypes: Phenotypes,
                            phenotypeEvals: DoEvals(Phenotypes, randy),
                            sorterCompPoolStageType: SorterCompPoolStageType.MakeNextGeneration,
                            keyCount: KeyCount,
                            orgCount: OrgCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            legacyCount: LegacyCount,
                            cubCount: CubCount,
                            phenotyperSpec: PhenotyperSpec,
                            phenotyperEvalSpec: PhenotyperEvalSpec,
                            nextGenSpec: NextGenSpec,
                            name: Name
                        );

                case SorterCompPoolStageType.MakeNextGeneration:

                    return new SorterCompPoolStandard
                        (
                            guid: randy.NextGuid(),
                            generation: Generation + 1,
                            genomes: NextGenerator(PhenotypeEvals, seed),
                            phenotypes: Phenotypes,
                            phenotypeEvals: PhenotypeEvals,
                            sorterCompPoolStageType: SorterCompPoolStageType.MakePhenotypes,
                            keyCount: KeyCount,
                            orgCount: OrgCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            legacyCount: LegacyCount,
                            cubCount: CubCount,
                            phenotyperSpec: PhenotyperSpec,
                            phenotyperEvalSpec: PhenotyperEvalSpec,
                            nextGenSpec: NextGenSpec,
                            name: Name
                        );
                default:
                    throw new Exception(SorterCompPoolStageType + " not handled");
            }
        }

        IReadOnlyDictionary<Guid, ISorterPhenotypeEval> DoEvals(IReadOnlyDictionary<Guid, ISorterPhenotype> phenotypes, IRando randy)
        {
            var kpvs =
                phenotypes.Values.Select(
                    p => new Tuple<ISorterPhenotype, Guid, Guid>(p, randy.NextGuid(), randy.NextGuid()));

            return kpvs.AsParallel()
                .Select(t => PhenotypeEvaluator(t.Item1, t.Item2, t.Item3))
                .ToDictionary(pe => pe.Guid);
        }


        private readonly int _keyCount;

        public int KeyCount
        {
            get { return _keyCount; }
        }


        private readonly int _orgCount;
        public int OrgCount
        {
            get { return _orgCount; }
        }

        private readonly double _deletionRate;
        public double DeletionRate
        {
            get { return _deletionRate; }
        }

        private readonly double _insertionRate;
        public double InsertionRate
        {
            get { return _insertionRate; }
        }

        private readonly double _mutationRate;
        public double MutationRate
        {
            get { return _mutationRate; }
        }

        private readonly int _legacyCount;
        public int LegacyCount
        {
            get { return _legacyCount; }
        }

        private readonly int _cubCount;
        public int CubCount
        {
            get { return _cubCount; }
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

        private readonly IReadOnlyDictionary<Guid, ISorterPhenotype> _phenotypes;

        public IReadOnlyDictionary<Guid, ISorterPhenotype> Phenotypes
        {
            get { return _phenotypes; }
        }

        private readonly IReadOnlyDictionary<Guid, ISorterPhenotypeEval> _phenotypeEvals;

        public IReadOnlyDictionary<Guid, ISorterPhenotypeEval> PhenotypeEvals
        {
            get { return _phenotypeEvals; }
        }

        private readonly INextGenSpec _nextGenSpec;
        public INextGenSpec NextGenSpec
        {
            get { return _nextGenSpec; }
        }

        private readonly IPhenotyperSpec _phenotyperSpec;
        public IPhenotyperSpec PhenotyperSpec
        {
            get { return _phenotyperSpec; }
        }

        private readonly IPhenotypeEvalSpec _phenotyperEvalSpec;
        public IPhenotypeEvalSpec PhenotyperEvalSpec
        {
            get { return _phenotyperEvalSpec; }
        }

        private Func<
            IGenome, 
            IRando, 
            IEnumerable<ISorterPhenotype>> _phenotyper;

        public Func<
            IGenome, 
            IRando, 
            IEnumerable<ISorterPhenotype>> Phenotyper
        {
            get
            {
                return _phenotyper ?? (_phenotyper = PhenotyperSpec.ToPenotyper(KeyCount));
                
            }
        }

        private Func<
            ISorterPhenotype,
            Guid,
            Guid,
            ISorterPhenotypeEval> _phenotypeEvaluator;

        public Func<
            ISorterPhenotype, 
            Guid, 
            Guid,
            ISorterPhenotypeEval> PhenotypeEvaluator
        {
            get
            {
                return _phenotypeEvaluator ??
                       (_phenotypeEvaluator = PhenotyperEvalSpec.ToPhenotypeEval());
            }
        }

        private Func<
            IReadOnlyDictionary<Guid, ISorterPhenotypeEval>, 
            int, 
            IReadOnlyDictionary<Guid, IGenome>> _nextGenerator;

        public Func<
            IReadOnlyDictionary<Guid, ISorterPhenotypeEval>, 
            int, 
            IReadOnlyDictionary<Guid, IGenome>> NextGenerator
        {
            get 
            { 
                return _nextGenerator ?? 
                (
                    _nextGenerator = NextGenSpec.ToNextGenerator
                    (
                        keyCount : KeyCount, 
                        orgCount : OrgCount,
                        deletionRate: DeletionRate,
                        insertionRate: InsertionRate,
                        mutationRate: MutationRate,
                        legacyCount: LegacyCount,
                        cubCount: CubCount
                    ));
            }
        }

        ISorterCompPool IRandomWalk<ISorterCompPool>.Step(int seed)
        {
            return Step(seed);
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public IEntity GetPart(Guid key)
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

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        public string EntityName
        {
            get { return "SorterCompPool.Standard"; }
        }
    }
}