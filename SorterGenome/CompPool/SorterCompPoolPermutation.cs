using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using MathUtils;
using MathUtils.Rand;
using SorterGenome.PhenotypeEvals;
using SorterGenome.Phenotypes;
using Utils;

namespace SorterGenome.CompPool
{
    public class SorterCompPoolPermutation : ISorterCompPool
    {
        public SorterCompPoolPermutation
            (
                Guid guid,
                int generation,
                IReadOnlyDictionary<Guid, IGenome> genomes,
                IReadOnlyDictionary<Guid, ISorterPhenotype> phenotypes,
                IReadOnlyDictionary<Guid, ISorterPhenotypeEval> phenotypeEvals,
                SorterCompPoolStageType sorterCompPoolStageType,
                int keyCount,
                int orgCount,
                double legacyRate,
                double deletionRate,
                double insertionRate,
                double mutationRate,
                double cubRate, 
                string phenotyperName, 
                string phenotyperEvaluatorName
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
            _cubRate = cubRate;
            _phenotyperName = phenotyperName;
            _phenotyperEvaluatorName = phenotyperEvaluatorName;
            _guid = guid;
            _legacyRate = legacyRate;
            _deletionRate = deletionRate;
            _insertionRate = insertionRate;
        }

        public ISorterCompPool Step(int seed)
        {
            switch (SorterCompPoolStageType)
            {
                case SorterCompPoolStageType.MakePhenotypes:

                    var randy = Rando.Fast(seed);
                    return new SorterCompPoolPermutation
                        (
                            guid: Guid.NewGuid(),
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
                            legacyRate: LegacyRate,
                            cubRate: CubRate,
                            phenotyperName: PhenotyperName,
                            phenotyperEvaluatorName: PhenotyperEvaluatorName
                        );

                case SorterCompPoolStageType.EvaluatePhenotypes:

                    var randy2 = Rando.Fast(seed);
                    return new SorterCompPoolPermutation
                        (
                            guid: Guid.NewGuid(),
                            generation: Generation,
                            genomes: Genomes,
                            phenotypes: Phenotypes,
                            phenotypeEvals: Phenotypes.Values
                                .Select(p => PhenotypeEvaluator(p, randy2))
                                .ToDictionary(pe => pe.Guid),
                            sorterCompPoolStageType: SorterCompPoolStageType.MakeNextGeneration,
                            keyCount: KeyCount,
                            orgCount: OrgCount,
                            deletionRate: DeletionRate,
                            insertionRate: InsertionRate,
                            mutationRate: MutationRate,
                            legacyRate: LegacyRate,
                            cubRate: CubRate,
                            phenotyperName: PhenotyperName,
                            phenotyperEvaluatorName: PhenotyperEvaluatorName
                        );

                case SorterCompPoolStageType.MakeNextGeneration:

                    return new SorterCompPoolPermutation
                        (
                            guid: Guid.NewGuid(),
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
                            legacyRate: LegacyRate,
                            cubRate: CubRate,
                            phenotyperName: PhenotyperName,
                            phenotyperEvaluatorName: PhenotyperEvaluatorName
                        );

                default:
                    throw new Exception(SorterCompPoolStageType + " not handled");
            }
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

        private readonly double _legacyRate;
        public double LegacyRate
        {
            get { return _legacyRate; }
        }

        private readonly double _cubRate;
        public double CubRate
        {
            get { return _cubRate; }
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

        private readonly string _phenotyperName;
        public string PhenotyperName
        {
            get { return _phenotyperName; }
        }

        private readonly string _phenotyperEvaluatorName;
        public string PhenotyperEvaluatorName
        {
            get { return _phenotyperEvaluatorName; }
        }

        private Func
            <
                IGenome, 
                IRando, 
                IEnumerable<ISorterPhenotype>
            > _phenotyper;

        public Func<
            IGenome, 
            IRando, 
            IEnumerable<ISorterPhenotype>> Phenotyper
        {
            get
            {
                return _phenotyper ?? 
                    (
                        _phenotyper = Phenotypers.MakePermuterSlider(KeyCount)
                    );
            }
        }

        private Func<
            ISorterPhenotype, 
            IRando, 
            ISorterPhenotypeEval> _phenotypeEvaluator;

        public Func<
            ISorterPhenotype, 
            IRando, 
            ISorterPhenotypeEval> PhenotypeEvaluator
        {
            get
            {
                return _phenotypeEvaluator ?? 
                    (_phenotypeEvaluator = null);
            }
        }

        private Func<
            IReadOnlyDictionary<Guid, ISorterPhenotypeEval>, 
            int, 
            IReadOnlyDictionary<Guid, IGenome>> _nextGenerator;

        public Func
            <
                IReadOnlyDictionary<Guid, ISorterPhenotypeEval>, 
                int, 
                IReadOnlyDictionary<Guid, IGenome>> NextGenerator
        {
            get
            {
                return _nextGenerator ??
                       (
                           _nextGenerator = new NextGeneratorForPermutationSorter
                               (
                                   keyCount: KeyCount,
                                   orgCount: OrgCount,
                                   deletionRate: DeletionRate,
                                   insertionRate: InsertionRate,
                                   mutationRate: MutationRate,
                                   legacyRate: LegacyRate,
                                   cubRate: CubRate
                               ).NextGenerator
                           );
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

        public string EntityName
        {
            get { return "SorterCompPool.Permutation"; }
        }
    }
}